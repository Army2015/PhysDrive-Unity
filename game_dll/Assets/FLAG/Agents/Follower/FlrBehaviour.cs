using UnityEngine;
using System.Collections;

/// <summary>
/// Controls when to run behaviours, how to do so, and animating depending on state
/// </summary>
[RequireComponent(typeof(GotoObject))]
[RequireComponent(typeof(FlrMain))]
public class FlrBehaviour : MonoBehaviour 
{
    //get component from object to get quick access to it
    private GotoObject m_GotoComponent;

    //behaviour type to use
    private FlrMain.FlrBehaviourType m_eBehType;
    //when to do a new movement coroutine
    private float m_fMovementInterval = 1f;
    //when to stop moving and use basic movement
    private float m_fIgnoreBehaviourRange = 4f;
    private Vector3 m_v3ObjToMoveToCurrPos;

    //Materials to use for animation, to show the 2 states currently supported
    [SerializeField] private Material m_MatGo;
    [SerializeField] private Material m_MatIdle;

    public void SetSettings(float _MoveInterval, float _IgnBehRange)
    {
        m_fMovementInterval = _MoveInterval;
        m_fIgnoreBehaviourRange = _IgnBehRange;
    }

    void Start()
    {
        //start by getting needed component references
        m_GotoComponent = gameObject.GetComponent<GotoObject>();
        m_eBehType = gameObject.GetComponent<FlrMain>().FlrBehType;

        //if not set to normal behaviour
        if (m_eBehType != FlrMain.FlrBehaviourType.Normal)
        {
            //switch off automatic movement
            m_GotoComponent.AutoMove = false;
            //start the overall coroutine with the sub-coroutine to be called
            switch (m_eBehType)
            {
                case FlrMain.FlrBehaviourType.Offset:
                    StartCoroutine("Movement", "OffsetMove");
                    break;

                case FlrMain.FlrBehaviourType.RandSpeed:
                    StartCoroutine("Movement", "RandMove");
                    break;

                case FlrMain.FlrBehaviourType.Wavey:
                    StartCoroutine("Movement", "SineMove");
                    break;
            }
        }
    }

    //set the material demonstrating which state the follower is in
    private void vSetMaterial(Material _mat)
    {
        if (gameObject.GetComponent<Renderer>() 
            && gameObject.GetComponent<Renderer>().material != _mat)
            gameObject.GetComponent<Renderer>().material = _mat;
    }
    
    //overall coroutine - this dictates when behaviour movement can occur,
    //and also handles material setting
    IEnumerator Movement(string _subCoroutineName)
    {
        while (true)
        {
            if (m_GotoComponent.bCanMove() && m_GotoComponent.ObjectFound
                && gameObject.GetComponent<FlrMain>().BehvrEnabled)
            {
                //get new position of the object to move to, set the material
                m_v3ObjToMoveToCurrPos = m_GotoComponent.ObjectFound.transform.position;
                vSetMaterial(m_MatGo);

                //if the object is within set range
                //ignore behaviour and move towards it
                if (m_GotoComponent.fDistToTarget < m_fIgnoreBehaviourRange)
                    StartCoroutine("BaseMove", m_fMovementInterval - 0.05f);
                else
                    StartCoroutine(_subCoroutineName, m_fMovementInterval - 0.05f);
            }
            //else no object or cannot move, so goto idle material
            else
            {
                vSetMaterial(m_MatIdle);
            }

            yield return new WaitForSeconds(m_fMovementInterval);
        }
    }
    //runs basic movement function in goto component
    IEnumerator BaseMove(float _length)
    {
        float _temp = _length;
        while (_temp > 0f)
        {
            _temp -= Time.deltaTime;

            m_GotoComponent.vSimpleMoveToObjToGoto();

            yield return new WaitForEndOfFrame();
        }
    }
    //rotates to an object, but changes speed every time it is called
    IEnumerator RandMove(float _length)
    {
        float _modifiedSpeed = m_GotoComponent.MoveSpeed + Random.Range(-m_GotoComponent.MoveSpeed, m_GotoComponent.MoveSpeed);
        float _temp = _length;
        while (_temp > 0f)
        {
            _temp -= Time.deltaTime;

            m_GotoComponent.vRotateToPoint(m_v3ObjToMoveToCurrPos);
            m_GotoComponent.vMove(_modifiedSpeed, Vector3.forward);

            yield return new WaitForEndOfFrame();
        }
    }
    //rotates away from the point to move to, and moves in that direction
    IEnumerator OffsetMove(float _length)
    {
        m_GotoComponent.vRotateAtPoint(m_v3ObjToMoveToCurrPos);
        m_GotoComponent.MoveTransform.Rotate(0f, Random.Range(-10f, 10f), 0f);

        float _temp = _length;
        while(_temp > 0f)
        {
            _temp -= Time.deltaTime;

            m_GotoComponent.vMove(Vector3.forward);

            yield return new WaitForEndOfFrame();
        }
    }
    //as it moves it moves left/right off the faced direction
    IEnumerator SineMove(float _length)
    {
        float _sinePower = 0.2f;
        float _sineSpeed = 10f;

        float _temp = _length;
        while (_temp > 0f)
        {
            _temp -= Time.deltaTime;

            m_GotoComponent.vRotateToPoint(m_v3ObjToMoveToCurrPos);
            m_GotoComponent.vMove(Vector3.forward);
            m_GotoComponent.vStrafe(
                m_GotoComponent.MoveSpeed, 
                (Mathf.Sin(Time.time * _sineSpeed) * _sinePower),
                Vector3.forward);

            yield return new WaitForEndOfFrame();
        }
    }
}
