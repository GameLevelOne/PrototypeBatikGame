using UnityEngine;

public class CameraMovement : MonoBehaviour {
	[HeaderAttribute("CameraMovement Attribute")]
	public Transform playerTransform;
	public Vector3 offset;
	public float smoothSpeed;
	public float zoomSpeed;
	
	[HeaderAttribute("Current")]
	public bool isZooming = false;
	public float zoomValue;
	

	

	//HOW TO CALCULATE ORTHOGRAPHIC CAMERA WIDTH
	// void Start()
	// {
	// 	float screenHeightInUnits = GetComponent<Camera>().orthographicSize * 2;
	// 	float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
	// 	 // Debug.Log("screenHeightInUnits = "+screenHeightInUnits);
	// 	 // Debug.Log("screenWidthInUnits = "+screenWidthInUnits);
	// }
}
