using UnityEngine;

public enum EnemyState{
	Idle,
	Patrol,
	Chase,
	Startled,
	Attack,
	Damaged,
	Die
}

public class Enemy : MonoBehaviour {
	[HeaderAttribute("Enemy Attributes")]
	public EnemyState state;
	public GameObject attackObject;
	[SerializeField] protected bool isEnemyHit = false;
	[SerializeField] protected bool isEnemyGetHurt = false;

	[SpaceAttribute(10f)]
	public float patrolRange;
	public float chaseRange;

	public float patrolSpeed;
	public float chaseSpeed;

	public float idleDuration;
	
	[HeaderAttribute("Current")]
	public Player playerThatHitsEnemy;
	public Transform playerTransform;
	public Vector2 patrolDestination;

	[SpaceAttribute(10f)]
	public bool initIdle = false;
	public bool initPatrol = false;
	public bool initAttack = false;
	public bool isAttack = false;
	public bool attackHit = false;
	public bool hasArmor = false;

	protected float tIdle;
	public float TIdle{
		get{return tIdle;}
		set{tIdle = value;}
	}

	public bool IsEnemyHit {
		get {return isEnemyHit;}
		set {
			if (isEnemyHit == value) return;

			isEnemyHit = value;
		}
	}

	public bool IsEnemyGetHurt {
		get 
		{
			if(hasArmor) return false;
			else return isEnemyGetHurt;
		}
		set 
		{
			if (isEnemyGetHurt == value) return;
			isEnemyGetHurt = value;
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