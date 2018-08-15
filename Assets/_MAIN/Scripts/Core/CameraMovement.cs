using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public Transform playerTransform;
	public Vector3 offset;
	
	public float smoothSpeed;

	//HOW TO CALCULATE ORTHOGRAPHIC CAMERA WIDTH
	// void Start()
	// {
	// 	float screenHeightInUnits = GetComponent<Camera>().orthographicSize * 2;
	// 	float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
	// 	Debug.Log("screenHeightInUnits = "+screenHeightInUnits);
	// 	Debug.Log("screenWidthInUnits = "+screenWidthInUnits);
	// }
}
