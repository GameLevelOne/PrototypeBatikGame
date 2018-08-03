using Unity.Entities;
using UnityEngine;

public class EnemyAISystem : ComponentSystem {

	struct EnemyComponent
	{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Animator> enemyAnimation;
	}

	#region injected component
	[InjectAttribute] EnemyComponent enemyComponent;

	//current
	Enemy currEnemy;
	Animator currEnemyAnimation;
	#endregion

	#region injected system
	[InjectAttribute] EnemyMovementSystem enemyMovementSystem;
	[InjectAttribute] EnemyAttackSystem enemyAttackSystem;

	#endregion

	protected override void OnUpdate()
	{		
		// for(int i = 0;i<enemyComponent.Length;i++){
		// 	currEnemy = enemyComponent.enemy[i];
		// 	currEnemyAnimation = enemyComponent.enemyAnimation[i];
		// 	CheckState();
		// }
	}
	
	void CheckState()
	{
		// if(currEnemy.state == EnemyState.Idle){
		// 	Idle();
		// }else if(currEnemy.state == EnemyState.Patrol){
		// 	Patrol();
		// }else if(currEnemy.state == EnemyState.Chase){
		// 	Chase();
		// }else if(currEnemy.state == EnemyState.Attack){
		// 	Attack();
		// }else if(currEnemy.state == EnemyState.Damaged){
		// 	Damaged();
		// }else if(currEnemy.state == EnemyState.Die){
		// 	Die();
		// }
	}

	void Idle()
	{
		// if(!currEnemy.initIdle){
		// 	currEnemy.t = currEnemy.idleDuration;
		// 	currEnemy.initIdle = true;
		// }else{
		// 	currEnemy.t -= Time.deltaTime;
		// 	if(currEnemy.t <= 0){
		// 		currEnemy.initIdle = false;
		// 		SetEnemyState(EnemyState.Patrol);
		// 	}
		// }
		
	}
	void Patrol()
	{
		// enemyMovementSystem.InitMove();
	}
	
	void Chase()
	{
		//enemyMovementSystem.InitChase();
	}
	
	void Attack()
	{
		
	}
	
	void Damaged()
	{

	}
	
	void Die()
	{

	}

	// public void SetEnemyState(EnemyState enemyState)
	// {
	// 	currEnemy.state = enemyState;
	// }

	void SetEnemyAnimation(/* any string or enum needed as parameter */)
	{
		//set enemy animation (int trigger bool float)
	}
}