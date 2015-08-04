using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles finding an object in the world of a particular tag, and returning the closest
/// </summary>
public class GetObject : MonoBehaviour {

    //which type of position to look for
    //in default package this separates looking for a PosPatScript/PosForScript
    public enum AgentType
    {
        Leader = 0,
        Follower = 1,
        VirtualLeader = 2
    }
    protected AgentType m_eGetObjType;
    public AgentType SetType { set { m_eGetObjType = value; } }

    //object that has been found, and get property to allow other scripts to interact with that object
    protected GameObject m_ObjectFound;
    public GameObject ObjFound { get { return m_ObjectFound; } }

    //how often to perform a new check,
    //this includes the case of it the agent has no object, it will wait till <variable> time is up
    //before searching again
    protected float m_fCheckObjFoundTimer = 3f;
    protected string m_sTagToGet = "";

    //how close to look and how far to look
    //this is primarily to avoid Leaders getting paths on themselves, but can be used for other means
    protected float m_fMinimumGetDistance = 1f;
    protected float m_fMaximumGetDistance = 100f;

    void Start()
    {
        StartCoroutine(CheckObjFound());
    }

    //public function to set values, used primarily by <...>Main Scripts
    public void SetSettings(float _checkTimer, string _tagTofind, float _minGetDis, float _maxGetDis)
    {
        if (_checkTimer < 0f)
            Debug.LogWarning("FLAG: A GetObject was given an invalid value: " + _checkTimer + " Check Timer, " + gameObject);
        else
            m_fCheckObjFoundTimer = _checkTimer;

        if (_tagTofind == "")
            Debug.LogWarning("FLAG: A GetObject was given an invalid value: " + _tagTofind + " Tag to find, " + gameObject);
        else
            m_sTagToGet = _tagTofind;

        if (_minGetDis < 0f || _maxGetDis < 0f)
            Debug.LogWarning("FLAG: A GetObject was given an invalid value: " + _minGetDis + " Min Dist., " + _maxGetDis + " Max Dist., " + gameObject);
        else
        {
            m_fMinimumGetDistance = _minGetDis;
            m_fMaximumGetDistance = _maxGetDis;
        }
    }

    //checks if there is an object, if not, get a new one
    protected IEnumerator CheckObjFound()
    {
        while (true)
        {
            //if this script instance has not found an object to goto
            if (m_ObjectFound == null)
            {
                //start coroutine with ReturnUnlockedObjects
                StartCoroutine(FindClosestObj(ReturnUnlockedObjects()));
            }

            yield return new WaitForSeconds(m_fCheckObjFoundTimer);
        }
    }

    //depending on agent type, returns a list of unlocked PositionObj scripts
    protected List<GameObject> ReturnUnlockedObjects()
    {
        List<GameObject> _unlocked = new List<GameObject>();
        GameObject[] _foundObjs = new GameObject[0];

        if (m_sTagToGet != "" && GameObject.FindGameObjectWithTag(m_sTagToGet))
            _foundObjs = GameObject.FindGameObjectsWithTag(m_sTagToGet);
        else
            return _unlocked;
            

        //if agent type
        if (m_eGetObjType == AgentType.Leader
            || m_eGetObjType == AgentType.Follower)
        {
            //loop throuh list
            foreach (GameObject _obj in _foundObjs)
            {
                //if object is unlocked, and has the correct enumeration type
                if (!_obj.GetComponent<PositionObj>().IsLocked)
                {
                    if (m_eGetObjType == AgentType.Leader
                        && _obj.GetComponent<PositionObj>().Type == PositionObj.PosType.Path)
                    {
                        //if it has a group number, and is equal to the Main's group number
                        //OR the position is set to -1 (all)
                        //add it to the list
                        if (_obj.GetComponent<PositionObj>().GroupNum == gameObject.GetComponent<AgentMain>().AgntGroupNum
                            || _obj.GetComponent<PositionObj>().GroupNum == -1)
                            _unlocked.Add(_obj);
                    }
                    else if (m_eGetObjType == AgentType.Follower
                        && _obj.GetComponent<PositionObj>().Type == PositionObj.PosType.Formation)
                    {
                        //same, except for it this agent is -1, add new formation
                        if (_obj.GetComponent<PositionObj>().GroupNum == gameObject.GetComponent<AgentMain>().AgntGroupNum
                            || gameObject.GetComponent<AgentMain>().AgntGroupNum == -1)
                            _unlocked.Add(_obj);
                    }
                }
            }
        }
        else if (m_eGetObjType == AgentType.VirtualLeader)
        {
            foreach (GameObject _obj in _foundObjs)
            {
                if (!_obj.GetComponent<LdrVirtualMain>().HasVirtualLeader)
                {
                    if (_obj.GetComponent<LdrVirtualMain>().AgntGroupNum == gameObject.GetComponent<AgentMain>().AgntGroupNum
                        || gameObject.GetComponent<AgentMain>().AgntGroupNum == -1)
                        _unlocked.Add(_obj);
                }
            }
        }

        return _unlocked;
    }

    //returns closest object from a list of objects
    protected IEnumerator FindClosestObj(List<GameObject> _objsToCheck)
    {
        //the current position of this object
        Vector3 _CurrPos = gameObject.transform.position;
        //whether an object has been found
        bool _found = false;

        //last magnitude to check against
        float _lastClosstmagn = m_fMaximumGetDistance;
        //last index that was the closest to the object
        int lastClosestIndex = 0;
        //current index in list
        int index = 0;

        //if there are no objects cancel loop
        if (_objsToCheck.Count < 1)
            _found = true;

        while(!_found)
        {
            //if the list end has been reached
            if (index >= _objsToCheck.Count)
            {
                //get the last closest
                m_ObjectFound = _objsToCheck[lastClosestIndex];
                //has found an object, so exit this while loop on next iteration
                _found = true;
                break;
            }

            //else if still in list, check magnitude
            Vector3 _newCheck = _CurrPos - _objsToCheck[index].transform.position;
            //if it is closer than the last, and is over minimum distance
            if (_newCheck.magnitude < _lastClosstmagn
                && _newCheck.magnitude > m_fMinimumGetDistance)
            {
                //this is the new closest if both have been met
                _lastClosstmagn = _newCheck.magnitude;
                lastClosestIndex = index;
            }

            index++;
            yield return new WaitForEndOfFrame();
        }

        //now check that the found object has not already been taken
        if (_objsToCheck.Count > 0)
        {
            if (m_ObjectFound.GetComponent<PositionObj>() && m_ObjectFound.GetComponent<PositionObj>().IsLocked)
            {
                m_ObjectFound = null;

                //loop and add any free positions to a new list, then start afresh
                List<GameObject> _newList = new List<GameObject>();
                foreach (GameObject _obj in _objsToCheck)
                {
                    if (!_obj.GetComponent<PositionObj>().IsLocked)
                        _newList.Add(_obj);
                }

                FindClosestObj(_newList);
            }
            else if (m_ObjectFound.GetComponent<LdrVirtualMain>() && m_ObjectFound.GetComponent<LdrVirtualMain>().HasVirtualLeader)
            {
                m_ObjectFound = null;

                //loop and add any free positions to a new list, then start afresh
                List<GameObject> _newList = new List<GameObject>();
                foreach (GameObject _obj in _objsToCheck)
                {
                    if (!_obj.GetComponent<PositionObj>().IsLocked)
                        _newList.Add(_obj);
                }

                FindClosestObj(_newList);
            }
            else
            {
                if (m_eGetObjType == AgentType.Leader)
                {
                    m_ObjectFound.GetComponent<PositionObj>().vSetLock(true);
                }
                else if (m_eGetObjType == AgentType.Follower)
                {
                    //if this follower is unset, make it equal to the group it has found
                    if (gameObject.GetComponent<AgentMain>().AgntGroupNum == -1)
                        gameObject.GetComponent<AgentMain>().AgntGroupNum = m_ObjectFound.GetComponent<PosForScript>().GroupNum;

                    m_ObjectFound.GetComponent<PositionObj>().vSetLock(true);
                }
                else if (m_eGetObjType == AgentType.VirtualLeader)
                {
                    if (gameObject.GetComponent<AgentMain>().AgntGroupNum == -1)
                        gameObject.GetComponent<AgentMain>().AgntGroupNum = m_ObjectFound.GetComponent<LdrVirtualMain>().AgntGroupNum;

                    gameObject.GetComponent<LdrCreate>().UpdatePositions();

                    m_ObjectFound.GetComponent<LdrVirtualMain>().HasVirtualLeader = true;
                }
            }
        }            
    }
}
