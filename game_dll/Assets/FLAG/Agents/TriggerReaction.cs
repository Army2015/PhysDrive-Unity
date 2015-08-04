using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides a basic script to react when the trigger has been entered by one of multiple tags
/// </summary>
public class TriggerReaction : MonoBehaviour 
{
    //when the trigger is entered, it will check if tag == any entry in here,
    private List<string> m_sTagsToCheck = new List<string>();
    //Pushback = this value * unit vector from self to trigger object
    protected float m_fReactPushBack = 1.5f;
    //how much to rotate by when trigger entered
    protected float m_fReactRotateAmount = 45f;

    public void SetSettings(List<string> _Tags, float _PushBack, float _ReactRot)
    {
        if (_Tags.Count > 0)
        {
            m_sTagsToCheck.Clear();
            m_sTagsToCheck = _Tags;
        }
        else
            Debug.LogWarning("FLAG: A TriggerReaction Tag List given has no values: " + gameObject);

        if (_PushBack < 0f || _ReactRot < 0f)
            Debug.LogWarning("FLAG: A TriggerReaction was given has invalid values: "
                + _PushBack + " Pushback " + _ReactRot + " React Rotation, " + gameObject);
        else
        {
            m_fReactPushBack = _PushBack;
            m_fReactRotateAmount = _ReactRot;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_sTagsToCheck.Count > 0)
        {
            foreach (string entry in m_sTagsToCheck)
            {
                if (other.gameObject.tag == entry)
                    Pushback(other.gameObject);
            }
        }
        else
            Debug.LogWarning("FLAG: TriggerReaction on: " + gameObject + " has no strings to check tag against.");
    }

    public virtual void Pushback(GameObject _other){ }
}
