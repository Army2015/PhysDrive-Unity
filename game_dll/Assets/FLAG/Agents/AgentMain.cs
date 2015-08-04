using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The main core for each Agent, holding base variables used by other base scripts.
/// </summary>
[RequireComponent(typeof(GetObject))]
[RequireComponent(typeof(GotoObject))]
[RequireComponent(typeof(TriggerReaction))]
public class AgentMain : MonoBehaviour 
{

    //which group number this agent acts on
    //for leaders this represents which group it moves
    //for followers this represents the group it is a part of
    [SerializeField] protected int m_iAgentGroupNum = -1;
    public int AgntGroupNum { get { return m_iAgentGroupNum; } set { m_iAgentGroupNum = value; } }

    //used by GotoObject
    [SerializeField] protected float m_fSlowDist = 1f;
    [SerializeField] protected float m_fStopDist = 1f;
    [SerializeField] protected float m_fMoveSpeed = 3.0f;
    [SerializeField] protected float m_fTurnSpeed = 10.0f;

    //used by getObject
    [SerializeField] private string m_sTagToMoveTo = "PatPos";
    [SerializeField] protected float m_fGetObjTimer = 1f;
    [SerializeField] protected float m_fGetMinDist = 1.5f;
    [SerializeField] protected float m_fGetMaxDist = 100f;

    //used by trigger reaction
    [SerializeField] private List<string> m_sTriggerStrings = new List<string>();
    [SerializeField] protected float m_fTriggerPushBack = 1.5f;
    [SerializeField] protected float m_fTriggerRotate = 45f;

    void Start()
    {
        gameObject.GetComponent<GetObject>().SetSettings(m_fGetObjTimer, m_sTagToMoveTo, m_fGetMinDist, m_fGetMaxDist);
        gameObject.GetComponent<GotoObject>().SetSettings(m_fMoveSpeed, m_fTurnSpeed, m_fStopDist, m_fSlowDist);
        gameObject.GetComponent<TriggerReaction>().SetSettings(m_sTriggerStrings, m_fTriggerPushBack, m_fTriggerRotate);

        FurtherSettings();
    }

    public virtual void FurtherSettings() { }
}
