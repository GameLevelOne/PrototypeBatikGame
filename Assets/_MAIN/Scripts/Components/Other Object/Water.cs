using UnityEngine;

public class Water : MonoBehaviour {
	public Collider waterBoundariesCol;
	public Collider waterMainCol;
	public Player player;
	
	// [SerializeField] bool isWaitingPlayerSwim = false;

	// public bool IsWaitingPlayerSwim {
	// 	get {return isWaitingPlayerSwim;}
	// 	set {
	// 		if (isWaitingPlayerSwim == value) return;

	// 		isWaitingPlayerSwim = value;
	// 	}
	// }

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.PLAYER) {
			player = col.GetComponent<Player>();
			// IsWaitingPlayerSwim = true;
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.PLAYER) {
			player = null;
			// IsWaitingPlayerSwim = false;
		}
	}
}
