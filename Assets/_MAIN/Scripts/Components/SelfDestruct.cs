using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	public Projectile projectile;

	public void DestroyMe () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}

	public void EndBigSummonFX () {
		Time.timeScale = 1;
	}

	public void DestroyBigSummonFX () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}
}
