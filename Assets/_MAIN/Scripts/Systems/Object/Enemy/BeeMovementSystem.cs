using Unity.Entities;
using UnityEngine;

public class BeeMovementSystem : ComponentSystem {
	public struct BeeMovementComponent{
		public readonly int Length;
		public ComponentArray<Bee> bee;
		public ComponentArray<BeeMovement> beeMovement;
		public ComponentArray<Rigidbody2D> beeRigidbody;
	}

	#region injected component
	[InjectAttribute] BeeMovementComponent beeMovementComponent;
	Bee currBee;
	BeeMovement currBeeMovement;
	Rigidbody2D currBeeRigidbody;
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
			CheckBeeMovement();
		}
	}

	void CheckBeeMovement()
	{
		if(currBee.beeState == BeeState.Idle){
			Idle();
		}else if(currBee.beeState == BeeState.Patrol){
			Patrol();
		}else if(currBee.beeState == BeeState.Chase){

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
			currBeeMovement.patrolDestination = GetRandomPatrolPos(currBeeMovement.beeHiveTransform.position);
			deltaTime = Time.deltaTime;
		}else{
			currBeeRigidbody.position = Vector2.MoveTowards(currBeeRigidbody.position,currBeeMovement.patrolDestination,currBeeMovement.patrolSpeed * deltaTime);

			if(Vector2.Distance(currBeeMovement.patrolDestination,currBeeRigidbody.position) < 0.1f){
				currBee.beeState = BeeState.Idle;
				currBeeMovement.initPatrol = false;
			}
		}
	}

	void Chase()
	{

	}

	Vector2 GetRandomPatrolPos(Vector3 origin)
	{
		float x = Random.Range(-2f,2f) + origin.x;
		float y = Random.Range(-2f,2f) + origin.y;

		return new Vector2(x,y);
	}
}
