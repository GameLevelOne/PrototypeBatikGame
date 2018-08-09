using Unity.Entities;
using UnityEngine;

public class WaterShooterEnemySystem : ComponentSystem {
	
	public struct WaterShooterEnemyComponent{
		public readonly int Length;
		public ComponentArray<WaterShooterEnemy> waterShooterEnemy;
		public ComponentArray<Rigidbody2D> waterShooterEnemyRigidbody;
		public ComponentArray<Animator> waterShooterEnemyAnim;
	}

	#region injected component
	[InjectAttribute] public WaterShooterEnemyComponent waterShooterEnemyComponent;
	WaterShooterEnemy currWaterShooterEnemy;
	Rigidbody2D currWaterShooterEnemyRidigbody;
	Animator currWaterShooterEnemyAnim;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<waterShooterEnemyComponent.Length;i++){
			currWaterShooterEnemy = waterShooterEnemyComponent.waterShooterEnemy[i];
			currWaterShooterEnemyRidigbody = waterShooterEnemyComponent.waterShooterEnemyRigidbody[i];
			currWaterShooterEnemyAnim = waterShooterEnemyComponent.waterShooterEnemyAnim[i];

			CheckState();
		}
	}
	
	void CheckState()
	{
		if(currWaterShooterEnemy.enemy.state == EnemyState.Idle){
			Idle();
		}else if(currWaterShooterEnemy.enemy.state == EnemyState.Attack){
			Attack();
		}
	}

	void Idle()
	{
		if(currWaterShooterEnemy.enemy.playerTransform != null){
			currWaterShooterEnemy.enemy.state = EnemyState.Attack;
			currWaterShooterEnemy.enemy.initIdle = false;
		}else{
			if(!currWaterShooterEnemy.enemy.initIdle){
				currWaterShooterEnemy.enemy.initIdle = true;
				currWaterShooterEnemyAnim.Play(EnemyState.Idle.ToString());
			}
		}
	}

	void Attack()
	{
		if(currWaterShooterEnemy.enemy.playerTransform == null){
			currWaterShooterEnemy.enemy.state = EnemyState.Idle;
			currWaterShooterEnemy.enemy.initAttack = false;
		}else{
			if(!currWaterShooterEnemy.enemy.initAttack){
				currWaterShooterEnemy.enemy.initAttack = true;
				currWaterShooterEnemy.TShootInterval = currWaterShooterEnemy.shootInterval;
			}else{
				currWaterShooterEnemy.TShootInterval -= Time.deltaTime;
				if(currWaterShooterEnemy.TShootInterval <= 0f){
					//shoot
					GameObject bullet = GameObject.Instantiate(currWaterShooterEnemy.bullet,currWaterShooterEnemyRidigbody.position,Quaternion.identity) as GameObject;
					bullet.GetComponent<WaterShooterBullet>().direction = GetProjectileDirection(bullet.transform.position,currWaterShooterEnemy.enemy.playerTransform.position);
					bullet.GetComponent<WaterShooterBullet>().init = true;
					
					currWaterShooterEnemy.enemy.initAttack = false;
				}
			}
		}
	}

	Vector2 GetProjectileDirection(Vector2 origin, Vector2 target)
	{
		Vector2 distance = target-origin;
		float magnitude = distance.magnitude;

		return distance/magnitude;
	}
}
