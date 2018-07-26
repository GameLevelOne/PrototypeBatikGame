using UnityEngine;

public class Net : MonoBehaviour {
	public GameObject gottenObject;

	[SerializeField] bool isGotSomething = false;
	
	public bool IsGotSomething {
		get {return isGotSomething;}
		set {
			if (isGotSomething == value) return;

			isGotSomething = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		Debug.Log("Net got something");
		//Check if hit something (by tag)
		gottenObject = col.gameObject;

		isGotSomething = true;
	}
}
