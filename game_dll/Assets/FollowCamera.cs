using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	// Use this for initialization
	public GameObject m_camera;

	// Update is called once per frame
	void Update () {
		transform.position = m_camera.transform.position + new Vector3 (0, 5, 0);
	}
}
