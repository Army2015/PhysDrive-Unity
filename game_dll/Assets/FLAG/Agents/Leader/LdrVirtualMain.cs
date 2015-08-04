using UnityEngine;
using System.Collections;

/// <summary>
/// Unlike Ldr2Main, this is used for the visible component of a virtual Leader,
/// In that Ldr2Main is the invisible one, which creates the formation, and this
/// acts for the visible object with basic follower behaviour
/// </summary>
[RequireComponent(typeof(LdrDelObjWthnMagntd))]
public class LdrVirtualMain : AgentMain
{
    //used by LdrDelObjWthnMagntd
    [SerializeField] private float m_fObjDelTimer = 1f;

    //same idea as a positionobj, this is set by a VirtualLeader-behaviour Ldr2Main Agent
    private bool m_bHasLeaderFollowing = false;
    public bool HasVirtualLeader { get { return m_bHasLeaderFollowing; } set { m_bHasLeaderFollowing = value; } }

    public override void FurtherSettings()
    {
        gameObject.GetComponent<LdrDelObjWthnMagntd>().SetSettings(m_fStopDist + 0.1f, m_fObjDelTimer);
        gameObject.GetComponent<GetObject>().SetType = GetObject.AgentType.Leader;
    }
}
