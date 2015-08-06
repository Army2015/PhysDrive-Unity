using UnityEngine;
using System.Collections;

public class FogMaker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for(int z = 0; z < 300; z=z+75){
		for (int y = 0; y < 50; y= y+20) {
			for (int x = 0; x < 180; x=x+80) {
				Instantiate(Resources.Load("newnewFog"), new Vector3(x, y, z), Quaternion.identity);
			}
		}
	
	}

}
}


