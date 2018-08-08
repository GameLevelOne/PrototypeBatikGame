using UnityEngine;

public enum FishingRodState {
	IDLE,
	THROW,
	STAY,
	RETURN,
}

public class FishingRod : MonoBehaviour {
	public FishingRodState state;
	public Player player;
	public FishCollectible fishCollectible;
	public FishCollectibleType fishType;

	public GameObject fishObj;
	public Collider2D baitCol;
	public float fishingRange;

	public bool isBaitLaunched = false;
	public bool isCatchSomething = false;
	public bool isBaitFish = false;

	FishState fishState;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.IsCanFishing = true;
		}
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.tag == Constants.Tag.FISH) {
			if (!isBaitLaunched || player.state != PlayerState.FISHING) return;

			if (fishCollectible == null) {
				fishCollectible = col.GetComponent<FishCollectible>();
			} else {
				fishState = fishCollectible.state;
				Debug.Log("Fish state : "+fishState+" , isBaitFish : "+isBaitFish);
				if ((fishState == FishState.IDLE || fishState == FishState.PATROL) && !isBaitFish) {
					Debug.Log("Get Fish");
					
					fishObj = fishCollectible.gameObject;
					fishType = fishCollectible.type;

					fishCollectible.fishingRod = this;
					fishCollectible.targetPos = this.transform.position;
					fishCollectible.state = FishState.CHASE;
					Debug.Log("Set Chase"); 

					isBaitFish = true;
				} else if (fishState == FishState.FLEE && isBaitFish) {
					Debug.Log("Flee");
					fishObj = null;
					fishCollectible.fishingRod = null;
					fishCollectible = null;
					isBaitFish = false;
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.IsCanFishing = false;
		}

		// if (col.tag == Constants.Tag.FISH) {
		// 	if (player.state == PlayerState.FISHING && fishCollectible == col.GetComponent<FishCollectible>()) {
		// 		fishObj = null;

		// 		fishCollectible.fishingRod = null;
		// 		fishCollectible = null;

		// 		isBaitFish = false;
		// 	}
		// }
	}
}
