using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	public Projectile projectile;

	public void DestroyMe () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}

	public void DestroyBigSummonFX () {
		// Destroy(gameObject);
		Time.timeScale = 1;
		projectile.isSelfDestroying = true;
	}
}
