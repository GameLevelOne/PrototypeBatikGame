using Unity.Entities;
using UnityEngine;

public class EnemyMovementSystem : ComponentSystem {
	
	public struct EnemyMovementComponent{
		public readonly int Length;
		public ComponentArray<Transform> transform;
		public ComponentArray<EnemyMovement> enemyMovement;
		
		public ComponentArray<Rigidbody2D> rigidbody;
	}

	#region injected component
	[InjectAttribute] EnemyMovementComponent enemyMovementComponent;
	Transform currTransform;
	EnemyMovement currEnemyMovement;
	#endregion
	//system
	[InjectAttribute] EnemyAnimationSystem EnemyAnimationSystem;

	protected override void OnUpdate()
	{
		// for(int i = 0;i<enemyMovementComponent.Length;i++){
		// 	currTransform = enemyMovementComponent.transform[i];
		// 	currEnemyMovement = enemyMovementComponent.enemyMovement[i];
			
		// 	Move();
		// }
	}

	public void Move()
	{
		if(!currEnemyMovement.isMoving){
			currEnemyMovement.isMoving = true;
			//create target
		}else{
			//move to target
			//if arrive, set enemy back to idle
		}
	}
}