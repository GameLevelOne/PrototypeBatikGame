using Unity.Entities;
using UnityEngine;

public class JatayuSystem : ComponentSystem {

	public struct JatayuComponent{
		public readonly int Length;
		public ComponentArray<Transform> jatayuTransform;
		public ComponentArray<Jatayu> jatayu;
		public ComponentArray<Animator> jatayuAnim;
		// public ComponentArray<Rigidbody> jatayuRb;
		public ComponentArray<Health> jatayuHealth;
	}

	[InjectAttribute] JatayuComponent jatayuComponent;
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
			jatayuTransform = jatayuComponent.jatayuTransform[i];
			jatayu = jatayuComponent.jatayu[i];
			jatayuAnim = jatayuComponent.jatayuAnim[i];
			// jatayuRb = jatayuComponent.jatayuRb[i];
			jatayuHealth = jatayuComponent.jatayuHealth[i];

			CheckInit();

			CheckHealth();
			CheckState();

			CheckHit();
			CheckBurned();

			FacePlayer();
		}
	}

	/// <summary>
    /// run once at start
    /// </summary>
	void CheckInit()
	{
		if(!jatayu.initJatayu){
			jatayu.headSprite.material.color = Color.white;
			jatayu.bodySprite.material.color = Color.white;
			jatayu.initJatayu = true;
			jatayu.maxHealth = jatayuHealth.EnemyHP;
			jatayu.movementAnim.speed = 0f;
			jatayu.movementAnim.Play("Animate",0, (12f/40f) );
			Debug.Log("Finish Init Jatayu");
		}
	}

	/// <summary>
    /// Checks whether jatayu is being hit. Jatayu's body and head color will turn to red and gradually become normal
    /// </summary>
	void CheckHit()
	{
		//if not invulnerable
		if(!jatayu.invulnerable){
			//if jatayu is hit
			if(jatayu.initHit){
				jatayu.initHit = false;

				//if its state is move
				if(jatayu.state == JatayuState.Move){
					//call hit animation
					SetJatayuAnim(JatayuState.Hit);
				}
				SoundManager.Instance.PlaySFX(SFX.JatayuHitWhileFlying);
				jatayu.hitParticle.Play();
				//at anytime, turn the color to red
				jatayu.headSprite.material.color = Color.red;
				jatayu.bodySprite.material.color = Color.red;
				jatayu.isColorFading = true;
			}
		
			//when fading
			if(jatayu.isColorFading){
				//fade color to white gradually
				jatayu.headSprite.material.color = Color.Lerp(Color.white,Color.red,jatayu.tColorFade);
				jatayu.bodySprite.material.color = Color.Lerp(Color.white,Color.red,jatayu.tColorFade);
				jatayu.tColorFade -= deltaTime * jatayu.hitColorFadeSpeed;
				if(jatayu.tColorFade <= 0f){
					jatayu.tColorFade = 1f;
					jatayu.headSprite.material.color = Color.white;
					jatayu.bodySprite.material.color = Color.white;
					jatayu.isColorFading = false;
				}	
			}

			if(jatayu.endHit){ //when animation hit is done
				jatayu.endHit = false;
				//set animation back
				SetJatayuAnim(jatayu.state);
			}
		}else{ //if invulnerable
			//at any state, turn color to white at force
			jatayu.headSprite.material.color = Color.white;
			jatayu.bodySprite.material.color = Color.white;
			jatayu.initHit = false;
		}
	}

	void CheckBurned()
	{
		if(jatayu.isBurned){
			if(jatayu.state != JatayuState.Burned){
				if(jatayu.state == JatayuState.Move){
					SetJatayuState(JatayuState.Burned);
					jatayu.isCBurnedCooldown = true;
					jatayu.tBurnedCooldown = jatayu.burnedCooldown;
				}	
			}
			jatayu.isBurned = false;
		}

		if(jatayu.isCBurnedCooldown){
			jatayu.tBurnedCooldown -= deltaTime;
			if(jatayu.tBurnedCooldown <= 0f){
				jatayu.isCBurnedCooldown = false;
			}
		}
	
		
	}

	void CheckHealth()
	{
		if(jatayuHealth.EnemyHP/jatayu.maxHealth <= 0.5f && !jatayu.initJatayuHP50){
			jatayu.initJatayuHP50 = true;
			SetJatayuState(JatayuState.HP50);
		}

		if(jatayuHealth.EnemyHP <= 0f)
		{
			SetJatayuState(JatayuState.Die);
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
		}else if(jatayu.state == JatayuState.Attack3Return){
			Attack3Return();
		} else if(jatayu.state == JatayuState.HP50){
			HP50();
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
			jatayu.invulnerable = true;
		}else{
			if(jatayu.endEntrance){
				jatayu.initEntrance = false;
				jatayu.endEntrance = false;
				SetJatayuState(JatayuState.Move);
			}
		}
	}
	
	void Move()
	{
		if(!jatayu.initMove){
			jatayu.initMove = true;
			SetAttackCounter();
			SetJatayuAnim(JatayuState.Move);
			jatayu.movementAnim.speed = jatayu.movementAnimSpeed;
		}else{
			jatayuTransform.position = jatayu.movementAnim.transform.position;

			if(jatayu.endMove){
				jatayu.endMove = false;
				ValidateAttackCounter();
			}
		}
	}

	void CloseWings()
	{
		if(!jatayu.initCloseWings){
			jatayu.initCloseWings = true;
			SetJatayuAnim(JatayuState.CloseWings);
		}else{
			if(jatayu.endCloseWings){
				jatayu.initCloseWings = false;
				jatayu.endCloseWings = false;
				SetJatayuState(JatayuState.Attack1);
			}
		}
	}

	void FlapFast()
	{
		if(!jatayu.initFlapFast){
			jatayu.initFlapFast = true;
			SetJatayuAnim(JatayuState.FlapFast);
		}else{
			if(jatayu.endInitFlapFast){
				jatayu.endInitFlapFast = false;
				jatayu.initFlapFast = false;
				SetJatayuState(JatayuState.Attack2);
			}
		}
	}

	void Attack1()
	{
		if(!jatayu.initAttack1){
			jatayu.initAttack1 = true;
			SetJatayuAnim(JatayuState.Attack1);
		}else{
			if(jatayu.endAttack1){
				jatayu.initAttack1 = false;
				jatayu.endAttack1 = false;
				SetJatayuState(JatayuState.Move);
			}
		}
	}

	void Attack2()
	{
		if(!jatayu.initAttack2){
			jatayu.initAttack2 = true;
			SetJatayuAnim(JatayuState.Attack2);
		}else{
			if(jatayu.endAttack2){
				jatayu.endAttack2 = false;
				jatayu.initAttack2 = false;
				SetJatayuState(JatayuState.Move);
			}
		}
	}

	void Attack3()
	{
		if(!jatayu.initAttack3){
			jatayu.initAttack3 = true;
			SetJatayuAnim(JatayuState.Attack3);
			jatayu.jatayuCollider.enabled = false;
		}else{
			if(jatayu.attack3SetPos){
				jatayu.attack3SetPos = false;
				jatayuTransform.position = jatayu.playerTransform.position + new Vector3(0,0,-0.7f);
			}

			if(jatayu.attack3Drop){
				jatayu.attack3Drop = false;
				jatayu.jatayuCollider.enabled = true;
			}

			if(jatayu.endAttack3){
				jatayu.endAttack3 = false;
				jatayu.initAttack3 = false;
				SetJatayuState(JatayuState.Attack3Return);
			}
		}
	}

	void Attack3Return()
	{
		if(!jatayu.initAttack3Return){
			jatayu.initAttack3Return = true;
			SetJatayuAnim(JatayuState.Move);

		}else{
			jatayuTransform.position = Vector3.MoveTowards(jatayuTransform.position,jatayu.movementAnim.transform.position,jatayu.attack3ReturnSpeed * deltaTime);
			if(Vector3.Distance(jatayuTransform.position,jatayu.movementAnim.transform.position) <= 0.1f){
				jatayu.initAttack3Return = false;
				SetJatayuState(JatayuState.Move);
			}
		}
	}

	void HP50()
	{
		if(!jatayu.initHP50){
			jatayu.initHP50 = true;

			jatayu.initAttack1 = false;
			jatayu.initAttack2 = false;
			jatayu.initAttack3 = false;
			jatayu.initFlapFast = false;
			jatayu.initCloseWings = false;
			jatayu.endAttack1 = false;
			jatayu.endAttack2 = false;
			jatayu.endAttack3 = false;
			jatayu.endInitFlapFast = false;
			jatayu.endCloseWings = false;
			jatayu.initMove = false;
			jatayu.endMove = false;

			jatayu.movementAnim.speed = 0f;
			SetJatayuAnim(JatayuState.HP50);
		}else{
			if(jatayu.endHP50){
				jatayu.endHP50 = false;
				jatayu.initHP50 = false;
				SetJatayuState(JatayuState.Move);
			}
		}
	}

	void Burned()
	{
		if(!jatayu.initBurned){
			jatayu.initAttack1 = false;
			jatayu.initAttack2 = false;
			jatayu.initAttack3 = false;
			jatayu.initFlapFast = false;
			jatayu.initCloseWings = false;
			jatayu.endAttack1 = false;
			jatayu.endAttack2 = false;
			jatayu.endAttack3 = false;
			jatayu.endInitFlapFast = false;
			jatayu.endCloseWings = false;
			jatayu.initMove = false;
			jatayu.endMove = false;

			jatayu.initBurned = true;

			jatayu.movementAnim.speed = 0f;
			jatayu.tBurned = jatayu.burnedDuration;
			SetJatayuAnim(JatayuState.Burned);
		}else{
			jatayu.tBurned -= deltaTime;
			if(jatayu.tBurned <= 0f){
				jatayu.initBurned = false;
				
				SetJatayuState(JatayuState.Move);
			}
		}
	}

	Color orange = new Color(1f,165f/255f,0f);
	void Die()
	{
		if(!jatayu.initDie){
			jatayu.initDie = true;
			jatayu.jatayuCollider.enabled = false;
			jatayu.headSprite.material.color = Color.white;
			jatayu.bodySprite.material.color = Color.white;
			SetJatayuAnim(JatayuState.Die);
		}else{

			if(jatayu.initParticleDie){
				jatayu.initParticleDie = false;
				jatayu.particleDie.transform.position = jatayuTransform.position;
				jatayu.particleDie.gameObject.SetActive(true);
			}

			jatayu.tDieColorChange += deltaTime;
			if(jatayu.tDieColorChange >= 0.1f){
				jatayu.tDieColorChange = 0f;
				if(jatayu.headSprite.material.color == Color.white){
					jatayu.headSprite.material.color = Color.red;
					jatayu.bodySprite.material.color = Color.red;
				}else if(jatayu.headSprite.material.color == Color.red){
					jatayu.headSprite.material.color = orange;
					jatayu.bodySprite.material.color = orange;
				}else if(jatayu.headSprite.material.color == orange){
					jatayu.headSprite.material.color = Color.white;
					jatayu.bodySprite.material.color = Color.white;
				}
			}

			if(jatayu.endDie){
				jatayu.endDie = false;
				jatayu.initDie = false;
				jatayu.particleDie.Stop(true,ParticleSystemStopBehavior.StopEmitting);
				GameObject.Destroy(jatayu.gameObject);
				UpdateInjectedComponentGroups();

				//resume timeline
			 	jatayu.timelineEventTrigger.JatayuDie();
			}
		}
	}
#endregion

#region mechanic
	/// <summary>
    /// Set number of swing movement of jatayu until it attacks (randomized between 5 to 7 strikes)
    /// </summary>
	void SetAttackCounter()
	{
		jatayu.AttackCounter = Random.Range(jatayu.minAttackCounter,jatayu.maxAttackCounter);	
	}
	
	/// <summary>
    /// Jatayu attacks when attackCounter is 0. HP above 50% = 70% Attack1, 30% Attack2 | HP below 50% = 40% Attack 1, 30% Attack2, 30% Attack3;
    /// </summary>
	void ValidateAttackCounter()
	{
		if(jatayu.AttackCounter > 0) jatayu.AttackCounter--;
		else{
			jatayu.initMove = false;
			jatayu.movementAnim.speed = 0f;
			

			if(jatayu.testAttack1Only){
				SetJatayuState(JatayuState.CloseWings);
			}else if(jatayu.testAttack2Only){
				SetJatayuState(JatayuState.FlapFast);
			}else if(jatayu.testAttack3Only){
				SetJatayuState(JatayuState.Attack3);
			}else{
				float rnd = Random.value;

				if(!jatayu.initJatayuHP50){
					//if health is more than 50%
					if(rnd < 0.7f){ 
						//CloseWings -> attack1
						SetJatayuState(JatayuState.CloseWings);
					}else{
						//FlapFast -> attack2
						SetJatayuState(JatayuState.FlapFast);
					}
				}else{
					//if health is less than 50%
					if(rnd > 0.6f){ 
						//CloseWings -> attack1
						SetJatayuState(JatayuState.CloseWings);
					}else if(rnd <= 0.6f && rnd > 0.3f){
						//FlapFast -> attack2
						SetJatayuState(JatayuState.FlapFast);
					}else{
						//Attack 3
						SetJatayuState(JatayuState.Attack3);
					}
				}
			}			
		}
	}
#endregion

	void FacePlayer()
	{
		if(jatayu.state != JatayuState.Burned){
			if(jatayu.tDelayFace <= 0f){
				jatayu.tDelayFace = 1f;

				if(jatayu.state != JatayuState.Burned && jatayu.state != JatayuState.Hit){
					float delta = jatayu.playerTransform.position.x - jatayu.headSprite.transform.position.x;
					if(delta >= 0) jatayu.headSprite.flipX = true;
					else jatayu.headSprite.flipX = false;
				}
				

			}else jatayu.tDelayFace -= deltaTime;
		}else{
			jatayu.headSprite.flipX = false;
		}
		
	}

#region mechanic
	void SetJatayuState(JatayuState state)
	{
		if(jatayu.state != state) jatayu.state = state;

		//set invulnerability
		if( jatayu.state == JatayuState.Entrance || 
			jatayu.state == JatayuState.CloseWings || 
			jatayu.state == JatayuState.HP50 || 
			jatayu.state == JatayuState.Attack3)

			jatayu.invulnerable = true;
		else 
			jatayu.invulnerable = false;

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
