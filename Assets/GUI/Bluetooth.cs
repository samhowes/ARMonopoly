using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Bluetooth {
	public static string deviceString;
	
	/* Interface to native implementation */
	
	
	
	/* Public interface for use inside C# / JS code */
	
	// Returns current lookup status
	public static string getHelloWorld()
	{

			return "Done";

	}
	
	public static void initBluetooth()
	{

			return;

	}
	
	public static bool isBluetoothOn()
	{

			return true;

	}
	
	public static void destroyBluetooth()
	{

			return;

	}
	
	public static string[] getFoundDeviceNames()
	{

			return new string[3] {"Matt", "Joanne", "Robert"};

		
	}
	
	public static void connectToDeviceAtIndex(int index)
	{

			return;
	
	}
	
	public static void disconnectDevice()
	{

			return;

	}
	
	public static bool isDeviceConnected() 
	{

			return true;

	}
	
	public static string readDeviceCharacteristic()
	{

			return deviceString;

	}
	
	public static void writeDeviceCharacteristic(string value)
	{
	
			deviceString = value;
			return;
	}
	
}
