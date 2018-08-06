using UnityEngine;

public class Water : MonoBehaviour {
	public Collider2D waterBoundariesCol;
	public Collider2D waterMainCol;
	public Player player;
	
	// [SerializeField] bool isWaitingPlayerSwim = false;

	// public bool IsWaitingPlayerSwim {
	// 	get {return isWaitingPlayerSwim;}
	// 	set {
	// 		if (isWaitingPlayerSwim == value) return;

	// 		isWaitingPlayerSwim = value;
	// 	}
	// }

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == Constants.Tag.PLAYER) {
			player = col.GetComponent<Player>();
			// IsWaitingPlayerSwim = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == Constants.Tag.PLAYER) {
			player = null;
			// IsWaitingPlayerSwim = false;
		}
	}
}
