﻿using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class GhostSystem : ComponentSystem {

	public struct GhostComponent{
		public readonly int Length;
		
		public ComponentArray<Transform> ghostTransform;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Ghost> ghost;
		public ComponentArray<Animator> ghostAnim;
		public ComponentArray<Rigidbody> ghostRigidbody;
		public ComponentArray<CapsuleCollider> ghostCollider;
		public ComponentArray<Health> ghostHealth;
	}

	[InjectAttribute] GhostComponent ghostComponent;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;
	Transform currGhostTransform;
	Enemy currEnemy;
	Ghost currGhost;
	Animator currGhostAnim;
	Rigidbody currGhostRigidbody;
	CapsuleCollider ghostCollider;
	Health currGhostHealth;

	EnemyState state;
	float deltaTime;
	float timeScale;
	Vector3 vector3Zero = Vector3.zero;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;
		timeScale = Time.timeScale;

		for(int i = 0;i<ghostComponent.Length;i++){
			currGhostTransform = ghostComponent.ghostTransform[i];
			currEnemy = ghostComponent.enemy[i];
			currGhost = ghostComponent.ghost[i];
			currGhostAnim = ghostComponent.ghostAnim[i];
			currGhostRigidbody = ghostComponent.ghostRigidbody[i];
			ghostCollider = ghostComponent.ghostCollider[i];
			currGhostHealth = ghostComponent.ghostHealth[i];
			// enemy = currEnemy;
			state = currEnemy.state;

			if (timeScale < 1f) {
				currGhostRigidbody.velocity = Vector3.zero;
				
				if (state == EnemyState.Damaged){
					DamagedByBulletTime();
				}
			} else {
				CheckHealth();
				CheckState();
				CheckPlayer();
			}
		}
	}

	void CheckHealth()
	{
		if(currGhostHealth.EnemyHP <= 0f){
			currEnemy.state = EnemyState.Die;
		} else {
			if (currEnemy.isBurned) {
				currGhost.burnedFX.Play(true);

				currEnemy.isBurned = false;
			}
		}
	}

	void CheckState()
	{
		if (state == EnemyState.Damaged){
			Damaged();
		} else if (state == EnemyState.Stun) {
			Stunned();
		} else {
			currGhostRigidbody.velocity = Vector3.zero;
			
			if(state == EnemyState.Idle){
				Idle();
			}else if(state == EnemyState.Patrol){
				Patrol();
			}else if(state == EnemyState.Chase){
				Chase();
			}else if(state == EnemyState.Attack){
				Attack();
			}else if(state == EnemyState.Die){
				Die();
			}
		}
	}

	void CheckPlayer()
	{
		if(state == EnemyState.Idle || state == EnemyState.Patrol){
			if(currEnemy.playerTransform != null){ 
				currEnemy.initIdle = false;
				currEnemy.initPatrol = false;	

				Player player = currEnemy.playerTransform.GetComponent<Player>();

				if (player.isCanBulletTime) {
					currGhostAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
					return;
				}
				
				currEnemy.state = EnemyState.Chase;
				currEnemy.chaseIndicator.Play(true);
				currGhostAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			}
		}
	}

	void Idle()
	{	
		if(!currEnemy.initIdle){
			currEnemy.initIdle = true;
			currEnemy.TIdle = currEnemy.idleDuration;
			// deltaTime = Time.deltaTime;
			currGhostAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
		}else{
			currEnemy.TIdle -= deltaTime;

			if(currEnemy.TIdle <= 0f){
				currEnemy.state = EnemyState.Patrol;
				currEnemy.initIdle = false;
			}
		}
	}
	
	void Patrol()
	{
		if(!currEnemy.initPatrol){
			currEnemy.initPatrol = true;

			Vector3 startPos = currGhostTransform.position;
			currEnemy.patrolDestination = GetRandomPatrolPos(currGhost.origin,currEnemy.patrolRange);
			// deltaTime = Time.deltaTime;
			currGhostAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			
		}else{
			currGhostRigidbody.position = 
				// currGhostRigidbody.position = 
					MoveToPos(currEnemy.patrolDestination, currEnemy.patrolSpeed);
				// Vector3.MoveTowards(
				// 		currGhostRigidbody.position,
				// 		currEnemy.patrolDestination,
				// 		currEnemy.patrolSpeed * deltaTime
				// );


			if(Vector3.Distance(currGhostRigidbody.position,currEnemy.patrolDestination) < 0.1f){
				currEnemy.initPatrol = false;
				currEnemy.state = EnemyState.Idle;
			}
		}
	}
	
	void Chase()
	{
		if(currGhost.isAttacking){
			currEnemy.state = EnemyState.Attack;
		} else {
			currGhostRigidbody.position = 
				MoveToPos(currEnemy.playerTransform.position, currEnemy.chaseSpeed);
				// Vector3.MoveTowards(
				// 	currGhostRigidbody.position,
				// 	currEnemy.playerTransform.position,
				// 	currEnemy.chaseSpeed * deltaTime
				// );
		}
	}
	
	void Attack()
	{
		if(!currEnemy.initAttack){
			//  // Debug.Log("!currEnemy initAttack");
			if(!currGhost.isAttacking){
				// currEnemy.initAttack = false;
				currEnemy.state = EnemyState.Chase;
				currGhostAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
				//  // Debug.Log("!currGhost isAttacking");
			}else{
				currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;

				currGhost.attackCodeFX.Play(true);
				currEnemy.initAttack = true;
				currGhostAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
				//  // Debug.Log("currGhost isAttacking");
			}
		}else{
			//  // Debug.Log("currEnemy initAttack");
			currEnemy.attackObject.SetActive(currEnemy.attackHit);
		}
	}

	void InitDamaged () {
		currEnemy.initIdle = false;
		currEnemy.initPatrol = false;
		currEnemy.initAttack = false;
		// currEnemy.isAttack = false;

		currEnemy.attackObject.SetActive(false);
		currGhost.hitParticle.Play();
		// currEnemy.TDamaged = currEnemy.damagedDuration;
		currGhostAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);
	}

	void AfterDamaged () {
		currGhostRigidbody.velocity = vector3Zero;
		// currEnemy.damageSourceTransform = null;
		// currGhostRigidbody.isKinematic = true;
		
		currEnemy.state = EnemyState.Chase;
		currGhostAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
		currEnemy.chaseIndicator.Play(true);
	}

	void Damaged()
	{
		if(!currEnemy.initDamaged){
			InitDamaged();
			currGhost.hitParticle.Play();
			KnockBack();

			currEnemy.initDamaged = true;
		}else{
			// if(currGhost.TDamaged <= 0f){
			if (currEnemy.isFinishDamaged) {
				AfterDamaged();
				
				currEnemy.isFinishDamaged = false;
			// }
			} else {
				// currGhost.TDamaged -= deltaTime;
			}
		}
	}

	void Stunned()
	{
		if(!currEnemy.initDamaged){
			InitDamaged();
			
			currEnemy.initDamaged = true;
		}else{
			if (currEnemy.isFinishDamaged) {
				AfterDamaged();
				
				currEnemy.isFinishDamaged = false;
			}
		}
	}

	void DamagedByBulletTime()
	{
		if(!currEnemy.initDamaged){
			InitDamaged();
			currGhost.hitParticle.Play();
			
			currEnemy.initDamaged = true;
		}else{
			if (currEnemy.isFinishDamaged) {
				AfterDamaged();
				
				currEnemy.isFinishDamaged = false;
			}
		}
	}

	void KnockBack () {
		//  // Debug.Log("Set KnockBack");
		Vector3 damageSourcePos = currEnemy.damageSourcePos;
		
		Vector3 resultPos = new Vector3 (currGhostTransform.position.x-damageSourcePos.x, 0f, currGhostTransform.position.z-damageSourcePos.z);

		// currGhostRigidbody.isKinematic = false;
		currGhostRigidbody.velocity = Vector3.zero;

		currGhostRigidbody.AddForce(resultPos * currEnemy.knockBackSpeed, ForceMode.Impulse);
	}

	void Die()
	{
		if(!currEnemy.initDie){
			//SPAWN DEATH FX
			GameObject.Instantiate(currEnemy.deathFX, currGhostTransform.position, Quaternion.identity);

			currEnemy.initDie = true;
			currGhostAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);
			// deltaTime = Time.deltaTime;
			currEnemy.TDie = currEnemy.dieDuration;
			currGhostRigidbody.velocity = Vector3.zero;
			ghostCollider.enabled = false;
			currGhost.addonCollider.enabled = false;
			currGhost.PlaySFX(GhostSFX.GhostDie);
			currGhost.particle.Play();
		}else{
			currEnemy.TDie -= deltaTime;
			if(currEnemy.TDie <= currEnemy.dieDuration/2f){
				currGhost.sprite.material.color = Color.clear;
			}
			if( currEnemy.TDie <= 0f){
				//SPAWN ITEM
				lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currGhostTransform.position);

				if (currGhost.questTrigger != null) {
					//SEND QUEST TRIGGER
					currGhost.questTrigger.isDoQuest.Add(true);
				} else {
					 // Debug.Log("No Quest Triggered");
				}

				if (currGhost.chestSpawner != null) {
					//SEND CHEST SPAWNER TRIGGER
					currGhost.chestSpawner.isTriggerSpawn.Add(true);
				} else {
					 // Debug.Log("No ChestSpawner Triggered");
				}

				GameObject.Destroy(currGhost.gameObject);
				UpdateInjectedComponentGroups();
			}
		}
		
	}

	Vector3 MoveToPos (Vector3 targetPos, float speed) {
		Vector3 deltaPos = targetPos-currGhostRigidbody.position;

		if (deltaPos.z < -0.2f || deltaPos.z > 0.2f) {
			Vector3 vecticalizeVector = Vector3.Scale(deltaPos.normalized, new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier));
			//  // Debug.Log("vecticalizeVector = "+vecticalizeVector+" -> "+(currBeeRigidbody.position + vecticalizeVector * speed * deltaTime));
			return currGhostRigidbody.position + vecticalizeVector * speed * deltaTime;
		} else {
			return currGhostRigidbody.position + deltaPos * speed * deltaTime;
		}
		
		#region OLD
		// Vector3 resultPos = Vector3.MoveTowards(currBeeRigidbody.position, targetPos, speed * deltaTime);
		// return resultPos;
		#endregion
	}

	Vector3 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float verticalRange = range * GameStorage.Instance.settings.verticalMultiplier;

		float x = Random.Range(-1 * range, range) + origin.x;
		float z = Random.Range(-1 * verticalRange, verticalRange) + origin.z;
		
		return new Vector3(x, currGhostTransform.position.y, z);
	}
}