using UnityEngine;
using System;
using System.Collections;

public class UserDeleteObj : MonoBehaviour {

    [SerializeField] private string m_sLdrTag;
    [SerializeField] private string m_sFlrTag;
    private KeyCode m_kcDelAll = KeyCode.Delete;
    private KeyCode m_kcDelLdrs = KeyCode.B;
    private KeyCode m_kcDelFlrs = KeyCode.N;

	void Update()
	{
		if(Input.GetKey(m_kcDelAll))
		{
            vDeleteAll();
        }
        else if (Input.GetKey(m_kcDelLdrs))
        {
            vDeleteLeaders();
        }
        else if (Input.GetKey(m_kcDelFlrs))
        {
            vDeleteFollowers();
        }
	}

    public void vDeleteAll()
    {
        vDeleteLeaders();
        vDeleteFollowers();
    }
    public void vDeleteLeaders()
    {
        GameObject[] _tempLdrs = GameObject.FindGameObjectsWithTag(m_sLdrTag);

        if (_tempLdrs.Length < 1)
            return;

        foreach (GameObject obj in _tempLdrs)
        {
            Destroy(obj);
        }

        Array.Clear(_tempLdrs, 0, _tempLdrs.Length);
    }
    public void vDeleteFollowers()
    {
        GameObject[] _tempFlrs = GameObject.FindGameObjectsWithTag(m_sFlrTag);

        if (_tempFlrs.Length < 1)
            return;

        foreach (GameObject obj in _tempFlrs)
        {
            Destroy(obj);
        }

        Array.Clear(_tempFlrs, 0, _tempFlrs.Length);


        GameObject[] _tempLdrs = GameObject.FindGameObjectsWithTag(m_sLdrTag);
        foreach (GameObject obj in _tempLdrs)
        {
            obj.GetComponent<LdrMain>().vRegenerate();
        }
    }
}
