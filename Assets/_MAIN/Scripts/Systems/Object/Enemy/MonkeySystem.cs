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

	EnemyState state;
	float deltaTime;
	float timeScale;
	Vector3 vector3Zero = Vector3.zero;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;
		timeScale = Time.timeScale;
		
		for(int i = 0;i<monkeyComponent.Length;i++){
			currMonkeyTransform = monkeyComponent.monkeyTransform[i];
			currEnemy = monkeyComponent.enemy[i];
			currMonkey = monkeyComponent.monkey[i];
			currMonkeyRigidbody = monkeyComponent.monkeyRigidbody[i];
			currMonkeyAnim = monkeyComponent.monkeyAnim[i];
			currMonkeyHealth = monkeyComponent.monkeyHealth[i];
			// enemy = currEnemy;
			state = currEnemy.state;

			if (timeScale < 1f) {
				currMonkeyRigidbody.velocity = Vector3.zero;
				
				if (state == EnemyState.Damaged){
					DamagedByBulletTime();
				}
			} else {
				CheckHealth();
				CheckState();
				CheckHit();
				CheckCollisionWithPlayer();
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
			currMonkeyRigidbody.velocity = Vector3.zero;
			
			if (state == EnemyState.Idle){
				Idle();
			} else if (state == EnemyState.Patrol){
				Patrol();
			} else if (state == EnemyState.Aggro){
				Aggro();
			} else if (state == EnemyState.Chase){
				Chase();
			} else if (state == EnemyState.Attack){
				Attack();
			}
		} 
	}

	void CheckHit()
	{
		if(!currMonkey.isHitByPlayer) {
			if(currEnemy.playerThatHitsEnemy != null && state == EnemyState.Damaged && state != EnemyState.Chase && state != EnemyState.Attack){ //IsEnemyHit
				//  // Debug.Log("Hit By Player");
				currEnemy.playerTransform = currEnemy.playerThatHitsEnemy.transform;

				foreach(Monkey m in currMonkey.nearbyMonkeys){
					if (m.enemy.state == EnemyState.Idle || m.enemy.state == EnemyState.Patrol) {
						m.enemy.playerTransform = currEnemy.playerTransform;
						m.enemy.initAggro = false;
						m.enemy.state = EnemyState.Aggro;
						// m.isHitByPlayer = true;
					}
				}

				currEnemy.initAggro = false;
				currEnemy.state = EnemyState.Aggro;
				currMonkey.isHitByPlayer = true;
			}
		}		
	}

	void CheckCollisionWithPlayer()
	{
		if(state != EnemyState.Aggro){
			if(currMonkey.isCollidingWithPlayer){
				currEnemy.initAggro = false;
				currEnemy.state = EnemyState.Aggro;
				currMonkey.isCollidingWithPlayer = false;
				// currEnemy.chaseIndicator.Play(true);
			}
		}
	}

	void Aggro () {
		if (!currEnemy.initAggro) {
			currEnemy.initIdle = false;
			currEnemy.initPatrol = false;
			currEnemy.initAttack = false;
			currEnemy.initAggro = true;

			// Player player = currEnemy.playerTransform.GetComponent<Player>();

			// if (player.isCanBulletTime) {
			// 	currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			// 	return;
			// }	

			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
			currEnemy.chaseIndicator.Play(true);
			PlaySFX(MonkeyAudio.AGGRO, true);
		} else if (currEnemy.isFinishAggro) {
			currEnemy.state = EnemyState.Chase;
			currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_CHASE);

			currEnemy.isFinishAggro = false;
		}
	}

	void InitDamaged () {
		currEnemy.initIdle = false;
		currEnemy.initPatrol = false;
		currEnemy.initAggro = false;
		currEnemy.initAttack = false;
		// currEnemy.isAttack = false;

		currEnemy.attackObject.SetActive(false);
		// currEnemy.TDamaged = currEnemy.damagedDuration;
		currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);
		currMonkey.audioSource.Stop();
	}

	void AfterDamaged () {
		currMonkeyRigidbody.velocity = vector3Zero;
		// currEnemy.damageSourceTransform = null;
		// currBeeRigidbody.isKinematic = true;
		
		currEnemy.state = EnemyState.Aggro;
		// currEnemy.chaseIndicator.Play(true);
	}

	void Damaged()
	{
		if(!currEnemy.initDamaged){
			InitDamaged();
			KnockBack();

			currEnemy.initDamaged = true;
		}else{
			// if(currEnemy.TDamaged <= 0f){
			//  // Debug.Log("isFinishDamaged : "+currEnemy.isFinishDamaged);
			if (currEnemy.isFinishDamaged) {
				AfterDamaged();
				
				currEnemy.isFinishDamaged = false;
			}
			// } else {
			// 	currEnemy.TDamaged -= deltaTime;
			// }
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
			
			currEnemy.initDamaged = true;
		}else{
			if (currEnemy.isFinishDamaged) {
				AfterDamaged();
				
				currEnemy.isFinishDamaged = false;
			}
		}
	}

	void KnockBack () {
		//  // Debug.Log("Set Monkey KnockBack");
		Vector3 damageSourcePos = currEnemy.damageSourcePos;
		
		Vector3 resultPos = new Vector3 (currMonkeyTransform.position.x-damageSourcePos.x, 0f, currMonkeyTransform.position.z-damageSourcePos.z);

		// currMonkeyRigidbody.isKinematic = false;
		currMonkeyRigidbody.velocity = Vector3.zero;

		currMonkeyRigidbody.AddForce(resultPos * currEnemy.knockBackSpeed, ForceMode.Impulse);
	}

	void CheckHealth()
	{
		if(currMonkeyHealth.EnemyHP <= 0f){
			//SPAWN DEATH FX
			GameObject.Instantiate(currEnemy.deathFX, currMonkeyTransform.position, Quaternion.identity);

			//SPAWN ITEM
			lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currMonkeyTransform.position);
			PlaySFXOneShot(MonkeyAudio.DIE);

			if (currMonkey.questTrigger != null) {
				//SEND QUEST TRIGGER
				currMonkey.questTrigger.isDoQuest.Add(true);
			} else {
				 // Debug.Log("No Quest Triggered");
			}

			if (currMonkey.chestSpawner != null) {
				//SEND CHEST SPAWNER TRIGGER
				currMonkey.chestSpawner.isTriggerSpawn.Add(true);
			} else {
				 // Debug.Log("No ChestSpawner Triggered");
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
				currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_CHASE);
			}else{
				currEnemy.attackObject.transform.position = currEnemy.playerTransform.position;

				currMonkey.attackCodeFX.Play(true);
				currEnemy.initAttack = true;
				currMonkeyAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
			}
		}else{
			currEnemy.attackObject.SetActive(currEnemy.attackHit);

			if (currMonkey.isInitSpawnAttackFX) {
				PlaySFXOneShot(MonkeyAudio.ATTACK);
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
			//  // Debug.Log("vecticalizeVector = "+vecticalizeVector+" -> "+(currBeeRigidbody.position + vecticalizeVector * speed * deltaTime));
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

	public void PlaySFX (MonkeyAudio audioType, bool isLoop) {
		if (isLoop) {
			currMonkey.audioSource.loop = true;
		} else {
			currMonkey.audioSource.loop = false;
		}

		// if (!input.audioSource.isPlaying) {
			currMonkey.audioSource.clip = currMonkey.audioClip[(int) audioType];
			currMonkey.audioSource.Play();
		// }
	}
}