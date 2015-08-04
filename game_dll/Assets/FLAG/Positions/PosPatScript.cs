using UnityEngine;
using System.Collections;

/// <summary>
/// Script for path positions, provides script to be edited if needed
/// </summary>
public class PosPatScript : PositionObj 
{
    public override void OnTrigCheckTrue(Collider _othercoll)
    {
        if (m_sTagsToCheck.Contains(_othercoll.gameObject.tag))
        {
            vDelete();
        }        
    }
}
