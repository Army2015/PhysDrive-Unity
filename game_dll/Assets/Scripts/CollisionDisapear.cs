using UnityEngine;
using System.Collections;

public class CollisionDisapear : MonoBehaviour {
	public GameObject target;
	public AudioClip collectSound;
	private AudioSource source;
	private bool enterOnce = false;
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
		if (distance <= 10 && !enterOnce) {
			enterOnce = true;
			source.PlayOneShot(collectSound,1f);

			Destroy(gameObject,.3f);
		

		}
	}

}
	
	
	
