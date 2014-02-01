using UnityEngine;
using System.Collections;
//using NewGuiScript;

[ExecuteInEditMode]
public class HUD : MonoBehaviour {
	public const int natHorRes = 1280;
	public const int natVerRes = 768;
	public const int CHOICE_NOT_CHOSEN = 0;
	public const int CHOICE_CONFIRM = 1;
	public const int CHOICE_CANCEL = 2;
	
	public GUISkin mySkin;
	
	public GUIStyle topSytle = new GUIStyle();
	
	private bool isLoading = false;
	
	private Rect fullScreenRect = new Rect(0,0,Screen.width, Screen.height);
	
	private bool displayPartyDialog = true;
	private bool displayAvatarDialog = true;
	
	void Start() {
		
	}
	
	void OnGUI () {
		GUI.skin = mySkin;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3 ((float)Screen.width / natHorRes, (float)Screen.height/natVerRes, 1f));
		if (isLoading) {
			GUILayout.BeginArea(new Rect(0,0,natHorRes, natVerRes));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			
			GUILayout.Label("Loading!");	
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		if (displayPartyDialog == true) {
			
			switch(DisplayDialog("You are playing solo,\n" +
				"Would you like to join a party?", "Yes, Please!", "No, Thanks")) 
			{
			case CHOICE_NOT_CHOSEN:
				break;
				
			case CHOICE_CONFIRM:
				NewGuiScript.guiState = NewGuiScript.SETTINGS;
				NewGuiScript.settingsState = NewGuiScript.NETWORK;
				Application.LoadLevel("HomeScreen");
				break;
				
			case CHOICE_CANCEL:
				displayPartyDialog = false;
				break;
			}
		} else if (displayAvatarDialog == true) {
			switch(DisplayDialog("You are playing with the default avatar\n" +
					"Would you like to customize your avatar?", "Customize", "Keep playing")) 
			{
			case CHOICE_NOT_CHOSEN:
				break;
			case CHOICE_CONFIRM:
				NewGuiScript.guiState = NewGuiScript.SETTINGS;
				NewGuiScript.settingsState = NewGuiScript.PROFILE;
				Application.LoadLevel("HomeScreen");
				break;
			case CHOICE_CANCEL:
				displayAvatarDialog = false;
				break;
			}
		}
		
		
		GUILayout.BeginArea(new Rect(0,0,natHorRes, 100), topSytle);
		GUILayout.BeginHorizontal(GUILayout.MinHeight(100));
		GUILayout.BeginVertical(GUILayout.MinHeight(500));
		if (GUILayout.Button("Back")) {
			isLoading = true;
			Application.LoadLevel("HomeScreen");	
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
	}
	
	private int DisplayDialog(string text, string confirmText, string cancelText) {

		GUILayout.BeginArea(fullScreenRect);
		GUILayout.BeginVertical();
		
		GUILayout.FlexibleSpace();				// <PushDown />	
		
		GUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();				// <PushRight />
		
		GUILayout.BeginVertical("box");				//<VAlign>
		GUILayout.FlexibleSpace();				//<PushDown />	
		
		GUILayout.Label(text);
		
		GUILayout.FlexibleSpace();				// <PushUp />
		GUILayout.BeginHorizontal();			// <ButtonHolder>
		
		if (GUILayout.Button(cancelText)) {
			return CHOICE_CANCEL;
		}
		
		GUILayout.FlexibleSpace();				// <PushOut />
		
		if (GUILayout.Button(confirmText)) {
			return CHOICE_CONFIRM;	
		}
		
		GUILayout.EndHorizontal();				// </ButtonHolder>
		GUILayout.EndVertical();				// </VAlign>
		
		GUILayout.FlexibleSpace();				// <PushLeft />
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();				// <PushUp />
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
		return CHOICE_NOT_CHOSEN;
	}
	
}
