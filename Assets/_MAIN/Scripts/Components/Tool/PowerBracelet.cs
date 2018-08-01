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
	public float liftPower;

	// [SerializeField] bool isInteracting = false;
	[SerializeField] float liftingMode = 0f;
	[SerializeField] int interactValue = 0;
	
	// public bool IsInteracting {
	// 	get {return isInteracting;}
	// 	set {
	// 		if (isInteracting == value) return;

	// 		isInteracting = value;
	// 	}
	// }

	//LIFTING -1, SWEATING 0, GRABBING 1
	public float LiftingMode {
		get {return liftingMode;}
		set {
			if (liftingMode == value) return;

			liftingMode = value;
		}
	}

	// READY 0, HOLD 1, ACT 2
	public int InteractValue {
		get {return interactValue;}
		set {
			if (interactValue == value) return;

			interactValue = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null) {
			Liftable liftable = col.GetComponent<Liftable>();
			LiftableType type = liftable.liftableType;

			if (type == LiftableType.LIFTABLE) {
				liftableObject = liftable;
				state = LiftState.CAN_LIFT;
			} else if (type == LiftableType.UNLIFTABLE) {
				state = LiftState.CANNOT_LIFT;
			} else if (type == LiftableType.GRABABLE) {
				state = LiftState.GRAB;
			}
		} else {
			state = LiftState.NONE;
		}
	}
}
