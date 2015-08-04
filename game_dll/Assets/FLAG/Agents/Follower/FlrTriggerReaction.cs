using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Detects other objects, and pushes back, however does not push away from other Followers if they
/// are of the same group as the detecting Follower
/// </summary>
public class FlrTriggerReaction : TriggerReaction 
{
    public override void Pushback(GameObject _other)
    {
        if (_other.gameObject.GetComponent<FlrMain>())
            if (_other.gameObject.GetComponent<FlrMain>().AgntGroupNum == gameObject.GetComponent<FlrMain>().AgntGroupNum)
                return;

        Vector3 _dir = -(gameObject.transform.position - _other.transform.position);
        _dir.y = 0f;
        _dir = gameObject.transform.position - (m_fReactPushBack * _dir.normalized);

        gameObject.transform.position = _dir;

        gameObject.transform.Rotate(0f, m_fReactRotateAmount, 0f);
    }
}
