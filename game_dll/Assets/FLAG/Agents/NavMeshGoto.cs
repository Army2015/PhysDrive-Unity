using UnityEngine;
using System.Collections;
/// <summary>
/// Requires NavMeshGet, uses a path made by that script to then create sub-points from the path corners
/// for the agent it is attachd to, to move towards
/// </summary>
[RequireComponent(typeof(NavMeshGet))]
public class NavMeshGoto : GotoObject
{
    //the path to use
    private NavMeshPath m_PathToTravel;
    //the point on the path, and the index of it
    private Vector3 m_v3PointToMoveTo = new Vector3();
    private int m_PathCorner = 0;
    //how close to get before moving to the next one
    [SerializeField] private float m_CancelPointRange = 0.2f;
    [SerializeField] private float m_ReGetPathTmer = 1f;

    void Start()
    {
        //start the movement coroutine
        StartCoroutine("NavMove");
        StartCoroutine("GetPath");
    }

    IEnumerator GetPath()
    {
        while (true)
        {
            m_PathToTravel = gameObject.GetComponent<NavMeshGet>().PathToUse;
            m_PathCorner = 1;

            yield return new WaitForSeconds(m_ReGetPathTmer);
        }
    }

    IEnumerator NavMove()
    { 
        while(true)
        {
            //if the point to move to is null, replace it, and also reset the point index
            if (m_goObjFound == null)
            {
                m_goObjFound = gameObject.GetComponent<GetObject>().ObjFound;
                m_PathCorner = 0;
            }
            //if this object can move, and is allowed to move
            else if (bCanMove() && m_bAutomaticMovement)
            {
                //if we are in range of the vector to move to, set the var. to 0
                if (fMagnitude(m_v3PointToMoveTo, m_trTransformToMove.position) < m_CancelPointRange)
                {
                    m_v3PointToMoveTo = Vector3.zero;
                }

                //if there is a point to move to, move to it
                if (m_v3PointToMoveTo != Vector3.zero)
                {
                    vRotateToPoint(m_v3PointToMoveTo);

                    if (fMagnitude(m_v3PointToMoveTo, m_trTransformToMove.position) > m_fSlowDistance)
                        vMove(m_fMoveSpeed * 1.5f, Vector3.forward);
                    else
                        vMove(m_fMoveSpeed, Vector3.forward);
                }
                else
                {
                    //if a corner left in the path to travel
                    if (m_PathToTravel != null && m_PathCorner < m_PathToTravel.corners.Length)
                    {
                        //set that corner as the vector, set it to be the same height as agent
                        m_v3PointToMoveTo = m_PathToTravel.corners[m_PathCorner];
                        m_v3PointToMoveTo.y = gameObject.transform.position.y;

                        //increment the one to move to
                        m_PathCorner++;
                    }
                }
                
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
