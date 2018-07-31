﻿using UnityEngine;

public enum EnemyState{
	Idle,
	Patrol,
	Chase,
	Attack,
	Damaged,
	Die,
	GET_HURT,
}

public class Enemy : MonoBehaviour {
	public Player playerThatHitsEnemy;
	public EnemyState state;
	public GameObject targetPlayer;

	public float idleDuration = 3f;
	public float t = 0;

	public bool initIdle = false;
	public bool initPatrol = false;

	[SerializeField] bool isEnemyHit = false;
    // [SerializeField] bool isEnemyGetHurt = false;
	// [SerializeField] bool isEnemyDie = false;

	public bool IsEnemyHit {
		get {return isEnemyHit;}
		set {
			if (isEnemyHit == value) return;

			isEnemyHit = value;
		}
	}

	// public bool IsEnemyGetHurt {
	// 	get {return isEnemyGetHurt;}
	// 	set {
	// 		if (isEnemyGetHurt == value) return;

	// 		isEnemyGetHurt = value;
	// 	}
	// }

	// public bool IsEnemyDie{
	// 	get{return isEnemyDie;}
	// 	set{
	// 		if(isEnemyDie == value) return;
	// 		isEnemyDie = value;
	// 	}
	// }
	
	#region ENEMY STATE 
	public void SetEnemyState (EnemyState enemyState) {
		state = enemyState;
	}

	public void SetEnemyIdle () {
		state = EnemyState.Idle;
	}
	#endregion
}