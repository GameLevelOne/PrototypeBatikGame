using UnityEngine;

public class Arrow : MonoBehaviour {
	public float speed;
	public Rigidbody2D rb;

	public bool isShot = false;
	public bool isHit = false;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == Constants.Tag.ENEMY || col.tag == Constants.Tag.STONE) {
			isHit = true;
		}
	}
}
