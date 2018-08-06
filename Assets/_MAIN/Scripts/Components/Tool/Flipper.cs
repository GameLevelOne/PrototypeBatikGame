using UnityEngine;

public class Flipper : MonoBehaviour {
	// public Collider2D waterCol;
	// public Player player;
	
	[SerializeField] bool isEquipped = false;

	public bool IsEquipped {
		get {return isEquipped;}
		set {
			if (isEquipped == value) return;

			isEquipped = value;
		}
	}

	// void OnTriggerStay2D (Collider2D col) {
	// 	if (col.tag == Constants.Tag.SWIM_AREA && IsEquipped) {
	// 		player.IsCanSwim = true;
	// 	}
	// }

	// void OnTriggerExit2D (Collider2D col) {
	// 	if (col.tag == Constants.Tag.SWIM_AREA) {
	// 		player.IsCanSwim = false;
	// 	}
	// }
}
