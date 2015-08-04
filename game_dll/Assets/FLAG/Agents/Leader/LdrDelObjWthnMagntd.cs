using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as a way to get rid of Path Positions, 
/// acting as an example of interacting with them:
/// For example this could be replaced with a Patrolling script to get the next position
/// </summary>
public class LdrDelObjWthnMagntd : MonoBehaviour {

    //used to set whether to delete or not
    //allows for controlled deletion as needed
    [SerializeField] private bool m_bDelete = true;
    public bool DeleteWhenInRange { get { return m_bDelete; } set { m_bDelete = value; } }

    private GameObject m_goObjFound;
    //How close to get in magnitude before calling delete
	private float m_fDelDistance = 1f;
    private float m_fCheckInterval = 2f;

    void Start()
    {
        StartCoroutine(CheckForDeletion());
    }

    public void SetSettings(float _delDist, float _checkTime)
    {
        if (_delDist < 0f || _checkTime < 0f)
            Debug.LogWarning("FLAG: An object with LdrDelObjWthnMagntd was given invalid values: "
                + _delDist + " Del Dist, " + _checkTime + " Check Time, " + gameObject);
        else
        {
            m_fDelDistance = _delDist;
            m_fCheckInterval = _checkTime;
        }
    }

    IEnumerator CheckForDeletion()
    {
        while(true)
        {
            if (m_goObjFound == null)
                m_goObjFound = gameObject.GetComponent<GetObject>().ObjFound;
            else
                if ((gameObject.transform.position - m_goObjFound.transform.position).magnitude <= m_fDelDistance && m_bDelete)
                    m_goObjFound.GetComponent<PosPatScript>().vDelete();

            yield return new WaitForSeconds(m_fCheckInterval);
        }
    }
}
