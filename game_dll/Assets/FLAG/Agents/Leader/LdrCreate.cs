using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles creating and clearing formation positions, provides formation to world-space vector function.
/// </summary>
public class LdrCreate : MonoBehaviour 
{
    //Used to select which method to generate
    public enum SpawnMethod
    {
        Block = 0,
        Diamond = 1,
        Circle = 2,
        Border = 3
    }
    //current spawn method used, for regeneration
    private SpawnMethod m_eSpawnMethod = SpawnMethod.Block;

    //Postion object to spawn
    private GameObject m_goSpawnPrefab;
    public GameObject SetPrefab { set { m_goSpawnPrefab = value; } }
    private Dictionary<Vector2, GameObject> m_goFormPosns = new Dictionary<Vector2, GameObject>();
    public Dictionary<Vector2, GameObject> FormationPositions { get { return m_goFormPosns; } }

    //Generation Values
    private int m_iXNegSpawns = 0;
    private int m_iXPosSpawns = 0;
    private int m_iYNegSpawns = 0;
    private int m_iYPosSpawns = 0;

    private float m_fXSpacing = 0f;
    private float m_fYSpacing = 0f;

    /// <summary>
    /// Sets values, checks spawn type, clears formation then calls generate function
    /// </summary>
    public void vGenerateFormation(SpawnMethod _type, int _xP, int _xN, int _yP, int _yN, float _xSpacing, float _ySpacing)
    {
        if (m_iXNegSpawns < 0 || m_iYNegSpawns < 0 || m_iXPosSpawns < 0 || m_iYPosSpawns < 0)
            Debug.LogWarning("FLAG: A LdrCreate was given invalid Generation values: "
                + m_iXPosSpawns + "x/" + m_iXNegSpawns + " -x/" + m_iYPosSpawns + " y/" + m_iYNegSpawns + " -y/" 
                + gameObject);
        else
        {
            m_iXNegSpawns = _xN;
            m_iYNegSpawns = _yN;
            m_iXPosSpawns = _xP;
            m_iYPosSpawns = _yP;
        }

        if (m_fXSpacing < 0f && m_fYSpacing < 0f)
            Debug.LogWarning("FLAG: A LdrCreate was given invalid Spacing values: " + m_fXSpacing + "x, " + m_fYSpacing + "y, " + gameObject);
        else
        {
            m_fXSpacing = _xSpacing;
            m_fYSpacing = _ySpacing;
        }

        if (Enum.IsDefined(typeof(SpawnMethod), _type))
            m_eSpawnMethod = _type;
        else
            Debug.LogWarning("FLAG: A LdrCreate was given an invalid enumeration type value: " + _type + ", " + gameObject);

        vGenerateSelect();
    }
    public void vRegeneratePrevious()
    {
        vClearFormation();
        vGenerateSelect();
    }

    /// <summary>
    /// Loops and clears formation
    /// </summary>
    public void vClearFormation()
    {
        foreach(GameObject _value in m_goFormPosns.Values)
        {
            _value.BroadcastMessage("vDelete");
        }

        m_goFormPosns.Clear();
    }

    /// <summary>
    /// Given an x/y in terms of the formation, returns a vector in world-space
    /// </summary>
    /// <param name="_xval"> Rig/Lef of Leader</param>
    /// <param name="_yval"> For/Bac of Leader</param>
    /// <returns>Rotated X/Y/Z from Leader Transform in World-Space </returns>
    public Vector3 v3PositionToVector(int _xval, int _yval)
    {
        Vector3 _temp = gameObject.transform.position;

        if(m_eSpawnMethod == SpawnMethod.Circle)
        {
            if (_yval % 2 == 0)
                _temp += (_xval * (gameObject.transform.right * m_fXSpacing));
            else
            {
                float _mod = 0.5f;

                if (_xval > 0f)
                    _temp += ((_xval - _mod) * (gameObject.transform.right * m_fXSpacing));
                else
                    _temp += ((_xval + _mod) * (gameObject.transform.right * m_fXSpacing));
            }

            _temp += (_yval * (gameObject.transform.forward * m_fYSpacing));
        }
        else
        {
            _temp += (_xval * (gameObject.transform.right * m_fXSpacing));
            _temp += (_yval * (gameObject.transform.forward * m_fYSpacing));
        }

        return _temp;
    }

    //selects the correct generation function from the set spawntype
    private void vGenerateSelect()
    {
        switch (m_eSpawnMethod)
        {
            case SpawnMethod.Block:
                vGenerateSquare();
                break;
            case SpawnMethod.Diamond:
                vGenerateDiamond();
                break;
            case SpawnMethod.Circle:
                vGenerateCircle();
                break;
            case SpawnMethod.Border:
                vGenerateBorder();
                break;
        }
    }
    //instantiates and sets up the spawn prefab given
    private void vSpawnPosition(int _xVal, int _yVal)
    {
        if (bPositionAtLocation(_xVal, _yVal))
            return;

        GameObject formationClone = (GameObject)Instantiate(m_goSpawnPrefab, v3PositionToVector(_xVal, _yVal), gameObject.transform.rotation);
        formationClone.transform.parent = gameObject.transform;

        int _ldrNum = gameObject.GetComponent<LdrMain>().AgntGroupNum;
        formationClone.GetComponent<PosForScript>().vSetValues(_xVal, _yVal, _ldrNum);

        Vector2 _dictKey = new Vector2(_xVal, _yVal);
        m_goFormPosns.Add(_dictKey, formationClone);
    }
    //loops through the current list of positions to check if there is already one present at location
    private bool bPositionAtLocation(int _xCheck, int _yCheck)
    {
        Vector2 _dictKey = new Vector2(_xCheck, _yCheck);
        if (m_goFormPosns.ContainsKey(_dictKey))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool bInvalidLocation(int _xCheck, int _yCheck)
    {
        if (_xCheck < 0 || _xCheck > (m_iXPosSpawns + m_iXNegSpawns)
            || _yCheck < 0 || _yCheck > (m_iYPosSpawns + m_iYNegSpawns))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Loop through all positions in formation and update values
    /// </summary>
    public void UpdatePositions()
    {
        int _ldrNum = gameObject.GetComponent<LdrMain>().AgntGroupNum;

        foreach (GameObject _value in m_goFormPosns.Values)
        {
            _value.GetComponent<PosForScript>().GroupNum = _ldrNum;
        }
    }

    //Each generates a set of positions via for loops
    private void vGenerateSquare()
    {
        //generate central position
        if (gameObject.GetComponent<LdrMain>().eLdrBehType != LdrMain.LdrBehaviourType.Virtual)
            vSpawnPosition(0, 0);

        //generate central depth forward/back of leader
        for (int y = 1; y <= m_iYNegSpawns; y++)
            vSpawnPosition(0, -y);

        for (int y = 1; y <= m_iYPosSpawns; y++)
            vSpawnPosition(0, y);
        

        //generate left width
        for (int x = 1; x <= m_iXNegSpawns; x++)
        {
            //along leader line
            vSpawnPosition(-x, 0);
            
            //then for every forward/backward depth, create a point
            for (int y = 1; y <= m_iYNegSpawns; y++)
                vSpawnPosition(-x, -y);

            for (int y = 1; y <= m_iYPosSpawns; y++)
                vSpawnPosition(-x, y);
        }

        //generate right width
        for (int x = 1; x <= m_iXPosSpawns; x++)
        {
            vSpawnPosition(x, 0);

            for (int y = 1; y <= m_iYNegSpawns; y++)
                vSpawnPosition(x, -y);

            for (int y = 1; y <= m_iYPosSpawns; y++)
                vSpawnPosition(x, y);
        }
    }
 
    private void vGenerateDiamond()
    {
        if (gameObject.GetComponent<LdrMain>().eLdrBehType != LdrMain.LdrBehaviourType.Virtual)
            vSpawnPosition(0, 0);

        for (int y = 1; y <= m_iYNegSpawns; y++)
            vSpawnPosition(0, -y);

        for (int y = 1; y <= m_iYPosSpawns; y++)
            vSpawnPosition(0, y);


        for (int x = 1; x <= m_iXNegSpawns; x++)
        {
            vSpawnPosition(-x, 0);

            for (int y = 1; y <= m_iYNegSpawns - x; y++)
                vSpawnPosition(-x, -y);

            for (int y = 1; y <= m_iYPosSpawns - x; y++)
                vSpawnPosition(-x, y);
        }

        for (int x = 1; x <= m_iXPosSpawns; x++)
        {
            vSpawnPosition(x, 0);

            for (int y = 1; y <= m_iYNegSpawns - x; y++)
                vSpawnPosition(x, -y);

            for (int y = 1; y <= m_iYPosSpawns - x; y++)
                vSpawnPosition(x, y);
        }
    }

    private void vGenerateCircle()
    {
        if (gameObject.GetComponent<LdrMain>().eLdrBehType != LdrMain.LdrBehaviourType.Virtual)
            vSpawnPosition(0, 0);

        //generate base leader width
        for (int x = 1; x <= m_iXPosSpawns; x++)
            vSpawnPosition(x, 0);
        for (int x = 1; x <= m_iXNegSpawns; x++)
            vSpawnPosition(-x, 0);

        //for every depth
        for (int _y = 1; _y <= m_iYPosSpawns; _y++)
        {
            //if the depth is an even value
            if (_y % 2 == 0)
            {
                //generate along leader line
                vSpawnPosition(0, _y);
                //and generate a normal width - current depth
                for (int _x = 1; _x <= m_iXPosSpawns - (_y - 1); _x++)
                    vSpawnPosition(_x, _y);
                for (int _x = 1; _x <= m_iXNegSpawns - (_y - 1); _x++)
                    vSpawnPosition(-_x, _y);
            }
            //else for every odd value:
                //take away y - 1, where return vector handles pushing them off by half
            else
            {
                for (int _x = 1; _x <= m_iXPosSpawns - (_y - 1); _x++)
                    vSpawnPosition(_x, _y);
                for (int _x = 1; _x <= m_iXNegSpawns - (_y - 1); _x++)
                    vSpawnPosition(-_x, _y);
            }
        }

        for (int _y = 1; _y <= m_iYPosSpawns; _y++)
        {
            if (_y % 2 == 0)
            {
                vSpawnPosition(0, -_y);
                for (int _x = 1; _x <= m_iXPosSpawns - (_y - 1); _x++)
                    vSpawnPosition(_x, -_y);
                for (int _x = 1; _x <= m_iXNegSpawns - (_y - 1); _x++)
                    vSpawnPosition(-_x,- _y);
            }
            else
            {
                for (int _x = 1; _x <= m_iXPosSpawns - (_y - 1); _x++)
                    vSpawnPosition(_x, -_y);
                for (int _x = 1; _x <= m_iXNegSpawns - (_y - 1); _x++)
                    vSpawnPosition(-_x, -_y);
            }
        }
    }

    private void vGenerateBorder()
    {
        //0 + this value to set border depth
        int _borderDepth = 1;

        if (gameObject.GetComponent<LdrMain>().eLdrBehType != LdrMain.LdrBehaviourType.Virtual)
            vSpawnPosition(0, 0);

        for (int y = 0; y <= m_iYNegSpawns + _borderDepth; y++)
        {
            if(y > 0)
                vSpawnPosition(0, -y);

            if (y >= (m_iYNegSpawns % 2))
            {
                for (int x = m_iXNegSpawns; x <= m_iXNegSpawns + _borderDepth; x++)
                    vSpawnPosition(-x, -y);

                for (int x = m_iYPosSpawns; x <= m_iYPosSpawns + _borderDepth; x++)
                    vSpawnPosition(x, -y);
            }
        }

        for (int y = 0; y <= m_iYPosSpawns + _borderDepth; y++)
        {
            if (y > 0)
                vSpawnPosition(0, y);

            if (y >= (m_iYPosSpawns % 2))
            {
                for (int x = m_iXNegSpawns; x <= m_iXNegSpawns + _borderDepth; x++)
                    vSpawnPosition(-x, y);

                for (int x = m_iYPosSpawns; x <= m_iYPosSpawns + _borderDepth; x++)
                    vSpawnPosition(x, y);
            }
        }


        for (int x = 0; x <= m_iXNegSpawns + _borderDepth; x++)
        {
            if (x > 0)
                vSpawnPosition(-x, 0);

            if (x >= (m_iXNegSpawns % 2))
            {
                for (int y = m_iYNegSpawns; y <= m_iYNegSpawns + _borderDepth; y++)
                    vSpawnPosition(-x, -y);

                for (int y = m_iYPosSpawns; y <= m_iYPosSpawns + _borderDepth; y++)
                    vSpawnPosition(-x, y);
            }
        }

        for (int x = 0; x <= m_iXPosSpawns + _borderDepth; x++)
        {
            if (x > 0)
                vSpawnPosition(x, 0);

            if (x >= (m_iXPosSpawns % 2))
            {
                for (int y = m_iYNegSpawns; y <= m_iYNegSpawns + _borderDepth; y++)
                    vSpawnPosition(x, -y);

                for (int y = m_iYPosSpawns; y <= m_iYPosSpawns + _borderDepth; y++)
                    vSpawnPosition(x, y);
            }
        }
    }
}
