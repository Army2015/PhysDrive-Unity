using UnityEngine;
using System.Collections;

public class JeepAccelerationSound : MonoBehaviour {
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
		print (Input.GetAxis ("Vertical"));
		if (Input.GetAxis ("Vertical") > 0 && !accelerationStart) {
			source.PlayOneShot (accelerationSound, .5f);
			accelerationStart = true;
		}
		if (accelerationStart) {
		//delay for 5 sec
			StartCoroutine(WaitOneSecond()); 
			accelerationStart = false;

		}
	}
	IEnumerator WaitOneSecond() { 
		yield return new WaitForSeconds(5); 
	}
}
