using UnityEngine;

public class Health : MonoBehaviour {
	public bool isDead;
	public float guardReduceDamage;

	Role role;
	PlayerInput playerInput;

	[SerializeField] float currentHP;

	[SerializeField] bool currentIsEnemyHit = false;
	[SerializeField] bool currentIsPlayerHit = false;

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
			playerInput = GetComponent<PlayerInput>();
		}
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		if (isDead) return;

		if (col.GetComponent<Damage>() == null) return;
		
		float damage = col.GetComponent<Damage>().damage;

		if (role.gameRole == GameRole.Player) {
			if (col.tag == Constants.Tag.ENEMY_ATTACK) {
				Data.isPlayerHit = true;

				if (playerInput.isGuarding) {
					HP -= (damage * guardReduceDamage);
				} else {
					HP -= damage;
				}

				CheckHealth ();
			}
		} else { //ENEMy
			if (col.tag == Constants.Tag.PLAYER_ATTACK) {
				Data.isEnemyHit = true;
				HP -= damage;
				CheckHealth ();
			} else if (col.tag == Constants.Tag.PLAYER_COUNTER) {
				HP -= damage;
				Debug.Log("Enemy is stunned");
				CheckHealth ();
			}
		}
	}

	void CheckHealth () {
		// if (role.gameRole == GameRole.Player) {
		// 	Data.isPlayerHit = false;
		// } else { //ENEMy
		// 	Data.isEnemyHit = false;
		// }

		if (HP <= 0f) {
			isDead = true;
		}
	}

	// public bool isEnemyHit {
	// 	get {return currentIsEnemyHit;}
	// 	set {
	// 		if (currentIsEnemyHit == value) return;

	// 		currentIsEnemyHit = value;
	// 		Debug.Log("currentIsEnemyHit " + currentIsEnemyHit);
	// 	}
	// }

	// public bool isPlayerHit {
	// 	get {return currentIsEnemyHit;}
	// 	set {
	// 		if (currentIsPlayerHit == value) return;

	// 		currentIsPlayerHit = value;
	// 		Debug.Log("currentIsPlayerHit " + currentIsPlayerHit);
	// 	}
	// }
}
