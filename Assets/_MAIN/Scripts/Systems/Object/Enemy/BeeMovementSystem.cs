using Unity.Entities;
using UnityEngine;

public class BeeMovementSystem : ComponentSystem {
	public struct BeeMovementComponent{
		public readonly int Length;
		public ComponentArray<Transform> beeTransform;
		public ComponentArray<Bee> bee;
		public ComponentArray<BeeMovement> beeMovement;
		public ComponentArray<Rigidbody2D> beeRigidbody;
		public ComponentArray<Animator> beeAnim;
	}

	#region injected component
	[InjectAttribute] BeeMovementComponent beeMovementComponent;
	Bee currBee;
	BeeMovement currBeeMovement;
	Rigidbody2D currBeeRigidbody;
	Animator currBeeAnim;
	#endregion

	#region injected system
	[InjectAttribute] BeeSystem beeSystem;
	[InjectAttribute] EnemyAnimationSystem enemyAnimationSystem;
	#endregion

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i = 0;i<beeMovementComponent.Length;i++){
			currBee = beeMovementComponent.bee[i];
			currBeeMovement = beeMovementComponent.beeMovement[i];
			currBeeRigidbody = beeMovementComponent.beeRigidbody[i];
			CheckPlayer();
			CheckBeeMovement();
		}
	}

	void CheckPlayer()
	{
		if(currBee.beeState == BeeState.Idle || currBee.beeState == BeeState.Patrol){
			if(currBee.playerTransform != null) currBee.beeState = BeeState.Chase;
		}
	}

	void CheckBeeMovement()
	{
		if(currBee.beeState == BeeState.Idle){
			Idle();
		}else if(currBee.beeState == BeeState.Patrol){
			Patrol();
		}else if(currBee.beeState == BeeState.Chase){
			Chase();
		}else if(currBee.beeState == BeeState.Startled){
			Startled();
		}
	}

	void Idle()
	{
		if(!currBeeMovement.initIdle){
			currBeeMovement.initIdle = true;
			currBeeMovement.TIdle = currBeeMovement.idleDuration;
			deltaTime = Time.deltaTime;

		}else{
			currBeeMovement.TIdle -= deltaTime;

			if(currBeeMovement.TIdle <= 0f){
				currBee.beeState = BeeState.Patrol;
				currBeeMovement.initIdle = false;
			}
		}
	}

	void Patrol()
	{
		if(!currBeeMovement.initPatrol){
			currBeeMovement.initPatrol = true;
			
			Vector3 origin = 
				currBee.isStartled ? 
				new Vector3(currBeeRigidbody.position.x,currBeeRigidbody.position.y,0f) : 
				currBeeMovement.beeHiveTransform.position;

			currBeeMovement.patrolDestination = GetRandomPatrolPos(origin,currBeeMovement.patrolRange);
			deltaTime = Time.deltaTime;
		}else{

			currBeeRigidbody.position = 
				Vector2.MoveTowards(
					currBeeRigidbody.position,
					currBeeMovement.patrolDestination,
					currBeeMovement.patrolSpeed * deltaTime
				);

			//Debug.Log("Running");
			if(Vector2.Distance(currBeeMovement.patrolDestination,currBeeRigidbody.position) < 0.1f){
				currBee.beeState = BeeState.Idle;
				currBeeMovement.initPatrol = false;
			}
		}
	}

	void Chase()
	{
		currBeeMovement.initIdle = false;
		currBeeMovement.initPatrol = false;

		currBeeRigidbody.position = 
			Vector2.MoveTowards(
				currBeeRigidbody.position,
				currBee.playerTransform.position,
				currBeeMovement.chaseSpeed * deltaTime
			);

		if(Vector2.Distance(currBeeRigidbody.position,currBee.playerTransform.position) >= currBeeMovement.chaseRange){
			currBee.beeState = BeeState.Idle;
			currBee.playerTransform = null;
			if(currBeeMovement.beeHiveTransform == null) currBee.isStartled = true;
		}
	}

	void Startled()
	{
		if(!currBeeMovement.initStartled){
			currBeeMovement.beeHiveTransform = null;
			currBeeMovement.initIdle = false;
			currBeeMovement.initPatrol = false;

			deltaTime = Time.deltaTime;
			currBeeMovement.initStartled = true;

			

			currBeeMovement.patrolDestination = GetRandomPatrolPos(currBeeRigidbody.position,currBeeMovement.startledRange);
		}else{
			//Debug.Log("STARTLING");
			currBeeRigidbody.position = 
				Vector2.MoveTowards(
					currBeeRigidbody.position,
					currBeeMovement.patrolDestination,
					currBeeMovement.chaseSpeed * deltaTime
				);

			if(Vector2.Distance(currBeeRigidbody.position,currBeeMovement.patrolDestination) < 0.1f){
				currBeeMovement.initStartled = false;
				currBee.beeState = BeeState.Idle;
			}
		}
	}

	Vector2 GetRandomPatrolPos(Vector3 origin, float range)
	{
		float x,y;
		if(currBeeMovement.initStartled){
			x = Random.value < 0.5f ? range + origin.x : (-1*range) + origin.x;
			y = Random.value < 0.5f ? range + origin.y : (-1*range) + origin.y;
		}else{
			x = Random.Range(-1 * range, range) + origin.x;
			y = Random.Range(-1 * range, range) + origin.y;
		}
		
		return new Vector2(x,y);
	}
}