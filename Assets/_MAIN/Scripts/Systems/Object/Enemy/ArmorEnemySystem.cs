﻿using Unity.Entities;
using UnityEngine;

public class ArmorEnemySystem : ComponentSystem {

	public struct ArmorEnemyComponent{
		public readonly int Length;
		public ComponentArray<Transform> armorEnemyTransform;
		public ComponentArray<ArmorEnemy> armorEnemy;
		public ComponentArray<Health> armorEnemyHealth;
		public ComponentArray<Rigidbody2D> armorEnemyRigidbody;
		public ComponentArray<Animator> armorEnemyAnim;
	}

	[InjectAttribute] ArmorEnemyComponent armorEnemyComponent;
	Transform currArmorEnemyTransform;
	ArmorEnemy currArmorEnemy;
	Health currArmorEnemyHealth;
	Rigidbody2D currArmorEnemyRigidbody;
	Animator currArmorEnemyAnim;

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i =0;i<armorEnemyComponent.Length;i++){
			currArmorEnemyTransform = armorEnemyComponent.armorEnemyTransform[i];
			currArmorEnemy = armorEnemyComponent.armorEnemy[i];
			currArmorEnemyHealth = armorEnemyComponent.armorEnemyHealth[i];
			currArmorEnemyRigidbody = armorEnemyComponent.armorEnemyRigidbody[i];
			currArmorEnemyAnim = armorEnemyComponent.armorEnemyAnim[i];

		CheckPlayer();
		CheckState();
		}
	}

	void CheckState()
	{
		if(currArmorEnemy.enemy.state == EnemyState.Idle){
			Idle();
		}else if(currArmorEnemy.enemy.state == EnemyState.Patrol){
			Patrol();
		}else if(currArmorEnemy.enemy.state == EnemyState.Attack){
			Roll();
		}
	}

	void CheckPlayer()
	{
		if(currArmorEnemy.enemy.hasArmor){
			if(currArmorEnemy.enemy.state == EnemyState.Idle || currArmorEnemy.enemy.state == EnemyState.Patrol){
				if(currArmorEnemy.enemy.playerTransform != null){
					currArmorEnemy.enemy.state = EnemyState.Attack;
					currArmorEnemy.enemy.initIdle = false;
					currArmorEnemy.enemy.initPatrol = false; 
				}
			}else if(currArmorEnemy.enemy.state == EnemyState.Attack){
				if(currArmorEnemy.enemy.playerTransform == null){
					currArmorEnemy.enemy.state = EnemyState.Idle;
					currArmorEnemy.initRoll = false;
					currArmorEnemy.startRoll = false;
				}
			}
		}else{
			if(currArmorEnemy.enemy.state != EnemyState.Idle){
				currArmorEnemy.enemy.state = EnemyState.Idle;
				currArmorEnemy.initRoll = false;
				currArmorEnemy.enemy.initPatrol = false;
			}
		}
	}

	void Idle()
	{
		if(!currArmorEnemy.enemy.initIdle){
			currArmorEnemy.enemy.initIdle = true;
			currArmorEnemyAnim.Play(currArmorEnemy.enemy.hasArmor ? EnemyState.Idle.ToString() : "IdleBare");
			deltaTime = Time.deltaTime;
			currArmorEnemy.enemy.TIdle = currArmorEnemy.enemy.idleDuration;
		}else{
			currArmorEnemy.enemy.TIdle -= deltaTime;

			if(currArmorEnemy.enemy.TIdle <= 0f){
				currArmorEnemy.enemy.state = EnemyState.Patrol;
				currArmorEnemy.enemy.initIdle = false;
			}
		}
	}

	void Patrol()
	{
		if(!currArmorEnemy.enemy.initPatrol){
			currArmorEnemy.enemy.initPatrol = true;
			deltaTime = Time.deltaTime;

			currArmorEnemy.enemy.patrolDestination = GetRandomPatrolPos(currArmorEnemy.patrolArea,currArmorEnemy.enemy.patrolRange);
			currArmorEnemyAnim.Play(currArmorEnemy.enemy.hasArmor ? EnemyState.Patrol.ToString() : "PatrolBare");
		}else{
			currArmorEnemyRigidbody.position = 
				Vector2.MoveTowards(
					currArmorEnemyRigidbody.position,
					currArmorEnemy.enemy.patrolDestination,
					currArmorEnemy.enemy.patrolSpeed * deltaTime
				);
			
			if(Vector2.Distance(currArmorEnemyRigidbody.position,currArmorEnemy.enemy.patrolDestination) < 0.1f){
				currArmorEnemy.enemy.initPatrol = false;
				currArmorEnemy.enemy.state = EnemyState.Idle;
			}
		}
	}

	void Roll()
	{
		if(!currArmorEnemy.initRoll){
			currArmorEnemy.initRoll = true;
			currArmorEnemy.TRollInit = currArmorEnemy.rollInitDuration;
			currArmorEnemyAnim.Play(EnemyState.Idle.ToString());
			deltaTime = Time.deltaTime;
		}else{
			currArmorEnemy.TRollInit -= deltaTime;
			if(currArmorEnemy.TRollInit <= 0f){
				currArmorEnemy.startRoll = true;
				currArmorEnemyAnim.Play("Roll");
				currArmorEnemy.rollTargetPos = currArmorEnemy.enemy.playerTransform.position;
			}
		}

		if(currArmorEnemy.startRoll){
			currArmorEnemyRigidbody.position = Vector2.MoveTowards(currArmorEnemyRigidbody.position,currArmorEnemy.rollTargetPos, currArmorEnemy.rollSpeed * deltaTime);
			
			
			if(Vector2.Distance(currArmorEnemyRigidbody.position,currArmorEnemy.rollTargetPos) <= 0.1f){
				currArmorEnemyAnim.Play(EnemyState.Idle.ToString());
				currArmorEnemy.startRoll = false;
				currArmorEnemy.initRoll = false;
			}
		}
	}
	
	Vector2 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float x = Random.Range(-1 * range, range) + origin.x;
		float y = Random.Range(-1 * range, range) + origin.y;
		
		return new Vector2(x,y);
	}
}
