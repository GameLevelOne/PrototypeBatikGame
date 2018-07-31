using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour {
	public GameObject diggingObj;
	// public Vector3 diggingCheckPos;
	public List<bool> listDig = new List<bool>();

	// [SerializeField] bool isNotCleanForDigging = false;

	// public bool IsNotCleanForDigging {
	// 	get {return isNotCleanForDigging;}
	// 	set {
	// 		if (isNotCleanForDigging == value) return;

	// 		isNotCleanForDigging = value;
	// 	}
	// }

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == Constants.Tag.DIG_RESULT) {
			// IsNotCleanForDigging = true;
			listDig.Add(true);
		} else {
			listDig.Add(false);
		}
	}
}
