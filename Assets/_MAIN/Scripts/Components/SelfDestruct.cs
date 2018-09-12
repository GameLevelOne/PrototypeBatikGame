using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	public Projectile projectile;

	public void DestroyMe () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}
}
