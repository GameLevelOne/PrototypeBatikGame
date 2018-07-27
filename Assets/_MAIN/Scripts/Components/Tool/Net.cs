using UnityEngine;

public class Net : MonoBehaviour {
	public Collectible collectibleObject;

	[SerializeField] bool isGotSomething = false;
	
	public bool IsGotSomething {
		get {return isGotSomething;}
		set {
			if (isGotSomething == value) return;

			isGotSomething = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Collectible>() != null) {
			Collectible collectible = col.GetComponent<Collectible>();
			collectibleObject = collectible;
			isGotSomething = true;
		}
	}
}
