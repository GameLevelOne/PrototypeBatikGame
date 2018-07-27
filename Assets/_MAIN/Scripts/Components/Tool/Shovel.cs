using UnityEngine;

public class Shovel : MonoBehaviour {
	public GameObject diggingCheckerObj;
	// public Vector3 diggingCheckPos;
	
	[SerializeField] bool isDiggingOnDigArea = false;

	public bool IsDiggingOnDigArea {
		get {return isDiggingOnDigArea;}
		set {
			if (isDiggingOnDigArea == value) return;

			isDiggingOnDigArea = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.DIG_AREA) {
			Debug.Log("Can Digging");
			
			IsDiggingOnDigArea = true;
		}
	}
}
