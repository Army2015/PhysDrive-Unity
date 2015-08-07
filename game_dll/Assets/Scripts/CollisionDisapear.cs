using UnityEngine;
using System.Collections;

public class CollisionDisapear : MonoBehaviour {
	public GameObject target;

	private bool flagDestroyed = false;

	public AudioClip collectSound;
	private AudioSource source;

	
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		
	}
	
	// Update is called once per frame
	void Update () {
	float distance = Vector3.Distance (target.transform.position, transform.position);
	print (distance);
		if (distance <= 10 && !flagDestroyed) {
			flagDestroyed = true;

			Destroy(gameObject,.14f);
		
		}

		if (getFlagDestroyed ()) {
			source.PlayOneShot(collectSound,1f);
		}



	}
	public bool getFlagDestroyed(){
		return flagDestroyed;
	}

}
	
	
	
