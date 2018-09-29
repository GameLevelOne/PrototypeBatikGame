using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class WaterShooterEnemySystem : ComponentSystem {
	
	public struct WaterShooterEnemyComponent{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<WaterShooterEnemy> waterShooterEnemy;
		public ComponentArray<Rigidbody> waterShooterEnemyRigidbody;
		public ComponentArray<Animator> waterShooterEnemyAnim;
		public ComponentArray<Health> waterShooterEnemyHealth;
	}

	#region injected component
	[InjectAttribute] public WaterShooterEnemyComponent waterShooterEnemyComponent;
	Enemy currEnemy;
	WaterShooterEnemy currWaterShooterEnemy;
	Rigidbody currWaterShooterEnemyRidigbody;
	Animator currWaterShooterEnemyAnim;
	Health currWaterShooterEnemyHealth;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<waterShooterEnemyComponent.Length;i++){
			currWaterShooterEnemy = waterShooterEnemyComponent.waterShooterEnemy[i];
			currEnemy = waterShooterEnemyComponent.enemy[i];
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
			if(currWaterShooterEnemy.enemy.state == EnemyState.Idle){
				Idle();
			} else if (currEnemy.state == EnemyState.Aggro){
				Aggro();
			}else if(currWaterShooterEnemy.enemy.state == EnemyState.Attack){
				Attack();
			}
		}
	}

	void CheckHealth()
	{
		if(currWaterShooterEnemyHealth.EnemyHP <= 0f){
			//SPAWN ITEM
			// lootableSpawnerSystem.CheckPlayerLuck(currEnemy.spawnItemProbability, currBeeTransform.position);

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

	void Idle()
	{
		if(currWaterShooterEnemy.enemy.playerTransform != null){
			currEnemy.state = EnemyState.Aggro;
			currWaterShooterEnemy.enemy.initIdle = false;
			currEnemy.chaseIndicator.Play(true);
			currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
		}else{
			if(!currWaterShooterEnemy.enemy.initIdle){
				currWaterShooterEnemy.enemy.initIdle = true;
				currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			}
		}
	}

	void Aggro () {
		if (currEnemy.isFinishAggro) {
			currWaterShooterEnemy.enemy.state = EnemyState.Attack;
			currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_ATTACK);

			currEnemy.initAttack = false; //
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
				currEnemy.state = EnemyState.Aggro;
				currWaterShooterEnemyAnim.Play(Constants.BlendTreeName.ENEMY_AGGRO);
				currEnemy.chaseIndicator.Play(true);
			}
			// } else {
			// 	currEnemy.TDamaged -= deltaTime;
			// }
		}
	}

	void Attack()
	{
		if (currWaterShooterEnemy.enemy.playerTransform == null){
			currWaterShooterEnemy.enemy.state = EnemyState.Idle;
			currWaterShooterEnemy.enemy.initAttack = false;
		} else {
			if (!currWaterShooterEnemy.enemy.initAttack){
				currWaterShooterEnemy.enemy.initAttack = true;
				// currWaterShooterEnemy.TShootInterval = currWaterShooterEnemy.shootInterval;
				if (currEnemy.attackHit) {
					SpawnWaterBulletObj (currWaterShooterEnemy.bullet);

					currEnemy.attackHit = false;
				}
			} else {
				// currWaterShooterEnemy.TShootInterval -= Time.deltaTime;
				// if(currWaterShooterEnemy.TShootInterval <= 0f){
				// 	//shoot
				// 	// GameObject bullet = GameObject.Instantiate(currWaterShooterEnemy.bullet, currWaterShooterEnemyRidigbody.position,Quaternion.identity) as GameObject;
				// 	// bullet.GetComponent<WaterShooterBullet>().direction = GetProjectileDirection(bullet.transform.position,currWaterShooterEnemy.enemy.playerTransform.position);

				// 	SpawnWaterBulletObj (currWaterShooterEnemy.bullet);
					
				// 	currWaterShooterEnemy.enemy.initAttack = false;
				// }
			}
		}
	}

	// Vector3 GetProjectileDirection(Vector3 origin, Vector3 target)
	// {
	// 	Vector3 distance = target-origin;
	// 	float magnitude = distance.magnitude;

	// 	return distance/magnitude;
	// }

	void SpawnWaterBulletObj (GameObject obj) {
		Vector3 targetPos = currWaterShooterEnemy.enemy.playerTransform.position;
        Vector3 initPos = currWaterShooterEnemyRidigbody.position;

		Vector3 deltaPos = targetPos - initPos;
		
        GameObject spawnedObj = GameObject.Instantiate(obj, currWaterShooterEnemyRidigbody.position, SetFacingParent(deltaPos));
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
}
