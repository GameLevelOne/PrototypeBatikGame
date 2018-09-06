using UnityEngine;

public enum PowerBraceletState {
	NONE,
	CAN_LIFT,
	CANNOT_LIFT,
	GRAB
}

public class PowerBracelet : MonoBehaviour {
	public Player player;
	public float manaCost = 0;
	public float liftPower;
	public float standLiftPower;
	public float throwRange;
	public float speed;
	// public Transform liftShadowParent;
	public Transform liftMainObjParent;

	[HeaderAttribute("Current")]
	public PowerBraceletState state;
	public Liftable liftable;
	// public Collider collider;

	public bool isInteracting = false;
	// public bool isColliderOn = false;

	void OnTriggerEnter (Collider col) {
		if (col.GetComponent<Liftable>() != null && !isInteracting && liftable == null) {
			liftable = col.GetComponent<Liftable>();
			isInteracting = true;
		}
	}

	void OnTriggerExit (Collider col) {
		if (liftable != null && col.GetComponent<Liftable>() != null && player.state != PlayerState.POWER_BRACELET) {

			if (liftable.gameObject == col.GetComponent<Liftable>().gameObject) {
				isInteracting = false;
				liftable = null;
				SetState(PowerBraceletState.NONE);
			}
		}
	}

	public void SetState (PowerBraceletState state) {
		this.state = state;
	}
}
