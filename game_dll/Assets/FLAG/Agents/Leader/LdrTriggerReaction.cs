using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Just pushes back away from any object that triggers it.
/// </summary>
public class LdrTriggerReaction : TriggerReaction
{
    public override void Pushback(GameObject _other)
    {
        Vector3 _dir = -(gameObject.transform.position - _other.transform.position);
        _dir.y = 0f;
        _dir = gameObject.transform.position - (m_fReactPushBack * _dir.normalized);

        gameObject.transform.position = _dir;

        gameObject.transform.Rotate(0f, m_fReactRotateAmount, 0f);
    }
}
