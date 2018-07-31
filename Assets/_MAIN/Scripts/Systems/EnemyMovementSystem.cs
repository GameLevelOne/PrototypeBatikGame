using Unity.Entities;
using UnityEngine;

public class EnemyMovementSystem : ComponentSystem {
	
	public struct EnemyMovementComponent{
		public readonly int Length;
		public ComponentArray<Transform> transform;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<EnemyMovement> enemyMovement;
		
		public ComponentArray<Rigidbody2D> rigidbody;
	}

	//component
	[InjectAttribute] EnemyMovementComponent enemyMovementComponent;

	//system
	[InjectAttribute] EnemyAnimationSystem EnemyAnimationSystem;

	protected override void OnUpdate()
	{
		for(int i = 0;i<enemyMovementComponent.Length;i++){
			if(enemyMovementComponent.enemy[i].state == EnemyState.Patrol || enemyMovementComponent.enemy[i].state == EnemyState.Chase){
				Move();
			}
		}
	}

	void Move()
	{
		
	}
}