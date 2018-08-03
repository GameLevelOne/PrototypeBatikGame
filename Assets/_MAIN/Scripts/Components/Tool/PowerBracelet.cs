using UnityEngine;

public enum LiftState {
	NONE,
	CAN_LIFT,
	CANNOT_LIFT,
	GRAB
}

public class PowerBracelet : MonoBehaviour {
	public LiftState state;
	public LiftableType type;
	public Transform liftParent;
	public Collider2D collider;
	public Transform liftableTransform;
	public Rigidbody2D liftableRigidbody;
	public Collider2D liftableCollider;
	
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
			Liftable liftable = col.GetComponent<Liftable>();

			liftableTransform = liftable.transform;
			type = liftable.liftableType;
			liftableRigidbody = col.GetComponent<Rigidbody2D>();
			liftableCollider = col;
			IsInteracting = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null && IsInteracting) {
			Liftable liftable = col.GetComponent<Liftable>();

			if (liftable.transform == liftableTransform) {
				liftableTransform = null;
				liftableRigidbody = null;
				liftableCollider = null;
				IsInteracting = false;
				state = LiftState.NONE;
			}
		}
	}
}
