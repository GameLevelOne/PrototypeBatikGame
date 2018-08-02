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
