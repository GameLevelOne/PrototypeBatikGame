using UnityEngine;

public class GainTreasure : MonoBehaviour {
	public Player player;
	public Transform liftingTreasureParent;

	public Lootable lootable;
	public Transform lootableTransform;

	public bool isLooting = false;
	
	void OnTriggerEnter2D (Collider2D col) {
		PlayerState state = player.state;

		if (col.tag == Constants.Tag.LOOTABLE && !isLooting && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
			Lootable lootable = col.GetComponent<Lootable>();

			if (!lootable.isLooted) {
				lootable.isLooted = true;
				lootableTransform = lootable.transform;
				isLooting = true;
			}
		} 
	}
}
