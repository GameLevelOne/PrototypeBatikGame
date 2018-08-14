using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public Transform playerTransform;
	public Vector2 minBound, maxBound;
	public bool isMovingX, isMovingY;
	public float smoothSpeed;
}
