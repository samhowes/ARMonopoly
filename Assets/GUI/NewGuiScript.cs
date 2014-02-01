using UnityEngine;
using System.Collections;

public class MyTuple: Object {
	public GUIStyle style;
	public Texture2D tex;
	
	public MyTuple(GUIStyle style, Texture2D tex){
		this.style = style;
		this.tex = tex;
	}
}

[ExecuteInEditMode]
public class NewGuiScript : MonoBehaviour {
	/****************************************************************************/
	/*								Variables and Constants		                */
	/****************************************************************************/

	//------------------------------- Class Constants 	-------------------------------// 
	public static int natHorRes = 1024;
	public static int natVerRes = 768;
	public const int CHOICE_NOT_CHOSEN = 0;
	public const int CHOICE_CONFIRM = 1;
	public const int CHOICE_CANCEL = 2;
	
	public string networkServerIP = "155.41.7.183";
	public int networkPortNo = 25000;

	//------------------------------- GUI Variables 	-------------------------------//
	public GUISkin mySkin;
	public int headerFontSize = -1; 
	public int labelFontSize = -1;
	public int buttonFontSize = -1;

	//------------------------------- Home screen Styles-------------------------------//
	public Texture2D logo;									// Main Logo Items
	public GUIStyle logoStyle = new GUIStyle();
	
	public Texture2D playButton;
	public GUIStyle playStyle = new GUIStyle();
	
	public Texture2D settingsButton;
	public GUIStyle settingsStyle = new GUIStyle();
	
	//------------------------------- Settings Styles	-------------------------------//
	public Texture2D defaultAvatar;
	public GUIStyle avatarStyle = new GUIStyle();
	public int avatarHeight;
	public int avatarWidth;

	//------------------------------- Scaling variables	-------------------------------//
	public float scale;
	private ArrayList resizeStyles = new ArrayList();
	
	private Rect fullScreenRect = new Rect(0,0,Screen.width, Screen.height);

	//------------------------- State contants and variables	----------------------//
	public const int HOME = 0;
	public const int SETTINGS = 1;
	public static int guiState = HOME;							// Internal state: home, settings, or network
	public bool isLoading = false;
	
	public const int SETTINGS_HOME = 0;
	public const int PROFILE = 1;
	public const int NETWORK = 2;
	public const int DEVICE = 3;
	public static int settingsState = SETTINGS_HOME;

	//-------------------- 	Bluetooth contants and variables	----------------------//
	private int 	btIndexOfDeviceToConnect = -1;
	private string 	btNameOfDeviceToConnect = null;
	private int 	btConnectState = 0;
	private string 	btDeviceString = null;
	private string 	btStringToSendToDevice = "Type display name here";

	public static int bluetoothConnectionState = BLUETOOTH_START;
	public static bool gameTileIsConnected = false; 
	private const int BLUETOOTH_START 			= 0;
	private const int BLUETOOTH_CHOOSE_DEVICE 	= 1;
	private const int BLUETOOTH_CONNECT 		= 2;
	private const int BLUETOOTH_WRITE_TO_DEVICE = 3;
	private const int BLUETOOTH_DISCONNECT 		= 4;

	public int test = 0;
	
	public string displayName = "CustomizeMe!";

	/****************************************************************************/
	/*									Methods					                */
	/****************************************************************************/

	// Use this for initialization
	void Start () {
		setupStyles();
		
	}
	
	
	private void resizeElements(){
		scale = ((float)Screen.height)/((float)natVerRes);
		
		for (int ii = 0; ii < resizeStyles.Count; ++ii) {
			MyTuple tuple = (MyTuple) resizeStyles[ii];
			tuple.style.fixedWidth = (int)(tuple.tex.width*scale);
			tuple.style.fixedHeight = (int)(tuple.tex.height*scale);
		}
		
		mySkin.customStyles[0].fontSize = (int)(headerFontSize*scale);
		mySkin.label.fontSize = (int)(labelFontSize*scale);
		mySkin.textField.fontSize = (int)(labelFontSize*scale);
		mySkin.button.fontSize = (int)(buttonFontSize*scale);
		
		avatarStyle.fixedHeight = (int)(avatarHeight*scale);
		avatarStyle.fixedWidth = (int)(avatarWidth*scale);
		
		/*logoStyle.fixedWidth = (int)(logo.width*scale);
		logoStyle.fixedHeight = (int)(logo.height*scale);
		
		playStyle.fixedWidth = (int)(playButton.width*scale);
		playStyle.fixedHeight = (int)(playButton.height*scale);
		
		settingsStyle.fixedWidth = (int)(settingsButton.width*scale);
		settingsStyle.fixedHeight = (int)(settingsButton.height*scale);
		
		settingsStyle.fixedWidth = (int)(settingsButton.width*scale);
		settingsStyle.fixedHeight = (int)(settingsButton.height*scale);*/
		
		fullScreenRect.width = Screen.width;
		fullScreenRect.height = Screen.height;
	}
	
	private void setupStyles(){
		
		playStyle.normal.background = playButton;
		settingsStyle.normal.background = settingsButton;
		
		avatarStyle.normal.background = defaultAvatar;
		avatarStyle.fixedHeight = avatarHeight;
		avatarStyle.fixedWidth = avatarWidth;
		
	//	headerFontSize = mySkin.customStyles[0].fontSize;
	//	labelFontSize = mySkin.label.fontSize;
	//	buttonFontSize = mySkin.button.fontSize;
		
		mySkin.textField.fontSize = labelFontSize;
		
		resizeStyles.Add(new MyTuple(logoStyle, logo));
		resizeStyles.Add(new MyTuple(playStyle, playButton));
		resizeStyles.Add(new MyTuple(settingsStyle, settingsButton));
		resizeStyles.Add(new MyTuple(avatarStyle, defaultAvatar));
		
		
	}

	/****************************************************************************/
	/*								Display Functions			                */
	/****************************************************************************/
	// Update is called once per frame
	void OnGUI () {
		resizeElements();

		//setupStyles();
		GUI.skin = mySkin;
		
		switch (guiState) {
		case HOME:
			DisplayHomeGUI();
			break;
		case SETTINGS:
			DisplaySettingsGUI();
			break;
		default:
			guiState = HOME;								// Go to the home screen if we end up in an invalid state
			break;
		}
		if (isLoading) {
			GUILayout.BeginArea(fullScreenRect);								//<Page>
			GUILayout.BeginHorizontal();										//<HHolder>
			GUILayout.FlexibleSpace();											//<PushRight />
		
			GUILayout.BeginVertical();											//<ContentHolder>
			GUILayout.FlexibleSpace();											//<PushDown /> 
		
			GUILayout.Label("Loading!", mySkin.customStyles[0]);
		
			GUILayout.FlexibleSpace();											//<PushUp />
			GUILayout.EndVertical();											//</ContetnHolder>		
			GUILayout.FlexibleSpace();											// <PushLeft />
			GUILayout.EndHorizontal();											// </HHolder>
			GUILayout.EndArea();
		}
		
	}


	//------------------------- Specific GUI Screens	----------------------// 
	private void DisplayHomeGUI(){
		GUILayout.BeginArea(fullScreenRect);	//<Container>
		
		GUILayout.BeginHorizontal();										//<HHolder>
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.BeginVertical();											//<LogoHolder>
		GUILayout.FlexibleSpace();											//<PushDown /> 
		
		GUILayout.Label(logo, logoStyle);
		
		GUILayout.FlexibleSpace();											//<PushUp />
		GUILayout.EndVertical();											//</VHolder>
		
		GUILayout.FlexibleSpace();											//<PushOut />
		
		GUILayout.BeginVertical();											//<ButtonHolder>
		
		GUILayout.FlexibleSpace();											//<PushDown />
		if (GUILayout.Button("", playStyle)) {
			isLoading = true;
			Application.LoadLevel("QCARPlayScene");
		}
		GUILayout.FlexibleSpace();											//<PushOut />
		if (GUILayout.Button("", settingsStyle)) {
			guiState = SETTINGS;
		}
		
		GUILayout.FlexibleSpace();											//<PushUp />
		
		GUILayout.EndVertical();											//</ButtonHolder>
		
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											// </HHolder>
		
		GUILayout.EndArea();												//</Container>
	}
	
	private void DisplaySettingsGUI() {
		switch (settingsState) {
		case SETTINGS_HOME:
			DisplaySet_Home();
			break;
		case PROFILE:
			DisplaySet_Profile();
			break;
		case NETWORK:
			DisplaySet_Network();
			break;
		case DEVICE:
			DisplaySet_Device();
			break;
		default:
			settingsState = SETTINGS_HOME;
			break;
		}
	}

	/****************************************************************************/
	/*								Settings Functions			                */
	/****************************************************************************/
	private void DisplaySet_Home() 
	{
		if (GUILayout.Button("Back")) {
			guiState = HOME;	
		}
		
		GUILayout.BeginArea(fullScreenRect);								//<Page>
		GUILayout.BeginHorizontal();										//<HHolder>
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.BeginVertical();											//<ContentHolder>
		GUILayout.FlexibleSpace();											//<PushDown /> 
		
		GUILayout.Label("Settings!", mySkin.customStyles[0]);
		
		if (GUILayout.Button("Customize Profile"))
		{
			settingsState = PROFILE;
		}

		if (GUILayout.Button("Connect to Players"))
		{
			settingsState = NETWORK;
		}

		if (GUILayout.Button("Configure Device"))
		{
			settingsState = DEVICE;
		}

		GUILayout.FlexibleSpace();											//<PushUp />
		GUILayout.EndVertical();											//</ContetnHolder>		
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											// </HHolder>
		GUILayout.EndArea();
	}

	//------------------------- Customize Profile	----------------------// 
	private void DisplaySet_Profile() {
		if (GUILayout.Button("Back")) {
			settingsState = SETTINGS_HOME;	
		}
		
		GUILayout.BeginArea(fullScreenRect);								//<Page>
		GUILayout.BeginHorizontal();										//<HHolder>
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.BeginVertical();											//<ContentHolder>
		GUILayout.FlexibleSpace();											//<PushDown /> 
		
		GUILayout.Label("Your Profile", mySkin.customStyles[0]);
		
		GUILayout.BeginHorizontal();										//<Field1>
		GUILayout.FlexibleSpace();											//<PushRight />
		GUILayout.Label("Display Name: ");
		displayName = GUILayout.TextField(displayName, 31);
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											//</Field1>
		
		GUILayout.Label("Your Avatar");
		GUILayout.BeginHorizontal();										//<Field2>
		
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.Box("",avatarStyle);	
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											//</Field2>
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();											//<PushRight />
		GUILayout.Button("Customize");
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();											//<PushUp />
		GUILayout.EndVertical();											//</ContetnHolder>		
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											// </HHolder>
		GUILayout.EndArea();
	}

	//------------------------- Network Screen	----------------------// 
	private void DisplaySet_Network() 
	{
		if (GUILayout.Button("Back")) {
			settingsState = SETTINGS_HOME;	
		}
		
		GUILayout.BeginArea(fullScreenRect);								//<Page>
		GUILayout.BeginHorizontal();										//<HHolder>
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.BeginVertical();											//<ContentHolder>
		GUILayout.FlexibleSpace();											//<PushDown /> 
		
		GUILayout.Label("Connect to Players", mySkin.customStyles[0]);
		
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			if(GUILayout.Button("Start Client"))
			{
				Network.Connect(networkServerIP, networkPortNo);
			}
			if(GUILayout.Button("Start Server"))
			{
				Network.InitializeServer(10,networkPortNo); 
			}
		}
		else{
			if(Network.peerType == NetworkPeerType.Client)
			{
				GUILayout.Label("Client");
				
				if(GUILayout.Button("Change Color"))
				{
					renderer.material.shader = Shader.Find ("Specular");
					// Set red specular highlights
					renderer.material.SetColor ("_SpecColor", Color.red);
					//renderer.material.color = Color.green; 
					//networkView.RPC("ChangeColor",RPCMode.All); 
				}
				
				if(GUILayout.Button("Logout"))
				{
					Network.Disconnect(250); 
				}
			}
			if(Network.peerType == NetworkPeerType.Server)
			{
				GUILayout.Label("Server"); 
				GUILayout.Label("Connection: " + Network.connections.Length);
				
				if(GUILayout.Button("Logout"))
				{
					Network.Disconnect(250); 
				}
			}
		}

		//GUILayout.Label("Naveen! Give me options to put here! :(");
		
		GUILayout.FlexibleSpace();											//<PushUp />
		GUILayout.EndVertical();											//</ContetnHolder>		
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											// </HHolder>
		GUILayout.EndArea();
	}

	//------------------------- Gametile connection	----------------------// 
	private void DisplaySet_Device() {
		if (GUILayout.Button("Back")) {
			settingsState = SETTINGS_HOME;
			bluetoothConnectionState = BLUETOOTH_START;
			Bluetooth.destroyBluetooth();
		}

		GUILayout.BeginArea(fullScreenRect);								//<Page>
		GUILayout.BeginHorizontal();										//<HHolder>
		GUILayout.FlexibleSpace();											//<PushRight />
		
		GUILayout.BeginVertical();											//<ContentHolder>
		GUILayout.FlexibleSpace();											//<PushDown /> 
		
		GUILayout.Label("Bluetooth Settings", mySkin.customStyles[0]);

		switch (bluetoothConnectionState)
		{
		case BLUETOOTH_START:											//--- Start the Bluetooth Manager and scan for devices
			if (GUILayout.Button("Start Scanning for Devices")) {
				Bluetooth.initBluetooth();
				bluetoothConnectionState = BLUETOOTH_CHOOSE_DEVICE;
			}
			break;
		
		case BLUETOOTH_CHOOSE_DEVICE:									//--- Have the user choose a device to connect
			string[] deviceList = null;
			int selection = -1;
			
			if (!Bluetooth.isBluetoothOn()) {
				GUILayout.Label("Waiting for Bluetooth to initialize...");
				GUILayout.Label("Make sure you have turned\n on Bluetooth in \"Settings\"");
				break;
			}

			GUILayout.Label("Scanning for Bluetooth 4.0 devices...");
			deviceList = Bluetooth.getFoundDeviceNames();
			if (deviceList == null) {
				break;
			}
			
			GUILayout.Label("Currently availble devices:");
			for (int ii = 0; ii < deviceList.Length; ii++) 				// Display the found devices to the user
			{
				if (GUILayout.Button(deviceList[ii])) {
					selection = ii;
				}
			}

			if (selection != -1) {										// The user made a selection, queue it for connection
				btIndexOfDeviceToConnect = selection;
				btNameOfDeviceToConnect = deviceList[selection];
				bluetoothConnectionState = BLUETOOTH_CONNECT;
			}
			break;

		case BLUETOOTH_CONNECT:											//--- Connect to a user-selected device
			string deviceString = null;

			GUILayout.Label("Connecting to device: " + 				
			                btNameOfDeviceToConnect);
			if (btConnectState == 0) {									// Make sure to only make one Bluetooth call per frame
				Bluetooth.connectToDeviceAtIndex(btIndexOfDeviceToConnect);
				btConnectState = 1;
			} 
			else if (btConnectState == 1) 
			{
				if (Bluetooth.isDeviceConnected()){
					GUILayout.Label("Connected!");
					gameTileIsConnected = true;
					btConnectState = 2;
				} else {
					GUILayout.Label("Connection in progress....");
				}
			} 
			else if (btConnectState == 2)
			{
				deviceString = Bluetooth.readDeviceCharacteristic();
				if (deviceString == "None") {
					GUILayout.Label("Retrieving information from the device....");
				} else {
					btDeviceString = deviceString;
					btConnectState = 0;
					bluetoothConnectionState = BLUETOOTH_WRITE_TO_DEVICE;
				}
			} else {
				btConnectState = 0;
			}

			break;

		
		
		case BLUETOOTH_WRITE_TO_DEVICE:									//--- Choose what to display on the device

			GUILayout.Label("Your tile is currently displaying: " + btDeviceString);
			GUILayout.Label("Type a value below to change:");

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			btStringToSendToDevice = GUILayout.TextField(btStringToSendToDevice, 31);

			if (GUILayout.Button("Send!")) {
				Bluetooth.writeDeviceCharacteristic(btStringToSendToDevice);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			break;

		default:
			bluetoothConnectionState = BLUETOOTH_CONNECT;
			break;
		}

		if (bluetoothConnectionState >= BLUETOOTH_CONNECT) 
		{
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Disconnect from Current Device")) 
			{
				Bluetooth.disconnectDevice();
			}
		} else {
			GUILayout.FlexibleSpace();
		}

													//<PushUp />
		GUILayout.EndVertical();											//</ContentHolder>		
		GUILayout.FlexibleSpace();											// <PushLeft />
		GUILayout.EndHorizontal();											// </HHolder>
		GUILayout.EndArea();
	}

	/****************************************************************************/
	/*								Utility Functions			                */
	/****************************************************************************/

	private int DisplayDialog(string text, string confirmText, string cancelText) 
	{

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

