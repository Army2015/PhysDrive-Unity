using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	public Texture backgroundTexture;
	public GUIStyle buttonColor;
	public GUIStyle MenuColor;

	
	public AudioClip clickSound;
	private AudioSource source;


	
	void Awake () {
		
		source = GetComponent<AudioSource>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		//Display backgroundTexture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
		GUI.Box (new Rect(Screen.width / 2 - 50,Screen.height / 30,100,100),"Main Menu", MenuColor);
		FirstMenu ();
	}

	public float rightOfset = 300f;
	public float leftOfset = -270f;
	void FirstMenu(){
			

		if(GUI.Button(BuildStringToGUI(0f,leftOfset),"Level 0",buttonColor)){	
			source.PlayOneShot(clickSound,1f);
			Application.LoadLevel("Scene0");
		
		}
		if(GUI.Button(BuildStringToGUI(150f,leftOfset),"Level 1",buttonColor)){	
			source.PlayOneShot(clickSound,1f);
			//level 6 is scene 1
			Application.LoadLevel("level6");
		}
		
		if(GUI.Button( BuildStringToGUI (300f,leftOfset),"Level 2",buttonColor)){
			source.PlayOneShot(clickSound,1f);
			Application.LoadLevel("Scene2");
		
			}

		if(GUI.Button(BuildStringToGUI(0f,rightOfset),"Level 3",buttonColor)){	
			source.PlayOneShot(clickSound,1f);
			Application.LoadLevel("Scene3");
		
			}

		if(GUI.Button(BuildStringToGUI(150f,rightOfset),"Level 4",buttonColor)){
			source.PlayOneShot(clickSound,1f);
			print ("4");
			//Application.LoadLevel("Scene4");
			}

		if(GUI.Button(BuildStringToGUI(300f,rightOfset),"Level 5",buttonColor)){
			source.PlayOneShot(clickSound,1f);
			Application.LoadLevel("Scene5");
		
		}
		
	}
	Rect BuildStringToGUI(float gap, float sideOfset){
		float calculateOfset = (-200 + gap);
		Rect r = new Rect(Screen.width /2 - 150 + sideOfset,Screen.height/2 +(calculateOfset), 260,70);
				return r;
	}


	/*
in case you wanted to make a sound wen the mouse hovers over the button
		if (!inHoverSpace)
			hoverOverButton0 = true;
		if (button0.Contains (Input.mousePosition) && hoverOverButton0 && !inHoverSpace) {
			source.PlayOneShot(clickSound,.1f);
			inHoverSpace = true;
		}
		if (!button0.Contains (Input.mousePosition)) {
			hoverOverButton0 = false;
			inHoverSpace = false;
		}
	*/












}
