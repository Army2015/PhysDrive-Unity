using UnityEngine;
using System.Collections;

public class JeepAccelerationSouncScene1 : MonoBehaviour {

	public AudioClip accelerationSound;
	private AudioSource source;
	private bool accelerationStart = false;
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//source.PlayOneShot (accelerationSound, 1f);

		if (Input.GetAxis ("Vertical") > 0) {
		
			source.PlayOneShot (accelerationSound, 1f);
			StartCoroutine (WaitOneSecond ());
			//source.PlayScheduled(1);
			//source.Stop();
		}

		
	}
	
	IEnumerator WaitOneSecond() { 
		yield return new WaitForSeconds(10); 

	}
}
