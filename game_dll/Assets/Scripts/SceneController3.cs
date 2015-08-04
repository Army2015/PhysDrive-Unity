﻿using UnityEngine;
using SimpleJSON;
using System.Collections;
using System;
using System.Runtime.InteropServices;



public class SceneController3 : MonoBehaviour {

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
	static extern IntPtr API_Get_Rocks ();

	private Hashtable name_obj;
	string[] obj_list = {"body", "fltire", "frtire", "bltire", "brtire"};
	void Start () {
		API_Init (3);
		name_obj = new Hashtable ();
		name_obj.Add ("body", jeep);
		name_obj.Add ("fltire", fltire);
		name_obj.Add ("frtire", frtire);
		name_obj.Add ("bltire", bltire);
		name_obj.Add ("brtire", brtire);
		//terrainData = (terrain).terrainData;
		//SetMines ();
		InitRocks ();
	}

	public GameObject rock;
	public GameObject[] rocks;
	public Material[] materials;
	void InitRocks(){
		int rocks_num = 50;
		rocks = new GameObject[rocks_num];
		int n = materials.Length;
		for (int i = 0; i < rocks_num; i++) {
			rocks[i] = (GameObject)Instantiate(rock, new Vector3(0,0,0), Quaternion.identity);
			rocks[i].GetComponent<Renderer>().material = materials[i%n];
		}
	}

	void GetRocks(JSONNode j){
		int rocks_num = 40;
		
		for (int i = 0; i < rocks_num; i++) {
			var pos = j[i];
			float x = pos[0].AsFloat, y = pos[1].AsFloat, z = pos[2].AsFloat;
			rocks[i].transform.position = new Vector3(x, y, z);
		}
	}
	
	void Update () {
		try{
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

			GetRocks(j["rocks"]);

		}
		catch{
		}
		finally{
		}

	}

	void OnDestroy(){
		 // API_Free_Game ();
		// UnloadImportedDll ("game_dll");
	}

	const int up = 1;
	const int down = 2;
	const int left = 3;
	const int right = 4;
	const int nitro = 5;
	const int stop = 6;

	void checkUserInput(){
		float temp = Input.GetAxis ("Horizontal");
		if (temp > 0.1)
			API_Input (left);
		if (temp < -0.1)
			API_Input (right);
		temp = Input.GetAxis ("Vertical");
		if (temp > 0.1)
			API_Input (up);
		if (temp < 0.1)
			API_Input (down);
		if (Input.GetKeyDown ("n"))
			API_Input (nitro);
		if (Input.GetKeyDown ("b"))
			API_Input (stop);
	}
}
