using UnityEngine;

public class Health : MonoBehaviour {
	public bool isDead;
	public float guardReduceDamage;

	Role role;
	Player player;
	Enemy enemy;

	[SerializeField] float currentHP;

	public float HP {
		get {return currentHP;}
		set {
			if (currentHP == value) return;

			currentHP = value;
		}
	}

	void Awake () {
		role = GetComponent<Role>();

		if (role.gameRole == GameRole.Player) {
			player = GetComponent<Player>();
		} else if (role.gameRole == GameRole.Enemy) {
			enemy = GetComponent<Enemy>();
		} else {
			Debug.Log("Unknown game object");
		}
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		if (isDead) return;

		if (col.GetComponent<Damage>() == null) return;
		
		float damage = col.GetComponent<Damage>().damage;

		if (role.gameRole == GameRole.Player) {
			if (col.tag == Constants.Tag.ENEMY_ATTACK) {
				Enemy enemy = col.GetComponentInParent<Enemy>();
				player.enemyThatHitsPlayer = enemy;
				// enemy.isHitAPlayer = true;

				player.isPlayerHit = true;

				if (player.isParrying || player.isBulletTiming || player.isSlowMotion || player.isRapidSlashing) {
					Debug.Log ("Player ignored damage");
				} else if (player.isGuarding) {
					ReduceHealth (damage * guardReduceDamage);
				} else {
					ReduceHealth (damage);
				}

				// Invoke ("StopResponseHitPlayer", 0.5f);
			}
		} else if (role.gameRole == GameRole.Enemy) {
			if (col.tag == Constants.Tag.PLAYER_ATTACK) {
				Player player = col.GetComponentInParent<Player>();
				enemy.playerThatHitsEnemy = player;
				player.isHitAnEnemy = true;

				enemy.isEnemyHit = true;

				ReduceHealth (damage);
			} else if (col.tag == Constants.Tag.PLAYER_COUNTER) {
				ReduceHealth (damage);
				Debug.Log("Enemy is stunned");
			}
		}
	}

	void ReduceHealth (float value) {
		HP -= value;

		if (HP <= 0f) {
			isDead = true;
		}
	}

	// void StopResponseHitPlayer () {
	// 	player.isPlayerHit = false;
	// }
}
