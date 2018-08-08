using UnityEngine;

public class Net : MonoBehaviour {
	public Lootable lootableObj;

	[SerializeField] bool isGotSomething = false;
	
	public bool IsGotSomething {
		get {return isGotSomething;}
		set {
			if (isGotSomething == value) return;

			isGotSomething = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Lootable>() != null) {
			Lootable lootable = col.GetComponent<Lootable>();
			lootableObj = lootable;
			isGotSomething = true;
		}
	}
}
