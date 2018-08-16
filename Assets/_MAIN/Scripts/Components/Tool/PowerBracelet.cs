using UnityEngine;

public enum PowerBraceletState {
	NONE,
	CAN_LIFT,
	CANNOT_LIFT,
	GRAB
}

public class PowerBracelet : MonoBehaviour {
	public int manaCost = 0;
	public PowerBraceletState state;
	public Liftable liftable;
	public Transform liftShadowParent;
	public Transform liftMainObjParent;
	public Collider2D collider;
	
	// public float liftPower;
	public float throwRange;
	public float speed;

	public bool isInteracting = false;
	public bool isColliderOn = false;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null && !isInteracting && liftable == null) {
			liftable = col.GetComponent<Liftable>();
			LiftableType type = liftable.liftableType;

			if (type == LiftableType.LIFTABLE) {
				// Debug.Log("CAN_LIFT");
				state = PowerBraceletState.CAN_LIFT;
			} else if (type == LiftableType.UNLIFTABLE) {
				state = PowerBraceletState.CANNOT_LIFT;
				// Debug.Log("CANNOT_LIFT");
			} else if (type == LiftableType.GRABABLE) {
				// Debug.Log("GRABABLE");
				state = PowerBraceletState.GRAB;
			} else {
				state = PowerBraceletState.NONE;
			}

			isInteracting = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null) {

			if (liftable.gameObject == col.GetComponent<Liftable>().gameObject) {
				liftable = null;
				isInteracting = false;
				state = PowerBraceletState.NONE;
			}
		}
	}
}
