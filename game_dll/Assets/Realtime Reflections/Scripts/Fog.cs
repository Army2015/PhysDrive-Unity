
//source: http://stackoverflow.com/questions/14553672/fog-in-unity-with-c-sharp

using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
	int count = 0;
	void Start () 
	{
		RenderSettings.fog =true;
		RenderSettings.fogDensity = 2.00f;
	}
	
	void update() 
	{
		count++;
		float num = Random.value;
		Debug.Log(num);
		if (count % 2 != 0)
		{ 
			RenderSettings.fogDensity+=num;

		}
		else
		{ 
			RenderSettings.fogDensity-=num;
		}

	}

}