using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
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
		public ComponentArray<Facing2D> Facing;
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
	Facing2D currBeeFacing;
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
			currBeeFacing = beeComponent.Facing[i];
			// enemy = currEnemy;

			if (Time.timeScale < 1f) {
				currBeeRigidbody.velocity = Vector3.zero;
				continue;
			}
			
			CheckHealth();
			CheckState();
			CheckPlayer();
		}
	}

	void CheckHealth()
	{
		if(currBeeHealth.EnemyHP <= 0f){
			// currBee.audioSource.PlayOneShot(currBee.audioClip[(int)BeeAudio.DIE]);
			//SPAWN DEATH FX
			GameObject.Instantiate(currEnemy.deathFX, currBeeTransform.position, Quaternion.identity);

			//SPAWN ITEM
			lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currBeeTransform.position);

			if (currBee.questTrigger != null) {
				//SEND QUEST TRIGGER
				currBee.questTrigger.isDoQuest = true;
			} else {
				// Debug.Log("No Quest Triggered");
			}

			if (currBee.beeHive!=null) {
				//REMOVE FROM BEEHIVE
				currBee.beeHive.currentBees.Remove(currBee);
				currBee.beeHive = null;
			}

			if (currBee.chestSpawner != null) {
				//SEND CHEST SPAWNER TRIGGER
				currBee.chestSpawner.isTriggerSpawn = true;
			} else {
				// Debug.Log("No ChestSpawner Triggered");
			}

			GameObject.Destroy(currBee.gameObject);
			UpdateInjectedComponentGroups();
		} else {
			if (currEnemy.isBurned) {
				currBee.burnedFX.Play(true);

				currEnemy.isBurned = false;
			}
		}
	}

	void CheckState()
	{
		if (currEnemy.state == EnemyState.Damaged){
			Damaged();
		} else {
			currBeeRigidbody.velocity = Vector3.zero;
			
			if (currEnemy.state == EnemyState.Idle){
				Idle();
			} else if (currEnemy.state == EnemyState.Patrol){
				Patrol();
			} else if (currEnemy.state == EnemyState.Aggro){
				Aggro();
			} else if (currEnemy.state == EnemyState.Chase){
				Chase();
			} else if (currEnemy.state == EnemyState.Startled){
				Startled();
			} else if (currEnemy.state == EnemyState.Attack){
				Attack();
			}
		}
	}

	void CheckPlayer()
	{
		if(currEnemy.state == EnemyState.Idle || currEnemy.state == EnemyState.Patrol){
			if(currEnemy.playerTransform != null){ 
				currEnemy.initIdle = false;
				currEnemy.initPatrol = false;	

				Player player = currEnemy.playerTransform.GetComponent<Player>();

				if (player.isCanBulletTime) {
					currBeeAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
					return;
				}

				// currEnemy.state = EnemyState.Chase;
				currBee.audioSource.PlayOneShot(currBee.audioClip[(int)BeeAudio.PREPARE]);
				currEnemy.state = EnemyState.Aggro;
				currEnemy.chaseIndicator.Play(true);
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
			}
		}
	}
	#endregion

	void Aggro () {
		if (currEnemy.isFinishAggro) {
			currEnemy.state = EnemyState.Chase;
			currBeeAnim.Play(Constants.BlendTreeName.ENEMY_CHASE);

			currEnemy.isFinishAggro = false;
		}
	}

	void Damaged()
	{
		if(!currEnemy.initDamaged){
			currEnemy.initIdle = false;
			currEnemy.initPatrol = false;
			currEnemy.initAttack = false;
			currEnemy.isAttack = false;
			currEnemy.attackObject.SetActive(false);

			SpawnBeeCutFX();
			
			// currEnemy.TDamaged = currEnemy.damagedDuration;
			currEnemy.initDamaged = true;
			currBeeAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);

			KnockBack();
			// currEnemy.TDamaged = currEnemy.damagedDuration;
		}else{
			// if(currEnemy.TDamaged <= 0f){
			if (currEnemy.isFinishDamaged) {
				currBeeRigidbody.velocity = Vector3.zero;
				// currEnemy.damageSourceTransform = null;
				// currBeeRigidbody.isKinematic = true;
				
				currEnemy.isFinishDamaged = false;
				currEnemy.state = EnemyState.Aggro;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
				currEnemy.chaseIndicator.Play(true);
			}
			// } else {
			// 	currEnemy.TDamaged -= deltaTime;
			// }
		}
	}

	void SpawnBeeCutFX () {
		GameObject cutFX = GameObject.Instantiate(currBee.beeCutFX, currBeeTransform.position, Quaternion.Euler(new Vector3(40f, 0f, 0f)));
		
		if (currBeeAnim.GetFloat(Constants.AnimatorParameter.Float.FACE_X) < 0f) {
			cutFX.GetComponent<SpriteRenderer>().flipX = true;
		}

		cutFX.SetActive(true);
	}

	void KnockBack () {
		// Debug.Log("Set KnockBack");
		Vector3 damageSourcePos = currEnemy.damageSourcePos;
		
		Vector3 resultPos = new Vector3 (currBeeTransform.position.x-damageSourcePos.x, 0f, currBeeTransform.position.z-damageSourcePos.z);

		// currBeeRigidbody.isKinematic = false;
		currBeeRigidbody.velocity = Vector3.zero;

		currBeeRigidbody.AddForce(resultPos * currEnemy.knockBackSpeed, ForceMode.Impulse);
	}

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
			currBee.audioSource.PlayOneShot(currBee.audioClip[(int)BeeAudio.ATTACK]);

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
			// 	currEnemy.state = EnemyState.Idle;
			// 	currEnemy.playerTransform = null;
			// 	// currEnemy.chaseIndicator.SetActive(false);

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
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_CHASE);
			}else{
				// Debug.Log("Check isAttack true");
				// if(!currBee.initAttackHitPosition){
				// 	currBee.initAttackHitPosition = true;
					currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;
				// }

				// currBee.attackCodeFX.Play(true);
				currEnemy.initAttack = true;
				currBeeAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
			}
		}else{
			currEnemy.attackObject.SetActive(currEnemy.attackHit);

			if (currEnemy.isInitPlayAttackCodeFX) {
				currBee.attackCodeFX.Play(true);
				currEnemy.isInitPlayAttackCodeFX = false;
			}

			if (currBee.isInitSpawnAttackFX) {
				// SpawnBeeThrustFX();
				currBee.isInitSpawnAttackFX = false;
			}
		}
	}	

	void SpawnBeeThrustFX () {
		GameObject thrustFX = GameObject.Instantiate(currBee.beeThrustFX, currEnemy.attackObject.transform.position, Quaternion.Euler(new Vector3(40f, 0f, 0f)));
		
		if (currBeeAnim.GetFloat(Constants.AnimatorParameter.Float.FACE_X) > 0f) {
			Vector3 rotThrustFX = thrustFX.transform.eulerAngles;
			//MIRROR BY BEE'S FACING DIRECTION
			thrustFX.transform.rotation = Quaternion.Euler(new Vector3(rotThrustFX.x, rotThrustFX.y + 180f, rotThrustFX.z));
		}

		thrustFX.SetActive(true);
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