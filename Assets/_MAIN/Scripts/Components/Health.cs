using UnityEngine;

public class Health : MonoBehaviour {
	public delegate void DamageCheck(Damage damage);
	public event DamageCheck OnDamageCheck;

	public delegate void HPChange ();
	public event HPChange OnHPChange;

	// public float guardReduceDamage;
	// public float damage;

	public Role role;
	public Player player;
	public Enemy enemy;
	public Jatayu jatayu;

	PlayerState playerState;
	EnemyState enemyState;

	public float healthPower;

	// public bool isTestHealth = false;

	void Awake () { //TEMP
		// if (isTestHealth) {
		// 	PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP, healthPower);
		// } 
	}


	public float EnemyHP { 
		get {return healthPower;}
		set {
			if (healthPower == value) return;

			healthPower = value;

			if (OnHPChange != null) {
				OnHPChange();
			}
		}
	}

	public float PlayerHP {
		get{
			//  // Debug.Log("get PlayerHP :"+PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP));
			return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP, healthPower);}
		set{
			if (healthPower == value) return;
			// float hpToSet = value;
			healthPower = value;

			//MAX 100 HP
			if (healthPower > player.MaxHP) healthPower = player.MaxHP;

			PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP, healthPower);
			
			//  // Debug.Log("set PlayerHP :"+PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP));
			if (OnHPChange != null) {
				OnHPChange();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{	
		if(other.GetComponent<Damage>() == null) return;
		if(OnDamageCheck != null) {
			OnDamageCheck(other.gameObject.GetComponent<Damage>());
		} 

		// if (role.gameRole == GameRole.Player) {
		// 	 // Debug.Log(other.name +" Have DAMAGE");
		// } else if (role.gameRole == GameRole.Enemy) {
		// }
	}
	
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<Damage>() == null) return;
		if(OnDamageCheck != null) {
			OnDamageCheck(other.gameObject.GetComponent<Damage>());
		} 
		//  // Debug.Log(other.gameObject.name +" Have DAMAGE");
	}

#region OLD
	// void onTriggerEnter2D (Collider2D col) {
		// if (col.GetComponent<Damage>() == null) return;
		
		// // damage = col.GetComponent<Damage>().damage;
		// // initialDamage = damage;

		// if (role.gameRole == GameRole.Player) {
		// 	playerState = player.state;

		// 	if (playerState == PlayerState.DIE) return;

		// 	if (col.tag == Constants.Tag.ENEMY_ATTACK) {
		// 		Enemy enemy = col.GetComponentInParent<Enemy>();
		// 		player.enemyThatHitsPlayer = enemy;
		// 		// enemy.isHitAPlayer = true;

		// 		player.isPlayerHit = true;

		// 		if (player.isParrying || player.isBulletTiming || (playerState == PlayerState.SLOW_MOTION) || (playerState == PlayerState.RAPID_SLASH)) {
		// 			 // Debug.Log ("Player ignored damage");
		// 		} else if (player.isGuarding) {
		// 			player.isPlayerGetHurt = true;
		// 			// damage *= guardReduceDamage;
		// 		} else {
		// 			player.isPlayerGetHurt = true;
		// 			// damage = initialDamage;
		// 		}

		// 		// Invoke ("StopResponseHitPlayer", 0.5f);
		// 	}
		// } else if (role.gameRole == GameRole.Enemy) {
		// 	enemyState = enemy.state;

		// 	if (enemyState == EnemyState.Die) return;
			
		// 	if (col.tag == Constants.Tag.PLAYER_COUNTER || col.tag == Constants.Tag.HAMMER || col.tag == Constants.Tag.BOW || col.tag == Constants.Tag.MAGIC_MEDALLION || col.tag == Constants.Tag.PLAYER_SLASH) {
		// 		Player player = col.GetComponentInParent<Player>();
		// 		enemy.playerThatHitsEnemy = player;

		// 		if (col.tag == Constants.Tag.PLAYER_SLASH) {
		// 			player.isHitAnEnemy = true;
		// 		}
				
		// 		// enemy.isEnemyHit = true;
		// 		// enemy.isEnemyGetHurt = true;
		// 		// damage = initialDamage;
		// 	} else {
		// 		 // Debug.Log("Enemy get damaged from other source");
		// 	}
		// }
	// }

	// void OnCollisionEnter2D (Collision2D col) {
		// if (col.gameObject.GetComponent<Damage>() == null) return;
		
		// // damage = col.gameObject.GetComponent<Damage>().damage;
		// // initialDamage = damage;

		// if (col.gameObject.tag == Constants.Tag.PLAYER) {
		// 	Player player = col.gameObject.GetComponent<Player>();

		// 	if (player.state == PlayerState.DASH) {
		// 		enemy.playerThatHitsEnemy = player;
		// 		player.isHitAnEnemy = true;

		// 		// enemy.isEnemyHit = true;
		// 		// enemy.isEnemyGetHurt = true;
		// 		// damage = initialDamage;
		// 		 // Debug.Log("Enemy get damaged from player dash");
		// 	}
		// }
	// }
#endregion
}
