using UnityEngine;

public class Arrow : MonoBehaviour {
	public float speed;
	public Rigidbody2D rb;

	bool isShot = false;
	
	public bool IsShot {
		get {return isShot;}
		set {
			if (isShot == value) return;

			isShot = value;
		}
	}

	// void Awake () {
	// 	rb = GetComponent<Rigidbody2D>();
	// 	// role = GetComponent<Role>();

	// 	rb.AddForce (transform.right * speed * 50f);
	// }
}
