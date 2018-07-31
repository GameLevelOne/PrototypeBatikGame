using Unity.Entities;
using UnityEngine;

public class EnemyMovementSystem : ComponentSystem {
	
	public struct EnemyMovementComponent{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Transform> transform;
		public ComponentArray<EnemyMovement> enemyMovement;
		public ComponentArray<Rigidbody2D> rigidbody;
	}

	#region injected component
	[InjectAttribute] EnemyMovementComponent enemyMovementComponent;
	Enemy currEnemy;
	Transform currTransform;
	EnemyMovement currEnemyMovement;
	Rigidbody2D currRigidbody;
	#endregion
	//system
	[InjectAttribute] EnemyAnimationSystem EnemyAnimationSystem;

	protected override void OnUpdate()
	{
		// for(int i = 0;i<enemyMovementComponent.Length;i++){
		//	currEnemy = enemyMovementComponent.enemy[i];
		// 	currEnemyMovement = enemyMovementComponent.enemyMovement[i];
		// 	currTransform = enemyMovementComponent.transform[i];
		// 	currRigidbody = enemyMovementComponent.rigidbody[i];
		// 	Move();
		// }
	}

	public void InitMove()
	{
		if(!currEnemyMovement.isMoving){
			currEnemyMovement.isMoving = true;
			currEnemyMovement.targetPos = GetRantomPatrolTarget(currTransform);
		}
	}

	void Move()
	{
		if(currEnemyMovement.isMoving){
			float deltaTime = Time.deltaTime;

			currRigidbody.position = Vector2.MoveTowards(currRigidbody.position, currEnemyMovement.targetPos, currEnemyMovement.speed * deltaTime);

			//if arrived, set back to idle
		}
	}

	Vector2 GetRantomPatrolTarget(Transform current)
	{
		Vector3 currPos = current.position;
		float xRnd = currPos.x += Random.Range(-3f,3f);
		float yRnd = currPos.y += Random.Range(-3f,3f);

		return new Vector3(xRnd,yRnd);
	}
}