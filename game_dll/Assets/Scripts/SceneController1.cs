using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;
using System;
using System.Runtime.InteropServices;



public class SceneController1 : MonoBehaviour {
	//public AudioClip jeepSound;
	//private AudioSource source;
	//private bool soundNotPlaying = false;
	
	void Awake () {
		//source = GetComponent<AudioSource>();
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
		name_obj = new Hashtable ();
		name_obj.Add ("body", jeep);
		name_obj.Add ("fltire", fltire);
		name_obj.Add ("frtire", frtire);
		name_obj.Add ("bltire", bltire);
		name_obj.Add ("brtire", brtire);

		API_Init (1);
	}

	void Update () {
		string s = Marshal.PtrToStringAnsi (API_Update ());
		JSONNode j = JSONNode.Parse(s);
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
		if (temp > 0.1) {
			API_Input (up);
		
		}
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
