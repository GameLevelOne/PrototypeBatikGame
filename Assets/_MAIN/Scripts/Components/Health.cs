using UnityEngine;

public class Health : MonoBehaviour {
	public bool isDead;

	Role role;

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
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		if (isDead) return;

		if (col.GetComponent<Damage>() == null) return;
		
		float damage = col.GetComponent<Damage>().damage;

		if (role.gameRole == GameRole.Player) {
			if (col.tag == Constants.Tag.ENEMY_ATTACK) {
				//
				HP -= damage;
				CheckHealth ();
				Debug.Log("player got hit"); 
			}
		} else { //ENEMy
			if (col.tag == Constants.Tag.PLAYER_ATTACK) {
				// Data.isEnemyHit = true;
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
