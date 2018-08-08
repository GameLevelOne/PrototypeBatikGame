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

	void OnTriggerEnter2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.IsCanFishing = true;
		}
		
		if (col.tag == Constants.Tag.FISH) {
			if (player.state == PlayerState.FISHING && fishObj == null && fishCollectible == null) {
				fishCollectible = col.GetComponent<FishCollectible>();
				fishObj = fishCollectible.gameObject;
				fishType = fishCollectible.type;

				fishCollectible.targetBait = this.gameObject;
				fishCollectible.fishingRod = this;
				fishCollectible.state = FishState.CATCH; //TEMP => CHASE first before CATCH
			}
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.IsCanFishing = false;
		}
	}
}
