using UnityEngine;

public class BeeMovement : MonoBehaviour {
	[HeaderAttribute("BeeMovement Attributes")]
	public float patrolSpeed;
	public float chaseSpeed;
	public float idleDuration;

	[HeaderAttribute("Current")]
	public Transform beeHiveTransform;
	public Transform playerTransform;
	public Vector2 patrolDestination;
	public bool initIdle = false;
	public bool initPatrol = false;

	float tIdle;

	public float TIdle{
		get{ return tIdle; }
		set{ tIdle = value; }
	}
}