using UnityEngine;

public enum Role {
	Enemy,
	Player
}

public class Health : MonoBehaviour {
	public bool isDead;
	public Role role;

	[SerializeField] float currentHP;

	public float HP {
		get {return currentHP;}
		set {
			if (currentHP == value) return;

			currentHP = value;
		}
	}
	
	void OnCollisionEnter2D (Collision2D col) {
		if (isDead) return;

		GameObject other = col.gameObject;
		float damage = other.GetComponent<Damage>().damage;

		#if Role == Player
		Debug.Log("player got hit"); 
		if (other.tag == "Enemy Attack") {
			HP -= damage;
			CheckHealth ();
		}
		#else 
		Debug.log("enemy got hit");
		if (other.tag == "Player Attack") {
			HP -= damage;
			CheckHealth ();
		}
		#endif
	}

	void CheckHealth () {
		if (HP <= 0f) {
			isDead = true;
		}
	}
}
