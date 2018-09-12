using UnityEngine;

public class Arrow : MonoBehaviour {
	public Projectile Projectile;
	public float manaCost = 0;
	// public float speed;
	// public Rigidbody rb;

	// public bool isShot = false;
	// public bool isHit = false;

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.ENEMY || col.tag == Constants.Tag.STONE || col.tag == Constants.Tag.BOUNDARIES) {
			Projectile.isSelfDestroying = true;
			// isHit = true;
		}
	}
}
