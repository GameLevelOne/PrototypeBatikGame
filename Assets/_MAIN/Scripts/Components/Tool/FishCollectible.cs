using UnityEngine;

public enum FishCollectibleType {
	NONE,
	HP_POTION,
	MANA_POTION
}

public enum FishState {
	IDLE,
	CHASE,
	CATCH,
	FLEE
}

public class FishCollectible : MonoBehaviour {
	public FishCollectibleType type;
	public FishState state;

	public GameObject targetBait;
	public FishingRod fishingRod;

	public float moveSpeed;
	public float timeToCatch;
}
