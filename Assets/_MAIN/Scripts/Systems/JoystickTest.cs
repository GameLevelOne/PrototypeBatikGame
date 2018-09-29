using UnityEngine;

public class JoystickTest : MonoBehaviour {
	public Transform dummy;

	void Awake () {
		Debug.Log("Start Init Joystick");
		for (int i=0;i<Input.GetJoystickNames().Length;i++) {
			if (Input.GetJoystickNames()[i] != "") {
				Debug.Log("Joystick "+i+" with name: " + Input.GetJoystickNames()[i]);
			} 
		}
		Debug.Log("Finish Init Joystick");
	}

	void Update () {
		CheckMovement();
		CheckButton();

		// Debug.Log(Input.GetAxis("Horizontal"));								
	}

	void CheckMovement () {
		float inputX = Input.GetAxis("Horizontal Javatale");
		float inputY = Input.GetAxis("Vertical Javatale");

		
		if (inputX > 0) {
			Debug.Log("Right");
			Debug.Log("inputX " + inputX);
		} else if (inputX < 0) {
			Debug.Log("Left");
			Debug.Log("inputX " + inputX);
		} else if (inputX == 0) {
			// Debug.Log("NeutralX");
		}

		if (inputY > 0) {
			Debug.Log("Up");
			Debug.Log("inputY " + inputY);
		} else if (inputY < 0) {
			Debug.Log("Down");
			Debug.Log("inputY " + inputY);
		} else if (inputY == 0) {
			// Debug.Log("NeutralY");
		}

		dummy.position = new Vector2 (dummy.position.x + inputX, dummy.position.y + inputY);
	}

	void CheckButton () {
		// for (int i=0; i<20; i++) {
		// 	if (Input.GetKeyDown("joystick 3 button " + i.ToString())) {
		// 		Debug.Log("joystick 1 button " + i);
		// 	}
		// }

		if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
			Debug.Log("KeyCode.Joystick1Button0");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {
			Debug.Log("KeyCode.Joystick1Button1");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {
			Debug.Log("KeyCode.Joystick1Button2");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button3)) {
			Debug.Log("KeyCode.Joystick1Button3");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button4)) {
			Debug.Log("KeyCode.Joystick1Button4");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) {
			Debug.Log("KeyCode.Joystick1Button5");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button6)) {
			Debug.Log("KeyCode.Joystick1Button6");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button7)) {
			Debug.Log("KeyCode.Joystick1Button7");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button8)) {
			Debug.Log("KeyCode.Joystick1Button8");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button9)) {
			Debug.Log("KeyCode.Joystick1Button9");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button10)) {
			Debug.Log("KeyCode.Joystick1Button10");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button11)) {
			Debug.Log("KeyCode.Joystick1Button11");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button12)) {
			Debug.Log("KeyCode.Joystick1Button12");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button13)) {
			Debug.Log("KeyCode.Joystick1Button13");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button14)) {
			Debug.Log("KeyCode.Joystick1Button14");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button15)) {
			Debug.Log("KeyCode.Joystick1Button15");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button16)) {
			Debug.Log("KeyCode.Joystick1Button16");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button17)) {
			Debug.Log("KeyCode.Joystick1Button17");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button18)) {
			Debug.Log("KeyCode.Joystick1Button18");
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button19)) {
			Debug.Log("KeyCode.Joystick1Button19");
		} 
	}
}
