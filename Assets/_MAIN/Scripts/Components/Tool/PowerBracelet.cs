using UnityEngine;

public enum PowerBraceletState {
	NONE,
	CAN_LIFT,
	CANNOT_LIFT,
	GRAB
}

public class PowerBracelet : MonoBehaviour {
	public PowerBraceletState state;
	public Liftable liftable;
	public Transform liftShadowParent;
	public Transform liftMainObjParent;
	public Collider2D collider;
	
	// public float liftPower;
	public float throwRange;
	public float speed;

	[SerializeField] bool isInteracting = false;
	[SerializeField] bool isColliderOn = false;
	
	public bool IsColliderOn {
		get {return isColliderOn;}
		set {
			if (isColliderOn == value) return;

			isColliderOn = value;
			collider.enabled = value;
		}
	}
	
	public bool IsInteracting {
		get {return isInteracting;}
		set {
			if (isInteracting == value) return;

			isInteracting = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null && !IsInteracting) {
			liftable = col.GetComponent<Liftable>();
			LiftableType type = liftable.liftableType;

			if (type == LiftableType.LIFTABLE) {
				Debug.Log("CAN_LIFT");
				state = PowerBraceletState.CAN_LIFT;
			} else if (type == LiftableType.UNLIFTABLE) {
				state = PowerBraceletState.CANNOT_LIFT;
				Debug.Log("CANNOT_LIFT");
			} else if (type == LiftableType.GRABABLE) {
				Debug.Log("GRABABLE");
				state = PowerBraceletState.GRAB;
			} else {
				state = PowerBraceletState.NONE;
			}
			
			IsInteracting = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null && IsInteracting) {

			if (liftable.gameObject == col.GetComponent<Liftable>().gameObject) {
				IsInteracting = false;
				state = PowerBraceletState.NONE;
			}
		}
	}
}
