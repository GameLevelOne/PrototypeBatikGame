using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed;

	Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();

		rb.AddForce (transform.right * speed * 50f);
	}
}
