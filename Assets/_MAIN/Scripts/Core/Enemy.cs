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
	public GameObject chaseIndicator; //TEMP
	public Health health;

	[SpaceAttribute(10f)]
	public float spawnItemProbability;
	public float patrolRange;
	public float chaseRange;

	public float patrolSpeed;
	public float chaseSpeed;

	public float idleDuration;
	public float damagedDuration;
	public float dieDuration;

	[HeaderAttribute("Current")]
	public Player playerThatHitsEnemy;
	public Transform playerTransform;
	public Vector3 patrolDestination;

	[SpaceAttribute(10f)]
	public bool initIdle = false;
	public bool initPatrol = false;
	public bool initAttack = false;
	public bool isAttack = false;
	public bool initDamaged = false;
	public bool initDie = false;
	public bool attackHit = false;
	public bool hasArmor = false;
	
	public bool isHit = false;
	public Damage damageReceive;

	protected float tIdle;
	public float TIdle{
		get{return tIdle;}
		set{tIdle = value;}
	}

	protected float tDamaged;
	public float TDamaged{
		get{return tDamaged;}
		set{tDamaged = value;}
	}

	protected float tDie;
	public float TDie{
		get{return tDie;}
		set{tDie = value;}
	}


	#region delegate event
	void OnEnable()
	{
		health.OnDamageCheck += DamageCheck;
	}

	void OnDisable()
	{
		health.OnDamageCheck -= DamageCheck;
	}

	void DamageCheck(Damage damage)
	{
		damageReceive = damage;
		isHit = true;
	}
	#endregion

#region ENEMY STATE 
	// public void SetEnemyState (EnemyState enemyState) {
	// 	state = enemyState;
	// }

	// public void SetEnemyIdle () {
	// 	state = EnemyState.Idle;
	// }
#endregion
}