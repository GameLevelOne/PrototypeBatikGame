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
	
	void OnCollisionEnter2D (Collision2D col) {
		if (isDead) return;

		GameObject other = col.gameObject;
		float damage = other.GetComponent<Damage>().damage;

		if (role.gameRole == GameRole.Player) {
			Debug.Log("player got hit"); 
			if (other.tag == "Enemy Attack") {
				HP -= damage;
				CheckHealth ();
			}
		} else { //ENEMy
			Debug.Log("enemy got hit");
			if (other.tag == "Player Attack") {
				HP -= damage;
				CheckHealth ();
			}
		}
	}

	void CheckHealth () {
		if (HP <= 0f) {
			isDead = true;
		}
	}
}
