using UnityEngine;
using System.Collections;

public class ForMainMenu : MonoBehaviour {

		
		private string myText = "";
		private string myTextField = "";
		
		void OnGUI(){
			GUI.Box(new Rect(150,150,400,Screen.width / 2), "Main Menu");
			GUILayout.BeginArea (new Rect (250, 250, 400, Screen.width / 2));
			
			
			//other levels 
			CreateButton (0);
			CreateButton (1);
			CreateButton (2); 
			CreateButton (3);
			CreateButton (4);
			
			
			GUILayout.EndArea();
			
		}
		public void CreateButton(int level){
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Level " + level.ToString(), GUILayout.Width (200))) {
				print("This is where level " + level.ToString() + " would go ");
				//Application.LoadLevel("New Level");
			}
			GUILayout.EndHorizontal ();
			
			
		}

	
	

}
