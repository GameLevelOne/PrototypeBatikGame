using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class WaterShooterEnemySystem : ComponentSystem {
	
	public struct WaterShooterEnemyComponent{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<WaterShooterEnemy> waterShooterEnemy;
		public ComponentArray<Transform> waterShooterEnemyTransform;
		public ComponentArray<Rigidbody> waterShooterEnemyRigidbody;
		public ComponentArray<Animator> waterShooterEnemyAnim;
		public ComponentArray<Health> waterShooterEnemyHealth;
	}

	#region injected component
	[InjectAttribute] public WaterShooterEnemyComponent waterShooterEnemyComponent;
	Enemy currEnemy;
	WaterShooterEnemy currWaterShooterEnemy;
	Transform currWaterShooterEnemyTransform;
	Rigidbody currWaterShooterEnemyRidigbody;
	Animator currWaterShooterEnemyAnim;
	Health currWaterShooterEnemyHealth;
	#endregion
	
	float deltaTime;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		for(int i = 0;i<waterShooterEnemyComponent.Length;i++){
			currWaterShooterEnemy = waterShooterEnemyComponent.waterShooterEnemy[i];
			currEnemy = waterShooterEnemyComponent.enemy[i];
			currWaterShooterEnemyTransform = waterShooterEnemyComponent.waterShooterEnemyTransform[i];
			currWaterShooterEnemyRidigbody = waterShooterEnemyComponent.waterShooterEnemyRigidbody[i];
			currWaterShooterEnemyAnim = waterShooterEnemyComponent.waterShooterEnemyAnim[i];
			currWaterShooterEnemyHealth = waterShooterEnemyComponent.waterShooterEnemyHealth[i];

			CheckHealth();
			CheckState();
		}
	}
	
	void CheckState()
	{
		if(currEnemy.state == EnemyState.Damaged){
			Damaged();
		} else {
			if (currEnemy.state == EnemyState.Idle){
				Idle();
			} else if (currEnemy.state == EnemyState.Aggro){
				Aggro();
			} else if (currEnemy.state == EnemyState.Attack){
				Attack();
			} else if (currEnemy.state == EnemyState.Die){
				Drown();
			}
		}
	}

	void CheckHealth()
	{
		if(currWaterShooterEnemyHealth.EnemyHP <= 0f){
			//SPAWN DEATH FX
			GameObject.Instantiate(currEnemy.deathFX, currWaterShooterEnemyTransform.position, Quaternion.identity);

			//SPAWN ITEM
			// lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currBeeTransform.position);
			// PlaySFXOneShot(LotusAudio.DIE);

			if (currWaterShooterEnemy.questTrigger != null) {
				//SEND QUEST TRIGGER
				currWaterShooterEnemy.questTrigger.isDoQuest = true;
			} else {
				Debug.Log("No Quest Triggered");
			}

			if (currWaterShooterEnemy.chestSpawner != null) {
				//SEND CHEST SPAWNER TRIGGER
				currWaterShooterEnemy.chestSpawner.isTriggerSpawn = true;
			} else {
				Debug.Log("No ChestSpawner Triggered");
			}

			GameObject.Destroy(currWaterShooterEnemy.gameObject);
			UpdateInjectedComponentGroups();
		} else {
			if (currEnemy.isBurned) {
				currWaterShooterEnemy.burnedFX.Play(true);

				currEnemy.isBurned = false;
			}
		}
	}

	void Idle ()
	{
		if(currEnemy.playerTransform != null){
			if (currWaterShooterEnemy.currPlayerTransform == null) {
				currWaterShooterEnemy.currPlayerTransform = currEnemy.playerTransform;
			}
			
			SetStateAggro();
		}else{
			if(!currEnemy.initIdle){
				currEnemy.initIdle = true;
				currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}
		}
	}

	void Drown () {
		if (currWaterShooterEnemy.isFinisDrowning) {
			currEnemy.state = EnemyState.Idle;
			currWaterShooterEnemy.isFinisDrowning = false;
		}
	}

	void SetStateAggro () {
		currEnemy.initIdle = false;
		currEnemy.state = EnemyState.Aggro;
		currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
		currEnemy.chaseIndicator.Play(true);
		// PlaySFXOneShot(LotusAudio.AGGRO);
	}

	void Aggro () {
		if (currEnemy.isFinishAggro) {
			currEnemy.state = EnemyState.Attack;
			currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);

			currWaterShooterEnemy.startAttack = true;
			currEnemy.initAttack = true; //
			currEnemy.attackHit = false;
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
			
			// currEnemy.TDamaged = currEnemy.damagedDuration;
			currEnemy.initDamaged = true;
			currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_DAMAGED);

			// currEnemy.TDamaged = currEnemy.damagedDuration;
		}else{
			// if(currEnemy.TDamaged <= 0f){
			if (currEnemy.isFinishDamaged) {				
				currEnemy.isFinishDamaged = false;
				// SetStateAggro(); //OLD
				currEnemy.state = EnemyState.Die;
				currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_DIE);
			}
			// } else {
			// 	currEnemy.TDamaged -= deltaTime;
			// }
		}
	}

	void Attack()
	{
		// if (currEnemy.playerTransform == null){
		// 	currWaterShooterEnemy.startAttack = false;
			
		// 	if (!currEnemy.initAttack) {
		// 		currEnemy.state = EnemyState.Die;
		// 		// currEnemy.initAttack = false;
		// 		currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_DIE);
		// 	}
		// } else {
			if (!currEnemy.initAttack) {
				// currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
				currEnemy.state = EnemyState.Die;
				currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_DIE);
				currEnemy.initAttack = true;
			} else {
				if (currWaterShooterEnemy.startAttack){
					if (currEnemy.attackHit) {
						// if (currEnemy.playerTransform != null) {
							SpawnWaterBulletObj (currWaterShooterEnemy.bullet);
							PlaySFX(LotusAudio.SHOOT);
						// }
							
						// currWaterShooterEnemy.TShootInterval = currWaterShooterEnemy.shootInterval;
						currWaterShooterEnemy.startAttack = false;
						currEnemy.attackHit = false;
					} else {
						if (currEnemy.playerTransform != null) {
							currWaterShooterEnemy.currPlayerTransform = currEnemy.playerTransform;
						// } else {
						// 	currWaterShooterEnemy.currPlayerTransform = null;
						}
						// if (currEnemy.playerTransform == null) {
						// 	currWaterShooterEnemy.startAttack = false;
						// }
					}
				// } else {
					// if (currWaterShooterEnemy.TShootInterval <= 0f) {
					// 	currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);
					// 	currWaterShooterEnemy.startAttack = true;
					// 	currEnemy.attackHit = false;
					// } else {
					// 	currWaterShooterEnemy.TShootInterval -= deltaTime;
					// }
				}
			}
		// }
	}

	// Vector3 GetProjectileDirection(Vector3 origin, Vector3 target)
	// {
	// 	Vector3 distance = target-origin;
	// 	float magnitude = distance.magnitude;

	// 	return distance/magnitude;
	// }

	void SpawnWaterBulletObj (GameObject obj) {
		// Vector3 targetPos = currEnemy.playerTransform.position;
		Vector3 targetPos = currWaterShooterEnemy.currPlayerTransform.position;
        Vector3 initPos = currWaterShooterEnemy.bulletSpawnPoint.position;

		Vector3 deltaPos = targetPos - initPos;
		
        GameObject spawnedObj = GameObject.Instantiate(obj, initPos, SetFacingParent(deltaPos));
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
		
		// Debug.Log(spawnedObj.transform.GetChild(0).name);
		spawnedObj.transform.GetChild(0).rotation = SetFacingChild(deltaPos);

        spawnedObj.SetActive(true);
    }
	
	Quaternion SetFacingParent (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.x, resultPos.z) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, angle - 90f, 0f));

        return targetRot;
	}

    Quaternion SetFacingChild (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.z, resultPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle - 180f));

        return targetRot;
    }

	public void PlaySFXOneShot (LotusAudio audioType) {
		currWaterShooterEnemy.audioSource.PlayOneShot(currWaterShooterEnemy.audioClip[(int) audioType]);
	}

	public void PlaySFX (LotusAudio audioType) {
		if (!currWaterShooterEnemy.audioSource.isPlaying) {
			currWaterShooterEnemy.audioSource.clip = currWaterShooterEnemy.audioClip[(int) audioType];
			currWaterShooterEnemy.audioSource.Play();
		}
	}
}
