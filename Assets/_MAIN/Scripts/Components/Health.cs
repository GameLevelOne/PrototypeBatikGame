using UnityEngine;

public class Health : MonoBehaviour {
	public float guardReduceDamage;
	public float damage;

	Role role;
	Player player;
	Enemy enemy;

	[SerializeField] float healthPower;
	[SerializeField] float initialDamage;

	public float HealthPower {
		get {return healthPower;}
		set {
			if (healthPower == value) return;

			healthPower = value;
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
		if (col.GetComponent<Damage>() == null) return;
		
		damage = col.GetComponent<Damage>().damage;
		initialDamage = damage;

		if (role.gameRole == GameRole.Player) {
			if (player.IsPlayerDie) return;

			if (col.tag == Constants.Tag.ENEMY_ATTACK) {
				Enemy enemy = col.GetComponentInParent<Enemy>();
				player.enemyThatHitsPlayer = enemy;
				// enemy.IsHitAPlayer = true;

				player.IsPlayerHit = true;

				if (player.IsParrying || player.IsBulletTiming || player.IsSlowMotion || player.IsRapidSlashing) {
					Debug.Log ("Player ignored damage");
				} else if (player.IsGuarding) {
					player.IsPlayerGetHurt = true;
					damage *= guardReduceDamage;
				} else {
					player.IsPlayerGetHurt = true;
					damage = initialDamage;
				}

				// Invoke ("StopResponseHitPlayer", 0.5f);
			}
		} else if (role.gameRole == GameRole.Enemy) {
			if (enemy.IsEnemyDie) return;
			
			if (col.tag == Constants.Tag.PLAYER_ATTACK) {
				Player player = col.GetComponentInParent<Player>();
				enemy.playerThatHitsEnemy = player;
				player.IsHitAnEnemy = true;

				enemy.IsEnemyHit = true;
				
				enemy.IsEnemyGetHurt = true;
				damage = initialDamage;
			} else if (col.tag == Constants.Tag.PLAYER_COUNTER) {
				enemy.IsEnemyGetHurt = true;
				damage = initialDamage;
				Debug.Log("Enemy Is stunned");
			}
		}
	}

	// void StopResponseHitPlayer () {
	// 	player.IsPlayerHit = false;
	// }
}
