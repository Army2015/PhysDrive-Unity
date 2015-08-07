using UnityEngine;
using System.Collections;

public class JeepAccelerationSound : MonoBehaviour {

	public SceneController scene;

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
		if (Input.GetAxis ("Vertical") > 0 && !accelerationStart && !scene.didMineExplode()) {
			source.PlayOneShot (accelerationSound, .5f);
			accelerationStart = true;
		}
		if (accelerationStart) {
			//delay for 5 sec
			StartCoroutine (WaitOneSecond ()); 
			accelerationStart = false;
		}
		if(scene.didMineExplode()){
			source.Stop ();
			}
	}

	IEnumerator WaitOneSecond() { 
		yield return new WaitForSeconds(10); 
	}
}
