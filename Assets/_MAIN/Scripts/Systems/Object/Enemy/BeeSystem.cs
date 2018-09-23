using UnityEngine;
using Unity.Entities;

public class BeeSystem : ComponentSystem {

	public struct BeeComponent
	{
		public readonly int Length;
		public ComponentArray<Transform> beeTransform;
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
	Bee currBee;
	Rigidbody currBeeRigidbody;
	Animator currBeeAnim;
	Health currBeeHealth;
	Enemy enemy;
	#endregion

	float deltaTime;
	// Vector3 verticalMultVector = new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier);

	#region Update 
	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		for(int i = 0;i<beeComponent.Length;i++){
			currBeeTransform = beeComponent.beeTransform[i];
			currBee = beeComponent.bee[i];
			currBeeRigidbody = beeComponent.beeRigidbody[i];
			currBeeAnim = beeComponent.beeAnim[i];
			currBeeHealth = beeComponent.beeHealth[i];

			enemy = currBee.enemy;
			
			CheckHealth();
			CheckState();
			CheckPlayer();
		}
	}

	void CheckHealth()
	{
		if(currBeeHealth.EnemyHP <= 0f){
			//SPAWN ITEM
			lootableSpawnerSystem.CheckPlayerLuck(enemy.spawnItemProbability, currBeeTransform.position);

			if (currBee.questTrigger != null) {
				//SEND QUEST TRIGGER
				currBee.questTrigger.isDoQuest = true;
			} else {
				Debug.Log("No Quest Triggered");
			}

			GameObject.Destroy(currBee.gameObject);
			UpdateInjectedComponentGroups();
		}
	}

	void CheckState()
	{
		if(currBee.enemy.state == EnemyState.Idle){
			Idle();
		}else if(currBee.enemy.state == EnemyState.Patrol){
			Patrol();
		}else if(currBee.enemy.state == EnemyState.Chase){
			Chase();
		}else if(currBee.enemy.state == EnemyState.Startled){
			Startled();
		}else if(currBee.enemy.state == EnemyState.Attack){
			Attack();
		}
	}

	void CheckPlayer()
	{
		if(currBee.enemy.state == EnemyState.Idle || currBee.enemy.state == EnemyState.Patrol){
			if(currBee.enemy.playerTransform != null){ 
				currBee.enemy.state = EnemyState.Chase;
				currBee.enemy.initIdle = false;
				currBee.enemy.initPatrol = false;	
				currBee.enemy.chaseIndicator.Play(true);
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}
		}
	}
	#endregion

	void Idle()
	{
		if(!currBee.enemy.initIdle){
			currBee.enemy.initIdle = true;
			currBee.enemy.TIdle = currBee.enemy.idleDuration;
			// deltaTime = Time.deltaTime;
		}else{
			currBee.enemy.TIdle -= deltaTime;

			if(currBee.enemy.TIdle <= 0f){
				currBee.enemy.state = EnemyState.Patrol;
				currBee.enemy.initIdle = false;
			}
		}
	}

	void Patrol()
	{
		if(!currBee.enemy.initPatrol){
			currBee.enemy.initPatrol = true;
			
			Vector3 origin = 
				currBee.isStartled ?  currBee.transform.position : 
					currBee.beeHiveTransform != null ? currBee.beeHiveTransform.position : currBee.transform.position;


			currBee.enemy.patrolDestination = GetRandomPatrolPos(origin,currBee.enemy.patrolRange);
			// deltaTime = Time.deltaTime;
			currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			// currMonkeyAnim.Play(EnemyState.Patrol.ToString()); //NO
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currBee.enemy.patrolDestination, currBee.enemy.patrolSpeed);
				// Vector3.MoveTowards(
				// 	currBeeRigidbody.position,
				// 	currBee.enemy.patrolDestination,
				// 	currBee.enemy.patrolSpeed * deltaTime
				// );

			if(Vector3.Distance(currBee.enemy.patrolDestination,currBeeRigidbody.position) < 0.1f){
				currBee.enemy.state = EnemyState.Idle;
				currBee.enemy.initPatrol = false;
			}
		}
	}

	void Chase()
	{
		if(currBee.enemy.isAttack){
			currBee.enemy.state = EnemyState.Attack;
			// currBee.enemy.attackObject.transform.position = currBee.enemy.playerTransform.position;
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currBee.enemy.playerTransform.position, currBee.enemy.chaseSpeed);
				// Vector3.Scale(verticalMultVector, 
				// 	Vector3.MoveTowards(
				// 		currBeeRigidbody.position,
				// 		currBee.enemy.playerTransform.position,
				// 		currBee.enemy.chaseSpeed * deltaTime
				// 	)
				// );

			if(Vector3.Distance(currBeeRigidbody.position,currBee.enemy.playerTransform.position) >= currBee.enemy.chaseRange){
				currBee.enemy.state = EnemyState.Idle;
				currBee.enemy.playerTransform = null;
				// currBee.enemy.chaseIndicator.SetActive(false);

				if(currBee.beeHiveTransform == null) currBee.isStartled = true;
			}
		}
	}

	void Attack()
	{
		if(!currBee.enemy.initAttack){
			// Debug.Log("Check initAttack false");
			// currBee.attackCodeFX.SetActive(false);
			if(!currBee.enemy.isAttack){
				// Debug.Log("Check isAttack false");
				currBee.enemy.state = EnemyState.Chase;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}else{
				// Debug.Log("Check isAttack true");
				// if(!currBee.initAttackHitPosition){
				// 	currBee.initAttackHitPosition = true;
					currBee.enemy.attackObject.transform.position = currBee.enemy.playerTransform.position;
				// }

				currBee.attackCodeFX.Play(true);
				currBee.enemy.initAttack = true;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
			}
		}else{
			// Debug.Log("Check initAttack true");
			currBee.enemy.attackObject.SetActive(currBee.enemy.attackHit);
		}
	}	

	void Startled()
	{
		if(!currBee.initStartled){
			currBee.beeHiveTransform = null;
			currBee.enemy.initIdle = false;
			currBee.enemy.initPatrol = false;

			// deltaTime = Time.deltaTime;
			currBee.initStartled = true;

			currBee.enemy.patrolDestination = GetRandomPatrolPos(currBeeRigidbody.position,currBee.startledRange);
		}else{
			currBeeRigidbody.position = 
				MoveToPos(currBee.enemy.patrolDestination, currBee.enemy.chaseSpeed);
				// Vector3.Scale(verticalMultVector, 
				// 	Vector3.MoveTowards(
				// 		currBeeRigidbody.position,
				// 		currBee.enemy.patrolDestination,
				// 		currBee.enemy.chaseSpeed * deltaTime
				// 	)
				// );

			if(Vector3.Distance(currBeeRigidbody.position,currBee.enemy.patrolDestination) < 0.1f){
				currBee.initStartled = false;
				currBee.enemy.state = EnemyState.Idle;
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