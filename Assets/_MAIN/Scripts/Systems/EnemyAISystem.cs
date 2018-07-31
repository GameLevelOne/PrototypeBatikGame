using Unity.Entities;
using UnityEngine;

public class EnemyAISystem : ComponentSystem {

	struct EnemyComponent{
		public Enemy enemy;
		public Animator enemyAnimation;
	}

	[InjectAttribute] EnemyComponent enemyComponent;

	// inject enemy movement
	// inject enemy animation
	// inject enemy rigidbody

	protected override void OnUpdate()
	{
		// for(int i = 0;i<enemyComponent.Length;i++){
		// 	CheckState(enemyComponent.enemy[i]);
		// }

		foreach( var e in GetEntities<EnemyComponent>())
		{
			CheckState(e);
		}
	}
	
	void CheckState(EnemyComponent e)
	{
		
		if(e.enemy.state == EnemyState.Idle){
			Idle(e);
		}else if(e.enemy.state == EnemyState.Patrol){
			Patrol(e);
		}else if(e.enemy.state == EnemyState.Chase){
			Chase(e);
		}else if(e.enemy.state == EnemyState.Attack){
			Attack(e);
		}else if(e.enemy.state == EnemyState.Damaged){
			Damaged(e);
		}else if(e.enemy.state == EnemyState.Die){
			Die(e);
		}
	}

	float t;
	bool idleInit = false;

	void Idle(EnemyComponent e)
	{
		if(!idleInit){
			t = e.enemy.idleDuration;
			idleInit = true;
		}else{
			t -= Time.deltaTime;
			if(t <= 0){
				SetEnemyState(EnemyState.Patrol);
				idleInit = false;
			}
		}
		
	}
	void Patrol(EnemyComponent e){}
	void Chase(EnemyComponent e){}
	void Attack(EnemyComponent e){}
	void Damaged(EnemyComponent e){}
	void Die(EnemyComponent e){}

	void SetEnemyState(EnemyState enemyState)
	{
		
	}

	void SetEnemyAnimation(/* any string or enum needed as parameter */)
	{
		//set enemy animation (int trigger bool float)
	}
}
