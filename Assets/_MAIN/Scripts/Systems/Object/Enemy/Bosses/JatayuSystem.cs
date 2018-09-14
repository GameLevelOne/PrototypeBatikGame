using Unity.Entities;
using UnityEngine;

public class JatayuSystem : ComponentSystem {

	public struct JatayuComponent{
		public readonly int Length;
		public ComponentArray<Boss> boss;
		public ComponentArray<Jatayu> jatayu;
		public ComponentArray<Animator> jatayuAnim;
		public ComponentArray<Rigidbody> jatayuRigidbody;
		public ComponentArray<Health> jatayuHealth;
	}

	[InjectAttribute] public JatayuComponent jatayuComponent;
	Boss currBoss;
	Jatayu currJatayu;
	Animator currJatayuAnim;
	Rigidbody currJatayuRigidbody;
	Health currJatayuHealth;

	protected override void OnUpdate()
	{

		for(int i = 0;i<jatayuComponent.Length;i++){
			currBoss = jatayuComponent.boss[i];
			currJatayu = jatayuComponent.jatayu[i];
			currJatayuAnim = jatayuComponent.jatayuAnim[i];
			currJatayuRigidbody = jatayuComponent.jatayuRigidbody[i];
			currJatayuHealth = jatayuComponent.jatayuHealth[i];
		}
	}

	void CheckState()
	{
		if(currJatayu.state == JatayuState.Appear){

		}else if(currJatayu.state == JatayuState.Idle){

		}else if(currJatayu.state == JatayuState.Attack1){

		}else if(currJatayu.state == JatayuState.Attack2){

		}else if(currJatayu.state == JatayuState.Die){

		}
	}

	void Appear()
	{
		if(!currJatayu.initAppear){
			currJatayu.initAppear = true;
			
		}else{

		}
	}
	void Idle()
	{
		if(!currJatayu.initIdle){
			currJatayu.initIdle = true;

		}else{

		}
	}
	void Attack1()
	{
		if(!currJatayu.initAttack1){
			currJatayu.initAttack1 = true;

		}else{

		}
	}
	void Attack2()
	{
		if(!currJatayu.initAttack2){
			currJatayu.initAttack2 = true;

		}else{

		}
	}
	void Die()
	{
		if(!currJatayu.initDie){
			currJatayu.initDie = true;

		}else{

		}
	}
}
