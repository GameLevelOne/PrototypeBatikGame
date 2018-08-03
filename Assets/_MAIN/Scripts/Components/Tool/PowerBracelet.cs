using UnityEngine;

public enum LiftState {
	NONE,
	CAN_LIFT,
	CANNOT_LIFT,
	GRAB
}

public class PowerBracelet : MonoBehaviour {
	public LiftState state;
	public Liftable liftableObject;
	public LiftableType type;

	public Rigidbody2D rigidbody;
	public Collider2D collider;
	// public float liftPower;

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
			liftableObject = col.GetComponent<Liftable>();
			rigidbody = col.GetComponent<Rigidbody2D>();
			type = liftableObject.liftableType;
			IsInteracting = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Liftable>() == null) return;

		if (col.GetComponent<Liftable>() == liftableObject && IsInteracting) {
			liftableObject = null;
			rigidbody = null;
			IsInteracting = false;
			state = LiftState.NONE;
		}
	}
}
