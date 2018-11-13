using UnityEngine;

public class PlayerParryTrigger : MonoBehaviour {
	public delegate void ParryTrigger();
	public event ParryTrigger OnParryTrigger;

	public float minDiagonalThreshold = 0f;

	[HeaderAttribute("Current")]
	public Player player;

	void OnTriggerEnter (Collider other) {
		if (other.GetComponent<Damage>() != null) {
			DamageCharacteristic damageChar = other.GetComponent<Damage>().damageChar;
            // Debug.Log("OnTriggerEnter");

			if (damageChar == DamageCharacteristic.PARRYABLE || damageChar == DamageCharacteristic.COUNTER_AND_PARRYABLE) {
                // Debug.Log("PARRYABLE");
                if (other.GetComponentInParent<Enemy>() != null) {
                    Enemy currEnemy = other.GetComponentInParent<Enemy>();

                    if (CheckParryDir(currEnemy.transform.position, player.transform.position)) {
                        SetEnemyStun(currEnemy);

                        if (OnParryTrigger != null) {
                            OnParryTrigger();
                        }
                    }
                } else {
                    // Debug.Log("PARRY without parent");
                    if (CheckParryDir(other.transform.position, player.transform.position)) {

                        if (OnParryTrigger != null) {
                            OnParryTrigger();
                        }
                    }
                }
			}
		}
	}	

	bool CheckParryDir (Vector3 currEnemyPos, Vector3 playerPos) {
		Vector3 deltaPos = (currEnemyPos - playerPos).normalized;
		//  Debug.Log("enemyPos "+currEnemyPos);
		//  Debug.Log("playerPos "+playerPos);
		//  Debug.Log("normalized " +deltaPos);

		switch (player.parryDir) {
			case 1: 
				//  // Debug.Log("1 DOWN");
				if ((deltaPos.z < 0f && deltaPos.z >= -1f) && (deltaPos.x >= -minDiagonalThreshold && deltaPos.x <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 2: 
				//  // Debug.Log("2 LEFT");
				if ((deltaPos.x < 0f && deltaPos.x >= -1f) && (deltaPos.z >= -minDiagonalThreshold && deltaPos.z <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 3: 
				//  // Debug.Log("3 UP");
				if ((deltaPos.z > 0f && deltaPos.z <= 1f) && (deltaPos.x >= -minDiagonalThreshold && deltaPos.x <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 4: 
				//  // Debug.Log("4 RIGHT");
				if ((deltaPos.x > 0f &&deltaPos.x <= 1f) && (deltaPos.z >= -minDiagonalThreshold && deltaPos.z <= minDiagonalThreshold)) {
					return true;
				} else return false;
			default:
				return false;
		}
	}

	void SetEnemyStun (Enemy currEnemy) {
		// currEnemy.initAttack = false;
		// currEnemy.isAttack = false;
		currEnemy.attackHit = false;
		currEnemy.initDamaged = false;
		currEnemy.SetEnemyState(EnemyState.Stun);
		currEnemy.attackObject.SetActive(false);
	}
}
