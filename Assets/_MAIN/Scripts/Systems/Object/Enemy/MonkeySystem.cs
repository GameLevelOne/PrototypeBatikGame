﻿using Unity.Entities;
using UnityEngine;

public class MonkeySystem : ComponentSystem {
	public struct MonkeyComponent
	{
		public readonly int Length;
		public ComponentArray<Transform> monkeyTransform;
		public ComponentArray<Monkey> monkey;
		public ComponentArray<Rigidbody2D> monkeyRigidbody;
		public ComponentArray<Animator> monkeyAnim;
		public ComponentArray<Health> monkeyHealth;
	}
	
	#region injected Component
	[InjectAttribute] public MonkeyComponent monkeyComponent;
	public Transform currMonkeyTransform;
	public Monkey currMonkey;
	public Rigidbody2D currMonkeyRigidbody;
	public Animator currMonkeyAnim;
	public Health currMonkeyHealth;
	#endregion

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i = 0;i<monkeyComponent.Length;i++){
			currMonkeyTransform = monkeyComponent.monkeyTransform[i];
			currMonkey = monkeyComponent.monkey[i];
			currMonkeyRigidbody = monkeyComponent.monkeyRigidbody[i];
			currMonkeyAnim = monkeyComponent.monkeyAnim[i];
			currMonkeyHealth = monkeyComponent.monkeyHealth[i];

			CheckState();
			CheckHit();
			CheckHealth();
		}
	}

	void CheckState()
	{
		currMonkeyRigidbody.velocity = Vector2.zero;
		if(currMonkey.enemy.state == EnemyState.Idle){
			Idle();
		}else if(currMonkey.enemy.state == EnemyState.Patrol){
			Patrol();
		}else if(currMonkey.enemy.state == EnemyState.Chase){
			Chase();
		}else if(currMonkey.enemy.state == EnemyState.Attack){
			Attack();
		}
	}

	void CheckHit()
	{
		if(!currMonkey.isHitByPlayer){
			if(currMonkey.enemy.IsEnemyHit){
				currMonkey.enemy.playerTransform = currMonkey.enemy.playerThatHitsEnemy.transform;
				foreach(Monkey m in currMonkey.nearbyMonkeys){
					m.enemy.playerTransform = currMonkey.enemy.playerTransform;
					m.enemy.state = EnemyState.Chase;
					m.enemy.initIdle = false;
					m.enemy.initPatrol = false;
				}
				currMonkey.enemy.initIdle = false;
				currMonkey.enemy.initPatrol = false;
				currMonkey.enemy.state = EnemyState.Chase;
				currMonkey.isHitByPlayer = true;
			}
		}		
	}

	void CheckHealth()
	{
		if(currMonkeyHealth.HealthPower <= 0f){
			GameObject.Destroy(currMonkey.gameObject);
			UpdateInjectedComponentGroups();
		}
	}

	void Idle()
	{
		if(!currMonkey.enemy.initIdle){
			currMonkey.enemy.initIdle = true;
			currMonkey.enemy.TIdle = currMonkey.enemy.idleDuration;
			deltaTime = Time.deltaTime;
			currMonkeyAnim.Play(EnemyState.Idle.ToString());
		}else{
			currMonkey.enemy.TIdle -= deltaTime;
			if(currMonkey.enemy.TIdle <= 0f){
				currMonkey.enemy.state = EnemyState.Patrol;
				currMonkey.enemy.initIdle = false;
			}
		}
	}
	
	void Patrol()
	{
		if(!currMonkey.enemy.initPatrol){
			currMonkey.enemy.initPatrol = true;
			currMonkey.enemy.patrolDestination = GetRandomPatrolPos(currMonkey.patrolArea,currMonkey.enemy.patrolRange);
			deltaTime = Time.deltaTime;
			currMonkeyAnim.Play(EnemyState.Patrol.ToString());
		}else{
			currMonkeyRigidbody.position = 
				Vector2.MoveTowards(
					currMonkeyRigidbody.position,
					currMonkey.enemy.patrolDestination,
					currMonkey.enemy.patrolSpeed * deltaTime
				);

			if(Vector2.Distance(currMonkeyRigidbody.position,currMonkey.enemy.patrolDestination) < 0.1f){
				currMonkey.enemy.initPatrol = false;
				currMonkey.enemy.state = EnemyState.Idle;
			}
		}
	}
	
	void Chase()
	{
		if(currMonkey.enemy.isAttack){
			currMonkey.enemy.state = EnemyState.Attack;
			currMonkey.enemy.attackObject.transform.position = currMonkey.enemy.playerTransform.position;
		}else{
			deltaTime = Time.deltaTime;
			currMonkeyAnim.Play(EnemyState.Patrol.ToString());
			currMonkeyRigidbody.position = 
				Vector2.MoveTowards(
					currMonkeyRigidbody.position,
					currMonkey.enemy.playerTransform.position,
					currMonkey.enemy.chaseSpeed * deltaTime
				);
			
			if(Vector2.Distance(currMonkeyRigidbody.position,currMonkey.enemy.playerTransform.position) >= currMonkey.enemy.chaseRange){
				currMonkey.isHitByPlayer = false;
				currMonkey.enemy.state = EnemyState.Idle;
				currMonkey.enemy.playerTransform = null;
			}
		}
	}
	
	void Attack()
	{
		if(!currMonkey.enemy.initAttack){
			if(!currMonkey.enemy.isAttack){
				currMonkey.enemy.initAttack = false;
				currMonkey.enemy.state = EnemyState.Chase;
			}else{
				currMonkey.enemy.initAttack = true;
				currMonkeyAnim.Play(EnemyState.Attack.ToString());
			}
		}else{
			currMonkey.enemy.attackObject.SetActive(currMonkey.enemy.attackHit);
		}
	}

	Vector2 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float x = Random.Range(-1 * range, range) + origin.x;
		float y = Random.Range(-1 * range, range) + origin.y;
		
		return new Vector2(x,y);
	}
}
