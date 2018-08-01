using UnityEngine;

public class PowerBracelet : MonoBehaviour {
	public Liftable liftableObject;
	public float liftPower;

	[SerializeField] bool isLiftSomething = false;
	
	public bool IsLiftSomething {
		get {return isLiftSomething;}
		set {
			if (isLiftSomething == value) return;

			isLiftSomething = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Liftable>() != null) {
			Liftable liftable = col.GetComponent<Liftable>();

			if (liftPower >= liftable.weight) {
				liftableObject = liftable;
				IsLiftSomething = true;	
			}
		}
	}
}
