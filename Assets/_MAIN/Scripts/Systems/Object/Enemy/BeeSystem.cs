using UnityEngine;
using Unity.Entities;

public class BeeSystem : ComponentSystem {

	public struct BeeComponent
	{
		public readonly int Length;
		public ComponentArray<Transform> beeTransform;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Bee> bee;
		public ComponentArray<Rigidbody> beeRigidbody;
		public ComponentArray<Animator> beeAnim;
		public ComponentArray<Health> beeHealth;
	}

	#region injected component
	[InjectAttribute] public BeeComponent beeComponent;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	// [InjectAttribute] GameFXSystem gameFXSystem;
	public Transform currBeeTransform;
	Enemy currEnemy;
	Bee currBee;
	Rigidbody currBeeRigidbody;
	Animator currBeeAnim;
	Health currBeeHealth;
	// Enemy enemy;
	#endregion

	float deltaTime;
	// Vector3 verticalMultVector = new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier);

	#region Update 
	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		for(int i = 0;i<beeComponent.Length;i++){
			currBeeTransform = beeComponent.beeTransform[i];
			currEnemy = beeComponent.enemy[i];
			currBee = beeComponent.bee[i];
			currBeeRigidbody = beeComponent.beeRigidbody[i];
			currBeeAnim = beeComponent.beeAnim[i];
			currBeeHealth = beeComponent.beeHealth[i];

			// enemy = currEnemy;
			
			CheckHealth();
			CheckState();
			CheckPlayer();
		}
	}

	void CheckHealth()
	{
		if(currBeeHealth.EnemyHP <= 0f){
			//SPAWN ITEM
			lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currBeeTransform.position);

			if (currBee.questTrigger != null) {
				//SEND QUEST TRIGGER
				currBee.questTrigger.isDoQuest = true;
			} else {
				Debug.Log("No Quest Triggered");
			}

			if (currBee.chestSpawner != null) {
				//SEND CHEST SPAWNER TRIGGER
				currBee.chestSpawner.isTriggerSpawn = true;
			} else {
				Debug.Log("No ChestSpawner Triggered");
			}

			GameObject.Destroy(currBee.gameObject);
			UpdateInjectedComponentGroups();
		}
	}

	void CheckState()
	{
		if(currEnemy.state == EnemyState.Idle){
			Idle();
		}else if(currEnemy.state == EnemyState.Patrol){
			Patrol();
		}else if(currEnemy.state == EnemyState.Chase){
			Chase();
		}else if(currEnemy.state == EnemyState.Startled){
			Startled();
		}else if(currEnemy.state == EnemyState.Attack){
			Attack();
		}
	}

	void CheckPlayer()
	{
		if(currEnemy.state == EnemyState.Idle || currEnemy.state == EnemyState.Patrol){
			if(currEnemy.playerTransform != null){ 
				currEnemy.state = EnemyState.Chase;
				currEnemy.initIdle = false;
				currEnemy.initPatrol = false;	
				currEnemy.chaseIndicator.Play(true);
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}
		}
	}
	#endregion

	void Idle()
	{
		if(!currEnemy.initIdle){
			currEnemy.initIdle = true;
			currEnemy.TIdle = currEnemy.idleDuration;
			// deltaTime = Time.deltaTime;
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
			
			Vector3 origin = 
				currBee.isStartled ?  currBee.transform.position : 
					currBee.beeHiveTransform != null ? currBee.beeHiveTransform.position : currBee.transform.position;


			currEnemy.patrolDestination = GetRandomPatrolPos(origin,currEnemy.patrolRange);
			// deltaTime = Time.deltaTime;
			currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			// currMonkeyAnim.Play(EnemyState.Patrol.ToString()); //NO
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currEnemy.patrolDestination, currEnemy.patrolSpeed);
				// Vector3.MoveTowards(
				// 	currBeeRigidbody.position,
				// 	currEnemy.patrolDestination,
				// 	currEnemy.patrolSpeed * deltaTime
				// );

			if(Vector3.Distance(currEnemy.patrolDestination,currBeeRigidbody.position) < 0.1f){
				currEnemy.state = EnemyState.Idle;
				currEnemy.initPatrol = false;
			}
		}
	}

	void Chase()
	{
		if(currEnemy.isAttack){
			currEnemy.state = EnemyState.Attack;
			// currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currEnemy.playerTransform.position, currEnemy.chaseSpeed);
				// Vector3.Scale(verticalMultVector, 
				// 	Vector3.MoveTowards(
				// 		currBeeRigidbody.position,
				// 		currEnemy.playerTransform.position,
				// 		currEnemy.chaseSpeed * deltaTime
				// 	)
				// );

			if(Vector3.Distance(currBeeRigidbody.position,currEnemy.playerTransform.position) >= currEnemy.chaseRange){
				currEnemy.state = EnemyState.Idle;
				currEnemy.playerTransform = null;
				// currEnemy.chaseIndicator.SetActive(false);

				if(currBee.beeHiveTransform == null) currBee.isStartled = true;
			}
		}
	}

	void Attack()
	{
		if(!currEnemy.initAttack){
			// Debug.Log("Check initAttack false");
			// currBee.attackCodeFX.SetActive(false);
			if(!currEnemy.isAttack){
				// Debug.Log("Check isAttack false");
				currEnemy.state = EnemyState.Chase;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}else{
				// Debug.Log("Check isAttack true");
				// if(!currBee.initAttackHitPosition){
				// 	currBee.initAttackHitPosition = true;
					currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;
				// }

				currBee.attackCodeFX.Play(true);
				currEnemy.initAttack = true;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
			}
		}else{
			// Debug.Log("Check initAttack true");
			currEnemy.attackObject.SetActive(currEnemy.attackHit);
		}
	}	

	void Startled()
	{
		if(!currBee.initStartled){
			currBee.beeHiveTransform = null;
			currEnemy.initIdle = false;
			currEnemy.initPatrol = false;

			// deltaTime = Time.deltaTime;
			currBee.initStartled = true;

			currEnemy.patrolDestination = GetRandomPatrolPos(currBeeRigidbody.position,currBee.startledRange);
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currEnemy.patrolDestination, currEnemy.chaseSpeed);
				// Vector3.Scale(verticalMultVector, 
				// 	Vector3.MoveTowards(
				// 		currBeeRigidbody.position,
				// 		currEnemy.patrolDestination,
				// 		currEnemy.chaseSpeed * deltaTime
				// 	)
				// );

			if(Vector3.Distance(currBeeRigidbody.position,currEnemy.patrolDestination) < 0.1f){
				currBee.initStartled = false;
				currEnemy.state = EnemyState.Idle;
			}
		}
	}

	Vector3 MoveToPos (Vector3 targetPos, float speed) {
		Vector3 deltaPos = targetPos-currBeeRigidbody.position;

		if (deltaPos.z < -0.2f || deltaPos.z > 0.2f) {
			Vector3 vecticalizeVector = Vector3.Scale(deltaPos.normalized, new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier));
			// Debug.Log("vecticalizeVector = "+vecticalizeVector+" -> "+(currBeeRigidbody.position + vecticalizeVector * speed * deltaTime));
			return currBeeRigidbody.position + vecticalizeVector * speed * deltaTime;
		} else {
			return currBeeRigidbody.position + deltaPos * speed * deltaTime;
		}
		
		#region OLD
		// Vector3 resultPos = Vector3.MoveTowards(currBeeRigidbody.position, targetPos, speed * deltaTime);
		// return resultPos;
		#endregion
	}

	Vector3 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float x,z;
		// float verticalRange = range;
		float verticalRange = range * GameStorage.Instance.settings.verticalMultiplier;

		if(currBee.initStartled){
			x = Random.value < 0.5f ? range + origin.x : (-1 * range) + origin.x;
			z = Random.value < 0.5f ? verticalRange + origin.z : (-1 * verticalRange) + origin.z;
		}else{
			x = Random.Range(-1 * range, range) + origin.x;
			z = Random.Range(-1 * verticalRange, verticalRange) + origin.z;
		}
		
		return new Vector3(x, currBeeTransform.position.y, z);
	}
}