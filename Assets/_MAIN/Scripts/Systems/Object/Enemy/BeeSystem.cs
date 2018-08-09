using UnityEngine;
using Unity.Entities;

public class BeeSystem : ComponentSystem {

	public struct BeeComponent
	{
		public readonly int Length;
		public ComponentArray<Bee> bee;
		public ComponentArray<Rigidbody2D> beeRigidbody;
		public ComponentArray<Animator> beeAnim;
		public ComponentArray<Health> beeHealth;
	}

	#region injected component
	[InjectAttribute] public BeeComponent beeComponent;
	Bee currBee;
	Rigidbody2D currBeeRigidbody;
	Animator currBeeAnim;
	Health currBeeHealth;
	#endregion

	float deltaTime;

	#region Update 
	protected override void OnUpdate()
	{
		for(int i = 0;i<beeComponent.Length;i++){
			currBee = beeComponent.bee[i];
			currBeeRigidbody = beeComponent.beeRigidbody[i];
			currBeeAnim = beeComponent.beeAnim[i];
			currBeeHealth = beeComponent.beeHealth[i];
			
			CheckHealth();
			CheckState();
			CheckPlayer();
		}
	}

	void CheckHealth()
	{
		if(currBeeHealth.HealthPower <= 0f){
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
			}
		}
	}
	#endregion

	void Idle()
	{
		if(!currBee.enemy.initIdle){
			currBee.enemy.initIdle = true;
			currBee.enemy.TIdle = currBee.enemy.idleDuration;
			deltaTime = Time.deltaTime;

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
			deltaTime = Time.deltaTime;
		}else{
			currBeeRigidbody.position = 
				Vector2.MoveTowards(
					currBeeRigidbody.position,
					currBee.enemy.patrolDestination,
					currBee.enemy.patrolSpeed * deltaTime
				);

			if(Vector2.Distance(currBee.enemy.patrolDestination,currBeeRigidbody.position) < 0.1f){
				currBee.enemy.state = EnemyState.Idle;
				currBee.enemy.initPatrol = false;
			}
		}
	}

	void Chase()
	{
		currBeeAnim.Play(EnemyState.Idle.ToString());
		if(currBee.enemy.isAttack){
			currBee.enemy.state = EnemyState.Attack;
			currBee.enemy.attackObject.transform.position = currBee.enemy.playerTransform.position;
		}else{
			currBeeRigidbody.position = 
				Vector2.MoveTowards(
					currBeeRigidbody.position,
					currBee.enemy.playerTransform.position,
					currBee.enemy.chaseSpeed * deltaTime
				);

			if(Vector2.Distance(currBeeRigidbody.position,currBee.enemy.playerTransform.position) >= currBee.enemy.chaseRange){
				currBee.enemy.state = EnemyState.Idle;
				currBee.enemy.playerTransform = null;
				if(currBee.beeHiveTransform == null) currBee.isStartled = true;
			}
		}
	}

	void Attack()
	{
		if(!currBee.enemy.initAttack){
			if(!currBee.enemy.isAttack){
				currBee.enemy.state = EnemyState.Chase;
			}else{
				currBee.enemy.initAttack = true;
				currBeeAnim.Play(EnemyState.Attack.ToString());
			}
		}else{
			currBee.enemy.attackObject.SetActive(currBee.enemy.attackHit);
		}
	}	

	void Startled()
	{
		if(!currBee.initStartled){
			currBee.beeHiveTransform = null;
			currBee.enemy.initIdle = false;
			currBee.enemy.initPatrol = false;

			deltaTime = Time.deltaTime;
			currBee.initStartled = true;

			currBee.enemy.patrolDestination = GetRandomPatrolPos(currBeeRigidbody.position,currBee.startledRange);
		}else{
			currBeeRigidbody.position = 
				Vector2.MoveTowards(
					currBeeRigidbody.position,
					currBee.enemy.patrolDestination,
					currBee.enemy.chaseSpeed * deltaTime
				);

			if(Vector2.Distance(currBeeRigidbody.position,currBee.enemy.patrolDestination) < 0.1f){
				currBee.initStartled = false;
				currBee.enemy.state = EnemyState.Idle;
			}
		}
	}

	Vector2 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float x,y;
		if(currBee.initStartled){
			x = Random.value < 0.5f ? range + origin.x : (-1*range) + origin.x;
			y = Random.value < 0.5f ? range + origin.y : (-1*range) + origin.y;
		}else{
			x = Random.Range(-1 * range, range) + origin.x;
			y = Random.Range(-1 * range, range) + origin.y;
		}
		
		return new Vector2(x,y);
	}
}