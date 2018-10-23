using UnityEngine;

public class DigChecker : MonoBehaviour {
	// public GameObject diggingResultObj;
	// // public Vector3 diggingResultPos;
	
	// [SerializeField] bool isCleanForDigging = false;

	// public bool IsCleanForDigging {
	// 	get {return isCleanForDigging;}
	// 	set {
	// 		if (isCleanForDigging == value) return;

	// 		isCleanForDigging = value;
	// 	}
	// }

	// void OnTriggerEnter2D (Collider2D col) {
		
	// 	if (col.tag != Constants.Tag.DIG_RESULT) {
	// 		 // Debug.Log("Can Result Digging");
			
	// 		IsCleanForDigging = true;
	// 	}
	// }
	public Player player;
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.DIG_AREA) {
			player.isCanDigging = true;
		} 
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.DIG_AREA) {
			player.isCanDigging = false;
		}
	}
}
