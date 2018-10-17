using UnityEngine;

public class Arrow : MonoBehaviour {
	public Projectile projectile;
	public float manaCost = 0;
	// public float speed;
	// public Rigidbody rb;

	// public bool isShot = false;
	// public bool isHit = false;
	[HeaderAttribute("Current")]
	public Transform playerTransform;

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.BOUNDARIES) {
			projectile.isSelfDestroying = true;
			// isHit = true;
		}
	}
}
