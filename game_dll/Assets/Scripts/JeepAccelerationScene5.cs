using UnityEngine;
using System.Collections;

public class JeepAccelerationScene5 : MonoBehaviour {
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
		if (Input.GetAxis ("Vertical") > 0 && !accelerationStart ) {
			print ("In the looop");
			source.PlayOneShot (accelerationSound, 1f);
			accelerationStart = true;
		}
		if (accelerationStart) {
			//delay for 5 sec
			StartCoroutine (WaitOneSecond ()); 
			accelerationStart = false;
			source.Stop ();

		}

	}
	
	IEnumerator WaitOneSecond() { 
		yield return new WaitForSeconds(1); 
	}
}
