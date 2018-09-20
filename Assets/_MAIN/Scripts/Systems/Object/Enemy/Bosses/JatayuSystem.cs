using Unity.Entities;
using UnityEngine;

public class JatayuSystem : ComponentSystem {

	public struct JatayuComponent{
		public readonly int Length;
		public ComponentArray<Boss> boss;
		public ComponentArray<Transform> jatayuTransform;
		public ComponentArray<Jatayu> jatayu;
		public ComponentArray<Animator> jatayuAnim;
		public ComponentArray<Rigidbody> jatayuRb;
		public ComponentArray<Health> jatayuHealth;
	}

	[InjectAttribute] JatayuComponent jatayuComponent;
	Boss boss;
	Transform jatayuTransform;
	Jatayu jatayu;
	Animator jatayuAnim;
	Rigidbody jatayuRb;
	Health jatayuHealth;

	float deltaTime;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		for(int i = 0;i<jatayuComponent.Length;i++){
			boss = jatayuComponent.boss[i];
			jatayuTransform = jatayuComponent.jatayuTransform[i];
			jatayu = jatayuComponent.jatayu[i];
			jatayuAnim = jatayuComponent.jatayuAnim[i];
			jatayuRb = jatayuComponent.jatayuRb[i];
			jatayuHealth = jatayuComponent.jatayuHealth[i];

			CheckState();
			FacePlayer();
		}
	}

#region state
	void CheckState()
	{
		if(jatayu.state == JatayuState.Entrance){
			Entrance();
		}else if(jatayu.state == JatayuState.Move){
			Move();
		}else if(jatayu.state == JatayuState.CloseWings){
			CloseWings();
		}else if(jatayu.state == JatayuState.FlapFast){
			FlapFast();
		}else if(jatayu.state == JatayuState.Attack1){
			Attack1();
		}else if(jatayu.state == JatayuState.Attack2){
			Attack2();
		}else if(jatayu.state == JatayuState.Attack3){
			Attack3();
		}else if(jatayu.state == JatayuState.HP50){
			HP50();
		}else if(jatayu.state == JatayuState.Hit){
			Hit();
		}else if(jatayu.state == JatayuState.Burned){
			Burned();
		}else if(jatayu.state == JatayuState.Die){
			Die();
		}
	}

	void Entrance()
	{
		if(!jatayu.initEntrance){
			jatayu.initEntrance = true;

		}else{

		}
	}
	
	void Move()
	{
		if(!jatayu.initMove){
			jatayu.initMove = true;

		}else{

		}
	}

	void CloseWings()
	{
		if(!jatayu.initCloseWings){
			jatayu.initCloseWings = true;

		}else{

		}
	}

	void FlapFast()
	{
		if(!jatayu.initFlapFast){
			jatayu.initFlapFast = true;

		}else{

		}
	}

	void Attack1()
	{
		if(!jatayu.initAttack1){
			jatayu.initAttack1 = true;

		}else{

		}
	}

	void Attack2()
	{
		if(!jatayu.initAttack2){
			jatayu.initAttack2 = true;

		}else{

		}
	}

	void Attack3()
	{
		if(!jatayu.initAttack3){
			jatayu.initAttack3 = true;


		}else{

		}
	}

	void HP50()
	{
		if(!jatayu.initHP50){
			jatayu.initHP50 = true;

		}else{

		}
	}

	void Hit()
	{
		if(!jatayu.initHit){
			jatayu.initHit = true;

		}else{

		}
	}

	void Burned()
	{
		if(!jatayu.initBurned){
			jatayu.initBurned = true;

		}else{

		}
	}

	void Die()
	{
		if(!jatayu.initDie){
			jatayu.initDie = true;

		}else{

		}
	}
#endregion

	void FacePlayer()
	{
		if(jatayu.tDelayFace <= 0f){
			jatayu.tDelayFace = 1f;

			if(jatayu.state != JatayuState.Burned && jatayu.state != JatayuState.Hit){
				float delta = jatayu.playerTransform.position.x - jatayu.headSprite.transform.position.x;
				if(delta >= 0) jatayu.headSprite.flipX = true;
				else jatayu.headSprite.flipX = false;
			}
			

		}else jatayu.tDelayFace -= deltaTime;
	}

#region mechanic
	void SetJatayuState(JatayuState state)
	{
		if(jatayu.state != state) jatayu.state = state;

		//set invulnerability
		if(jatayu.state == JatayuState.Entrance || 
		jatayu.state == JatayuState.CloseWings || 
		jatayu.state == JatayuState.HP50) 
			jatayu.invulnerable = true;

		else jatayu.invulnerable = false;

		//disable facing player
		if(jatayu.state == JatayuState.Burned || 
		jatayu.state == JatayuState.Hit) 
			jatayu.headSprite.flipX = false;
	}

	void SetJatayuAnim(JatayuState state)
	{
		jatayuAnim.Play(state.ToString());
	}
#endregion
}
