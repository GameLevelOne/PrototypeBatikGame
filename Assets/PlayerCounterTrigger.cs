using UnityEngine;

public class PlayerCounterTrigger : MonoBehaviour {
	public delegate void CounterTrigger();
	public event CounterTrigger OnCounterTrigger;

	public float minDiagonalThreshold = 0f;

	[HeaderAttribute("Current")]
	public Player player;

	void OnTriggerEnter (Collider other) {
		if (other.GetComponent<Damage>() != null) {
			DamageCharacteristic damageChar = other.GetComponent<Damage>().damageChar;

			if (damageChar == DamageCharacteristic.COUNTERABLE || damageChar == DamageCharacteristic.COUNTER_AND_PARRYABLE) {
				Enemy currEnemy = other.GetComponentInParent<Enemy>();

				if (CheckCounterDir(currEnemy.transform.position, player.transform.position)) {
					SetEnemyIdle(currEnemy);

					if (OnCounterTrigger != null) {
						OnCounterTrigger();
					}
				}
			}
		}
	}	

	bool CheckCounterDir (Vector3 currEnemyPos, Vector3 playerPos) {
		Vector3 deltaPos = (currEnemyPos - playerPos).normalized;
		// Debug.Log("enemyPos "+currEnemyPos);
		// Debug.Log("playerPos "+playerPos);
		// Debug.Log("normalized " +deltaPos);

		switch (player.counterDir) {
			case 1: 
				// Debug.Log("1 DOWN");
				if ((deltaPos.z > 0f && deltaPos.z <= 1f) && (deltaPos.x >= -minDiagonalThreshold && deltaPos.x <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 2: 
				// Debug.Log("2 LEFT");
				if ((deltaPos.x > 0f &&deltaPos.x <= 1f) && (deltaPos.z >= -minDiagonalThreshold && deltaPos.z <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 3: 
				// Debug.Log("3 UP");
				if ((deltaPos.z < 0f && deltaPos.z >= -1f) && (deltaPos.x >= -minDiagonalThreshold && deltaPos.x <= minDiagonalThreshold)) {
					return true;
				} else return false;
			case 4: 
				// Debug.Log("4 RIGHT");
				if ((deltaPos.x < 0f && deltaPos.x >= -1f) && (deltaPos.z >= -minDiagonalThreshold && deltaPos.z <= minDiagonalThreshold)) {
					return true;
				} else return false;
			default:
				return false;
		}
	}

	void SetEnemyIdle (Enemy currEnemy) {
		currEnemy.initAttack = false;
		currEnemy.isAttack = false;
		currEnemy.attackHit = false;
		currEnemy.SetEnemyState(EnemyState.Idle);
		currEnemy.attackObject.SetActive(false);
	}
}
