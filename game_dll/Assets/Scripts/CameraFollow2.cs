using UnityEngine;
using System.Collections;

public class CameraFollow2 : MonoBehaviour {

	public Transform target;            // The position that that camera will be following.
	public float smoothing = 15f;        // The speed with which the camera will be following.
	
	Vector3 offset;                     // The initial offset from the target.
	Vector3 center;
	
	void Start ()
	{
		transform.position = new Vector3 (-3, 7, 0);
		transform.Rotate (new Vector3 (5, 0, 0));
		center = new Vector3 (125, -40, 125);		
	}
	
	void Update ()
	{
		// Create a postion the camera is aiming for based on the offset from the target.
		offset = target.position - center;
		Vector3 targetCamPos = center + 1.3f * offset;
		targetCamPos += new Vector3 (0, 10, 0);
		transform.LookAt (target);
		transform.Rotate (new Vector3 (-5, 0, 0));
		
		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
