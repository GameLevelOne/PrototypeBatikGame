using UnityEngine;

public enum FishCollectibleType {
	NONE,
	HP_POTION,
	MANA_POTION
}

public enum FishState {
	IDLE,
	PATROL,
	CHASE,
	CATCH,
	FLEE
}

public class FishCollectible : MonoBehaviour {
	public FishCollectibleType type;
	public FishState state;

	public Collider2D parentPoolCol;
	public FishingRod fishingRod;

	public Vector2 targetPos;
	public float moveSpeed;
	public float chaseSpeed;
	public float fleeSpeed;
	public float timeToCatch;
	public float idleDuration;

	[SerializeField] float timeIdle;

	public float TimeIdle {
		get {return timeIdle;}
		set {timeIdle = value;}
	}
}
