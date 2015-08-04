using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Core of Leader, holds additional variables for formation related data
/// </summary>
[RequireComponent(typeof(LdrCreate))]
[RequireComponent(typeof(LdrFormationMovement))]
[RequireComponent(typeof(LdrDelObjWthnMagntd))]
public class LdrMain : AgentMain
{
    //Placeholder for behaviour types if you choose to use them
    public enum LdrBehaviourType
    {
        Normal,
        Virtual
    }
    //which type the Leader is
    [SerializeField] private LdrBehaviourType m_eBehaviour = LdrBehaviourType.Normal;
    public LdrBehaviourType eLdrBehType { get { return m_eBehaviour; } }

    //which shape type of formation to generate
    //has a property to return as enum, and as integer to support demo Leader Edit Popup
    [SerializeField] LdrCreate.SpawnMethod m_eGenerationType = LdrCreate.SpawnMethod.Block;
    public LdrCreate.SpawnMethod eLdrGenType 
    { 
        get 
        { 
            return m_eGenerationType; 
        } 
        set 
        { 
            m_eGenerationType = value; 
        } 
    }
    public int iLdrGenType 
    {
        get
        {
            return (int)m_eGenerationType;
        }
        set
        {
            if(Enum.IsDefined(typeof(LdrCreate.SpawnMethod), value))
                m_eGenerationType = (LdrCreate.SpawnMethod)value;
            else
                Debug.LogError("FLAG: iLdrGenType was given an invalid SpawnMethod int: " + value);
        }
    }

    //Size of the formation
    //Represented as Vector2's to reduce memory and code amount, where it is gotten/returned as an int cast
    [SerializeField] private Vector2 m_v2GenSizePos = new Vector2(0f, 0f);
    [SerializeField] private Vector2 m_v2GenSizeNeg = new Vector2(2f, 2f);
    [SerializeField] private Vector2 m_v2GenSizeSpacing = new Vector2(5f, 5f);

    [SerializeField] private GameObject m_goFormationPrefab;
    public GameObject FormationPrefab { get { return m_goFormationPrefab; } }

    //return values of the above 3 variables
    public int GenXP { get { return (int)m_v2GenSizePos.x; } set { m_v2GenSizePos.x = value; } }
    public int GenYP { get { return (int)m_v2GenSizePos.y; } set { m_v2GenSizePos.y = value; } }
    public int GenXN { get { return (int)m_v2GenSizeNeg.x; } set { m_v2GenSizeNeg.x = value; } }
    public int GenYN { get { return (int)m_v2GenSizeNeg.y; } set { m_v2GenSizeNeg.y = value; } }
    public float GenXSpcng { get { return m_v2GenSizeSpacing.x; } set { m_v2GenSizeSpacing.x = value; } }
    public float GenYSpcng { get { return m_v2GenSizeSpacing.y; } set { m_v2GenSizeSpacing.y = value; } }

    //used by Ldr2FormationMovement movement
    [SerializeField] private float m_fObjBackToOriginTime = 5f;
    [SerializeField] private float m_fResetPostFormTime = 3f;

    //used by LdrDelObjWthnMagntd
    [SerializeField] private float m_fObjDelTimer = 1f;

    public override void FurtherSettings()
    {
        gameObject.GetComponent<LdrCreate>().SetPrefab = m_goFormationPrefab;
        gameObject.GetComponent<LdrCreate>().vGenerateFormation(
            eLdrGenType,
            GenXP, GenXN,
            GenYP, GenYN,
            GenXSpcng, GenYSpcng);

        gameObject.GetComponent<LdrFormationMovement>().SetSettings(GenYP, GenXP, GenYN, GenXN, m_fObjBackToOriginTime, m_fResetPostFormTime);
        gameObject.GetComponent<LdrDelObjWthnMagntd>().SetSettings(m_fStopDist + 0.1f, m_fObjDelTimer);

        if (m_eBehaviour == LdrBehaviourType.Normal)
            gameObject.GetComponent<GetObject>().SetType = GetObject.AgentType.Leader;
        else if (m_eBehaviour == LdrBehaviourType.Virtual)
            gameObject.GetComponent<GetObject>().SetType = GetObject.AgentType.VirtualLeader;
    }
    public void vRegenerate()
    {
        gameObject.GetComponent<LdrCreate>().vClearFormation();
        gameObject.GetComponent<LdrCreate>().vGenerateFormation(
            eLdrGenType,
            GenXP, GenXN,
            GenYP, GenYN,
            GenXSpcng, GenYSpcng);

        gameObject.GetComponent<LdrFormationMovement>().SetSettings(GenYP, GenXP, GenYN, GenXN, m_fObjBackToOriginTime, m_fResetPostFormTime);
    }
}
