using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Reacts to triggers by requesting to be moved in a formation by parent object.
/// Requires parent to have a Ldr2FormationMovement component 
/// </summary>
public class PosForScript : PositionObj
{
    //the original x/y position in the formation as a vector
    private Vector2 m_v2OriginalForPos = new Vector2();
    //the new position when moved by a Leader's Ldr2FormationMovement
    private Vector2 m_v2PostForPos = new Vector2();

    public Vector2 v2OrigPosition { get { return m_v2OriginalForPos; } }
    public Vector2 v2PostPosition { get { return m_v2PostForPos; } set { m_v2PostForPos = value; } }

    //whether this position has been moved already, so as to support obstructed position areas
    private bool m_AskMoveOnce = false;
    public bool bAskedToMove { get { return m_AskMoveOnce; } set { m_AskMoveOnce = value; } }

    public void vSetValues(int x, int y, int _groupNum)
    {
        m_GroupNum = _groupNum;
        m_v2OriginalForPos.x = x;
        m_v2OriginalForPos.y = y;
    }
    
    public override void OnTrigCheckTrue(Collider _othercoll)
    {
        if (!gameObject.GetComponentInParent<LdrFormationMovement>())
        {
            Debug.LogWarning("FLAG: Object with PosForScript cannot find FormationMovement component on parent: " + gameObject.name);
        }
        else if (m_sTagsToCheck.Contains(_othercoll.gameObject.tag))
        {
            if (!m_AskMoveOnce)
            {
                m_AskMoveOnce = true;
                gameObject.GetComponentInParent<LdrFormationMovement>().EnterNewObjToMove(gameObject, false);
            }
            else
            {
                gameObject.GetComponentInParent<LdrFormationMovement>().EnterNewObjToMove(gameObject, true);
            }
        }
    }
}
