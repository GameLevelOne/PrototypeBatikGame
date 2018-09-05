using UnityEngine;

public enum FishCharacteristic {
	TAME, //CHASE directly to bait
	CALM, //WAIT for n seconds before CHASE
	WILD, //WAIT for n seconds then TRYINGGRAB for n times before GRAB
}

public enum FishState {
	IDLE,
	PATROL,
	THINK,
	WAIT,
	TRYINGGRAB,
	CHASE,
	CATCH,
	FLEE
}

public class Fish : MonoBehaviour {
	public FishCharacteristic fishChar;
	public FishState state;
	public GameObject[] lootableObjs;

	public Animator anim;
	public Collider selfCol;
	// public Rigidbody rigidbody;
	public float moveSpeed;
	public float chaseSpeed;
	public float fleeSpeed;
	public float timeToCatch;
	public float idleDuration;

	[HeaderAttribute("TAME and WILD Fish Attributes")]
	public float maxWaitingDuration;

	[HeaderAttribute("WILD Fish Attributes only")]
	public int maxTryingGrabTimes;

	[HeaderAttribute("Current")]
	public bool initIdle;
	public Transform parentPoolCol;
	public FishingRod fishingRod;
	public Vector3 targetPos;
	public float parentPoolRadius;
	
	[SerializeField] float timeIdle;

	public float TimeIdle {
		get {return timeIdle;}
		set {timeIdle = value;}
	}
}
