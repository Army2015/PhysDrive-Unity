using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides base variables needed for positions to be used by Agents in this system
/// </summary>
public class PositionObj : MonoBehaviour
{
    //which type of position this object is
    public enum PosType
    {
        Unset,
        Path,
        Formation
    }

    //which type of position this is
    [SerializeField] protected PosType m_PositionType = PosType.Unset;
    public PosType Type { get { return m_PositionType; } }

    //controls whether this position is locked, so as to stop repeat gets of the same position
    protected bool m_bLocked = false;
    public bool IsLocked { get { return m_bLocked; } }

    //which group number this object is
    //allows for multiple groups, eg separate pathing orders
    [SerializeField] protected int m_GroupNum = -1;
    public int GroupNum { get { return m_GroupNum; } set { m_GroupNum = value; } }

    //material to show whether the position is locked or not
    //in gameplay, these positions for most cases will be invisible
    [SerializeField] protected Material m_MatOpn;
    [SerializeField] protected Material m_MatLck;

    //the list of tags to check for trigger reactions
    [SerializeField] protected List<string> m_sTagsToCheck = new List<string>();
    public List<string> CheckTags { get { return m_sTagsToCheck; } }

    void Start()
    {
        if (m_PositionType == PosType.Unset)
            Debug.LogWarning("FLAG: A PositionObj has an unset type: " + m_PositionType + ", " + gameObject.name);
    }

    public void vDelete()
    {
        DestroyObject(this.gameObject);
    }
    /// <summary>
    /// Sets whether the position the script is on is locked or not, and sets material,
    /// if necessary
    /// </summary>
    public void vSetLock(bool _value)
    {
        m_bLocked = _value;
        if (_value == true)
            gameObject.GetComponent<Renderer>().material = m_MatLck;
        else
            gameObject.GetComponent<Renderer>().material = m_MatOpn;
    }

    void OnTriggerEnter(Collider other)
    {
        OnTrigCheckTrue(other);
    }
    void OnTriggerStay(Collider other)
    {
        OnTrigCheckTrue(other);
    }

    public virtual void OnTrigCheckTrue(Collider _other) { }
}
