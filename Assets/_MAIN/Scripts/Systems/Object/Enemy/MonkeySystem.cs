using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class MonkeySystem : ComponentSystem {
	public struct MonkeyComponent
	{
		public readonly int Length;
		public ComponentArray<Transform> monkeyTransform;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Monkey> monkey;
		public ComponentArray<Rigidbody> monkeyRigidbody;
		public ComponentArray<Animator> monkeyAnim;
		public ComponentArray<Health> monkeyHealth;
	}
	
	#region injected Component
	[InjectAttribute] public MonkeyComponent monkeyComponent;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	Transform currMonkeyTransform;
	Enemy currEnemy;
	Monkey currMonkey;
	Rigidbody currMonkeyRigidbody;
	Animator currMonkeyAnim;
	Health currMonkeyHealth;
	// Enemy enemy;
	#endregion

	float deltaTime;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;
		
		for(int i = 0;i<monkeyComponent.Length;i++){
			currMonkeyTransform = monkeyComponent.monkeyTransform[i];
			currEnemy = monkeyComponent.enemy[i];
			currMonkey = monkeyComponent.monkey[i];
			currMonkeyRigidbody = monkeyComponent.monkeyRigidbody[i];
			currMonkeyAnim = monkeyComponent.monkeyAnim[i];
			currMonkeyHealth = monkeyComponent.monkeyHealth[i];

			// enemy = currEnemy;

			CheckHealth();
			CheckState();
			CheckHit();
			CheckCollisionWithPlayer();
		}
	}

	void CheckState()
	{
		if (currEnemy.state == EnemyState.Damaged){
			Damaged();
		} else {
			currMonkeyRigidbody.velocity = Vector3.zero;
			
			if (currEnemy.state == EnemyState.Idle){
				Idle();
			} else if (currEnemy.state == EnemyState.Patrol){
				Patrol();
			} else if (currEnemy.state == EnemyState.Aggro){
				Aggro();
			} else if (currEnemy.state == EnemyState.Chase){
				Chase();
			} else if (currEnemy.state == EnemyState.Attack){
				Attack();
			}
		} 
	}

	void CheckHit()
	{
		if(!currMonkey.isHitByPlayer){
			if(currEnemy.playerThatHitsEnemy != null){ //IsEnemyHit
				// Debug.Log("Hit By Player");
				currEnemy.playerTransform = currEnemy.playerThatHitsEnemy.transform;

				foreach(Monkey m in currMonkey.nearbyMonkeys){
					m.enemy.playerTransform = currEnemy.playerTransform;
					SetStateAggro();
				}

				SetStateAggro();
				currMonkey.isHitByPlayer = true;
			}
		}		
	}

	void CheckCollisionWithPlayer()
	{
		if(currEnemy.state != EnemyState.Attack){
			if(currMonkey.isCollidingWithPlayer){
				currEnemy.state = EnemyState.Attack;
				currMonkey.isCollidingWithPlayer = false;
				currEnemy.chaseIndicator.Play(true);
			}
		}
	}

	void SetStateAggro () {
		currEnemy.initIdle = false;
		currEnemy.initPatrol = false;
		// currEnemy.state = EnemyState.Chase;
		currEnemy.state = EnemyState.Aggro;
		currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
		currEnemy.chaseIndicator.Play(true);
		PlaySFXOneShot(MonkeyAudio.AGGRO);
	}

	void Aggro () {
		if (currEnemy.isFinishAggro) {
			currEnemy.state = EnemyState.Chase;
			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);

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

			// currEnemy.TDamaged = currEnemy.damagedDuration;
			currEnemy.initDamaged = true;
			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);

			KnockBack();
			// currEnemy.TDamaged = currEnemy.damagedDuration;
		}else{
			// if(currEnemy.TDamaged <= 0f){
			// Debug.Log("isFinishDamaged : "+currEnemy.isFinishDamaged);
			if (currEnemy.isFinishDamaged) {
				currMonkeyRigidbody.velocity = Vector3.zero;
				// currEnemy.damageSourceTransform = null;
				// currMonkeyRigidbody.isKinematic = true;
				// Debug.Log("Set MonkeyRB Kinematic");
				
				currEnemy.isFinishDamaged = false;
				SetStateAggro();
			}
			// } else {
			// 	currEnemy.TDamaged -= deltaTime;
			// }
		}
	}

	void KnockBack () {
		// Debug.Log("Set Monkey KnockBack");
		Vector3 damageSourcePos = currEnemy.damageSourcePos;
		
		Vector3 resultPos = new Vector3 (currMonkeyTransform.position.x-damageSourcePos.x, 0f, currMonkeyTransform.position.z-damageSourcePos.z);

		// currMonkeyRigidbody.isKinematic = false;
		currMonkeyRigidbody.velocity = Vector3.zero;

		currMonkeyRigidbody.AddForce(resultPos * currEnemy.knockBackSpeed, ForceMode.Impulse);
	}

	void CheckHealth()
	{
		if(currMonkeyHealth.EnemyHP <= 0f){
			//SPAWN ITEM
			lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currMonkeyTransform.position);
			PlaySFXOneShot(MonkeyAudio.DIE);

			if (currMonkey.questTrigger != null) {
				//SEND QUEST TRIGGER
				currMonkey.questTrigger.isDoQuest = true;
			} else {
				Debug.Log("No Quest Triggered");
			}

			if (currMonkey.chestSpawner != null) {
				//SEND CHEST SPAWNER TRIGGER
				currMonkey.chestSpawner.isTriggerSpawn = true;
			} else {
				Debug.Log("No ChestSpawner Triggered");
			}

			GameObject.Destroy(currMonkey.gameObject);
			UpdateInjectedComponentGroups();
		} else {
			if (currEnemy.isBurned) {
				currMonkey.burnedFX.Play(true);

				currEnemy.isBurned = false;
			}
		}
	}

	void Idle()
	{
		if(!currEnemy.initIdle){
			currEnemy.initIdle = true;
			currEnemy.TIdle = currEnemy.idleDuration;
			// deltaTime = Time.deltaTime;
			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
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
			currEnemy.patrolDestination = GetRandomPatrolPos(currMonkey.patrolArea,currEnemy.patrolRange);
			// deltaTime = Time.deltaTime;
			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			PlaySFXOneShot(MonkeyAudio.WALK);
		}else{
			currMonkeyRigidbody.position = 
				MoveToPos(currEnemy.patrolDestination, currEnemy.patrolSpeed);
				// Vector3.MoveTowards(
				// 	currMonkeyRigidbody.position,
				// 	currEnemy.patrolDestination,
				// 	currMonkeyPatrolSpeed * deltaTime
				// );

			if(Vector3.Distance(currMonkeyRigidbody.position,currEnemy.patrolDestination) < 0.1f){
				currEnemy.initPatrol = false;
				currEnemy.state = EnemyState.Idle;
			}
		}
	}
	
	void Chase()
	{
		if(currEnemy.isAttack){
			currEnemy.state = EnemyState.Attack;
			// currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;
		}else{
			// deltaTime = Time.deltaTime;
			// currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			if(Vector3.Distance(currMonkeyRigidbody.position,currEnemy.playerTransform.position) > 0.4f){
				currMonkeyRigidbody.position = 
					MoveToPos(currEnemy.playerTransform.position, currEnemy.chaseSpeed);
			}
				// Vector3.MoveTowards(
				// 	currMonkeyRigidbody.position,
				// 	currEnemy.playerTransform.position,
				// 	currEnemy.chaseSpeed * deltaTime
				// );
			
			// if(Vector3.Distance(currMonkeyRigidbody.position,currEnemy.playerTransform.position) >= currEnemy.chaseRange){
			// 	currMonkey.isHitByPlayer = false;
			// 	currEnemy.state = EnemyState.Idle;
			// 	currEnemy.playerTransform = null;
			// 	// currEnemy.chaseIndicator.SetActive(false);
			// }
		}
	}
	
	void Attack()
	{
		if(!currEnemy.initAttack){
			if(!currEnemy.isAttack){
				currEnemy.initAttack = false;
				currEnemy.state = EnemyState.Chase;
				currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			}else{
				currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;

				currMonkey.attackCodeFX.Play(true);
				currEnemy.initAttack = true;
				currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
			}
		}else{
			currEnemy.attackObject.SetActive(currEnemy.attackHit);

			if (currMonkey.isInitSpawnAttackFX) {
				SpawnMonkeyClawFX();
				currMonkey.isInitSpawnAttackFX = false;
			}
		}
	}

	void SpawnMonkeyClawFX () {
		GameObject clawFX = GameObject.Instantiate(currMonkey.monkeyClawFX, currEnemy.attackObject.transform.position, Quaternion.Euler(new Vector3(40f, 0f, 0f)));
		
		if (currMonkeyAnim.GetFloat(Constants.AnimatorParameter.Float.FACE_X) > 0f) {
			clawFX.GetComponent<SpriteRenderer>().flipX = true;
		}

		clawFX.SetActive(true);
	}

	Vector3 MoveToPos (Vector3 targetPos, float speed) {
		Vector3 deltaPos = targetPos-currMonkeyRigidbody.position;

		if (deltaPos.z < -0.2f || deltaPos.z > 0.2f) {
			Vector3 vecticalizeVector = Vector3.Scale(deltaPos.normalized, new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier));
			// Debug.Log("vecticalizeVector = "+vecticalizeVector+" -> "+(currBeeRigidbody.position + vecticalizeVector * speed * deltaTime));
			return currMonkeyRigidbody.position + vecticalizeVector * speed * deltaTime;
		} else {
			return currMonkeyRigidbody.position + deltaPos * speed * deltaTime;
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
		
		return new Vector3(x, currMonkeyTransform.position.y, z);
	}

	public void PlaySFXOneShot (MonkeyAudio audioType) {
		currMonkey.audioSource.PlayOneShot(currMonkey.audioClip[(int) audioType]);
	}

	public void PlaySFX (MonkeyAudio audioType) {
		if (!currMonkey.audioSource.isPlaying) {
			currMonkey.audioSource.clip = currMonkey.audioClip[(int) audioType];
			currMonkey.audioSource.Play();
		}
	}
}