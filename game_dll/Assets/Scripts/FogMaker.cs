using UnityEngine;
using System.Collections;

public class FogMaker : MonoBehaviour {
	private int fogCreations = 60; 

	void Awake(){
		for(int i = 0 ; i < fogCreations; i++){
			float x = Random.Range (-200, 200);
			float y = Random.Range (0, 4);
			float z = Random.Range (-200, 200);
			Instantiate (Resources.Load ("BigFog"), new Vector3 (x, y, z), Quaternion.identity);
		}
		float xOfset = 0;
		float zOfset = 0;
		for (int j = 0; j < 5; j++) {
			for (int k = 0; k < 5; k++) {
				Instantiate (Resources.Load ("BigFog"), new Vector3 (transform.position.x + xOfset
				                                                     , 0, 
				                                                     transform.position.z +zOfset), 
				             Quaternion.identity);
				zOfset = zOfset + 100;
			}
			xOfset = xOfset -100;
			zOfset = 0;
		}
		                                      
	
	}
	// Use this for initialization
	void Start () {


	}
	void Update(){
	
	}
}


