using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserCreateObj : MonoBehaviour {

    private KeyCode m_kcSpawnLdr = KeyCode.T;
    private KeyCode m_kcSpawnFlr = KeyCode.Y;
    private KeyCode m_kcSpawnPat = KeyCode.U;

    [SerializeField] private bool m_bSpawnVirtualLeader = false;
    [SerializeField] private GameObject PrfbLdr1;
    [SerializeField] private GameObject PrfbLdr2;
    [SerializeField] private GameObject PrfbFlr;
    [SerializeField] private GameObject PrfbPat;

    [SerializeField] private InputField PathNumEnterField;

	void Update () 
    {
	    if(Input.GetKeyDown(m_kcSpawnLdr))
        {
            if (!m_bSpawnVirtualLeader)
                vSpawn(PrfbLdr1);
            else
            {
                vSpawn(PrfbLdr1);
                vSpawn(PrfbLdr2);
            }
        }
        else if (Input.GetKeyDown(m_kcSpawnFlr))
        {
            vSpawn(PrfbFlr);
        }
        else if (Input.GetKeyDown(m_kcSpawnPat))
        {
            vCreatePath();
        }
	}

    void vSpawn(GameObject _obj)
    {
        GameObject objClone = (GameObject)Instantiate(_obj, transform.position, transform.rotation);
    }

    public void vCreatePath()
    {
        GameObject objClone = (GameObject)Instantiate(PrfbPat, transform.position, transform.rotation);

        if (PathNumEnterField.text != "")
        {
            objClone.GetComponent<PosPatScript>().GroupNum = int.Parse(PathNumEnterField.text);
        }
    }
    public void vCreateLeader()
    {
        if (!m_bSpawnVirtualLeader)
            vSpawn(PrfbLdr1);
        else
        {
            vSpawn(PrfbLdr1);
            vSpawn(PrfbLdr2);
        }
    }
    public void vCreateFollower()
    {
        vSpawn(PrfbFlr);
    }
}
