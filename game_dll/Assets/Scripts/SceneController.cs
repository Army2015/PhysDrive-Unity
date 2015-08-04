using UnityEngine;
using SimpleJSON;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


public class SceneController : MonoBehaviour {

	// Use this for initialization
	public GameObject jeep;
	public GameObject fltire, frtire, bltire, brtire;
	[DllImport("game_dll")]
	static extern IntPtr API_Update_Frame ();
	[DllImport("game_dll")]
	static extern void API_Init (int lev);
	[DllImport("game_dll")]
	static extern void API_Free_Game ();
	[DllImport("game_dll")]
	static extern void API_Input (int code);
	[DllImport("game_dll")]
	static extern IntPtr API_Get_Mines ();
	[DllImport("game_dll")]
	static extern IntPtr API_Get_Terrain ();

	private Hashtable name_obj;

	string[] obj_list = {"body", "fltire", "frtire", "bltire", "brtire"};
	void Start () {
		explosion = (UnityEngine.GameObject)Instantiate (explosion_prefab);
		StopExplosion ();

		API_Init (0);
		name_obj = new Hashtable ();
		name_obj.Add ("body", jeep);
		name_obj.Add ("fltire", fltire);
		name_obj.Add ("frtire", frtire);
		name_obj.Add ("bltire", bltire);
		name_obj.Add ("brtire", brtire);
		//terrainData = (terrain).terrainData;

		SetMines ();
		OutputTerrain ();
	}

	public GameObject mine;
	public GameObject[] rocks;
	void SetMines(){
		string s = Marshal.PtrToStringAnsi (API_Get_Mines ());
		var j = JSONNode.Parse(s);
		var mines = j ["mines"];
		int mine_num = 200;
		int n = rocks.Length;
		for (int i = 0; i < mine_num; i++) {
			var pos = mines[i];
			float x = pos[0].AsFloat, y = pos[1].AsFloat, z = pos[2].AsFloat;
			/*if (x*x + z*z > 50 * 50) y -= 1;
			if (x*x + z*z > 100 * 100) y -= 2;*/
			Quaternion q = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
			GameObject temp = (UnityEngine.GameObject)Instantiate(rocks[i%n], new Vector3(x,y,z), q);

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

	/*void setPos(GameObject obj, var jv)
	{
		float x = jv ["pos"] [0].AsFloat, y = jv ["pos"] [1].AsFloat, z = jv ["pos"] [2].AsFloat;
		jeep.transform.position = new Vector3 (x, y, z);
		float rx = jv ["ori"] [0].AsFloat, ry = jv ["ori"] [1].AsFloat, rz = jv ["ori"] [2].AsFloat;
		jeep.transform.rotation = Quaternion.Euler (rx * 180 / 3.14F, ry * 180 / 3.14F , rz * 180 / 3.14F);
	}*/
	// Update is called once per frame

	void OutputTerrain(){
		terrain_data = terrain.terrainData;

		string s = Marshal.PtrToStringAnsi (API_Get_Terrain ());
		var j = (JSONNode.Parse(s));

		var pos = j["origin"];
		float x = pos[0].AsFloat, y = pos[1].AsFloat, z = pos[2].AsFloat;
		UnityEngine.Debug.Log ("cpp origin:"+x+", "+y+", "+z);
		y = terrain_data.GetInterpolatedHeight (x, z);
		UnityEngine.Debug.Log ("unity origin:"+x+", "+y+", "+z);

		pos = j ["correction"];
		x = pos [0].AsFloat; y = pos [1].AsFloat; z = pos[2].AsFloat;
		UnityEngine.Debug.Log ("cpp correction:"+x+", "+y+", "+z);
		y = terrain_data.GetInterpolatedHeight (x, z);
		UnityEngine.Debug.Log ("unity correction:"+x+", "+y+", "+z);
	}

	TerrainData terrain_data;
	public GameObject explosion_prefab;
	GameObject explosion;
	int counter = 0;
	void Update () {
		try{
			/*if (counter <= 10) 
			{
				if (counter == 10)
				{OutputTerrain();}
				counter ++;
			}*/
				
			string s = Marshal.PtrToStringAnsi (API_Update_Frame ());
			var j = JSONNode.Parse(s);
			foreach (string name in obj_list) {
				// setPos(name_obj[name], j[name]);
				GameObject obj = (GameObject) name_obj[name];
				var jv = j[name];
				float x = jv ["pos"] [0].AsFloat, y = jv ["pos"] [1].AsFloat, z = jv ["pos"] [2].AsFloat;
				obj.transform.position = new Vector3 (x, y, z);
				float rw = jv ["ori"] [0].AsFloat, rx = jv ["ori"] [1].AsFloat, ry = jv ["ori"] [2].AsFloat, rz = jv ["ori"] [3].AsFloat;
				obj.transform.rotation = new Quaternion(rx, ry, rz, rw);
				
			}		
			//		jeep.transform.Translate (new Vector3 (0, -2.5f, 0));
			fltire.transform.Rotate (new Vector3 (0, 90, 0));
			frtire.transform.Rotate (new Vector3 (0, 90, 0));
			bltire.transform.Rotate (new Vector3 (0, 90, 0));
			brtire.transform.Rotate (new Vector3 (0, 90, 0));
			
			checkUserInput ();
			//Instantiate(explosion, jeep.transform.position, Quaternion.identity);
			if (j ["explosion"].AsInt == 1) {
				// Instantiate(explosion, jeep.transform.position, Quaternion.identity);
				PlayExplosion(jeep.transform.position);
				UnityEngine.Debug.Log("explosion");
			}
			//updateMesh (j ["terrain"]);
		}
		catch{
		}
		finally{
		}

	}

	public Terrain terrain;
	TerrainData terrainData;
	void updateMesh(JSONNode jv)
	{
		int width = jv["width"].AsInt, height = jv["height"].AsInt;
		int pointer = jv ["pointer"].AsInt; int length = width * height;
		float[] data = new float[length];
		Marshal.Copy((IntPtr)pointer, data, 0, length);
		float[,] heights = new float[height, width];
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++){
				heights[i,j] = data[i*width+j];
			}
		}
		terrainData.SetHeights (0, 0, heights);
	}

	void OnDestroy(){
		API_Free_Game ();
		// UnloadImportedDll ("game_dll");
	}
	const int up = 1;
	const int down = 2;
	const int left = 3;
	const int right = 4;
	const int nitro = 5;

	void checkUserInput(){
		float temp = Input.GetAxis ("Horizontal");
		if (temp > 0.1)
			API_Input (right);
		if (temp < -0.1)
			API_Input (left);
		temp = Input.GetAxis ("Vertical");
		if (temp > 0.1)
			API_Input (up);
		if (temp < 0.1)
			API_Input (down);
		if (Input.GetKeyDown ("n"))
			API_Input (nitro);
	}
}
