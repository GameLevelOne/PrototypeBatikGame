using UnityEngine;



public class Arrow : MonoBehaviour {
	public float speed;
	public Rigidbody2D rb;

	bool isShot = false;
	[SerializeField] bool isHit = false;
	
	public bool IsShot {
		get {return isShot;}
		set {
			if (isShot == value) return;

			isShot = value;
		}
	}
	
	public bool IsHit {
		get {return isHit;}
		set {
			if (isHit == isHit) return;

			isHit = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		Debug.Log("arrow hits something");
		//Check if hit something (by tag)
		isHit = true;
	}

	// void Awake () {
	// 	rb = GetComponent<Rigidbody2D>();
	// 	// role = GetComponent<Role>();

	// 	rb.AddForce (transform.right * speed * 50f);
	// }
}
