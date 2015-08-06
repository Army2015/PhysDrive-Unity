using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;
using System;
using System.Runtime.InteropServices;



public class SceneController : MonoBehaviour {
	public AudioClip explosionSound;
	private AudioSource source;

	void Awake () {
		source = GetComponent<AudioSource>();
	}
	// Use this for initialization
	public GameObject jeep;
	public GameObject fltire, frtire, bltire, brtire;
	[DllImport("game_dll")]
	static extern IntPtr API_Update ();
	[DllImport("game_dll")]
	static extern IntPtr API_Init (int code);
	[DllImport("game_dll")]
	static extern void API_Input (int code);

	private Hashtable name_obj;

	string[] obj_list = {"body", "fltire", "frtire", "bltire", "brtire"};
	void Start () {
		explosion = (UnityEngine.GameObject)Instantiate (explosion_prefab);
		StopExplosion ();
		name_obj = new Hashtable ();
		name_obj.Add ("body", jeep);
		name_obj.Add ("fltire", fltire);
		name_obj.Add ("frtire", frtire);
		name_obj.Add ("bltire", bltire);
		name_obj.Add ("brtire", brtire);

		string s = Marshal.PtrToStringAnsi (API_Init (0));
		var j = JSONNode.Parse(s);
		var mines = j ["mines"];
		SetMines (mines);

		anim = game_canvas.GetComponent<Animator> ();

		// checkTerrain (mines);


	}

	void checkTerrain(JSONNode mines){
		terrain_data = terrain.terrainData;

//		var pos = mines[0];
//		float x = pos[0].AsFloat, y = pos[1].AsFloat, z = pos[2].AsFloat;
//		Debug.Log ("mine0: x: " + x + ", " + "z: " + z + ", " + "y: " + y);
//		Debug.Log ("InterpolatedHeight: " + terrain_data.GetInterpolatedHeight((x+150.0f)/300.0f, (z+150.0f)/300.0f));
//
//		pos = mines[1];
//		x = pos [0].AsFloat; y = pos [1].AsFloat; z = pos[2].AsFloat;
//		Debug.Log ("mine1: x: " + x + ", " + "z: " + z + ", " + "y: " + y);
//		Debug.Log ("InterpolatedHeight: " + terrain_data.GetInterpolatedHeight((x+150.0f)/300.0f, (z+150.0f)/300.0f));
//
//		pos = mines[2];
//		x = pos [0].AsFloat; y = pos[1].AsFloat; z = pos[2].AsFloat;
//		Debug.Log ("mine2: x: " + x + ", " + "z: " + z + ", " + "y: " + y);
//		Debug.Log ("InterpolatedHeight: " + terrain_data.GetInterpolatedHeight((x+150.0f)/300.0f, (z+150.0f)/300.0f));

		float[] x = new float[200];
		float[] y = new float[200];
		int mine_num = 200;
		for (int i = 0; i < mine_num; i++){
			var pos = mines[i];
			float px = pos[0].AsFloat, py = pos[1].AsFloat, pz = pos[2].AsFloat;
			x[i] = terrain_data.GetInterpolatedHeight((px+150.0f)/300.0f, (pz+150.0f)/300.0f);
			y[i] = py;
			Debug.Log(i);
		}

		float mxy = 0;
		float mx = 0;
		float mx2 = 0;
		float my = 0;
		for (int i = 0; i < mine_num; i++){
			mx += x[i];
			mx2 += x[i] * x[i];
			my += y[i];
			mxy += x[i]*y[i];
		}
		mx /= mine_num;
		mx2 /= mine_num;
		my /= mine_num;
		mxy /= mine_num;

		float a_up = mxy - mx * my;
		float a_down = mx2 - mx * mx;
		float a = a_up / a_down;

		float b = my - a * mx;

		Debug.Log("a: "+a);
		Debug.Log("b: "+b);

	}

	public GameObject mine;
	public GameObject[] rocks;
	void SetMines(JSONNode mines){
		terrain_data = terrain.terrainData;
		int mine_num = 200;
		int n = rocks.Length;
		for (int i = 0; i < mine_num; i++) {
			var pos = mines[i];
			float x = pos[0].AsFloat, y = pos[1].AsFloat, z = pos[2].AsFloat;
			y = terrain_data.GetInterpolatedHeight((x+150.0f)/300.0f, (z+150.0f)/300.0f)+terrain.transform.position.y;
			/*if (x*x + z*z > 50 * 50) y -= 1;
			if (x*x + z*z > 100 * 100) y -= 2;*/
			Quaternion q = Quaternion.Euler(0,0,0);
			//Quaternion q = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
			GameObject temp = (UnityEngine.GameObject)Instantiate(mine, new Vector3(x,y,z), q);

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
	
	public GameObject game_canvas;
	public Animator anim;
	public Terrain terrain;
	TerrainData terrain_data;
	public GameObject explosion_prefab;
	GameObject explosion;
	int counter = 0;

	void GameOver() {
		anim.SetTrigger("GameOver");
	}
	
	void Update () {
		string s = Marshal.PtrToStringAnsi (API_Update ());
		JSONNode j = JSONNode.Parse(s);
		if (j ["fail"].AsInt == 1) {
			GameOver();
		}
		if (j ["success"].AsInt == 1){
			anim.SetTrigger("GameOver");
		}
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

		jeep.transform.Translate (new Vector3 (0, -2.77f, 0));
		fltire.transform.Rotate (new Vector3 (0, 90, 0));
		frtire.transform.Rotate (new Vector3 (0, 90, 0));
		bltire.transform.Rotate (new Vector3 (0, 90, 0));
		brtire.transform.Rotate (new Vector3 (0, 90, 0));
		
		checkUserInput ();
		// checkUserInputGUI();
		//Instantiate(explosion, jeep.transform.position, Quaternion.identity);
		if (j ["explosion"].AsInt == 1) {
			var jv = j["explosion_pos"];
			float x = jv[0].AsFloat, y = jv[1].AsFloat, z = jv [2].AsFloat;
			Instantiate(explosion, new Vector3(x, y, z), Quaternion.identity);
			//sound Explosion
			source.PlayOneShot(explosionSound,1f);

		}
		


	}

	const int up = 1;
	const int down = 2;
	const int left = 3;
	const int right = 4;
	const int nitro = 5;

	void checkUserInput(){
		float temp = Input.GetAxis ("Horizontal");
		if (temp > 0.1)
			API_Input (left);
		if (temp < -0.1)
			API_Input (right);
		temp = Input.GetAxis ("Vertical");
		if (temp > 0.1)
			API_Input (up);
		if (temp < -0.1)
			API_Input (down);
		if (Input.GetKeyDown ("n"))
			API_Input (nitro);
	}

	public Slider left_right;
	public Slider up_down;
	void checkUserInputGUI(){
		float temp = left_right.value;
		if (temp > 0.3)
			API_Input (right);
		if (temp < -0.3)
			API_Input (left);
		temp = up_down.value;
		if (temp > 0.3)
			API_Input (up);
		if (temp < -0.3)
			API_Input (down);
	}
}
