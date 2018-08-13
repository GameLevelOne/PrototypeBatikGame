using UnityEngine;

public class Health : MonoBehaviour {
	public delegate void DamageCheck(Damage damage);
	public event DamageCheck OnDamageCheck;

	public float guardReduceDamage;
	public float damage;

	public Role role;
	public Player player;
	public Enemy enemy;

	PlayerState playerState;
	EnemyState enemyState;

	[SerializeField] float healthPower;
	[SerializeField] float initialDamage;

	public float HealthPower {
		get {return healthPower;}
		set {
			if (healthPower == value) return;

			healthPower = value;
		}
	}

	void onTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<Damage>() == null) return;
		if(OnDamageCheck != null) OnDamageCheck(other.GetComponent<Damage>());
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Damage>() == null) return;
		
		damage = col.GetComponent<Damage>().damage;
		initialDamage = damage;

		if (role.gameRole == GameRole.Player) {
			playerState = player.state;

			if (playerState == PlayerState.DIE) return;

			if (col.tag == Constants.Tag.ENEMY_ATTACK) {
				Enemy enemy = col.GetComponentInParent<Enemy>();
				player.enemyThatHitsPlayer = enemy;
				// enemy.IsHitAPlayer = true;

				player.IsPlayerHit = true;

				if (player.isParrying || player.IsBulletTiming || (playerState == PlayerState.SLOW_MOTION) || (playerState == PlayerState.RAPID_SLASH)) {
					Debug.Log ("Player ignored damage");
				} else if (player.isGuarding) {
					player.IsPlayerGetHurt = true;
					damage *= guardReduceDamage;
				} else {
					player.IsPlayerGetHurt = true;
					damage = initialDamage;
				}

				// Invoke ("StopResponseHitPlayer", 0.5f);
			}
		} else if (role.gameRole == GameRole.Enemy) {
			enemyState = enemy.state;

			if (enemyState == EnemyState.Die) return;
			
			if (col.tag == Constants.Tag.PLAYER_ATTACK || col.tag == Constants.Tag.PLAYER_COUNTER || col.tag == Constants.Tag.HAMMER || col.tag == Constants.Tag.BOW || col.tag == Constants.Tag.MAGIC_MEDALLION) {
				Player player = col.GetComponentInParent<Player>();
				enemy.playerThatHitsEnemy = player;
				player.IsHitAnEnemy = true;

				enemy.IsEnemyHit = true;
				enemy.IsEnemyGetHurt = true;
				damage = initialDamage;
			} else {
				Debug.Log("Enemy get damaged from other source");
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.GetComponent<Damage>() == null) return;
		
		damage = col.gameObject.GetComponent<Damage>().damage;
		initialDamage = damage;

		if (col.gameObject.tag == Constants.Tag.PLAYER) {
			Player player = col.gameObject.GetComponent<Player>();

			if (player.state == PlayerState.DASH) {
				enemy.playerThatHitsEnemy = player;
				player.IsHitAnEnemy = true;

				enemy.IsEnemyHit = true;
				enemy.IsEnemyGetHurt = true;
				damage = initialDamage;
				Debug.Log("Enemy get damaged from player dash");
			}
		}
	}
}
