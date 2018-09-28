using UnityEngine;

public class WaterShooterBullet : MonoBehaviour {
	public Projectile projectile;
	// public Vector3 direction;
	// public float speed;
	// public bool init = false;
	// public bool destroyed = false;

	// void OnCollisionEnter (Collision other)
	// {
	// 	if(other.gameObject.GetComponent<WaterShooterEnemy>() == null) Projectile.isCollideSomething = true;
	// }

	void OnTriggerEnter (Collider col) {
		if(col.tag == Constants.Tag.BOUNDARIES) {
			// Debug.Log("null "+col.name);
			projectile.isSelfDestroying = true;
		}
	}
}
