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

	public Collider2D col;
	// public float liftPower;

	[SerializeField] bool isInteracting = false;
	[SerializeField] bool isColliderOn = false;
	
	public bool IsColliderOn {
		get {return isColliderOn;}
		set {
			if (isColliderOn == value) return;

			isColliderOn = value;
			col.enabled = value;
			Debug.Log("isColliderOn " + isColliderOn);
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
		if (col.GetComponent<Liftable>() != null) {
			liftableObject = col.GetComponent<Liftable>();
			type = liftableObject.liftableType;
			IsInteracting = true;
		}
	}
}
