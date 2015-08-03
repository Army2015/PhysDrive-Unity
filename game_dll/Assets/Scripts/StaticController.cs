using UnityEngine;
using System.Collections;

public class StaticController : MonoBehaviour {

	// Use this for initialization
	public GameObject explosion_prefab;
	GameObject explosion;
	void Start () {
		explosion = (UnityEngine.GameObject)Instantiate (explosion_prefab);
		// StopExplosion ();
	}
	
	// Update is called once per frame
	int counter = 0;
	void Update () {
		if (counter == 0) {
			//StopExplosion();
		}
		if (counter < 10) {
			counter ++;
		}
		if (counter == 10) {
			PlayExplosion(new Vector3(1,1,1));
			counter++;
		}
	
	}

	void PlayExplosion(Vector3 pos){
		explosion.transform.position = pos;
		foreach (Transform child in explosion.transform) {
			if (child.transform.name != "Point light"){
				ParticleSystem p = child.GetComponent<ParticleSystem>();
				p.Play();
			}
		}
	}

	void StopExplosion(){
		foreach (Transform child in explosion.transform) {
			if (child.transform.name != "Point light"){
				ParticleSystem p = child.GetComponent<ParticleSystem>();
				p.Stop();
			}
		}
	}
}
