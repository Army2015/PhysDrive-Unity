using UnityEngine;
using System.Collections;

/// <summary>
/// When Follower is standing still, this will rotate it to face the formation direction, and slowly move on top of it's position
/// </summary>
public class FlrLockIntoForm : MonoBehaviour 
{
    private GameObject m_goObjFound;

    //how close to detect when to lock into formation
	private float m_fMinMagnitude = 1f;
    private float m_fCheckTimer = 1.5f;

    private Vector3 m_v3PastPosition;
    private Quaternion m_v3PastRotation;

	void Start () 
    {
		m_v3PastPosition = gameObject.transform.position;
        m_v3PastRotation = gameObject.transform.rotation;
        StartCoroutine(CheckToLock());
	}

    public void SetSettings(float _checkDist, float _checktime)
    {
        m_fMinMagnitude = _checkDist;
        m_fCheckTimer = _checktime;
    }

    IEnumerator CheckToLock()
    {
        //if in range, holds whether it has started to lock in
        bool m_StartedLock = false;

        while(true)
        {
            //if object is not what has been found, stop coroutines and get current
            if (m_goObjFound != gameObject.GetComponent<GetObject>().ObjFound)
            {
                StopCoroutine(RotateToPosition());
                StopCoroutine(MoveOntoPosition());

                m_goObjFound = gameObject.GetComponent<GetObject>().ObjFound;
            }
            else if(m_goObjFound != null)
            {
                //update current position
                m_v3PastPosition = gameObject.transform.position;

                //if in range to lock, and has not already started, begin locking
                if ((m_goObjFound.transform.position - m_v3PastPosition).magnitude < m_fMinMagnitude
                    && !m_StartedLock)
                {
                    m_StartedLock = true;
                    StartCoroutine(MoveOntoPosition());
                }
                //otherwise it has moved, so stop locking and reset bool
                else
                {
                    StopCoroutine(RotateToPosition());
                    StopCoroutine(MoveOntoPosition());
                    m_StartedLock = false;
                }
            }

            yield return new WaitForSeconds(m_fCheckTimer);
        }
    }

    //moves agent directly onto found position, then when finished calls to rotate
    IEnumerator MoveOntoPosition()
    {
        float _elapsedTime = 0f;

        Vector3 _temp = gameObject.transform.position;

        if(m_goObjFound)
            _temp = m_goObjFound.transform.position;
        _temp.y = gameObject.transform.position.y;

        while (_elapsedTime < m_fCheckTimer)
        {
            gameObject.transform.position = Vector3.Lerp(m_v3PastPosition, _temp, _elapsedTime);
            _elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(RotateToPosition());
    }
    //rotates to face position heading
    IEnumerator RotateToPosition()
    {
        float _elapsedTime = 0f;
        m_v3PastRotation = gameObject.transform.rotation;

        Quaternion _gotoRot = gameObject.transform.rotation;
        if (m_goObjFound)
            _gotoRot = m_goObjFound.transform.rotation;

        while (_elapsedTime < m_fCheckTimer)
        {
            gameObject.transform.rotation = Quaternion.Lerp(m_v3PastRotation, _gotoRot, _elapsedTime);
            _elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
