using UnityEngine;
using System.Collections;

/// <summary>
/// Core for follower agents
/// </summary>
[RequireComponent(typeof(FlrBehaviour))]
[RequireComponent(typeof(FlrLockIntoForm))]
public class FlrMain : AgentMain
{
    //behaviour type, in the default system this represents what movement type to use
    public enum FlrBehaviourType
    {
        Normal      = 1,
        Wavey       = 2,
        RandSpeed   = 3,
        Offset      = 4
    }
    //which behaviour type this follower is
    [SerializeField] private FlrBehaviourType m_eBehaviour = FlrBehaviourType.Normal;
    public FlrBehaviourType FlrBehType { get { return m_eBehaviour; } }

    //for testing or other scripts, this enables/disables behaviours on start/runtime
    [SerializeField] private bool m_bEnableBehaviours = true;
    public bool BehvrEnabled { get { return m_bEnableBehaviours; } set { m_bEnableBehaviours = value; } }
    
    //when to run behaviour moving, and when to ignore it
    [SerializeField] private float m_fBehMoveInterval = 1f;
    [SerializeField] private float m_fIgnoreBehRange = 4f;

    //when to perform
    [SerializeField] private float m_fRotateInterval = 1f;

    public override void FurtherSettings()
    {
        if (m_bEnableBehaviours)
            m_eBehaviour = (FlrBehaviourType)Random.Range(1,5);

        gameObject.GetComponent<FlrBehaviour>().SetSettings(m_fBehMoveInterval, m_fIgnoreBehRange);
        gameObject.GetComponent<FlrLockIntoForm>().SetSettings(m_fStopDist + 0.1f, m_fRotateInterval);
        gameObject.GetComponent<GetObject>().SetType = GetObject.AgentType.Follower;
    }
    public void vSetGroup(int _num)
    {
        m_iAgentGroupNum = _num;
    }
}
