using UnityEngine;

public enum EnemyState{
	Idle,
	Patrol,
	Chase,
	Attack,
	Damaged,
	Die
}

public class Enemy : MonoBehaviour {
	public Player playerThatHitsEnemy;
	public EnemyState state;

	[SerializeField] bool isEnemyHit = false;

	public bool IsEnemyHit {
		get {return isEnemyHit;}
		set {
			if (isEnemyHit == value) return;

			isEnemyHit = value;
		}
	}
	
	#region ENEMY STATE 
	public void SetEnemyState (EnemyState enemyState) {
		state = enemyState;
	}

	public void SetEnemyIdle () {
		state = EnemyState.Idle;
	}
	#endregion
}