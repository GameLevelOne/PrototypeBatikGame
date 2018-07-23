using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed;
	// public Player player;
	// public Enemy enemy;

	// Role role;
	Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		// role = GetComponent<Role>();

		rb.AddForce (transform.right * speed * 50f);
	}

	// void OnTrigerEnter2D (Collider2D col) {
	// 	if (role.gameRole == GameRole.Player) {
	// 		if (col.tag == Constants.Tag.ENEMY) {
	// 			player.isHitAnEnemy = true;

	// 			enemy = col.GetComponent<Enemy>();
	// 			enemy.isEnemyHit = true;
	// 		}
	// 	} else if (role.gameRole == GameRole.Enemy) {
	// 		if (col.tag == Constants.Tag.PLAYER) {
	// 			// enemy.isHitAnEnemy = true;

	// 			player = col.GetComponent<Player>();
	// 			player.isPlayerHit = true;
	// 		}
	// 	}
	// }
}
