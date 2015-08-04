using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls where to move positions in the formation that call to be moved
/// to another part of it.
/// </summary>
public class LdrFormationMovement : MonoBehaviour
{
    //Post-Formation Entry Status
    private enum PFEStatus
    {
        Form = -1,
        Empty = 0,
        Taken = 1,
        Obstr = 2
    }
    //list of bools set to size of formation, bool represents whether post space is taken
    private List<List<PFEStatus>> m_ePostFormSpaces = new List<List<PFEStatus>>();
    private float m_fObjBackToOriginTime = 3f;
    private float m_fResetPostFormTime = 3f;

    //total width/depth of formation
    private int m_iXTotWidth = 3;
    private int m_iYTotDepth = 3;
    //where to start checking from
    //i.e. if the formation works to -2, this script will post-form from -3
    private int m_iYMinDepth = 4;
    private int m_ixMinWidth = 3;
    
    //Sets up bool list
    void Awake()
    {
        StartCoroutine(RefreshObstructeds());
    }
    public void SetSettings(int _yPos, int _xPos, int _yNeg, int _xNeg, float _originTime, float _refreshPostFormTime)
    {
        m_ePostFormSpaces.Clear();

        if (_yPos < 0 || _xPos < 0 || _yNeg < 0 || _xNeg < 0)
            Debug.LogWarning("FLAG: A LdrFormationMovement was given invalid Generation values: "
                + _xPos + "x/" + _xNeg + " -x/" + _yPos + " y/" + _yNeg + " -y/"
                + gameObject);
        else
        {
            m_iXTotWidth = Mathf.CeilToInt((Mathf.Abs(_xNeg) + _xPos) * 1.2f);
            m_iYTotDepth = Mathf.CeilToInt((Mathf.Abs(_yNeg) + _yPos) * 1.5f);
            m_iYMinDepth = Mathf.Abs(_yNeg) + 2;
            m_ixMinWidth = Mathf.Abs(_xNeg);
        }

        if (m_fObjBackToOriginTime < 0f || m_fResetPostFormTime < 0f)
            Debug.LogWarning("FLAG: A LdrFormationMovement was given invalid Time values: "
                + m_fObjBackToOriginTime + " To origin time, " + m_fResetPostFormTime + " Reset Obstructed Entries Time," + gameObject);
        else
        {
            m_fObjBackToOriginTime = _originTime;
            m_fResetPostFormTime = _refreshPostFormTime;
        }

        for (int i = 0; i < m_iYTotDepth; i++)
        {
            List<PFEStatus> _enumEntry = new List<PFEStatus>();

            for (int ii = 0; ii < m_iXTotWidth; ii++)
            {
                _enumEntry.Add(PFEStatus.Empty);
            }

            m_ePostFormSpaces.Add(_enumEntry);
        }
    }

    //called by positions looking to move back
    public void EnterNewObjToMove(GameObject _obj, bool _obstructed)
    {
        //if obstructed, set the space value from the position to obstr
        if (_obstructed)
        {
            StatsGUIScript.Instance.UpObsCall();
            vSetSpaceValue(_obj.GetComponent<PosForScript>().v2PostPosition, PFEStatus.Obstr);
        }
        else
            StatsGUIScript.Instance.UpMoveCall();


        //calculate a new position in array space
        Vector2 _PositionEntry = v2ReturnNewSpace(_obj.GetComponent<PosForScript>().CheckTags);

        _obj.GetComponent<PosForScript>().v2PostPosition = _PositionEntry;
        //get a vector3 in world space with:
        //width space of formation
        //depth space, from last negative value, taking the given array space + total depth
        Vector3 _newPlace = gameObject.GetComponent<LdrCreate>().v3PositionToVector(
                                -m_ixMinWidth - -((int)_PositionEntry.x),
                                -m_iYMinDepth - -((int)_PositionEntry.y)
                                );

        _obj.transform.position = _newPlace;

        StartCoroutine(WaitToPutPositionBack( _obj));
    }

    //Coroutine called for each object, waits for ObjBackToOriginTime in seconds,
    //when done uses same calculation as Ldr2Create to get the proper location
    IEnumerator WaitToPutPositionBack(GameObject _objToMove)
    {
        float _countdown = m_fObjBackToOriginTime;
        while (_countdown > 0f)
        {
            _countdown -= Time.deltaTime;

            if (eGetSpaceValue(_objToMove.GetComponent<PosForScript>().v2PostPosition) == PFEStatus.Empty)
                vSetSpaceValue(_objToMove.GetComponent<PosForScript>().v2PostPosition, PFEStatus.Taken);

            yield return new WaitForEndOfFrame();
        }
        
        Vector3 _origin = gameObject.GetComponent<LdrCreate>().v3PositionToVector(
                                (int)_objToMove.GetComponent<PosForScript>().v2OrigPosition.x,
                                (int)_objToMove.GetComponent<PosForScript>().v2OrigPosition.y
                                );

        _objToMove.transform.position = _origin;

        if (eGetSpaceValue(_objToMove.GetComponent<PosForScript>().v2PostPosition) == PFEStatus.Taken)
            vSetSpaceValue(_objToMove.GetComponent<PosForScript>().v2PostPosition, PFEStatus.Empty);

        _objToMove.GetComponent<PosForScript>().bAskedToMove = false;
        _objToMove.GetComponent<PosForScript>().v2PostPosition = Vector2.zero;
    }
    IEnumerator RefreshObstructeds()
    {
        while(true)
        {
            yield return new WaitForSeconds(m_fResetPostFormTime);

            for (int _yDepth = 0; _yDepth < m_ePostFormSpaces.Count; _yDepth++)
                for (int _xDepth = 0; _xDepth < m_ePostFormSpaces[0].Count; _xDepth++)
                    if (m_ePostFormSpaces[_yDepth][_xDepth] == PFEStatus.Obstr)
                        m_ePostFormSpaces[_yDepth][_xDepth] = PFEStatus.Empty;
        }
    }

    //loops through the private List<List<bool>> to find the first empty space on each row
    //returns a vector2 in terms of array space
    private Vector2 v2ReturnNewSpace(List<string> _castStrings)
    {
        Vector2 _ReturnData = new Vector2();

        //foreach y depth
        for (int _yDepth = 0; _yDepth < m_ePostFormSpaces.Count; _yDepth++)
        {
            //foreach x width
            for (int _xWidth = 0; _xWidth < m_ePostFormSpaces[0].Count; _xWidth++)
            {
                //if the found position is empty
                if (m_ePostFormSpaces[_yDepth][_xWidth] == PFEStatus.Empty)
                {
                    _ReturnData.y = _yDepth;
                    _ReturnData.x = _xWidth;

                    m_ePostFormSpaces[_yDepth][_xWidth] = PFEStatus.Taken;
                    return _ReturnData;
                }
            }
        }
        //else no false found, set to min + half width
        _ReturnData.y = 0;
        _ReturnData.x = Mathf.Abs(m_iXTotWidth / 2);
        return _ReturnData;
    }

    //given a space in terms of this post-formation array:
    //will return value from it
    private PFEStatus eGetSpaceValue(Vector2 _Space)
    {
        return m_ePostFormSpaces[(int)_Space.y][(int)_Space.x];
    }
    //will set value to it
    private void vSetSpaceValue(Vector2 _Space, PFEStatus _state)
    {
        if ((int)_Space.y < m_ePostFormSpaces.Count
            && (int)_Space.x < m_ePostFormSpaces[0].Count)
        {
            m_ePostFormSpaces[(int)_Space.y][(int)_Space.x] = _state;
        }
        else
        {
            Debug.LogError("FLAG: vSetSpaceValue got an invalid array entry: y" + _Space.y + ", x" + _Space.x + ", " + gameObject);
        }
    }
}