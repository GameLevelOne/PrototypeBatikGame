using UnityEngine;
using Unity.Entities;

public class BeeSystem : ComponentSystem {

	public struct BeeComponent
	{
		public readonly int Length;
		public ComponentArray<Bee> bee;
		public ComponentArray<BeeMovement> beeMovement;
		public ComponentArray<Animator> beeAnim;
		public ComponentArray<Health> beeHealth;
	}

	#region injected component
	[InjectAttribute] public BeeComponent beeComponent;
	Bee currBee;
	BeeMovement currBeeMovement;
	Animator currBeeAnim;
	Health currBeeHealth;
	#endregion

	#region injected system
	[InjectAttribute] public BeeMovementSystem beeMovementSystem;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<beeComponent.Length;i++){
			currBee = beeComponent.bee[i];
			currBeeMovement = beeComponent.beeMovement[i];
			currBeeAnim = beeComponent.beeAnim[i];
			currBeeHealth = beeComponent.beeHealth[i];
			
			CheckHealth();
			CheckState();
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
		if(currBee.beeState == BeeState.Chase){
			Chase();
		}else if(currBee.beeState == BeeState.Attack){
			Attack();
		}

		if(currBeeHealth.HealthPower <= 0f){
			GameObject.Destroy(currBee.gameObject);
			UpdateInjectedComponentGroups();
		}
	}

	void Chase()
	{
		currBeeAnim.Play("Idle");
		if(currBee.isAttack){
			currBee.beeState = BeeState.Attack;
			currBee.beeAttackObject.transform.position = currBee.playerTransform.position;
		}
	}

	void Attack()
	{
		if(!currBee.initAttack){
			if(!currBee.isAttack){
				currBee.beeState = BeeState.Chase;
			}else{
				currBee.initAttack = true;
				currBeeAnim.Play("Attack");
			}
		}else{
			currBee.beeAttackObject.SetActive(currBee.attackHit);
		}
	}	
}
