using UnityEngine;

public class ChestOpener : MonoBehaviour {
	public Player player;
	public Chest chest;

	// public Collider2D chestOpenerCol;

	public bool isInteracting = false;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.CHEST && player.isHitChestObject) {
			chest = col.GetComponent<Chest>();

			if (!chest.isOpened && !chest.isSelected) {
				chest.isSelected = true;
				// chestOpenerCol.enabled = false;

				isInteracting = true;
			}
		} 
		// else {
		// 	chestOpenerCol.enabled = true;
		// }
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.CHEST) {
			Chest newChest = col.GetComponent<Chest>();
			if (chest == newChest && !newChest.isOpened && newChest.isSelected) {
				chest.isSelected = false;
				chest = null;
				isInteracting = false;
			}
		}
	}
}
