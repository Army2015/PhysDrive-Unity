using UnityEngine;
using System.Collections;

public class UserCamControl : MonoBehaviour {

    private KeyCode m_kcZoomUp = KeyCode.F;
    private KeyCode m_kcZoomDw = KeyCode.R;

    [SerializeField] private float fZoomAmount = 5.0f;

	void Update () 
	{
        if (Input.GetKey(m_kcZoomUp)
            && gameObject.transform.localPosition.y < 80f)
		{
            gameObject.transform.Translate(0f, +fZoomAmount * Time.deltaTime, -(fZoomAmount * 0.6f) * Time.deltaTime, Space.World);
		}
        else if (Input.GetKey(m_kcZoomDw)
            && gameObject.transform.localPosition.y > 15f)
		{
            gameObject.transform.Translate(0f, -fZoomAmount * Time.deltaTime, +(fZoomAmount * 0.6f) * Time.deltaTime, Space.World);
		}
	}
}
