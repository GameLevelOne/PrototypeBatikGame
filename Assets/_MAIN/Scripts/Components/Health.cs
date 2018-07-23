using UnityEngine;

public class Health : MonoBehaviour {
	public bool isDead;
	public float guardReduceDamage;

	Role role;
	PlayerInput playerInput;

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
				Debug.Log("player got hit"); 
			}
		} else { //ENEMy
			if (col.tag == Constants.Tag.PLAYER_ATTACK) {
				Data.isEnemyHit = true;
				HP -= damage;
				CheckHealth ();
				Debug.Log("enemy got hit");
			}
		}
	}

	void CheckHealth () {
		if (HP <= 0f) {
			isDead = true;
		}
	}
}
