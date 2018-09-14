using Unity.Entities;
using UnityEngine;

public class GhostSystem : ComponentSystem {

	public struct GhostComponent{
		public readonly int Length;
		
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Ghost> ghost;
		public ComponentArray<Animator> ghostAnim;
		public ComponentArray<Rigidbody> ghostRigidbody;
		public ComponentArray<Health> ghostHealth;
	}

	[InjectAttribute] public GhostComponent ghostComponent;
	Enemy currEnemy;
	Ghost currGhost;
	Animator currGhostAnim;
	Rigidbody currGhostRigidbody;
	Health currGhostHealth;

	protected override void OnUpdate()
	{
		for(int i = 0;i<ghostComponent.Length;i++){
			currEnemy = ghostComponent.enemy[i];
			currGhost = ghostComponent.ghost[i];
			currGhostAnim = ghostComponent.ghostAnim[i];
			currGhostRigidbody = ghostComponent.ghostRigidbody[i];
			currGhostHealth = ghostComponent.ghostHealth[i];

			CheckHealth();
			CheckState();
		}
	}

	void CheckHealth(){}

	void CheckState()
	{
		if(currEnemy.state == EnemyState.Idle){

		}else if(currEnemy.state == EnemyState.Patrol){

		}else if(currEnemy.state == EnemyState.Chase){

		}else if(currEnemy.state == EnemyState.Attack){

		}
	}

	
	void Idle()
	{
		
	}
	
	void Patrol()
	{

	}
	
	void Chase()
	{

	}
	
	void Attack()
	{

	}


}