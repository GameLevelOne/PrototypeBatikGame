using UnityEngine;

public class BeeMovement : MonoBehaviour {
	[HeaderAttribute("BeeMovement Attributes")]
	public float patrolRange;
	public float chaseRange;
	public float startledRange;
	public float patrolSpeed;
	public float chaseSpeed;
	public float idleDuration;

	[HeaderAttribute("Current")]
	public Transform beeHiveTransform;
	public Vector2 patrolDestination;
	public bool initIdle = false;
	public bool initPatrol = false;
	public bool initStartled = false;

	float tIdle;

	public float TIdle{
		get{ return tIdle; }
		set{ tIdle = value; }
	}
}