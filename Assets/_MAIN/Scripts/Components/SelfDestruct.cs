using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	[HeaderAttribute("If Using Projectile Only !")]
	public Projectile projectile;

	public void DestroyMe () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}
	public void InstaDestroyMe () {
		Destroy(gameObject);
	}

	public void EndBigSummonFX () {
		Time.timeScale = 1;
	}

	public void DestroyBigSummonFX () {
		// Destroy(gameObject);
		projectile.isSelfDestroying = true;
	}
}
