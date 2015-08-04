using UnityEngine;
using System.Collections;

/// <summary>
/// This script handles moving Agents around the world,
/// as well as make public movement functions for use by others such as FlrBehaviour.
/// Requires GetObject or a script of the same class to find which object to move towards
/// </summary>
[RequireComponent(typeof(GetObject))]
public class GotoObject : MonoBehaviour {

    //bool controlling whether this script moves the object, where it will rotate and move forwards
    //Allows other scripts such as FlrBehaviour to utilise this script on/off
    //Example in this demo being to switch this on when in a certain range of the target
    protected bool m_bAutomaticMovement = true;
    public bool AutoMove { get { return m_bAutomaticMovement; } set { m_bAutomaticMovement = value; } }

    protected GameObject m_goObjFound;
    public GameObject ObjectFound { get { return m_goObjFound; }  }
    public float fDistToTarget
    {
        get
        {
            return (m_goObjFound.transform.position - m_trTransformToMove.position).magnitude;
        }
    }

    //The transform to move in the world
    //for this demo this is just the gameObject's transform
    protected Transform m_trTransformToMove;
    public Transform MoveTransform { get { return m_trTransformToMove; } }

    //At what Magnitudes does this script move at a slower speed/stop altogether
    protected float m_fSlowDistance = 4f;
    protected float m_fStopDistance = 1f;

    //variables
    protected float m_fMoveSpeed = 3.0f;
    public float MoveSpeed { get { return m_fMoveSpeed; } }
    protected float m_fTurnSpeed = 10.0f;

    //Function that allows FlrMain/Ldr2Main to assign variables in one line
    public void SetSettings(float _movesp, float _turnsp, float _stopdis, float _slowdis)
    {
        m_trTransformToMove = gameObject.transform;

        if (_movesp < 0f || _turnsp < 0f)
            Debug.LogWarning("FLAG: A GetObject was given an invalid value: "
                + _movesp + " Move Speed, " + _turnsp + " Turn Speed, " + gameObject);
        else
        {
            m_fSlowDistance = _slowdis;
            m_fStopDistance = _stopdis;
        }

        if (_stopdis < 0f || _slowdis < 0f)
            Debug.LogWarning("FLAG: A GetObject was given an invalid value: "
                + _stopdis + " Stop Dist., " + _slowdis + " Slow Dist., " + gameObject);
        else
        {
            m_fMoveSpeed = _movesp;
            m_fTurnSpeed = _turnsp;
        }

    }

    void Start()
    {
        StartCoroutine("Movement");
    }

    //Handles getting an object to move to, and then moving towards it
    IEnumerator Movement()
    {
        while (true)
        {

            if (m_goObjFound != gameObject.GetComponent<GetObject>().ObjFound)
            {
                m_goObjFound = gameObject.GetComponent<GetObject>().ObjFound;
            }
            else if (m_goObjFound != null)
            {
                if(bCanMove() && m_bAutomaticMovement)
                {
                    vRotateToPoint(m_goObjFound.transform.position);

                    if (fMagnitude(m_goObjFound.transform.position, m_trTransformToMove.position) > m_fSlowDistance)
                        vMove(m_fMoveSpeed * 1.5f, Vector3.forward);
                    else
                        vMove(m_fMoveSpeed, Vector3.forward);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    //Simplifies checks for movement into one function
    public bool bCanMove()
    {
        if (m_goObjFound != null &&
            fMagnitude(m_goObjFound.transform.position, m_trTransformToMove.position) > m_fStopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Rotates the transform to face a point given at the normal turn speed
    public void vRotateToPoint(Vector3 _point)
    {
        Vector3 _direction = (_point - m_trTransformToMove.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        m_trTransformToMove.rotation = Quaternion.Slerp(m_trTransformToMove.rotation, _lookRotation, Time.deltaTime * m_fTurnSpeed);
    }
    //Rotates the transform to face a point given directly
    public void vRotateAtPoint(Vector3 _point)
    {
        Vector3 _direction = _point;
        _direction.y = gameObject.transform.position.y;
        transform.LookAt(_direction);
    }

    protected float fMagnitude(Vector3 _1st, Vector3 _2nd)
    {
        Vector3 _temp = _1st - _2nd;
        return _temp.magnitude;
    }

    //Move and rotate towards the target object in a function
    public void vSimpleMoveToObjToGoto()
    {
        if (m_goObjFound)
        {
            vRotateToPoint(m_goObjFound.transform.position);
            vMove(m_fMoveSpeed, Vector3.forward);
        }
    }
    //Takes transform and moves forward by given amount/normal move speed in a given direction
    public void vMove(float _amount, Vector3 _direction)
    {
        m_trTransformToMove.Translate(_direction * _amount * Time.deltaTime);
    }
    public void vMove(Vector3 _direction)
    {
        m_trTransformToMove.Translate(_direction * m_fMoveSpeed * Time.deltaTime);
    }

    //Given values, will translate the object by the amount * multiplier, in the given forward direction
    public void vStrafe(float _Multiply, float _StraftAmount, Vector3 _forwdirection)
    {
        Vector3 _temp = _forwdirection;
        _temp.x += _StraftAmount;

        m_trTransformToMove.Translate(_temp * _Multiply * Time.deltaTime);
    }
}
