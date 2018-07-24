using UnityEngine;

public class MinatoKunai : MonoBehaviour {
	public float speed;

	Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();

		rb.AddForce (transform.right * speed * 50f);
	}

	void OnTriggerExit2D (Collider2D col) {
		Debug.Log("Teleport");
		rb.velocity = Vector2.zero;
	}
}
