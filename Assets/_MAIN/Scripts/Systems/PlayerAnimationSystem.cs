using UnityEngine;
using Unity.Entities;
using UnityEngine.PostProcessing;

public class PlayerAnimationSystem : ComponentSystem {
	public struct AnimationData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
		public ComponentArray<Transform> Transform;
		public ComponentArray<Rigidbody> Rigidbody;
	}
	[InjectAttribute] AnimationData animationData;
	
	[InjectAttribute] PlayerAttackSystem playerAttackSystem;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] StandAnimationSystem standAnimationSystem;
	[InjectAttribute] PowerBraceletSystem powerBraceletSystem;
	[InjectAttribute] FishingRodSystem fishingRodSystem;
	[InjectAttribute] GainTreasureSystem gainTreasureSystem;
	[InjectAttribute] ChestOpenerSystem chestOpenerSystem;
	[InjectAttribute] GameFXSystem gameFXSystem;
	
	public Facing2D facing;
	public Animator animator;
	public Animation2D anim;
	
	PlayerInput input;
	Player player;
	Attack attack;
	PlayerTool tool;
	Transform playerTransform;
	Rigidbody playerRigidbody;

	PlayerState state;
	
	Vector3 moveDir;
	Vector3 currentMoveDir;
	// Vector3 currentDir;
	
	int currentDirID;

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (tool == null || attack == null) {
			tool = standAnimationSystem.tool;
			attack = playerAttackSystem.attack;
			// Debug.Log("Player animation won't running");
			return;
		}

		for (int i=0; i<animationData.Length; i++) {
			input = animationData.PlayerInput[i];
			player = animationData.Player[i];
			anim = animationData.Animation[i];
			facing = animationData.Facing[i];
			playerTransform = animationData.Transform[i];
			playerRigidbody = animationData.Rigidbody[i];
			
			state = player.state;
			animator = anim.animator; 
			moveDir = input.moveDir;

			if (!anim.isInitAnimation) {
				InitAnimation();
			} else {
				if (CheckIfAllowedToChangeDir()) {
					SetAnimationFaceDirection ();
				} 

				CheckPlayerState ();
				CheckStartAnimation ();
				CheckSpawnOnAnimation ();
				CheckEndAnimation ();
			}
		}
	}

	void InitAnimation () {	
		moveDir = Vector3.zero;
		currentMoveDir = Vector3.zero;

		// isEnableChargeEffect = false;
		currentDirID = facing.initFacingDirID;
		SetFacingDirID (currentDirID);
		gameFXSystem.ToggleRunFX(false);
		anim.isFinishAnyAnimation = true;
		anim.isFinishAttackAnimation = true;

		anim.isInitAnimation = true;
	}

	void SetAnimation (string animName, bool finishAnimValue) {
		animator.Play(animName);
		anim.currentAnimName = animName;
		anim.isFinishAnyAnimation = finishAnimValue;

		if (state == PlayerState.IDLE || state == PlayerState.MOVE) {
			anim.isFinishAttackAnimation = true;
		}
	}

	void PlayLoopAnimation (string animName, bool finishAnimValue) {
		if (anim.currentAnimName != animName) {
			SetAnimation(animName, finishAnimValue);

			//POWER BRACELET BUG FIX
			if (animName == Constants.BlendTreeName.IDLE_LIFT || animName == Constants.BlendTreeName.MOVE_LIFT) {
				powerBraceletSystem.SetLiftObjectParent();
			}

			//CHARGE BUG FIX (&)
			if (animName != Constants.BlendTreeName.IDLE_CHARGE && animName != Constants.BlendTreeName.MOVE_CHARGE) {
				gameFXSystem.ToggleObjectEffect(gameFXSystem.gameFX.chargingEffect, false);
			}
		}
	}

	void PlayOneShotAnimation (string animName, bool finishAnimValue) {
		if (anim.currentAnimName != animName) {
			SetAnimation(animName, finishAnimValue);

			//Check Start Animation
			anim.isCheckBeforeAnimation = false;
			// if (animName == Constants.BlendTreeName.NORMAL_ATTACK_1) {
			// 	// Debug.Log("Play Anim Slash 1");
			// }
		} 
	}

	void CheckPlayerState () {
		if (anim.isFinishAnyAnimation) {
			int attackMode = input.attackMode;

			switch (state) {
				case PlayerState.IDLE:
					switch (input.moveMode) {
						case 0: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_STAND, true);
							break;
						case 1: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_CHARGE, true);
							break;
						case 2: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_GUARD, true);
							break;
					}

					break;
				case PlayerState.MOVE:
					switch (input.moveMode) {
						case 0: 
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_RUN, true);
							break;
						case 1: 
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_CHARGE, true);
							break;
					}
					break;
				case PlayerState.DODGE:
					PlayOneShotAnimation(Constants.BlendTreeName.MOVE_DODGE, true);
					break;
				case PlayerState.SWIM: 
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.GRABBING, true); //TEMP
					} else if (input.interactValue == 1) {
						if (moveDir != Vector3.zero) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_SWIM, true);						
						} else {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_SWIM, true);						
						}	
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING, true); //TEMP
					}
					
					break;
				case PlayerState.PARRY: 
					PlayOneShotAnimation(Constants.BlendTreeName.PARRY, true);
					break;
				case PlayerState.SLOW_MOTION:
					if (input.attackMode == -3) {
						// facing.DirID = CheckDirID(-currentDir.x, -currentDir.z); //OLD
						// ReverseDir();

						PlayOneShotAnimation(Constants.BlendTreeName.IDLE_BULLET_TIME, true);
					}
					break;
				case PlayerState.ENGAGE: 
					PlayLoopAnimation(Constants.BlendTreeName.MOVE_DASH, true);
					break;
				case PlayerState.RAPID_SLASH: 
					if (attackMode == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_1, false);
					} else if (attackMode == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_2, false);
					} else if (attackMode == 3) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_3, false);
					} else if (attackMode == -3) {
						PlayOneShotAnimation(Constants.BlendTreeName.RAPID_SLASH_BULLET_TIME, false);
					}
					break;
				case PlayerState.ATTACK:
					if (attackMode == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_1, true);
					} else if (attackMode == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_2, true);
					} else if (attackMode == 3) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_3, true);
					} 
					break;
				case PlayerState.CHARGE:
					PlayOneShotAnimation(Constants.BlendTreeName.CHARGE_ATTACK, false);
					break;
				case PlayerState.DASH: 
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.IDLE_DASH, true);
					} else if (input.interactValue == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.MOVE_DASH, true);
					} else if (input.interactValue == 2) {
						if (player.isBouncing) {
							PlayOneShotAnimation(Constants.BlendTreeName.IDLE_BRAKE, true);
						} else {
							gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dashEffect, false);
							attack.isDashing = false;
							StopAnyAnimation();
						}
					}
					break;
				case PlayerState.USING_TOOL: 
					if (tool.currentTool == ToolType.Hammer) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_HAMMER, false);
					} else if (tool.currentTool == ToolType.Shovel) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_SHOVEL, false);
					} else if (tool.currentTool == ToolType.MagicMedallion) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_MAGIC_MEDALLION, false);
					} 

					break;
				case PlayerState.POWER_BRACELET:
					if (input.interactValue == 0) {
						PowerBracelet powerBracelet = powerBraceletSystem.powerBracelet;

						if (powerBracelet.liftable != null) {
							if (powerBracelet.state == PowerBraceletState.GRAB) {
								// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
								powerBraceletSystem.SetTargetRigidbodyType(1);
							} else if (powerBracelet.state == PowerBraceletState.CAN_LIFT) {
								// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
								powerBraceletSystem.SetTargetRigidbodyType(2);
								// powerBraceletSystem.SetLiftObjectParent();
							}
						} else {
							powerBraceletSystem.ResetPowerBracelet();
						}

						input.interactValue = 1;
						anim.isFinishAnyAnimation = true; //
					} else if (input.interactValue == 1) {
						if (input.liftingMode == 0) {
							PlayLoopAnimation(Constants.BlendTreeName.SWEATING_GRAB, true);
						} else if (input.liftingMode == -1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_LIFT, true);
						} else if (input.liftingMode == 1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_PUSH, true);
						} else if (input.liftingMode == -2) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_LIFT, true);
						} else if (input.liftingMode == 2) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_PUSH, true);
						} else if (input.liftingMode == -3) {
							PlayOneShotAnimation(Constants.BlendTreeName.LIFTING, false);
						}
					} else if (input.interactValue == 2) {
						if (input.liftingMode == 0) {
							PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING, false);
						} else if (input.liftingMode == -1) {
							PlayOneShotAnimation(Constants.BlendTreeName.THROWING_LIFT, false);
						} else if (input.liftingMode == 1) {
							PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING, false);
						}
					}
					
					break;
				case PlayerState.BOW:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.TAKE_AIM_BOW, true);
					} else if (input.interactValue == 1) {
						PlayLoopAnimation(Constants.BlendTreeName.AIMING_BOW, true);
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.SHOT_BOW, true);
					} else if (input.interactValue == -1) {
						StopAnyAnimation();
					}

					break;
				case PlayerState.DIE: 
					PlayOneShotAnimation(Constants.BlendTreeName.IDLE_DIE, false);
					break;
				case PlayerState.GET_HURT: 
					PlayOneShotAnimation(Constants.BlendTreeName.GET_HURT, true);

					break;
				case PlayerState.BLOCK_ATTACK:
					PlayOneShotAnimation(Constants.BlendTreeName.BLOCK_ATTACK, true);
					
					break;
				case PlayerState.FISHING:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.THROW_FISH_BAIT, false);
					} else if (input.interactValue == 1) {
						if (fishingRodSystem.fishingRod.isCatchSomething) {
							PlayLoopAnimation(Constants.BlendTreeName.FISHING_CAUGHT, true);
						} else {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_FISHING, true);
						}
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.RETURN_FISH_BAIT, false);
					} else if (input.interactValue == -1) {
						PlayOneShotAnimation(Constants.BlendTreeName.FISHING_FAIL, false);
					}

					break;
				case PlayerState.GET_TREASURE:
					if (input.interactMode == 6) { //GET TREASURE
						if (input.interactValue == 1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_LIFT_TREASURE, true);
						} else if (input.interactValue == 2) {
							PlayOneShotAnimation(Constants.BlendTreeName.END_LIFT_TREASURE, false);
						}
					} 
					// else if (input.interactMode == 7) { //GET BIG TREASURE
					// 	if (input.interactValue == 0) {
					// 		Debug.Log("LIFT UP TREASURE ANIMATION");
					// 	} else if (input.interactValue == 1) {
					// 		Debug.Log("LIFTING TREASURE ANIMATION");
					// 	} else if (input.interactValue == 2) {
					// 		Debug.Log("LIFT DOWN TREASURE ANIMATION");
					// 	}
					// }

					break;
				case PlayerState.OPEN_CHEST:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.OPENING_CHEST, false);
					} else if (input.interactValue == 1) {
						//
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.AFTER_OPEN_CHEST, false);
					}

					break;
			}
		}
	}

	public void SetAnimationFaceDirection () {			
		if (currentMoveDir != moveDir) {
			currentMoveDir = moveDir;
			
			if (currentMoveDir == Vector3.zero) {

				if (input.liftingMode == -2) {
					input.liftingMode = -1;
				} else if (input.liftingMode == 2) {
					input.liftingMode = 1;
				}
			} else {
				if (state == PlayerState.DODGE) {
					animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, currentMoveDir.x);
					animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, currentMoveDir.z);
					// SetFacingDirID (currentMoveDir.x, currentMoveDir.z);
					SetFacingDirID(CheckDirID(currentMoveDir.x, currentMoveDir.z));
				} else {
					SetFacingDirID(input.direction);
					// SetFacingDirection ();
				}

				// SetFaceDir (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
				// SetFaceDir (Constants.AnimatorParameter.Float.FACE_Y, currentMove.z, true);

				if (input.liftingMode == -1) {
					input.liftingMode = -2;
				} else if (input.liftingMode == 1) {
					input.liftingMode = 2;
				}
			}
		}
	}

	void CheckSpawnOnAnimation () {
		if (anim.isSpawnSomethingOnAnimation) {
			switch(state) {
				case PlayerState.ATTACK: 
					attack.isAttacking = true;	
					anim.isSpawnSomethingOnAnimation = true;
					// anim.isFinishAttackAnimation = true;
					break;
				case PlayerState.CHARGE: 
					attack.isAttacking = true;
					break;
				case PlayerState.RAPID_SLASH:
					attack.isAttacking  = true;
					break;
				case PlayerState.USING_TOOL:
					//
					tool.isActToolReady = true;
					break;
				case PlayerState.BOW:
					if (input.interactValue == 2) { 
						if (!player.isUsingStand) {
							attack.isAttacking = true;
						} else {
							tool.isActToolReady = true;
						}
					}
					Debug.Log("Bow");
					anim.isFinishAttackAnimation = true;
					break;
				default:
					Debug.LogWarning ("Unknown Animation played");
					break;
			}
			
			gameFXSystem.ToggleRunFX(false);
			anim.isSpawnSomethingOnAnimation = false;
		}
	}

	void CheckStartAnimation () {
		if (!anim.isCheckBeforeAnimation) {
			
			// isFinishAnyAnimation = false;
			
			switch(state) {
				case PlayerState.DODGE:
					anim.isFinishAnyAnimation = true;
					anim.isFinishAttackAnimation = true;
					PlaySFXOneShot(AnimationAudio.DODGE);
					break;
				// case PlayerState.COUNTER:
					// attack.isAttacking  = true;
					// break;
				case PlayerState.SWIM:
					//
					break;
				case PlayerState.PARRY:
					//
					PlaySFXOneShot(AnimationAudio.PARRY);
					break;
				case PlayerState.SLOW_MOTION:
					gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dodgeEffect, false);
					// gameFXSystem.PlayCounterChargeEffect();
					// animator.speed = 5f;
					Camera.main.GetComponent<PostProcessingBehaviour>().enabled = true;
					break;
				case PlayerState.ENGAGE:
					//
					break;
				case PlayerState.RAPID_SLASH:
					anim.isSpawnSomethingOnAnimation = true;
					break;
				case PlayerState.ATTACK: 
					// Debug.Log("Start Attack "+input.attackMode);
					player.isMoveAttack = true;	
					anim.isSpawnSomethingOnAnimation = true;
					break;
				case PlayerState.CHARGE: 
					player.isMoveAttack = true;
					anim.isSpawnSomethingOnAnimation = true;
					PlaySFXOneShot(AnimationAudio.CHARGE_RELEASE);
					break;
				case PlayerState.DASH:
					if (input.interactValue == 0) {
						attack.isDashing = false;
					} else if (input.interactValue == 1) {
						attack.isDashing = true;
					} else if (input.interactValue == 2) {
						if (player.isBouncing) {
							PlaySFXOneShot(AnimationAudio.BOUNCE);
						}
						
						gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dashEffect, false);
						attack.isDashing = false;
					}
					break;
				case PlayerState.USING_TOOL:
					if (tool.currentTool == ToolType.Hammer) {
						PlaySFXOneShot(AnimationAudio.HAMMER);
					} else if (tool.currentTool == ToolType.Shovel) {
						PlaySFXOneShot(AnimationAudio.SHOVEL);
					} else if (tool.currentTool == ToolType.MagicMedallion) {
						// anim.audioSource.PlayOneShot();
					}
					break;
				case PlayerState.POWER_BRACELET:
					if (input.liftingMode == -1 || input.liftingMode == -2) {
						powerBraceletSystem.UnSetLiftObjectParent(currentDirID);
						powerBraceletSystem.AddForceRigidbody();
						powerBraceletSystem.ResetPowerBracelet();

						// Debug.Log("Reset PB)");
					} else if (input.liftingMode == 1) {
						// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
						powerBraceletSystem.SetTargetRigidbodyType(0);
					}

					break;
				case PlayerState.BOW:
					if (input.interactValue == 0) {
						anim.isFinishAttackAnimation = true;
					} else if (input.interactValue == 1) {
						// anim.isFinishAttackAnimation = true;
					} else if (input.interactValue == 2) {
						anim.isSpawnSomethingOnAnimation = true;
					}

					break;
				case PlayerState.DIE: 
					PlaySFXOneShot(AnimationAudio.DIE);
					break;
				case PlayerState.GET_HURT:
					input.attackMode = 0;
					anim.isFinishAnyAnimation = true;
					anim.isFinishAttackAnimation = true;
					// PlaySFXOneShot(AnimationAudio.HURT);
					gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dashEffect, false);
					gameFXSystem.ToggleRunFX(false);
					
					//CLOSE CHARGE ATTACK
					input.moveMode = 0;
					gameFXSystem.ToggleObjectEffect(gameFXSystem.gameFX.chargingEffect, false);

					if (powerBraceletSystem.powerBracelet.liftable == null) {
						powerBraceletSystem.ResetPowerBracelet();
					}
					
					break;
				case PlayerState.BLOCK_ATTACK:
					//
					break;
				case PlayerState.FISHING:
					//
					break;
				case PlayerState.GET_TREASURE: 
					//
					break;
				case PlayerState.OPEN_CHEST: 
					//
					break;
				default:
					Debug.LogWarning ("Unknown Animation played");
					break;
			}

			anim.isCheckBeforeAnimation = true;
		}
	}

	// void StopAttackAnimation () {
	// 	// attack.isAttacking = false;
	// 	StopAnyAnimation();
	// 	// input.attackMode = 0;
	// 	// Debug.Log("Reset AttackMode - StopAttacAnimation");
	// 	// player.isHitAnEnemy = false;
	// }

	void StopAnyAnimation () {
		// Debug.Log("StopAnyAnimation");
		player.SetPlayerIdle();
		anim.isFinishAnyAnimation = true;
		anim.isFinishAttackAnimation = true;	
		input.attackMode = 0;
		// input.moveMode = 0;
		input.interactValue = 0;
		input.interactMode = 0;
		// input.liftingMode = 0;
		// Debug.Log("Reset AttackMode - StopAnyAnimation");
	}

	// void CheckAttackCombo () {
	// 	Debug.Log("CheckAttackCombo "+ input.slashComboVal.Count);
	// 	if (input.slashComboVal.Count == 0) {
	// 		// player.isHitAnEnemy = false;
	// 		StopAttackAnimation ();
	// 	} else {
	// 		isFinishAnyAnimation = true;
	// 	}
	// }

	void CheckEndAnimation () {
		if (!anim.isCheckAfterAnimation) {
			switch(state) {
				case PlayerState.ATTACK: 
					// if (input.AttackMode > 0 && input.AttackMode <= 3) {
					// 	if (input.slashComboVal.Count > 0) {			
					// 		if (attackCombo == 3) {					
					// 			input.slashComboVal.Clear();
					// 			// Debug.Log("CheckEndAnimation AttackList CLEAR");
					// 		} else {
					// 			//input.slashComboVal.RemoveAt(0);
					// 		}
					// 		Debug.Log("CheckEndAnimation "+ input.slashComboVal.Count);

					// 		StopAttackAnimation ();
					// 		// CheckAttackCombo ();
					// 	} else {
					// 		StopAttackAnimation ();
					// 	}
					// }
					//player.isHitAnEnemy = false;
					// isFinishAttackAnimation	= true;	
					if (input.isInitChargeAttack) {
						playerInputSystem.SetMovement(1);
					}

					if (moveDir != Vector3.zero) {
						if (!input.isUIOpen) gameFXSystem.ToggleRunFX(true);
					}

					if (anim.isFinishAttackAnimation) {
						// StopAttackAnimation();
						StopAnyAnimation();
					}

					// StopAttackAnimation();
					break;
				case PlayerState.CHARGE: 
					if (moveDir != Vector3.zero) {
						if (!input.isUIOpen) gameFXSystem.ToggleRunFX(true);
					}

					// StopAttackAnimation();
					StopAnyAnimation();
					break;
				case PlayerState.DODGE:
					gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dodgeEffect, false);
					gameFXSystem.ToggleRunFX(false);
					input.moveDir = Vector3.zero;

					StopAnyAnimation();
					break;
				case PlayerState.SLOW_MOTION:
					player.isInitRapidSlash = true;
					animator.speed = input.rapidSlashAnimatorSpeed;
					anim.isFinishAnyAnimation = true;
					break;
				case PlayerState.RAPID_SLASH:
					if (input.attackMode == -3) {
						input.attackMode = 1;
					} else {
						if (input.attackMode < 3) {
							input.attackMode++;
						} else {
							input.attackMode = 1;
						}

						input.bulletTimeAttackQty--;
						// input.textRapidSlashHit.text = input.bulletTimeAttackQty.ToString();
					}

					if (input.bulletTimeAttackQty <= 0) {
						// player.isHitAnEnemy = false;
						player.somethingThatHitsPlayer = null;
						// StopAttackAnimation();
						// input.attackMode = 0;
						// animator.updateMode = AnimatorUpdateMode.Normal;
						Time.timeScale = 1f;
						animator.speed = 1f;
						Camera.main.GetComponent<PostProcessingBehaviour>().enabled = false;
						input.isInitAddRapidSlashQty = false;
						input.imagePressAttack.SetActive(false);
						input.imageRapidSlashHit.SetActive(false);

						StopAnyAnimation();
					} else {
						anim.isFinishAnyAnimation = true;
					}

					break;
				case PlayerState.GET_HURT:
					//
					StopAnyAnimation();
					break;
				case PlayerState.DASH:
					if (input.interactValue == 0) { 
						input.interactValue = 1;
						gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dashEffect, true);
						gameFXSystem.ToggleRunFX(true);
						
						anim.isFinishAnyAnimation = true;
					} else if (input.interactValue == 1) { 
						//
					} else if (input.interactValue == 2) { 
						StopAnyAnimation();
					}

					break;
				case PlayerState.USING_TOOL:
					StopAnyAnimation();
					break;
				case PlayerState.BLOCK_ATTACK:
					// player.isPlayerHit = false;
					StopAnyAnimation();
					break;
				case PlayerState.PARRY:
					// player.isPlayerHit = false;
					StopAnyAnimation();
					break;
				case PlayerState.POWER_BRACELET:
					if (input.interactValue == 1) {
						if (input.liftingMode == -3) {
							if (moveDir == Vector3.zero) {
								input.liftingMode = -1;
							} else {
								input.liftingMode = -2;
							}
							anim.isFinishAnyAnimation = true;
						} else {
							//
						}
					} else if (input.interactValue == 2) {
						StopAnyAnimation();
					}

					break;
				case PlayerState.BOW:
					if (input.interactValue == 0) { 
						input.interactValue = 1;
						
						anim.isFinishAnyAnimation = true;
					} else if (input.interactValue == 1) { 
						//
					} else if (input.interactValue == 2) { 
						if (!player.isUsingStand) {
							// StopAttackAnimation();
							StopAnyAnimation();
						} else {
							StopAnyAnimation();
						}
					}

					break;
				case PlayerState.SWIM:
					if (input.interactValue == 0) { 
						input.interactValue = 1;
						
						anim.isFinishAnyAnimation = true;
					} else if (input.interactValue == 1) { 
						//
					} else if (input.interactValue == 2) { 
						StopAnyAnimation();
					}

					break;
				case PlayerState.DIE: 
						//
					break;
				case PlayerState.FISHING:
					if (input.interactValue == 0) { 
						input.interactValue = 1;
						tool.isActToolReady = true;
						PlaySFXOneShot(AnimationAudio.FISHING_THROW);

						anim.isFinishAnyAnimation = true;
					} else if (input.interactValue == 1) { 
						//
					} else if (input.interactValue == 2) {
						if (input.interactMode == -3) { //AFTER FISHING FAIL
							input.interactValue = -1;
						} else {
							StopAnyAnimation ();
							
							if (fishingRodSystem.fishingRod.isCatchSomething) {
								fishingRodSystem.ProcessFish();
							}
							
							fishingRodSystem.ResetFishingRod();
						}
					} else if (input.interactValue == -1) {
						StopAnyAnimation ();
						fishingRodSystem.ResetFishingRod();
					}

					break;
				case PlayerState.GET_TREASURE: 
					if (input.interactValue == 2) { 
						gainTreasureSystem.UseAndDestroyTreasure();
						StopAnyAnimation();
					}

					break;
				case PlayerState.OPEN_CHEST: 
					if (input.interactValue == 0) { 
						input.interactValue = 2;
						chestOpenerSystem.OpenChest();
						
						anim.isFinishAnyAnimation = true;
					} else if (input.interactValue == 2) { 
						chestOpenerSystem.SpawnTreasure(player.transform.position);
						StopAnyAnimation();
					}

					break;
				default:
					Debug.LogWarning ("Unknown Animation played for Player State : "+state);
					break;
			}

			anim.isCheckAfterAnimation = true;
		}
	}

	bool CheckIfAllowedToChangeDir () {
		if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH || state == PlayerState.RAPID_SLASH || state == PlayerState.SLOW_MOTION || state == PlayerState.ENGAGE) {
			return false;
		} else if (state == PlayerState.ATTACK) {
			if (anim.isFinishAttackAnimation) {
				if (GameInput.IsAttackPressed) {
					return true;	
				} else return false;
			} else return false;
		} else {
			return true;
		}
	}

	void SetFacingDirID (int dirID) {
		currentDirID = dirID;
		facing.DirID = currentDirID;
	
		switch (dirID) {
			case 1:
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, 0f);
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, -1f);
				break;
			case 2:
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, -1f);
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, 0f);
				break;
			case 3:
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, 0f);
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, 1f);
				break;
			case 4:
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, 1f);
				animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, 0f);
				break;
		}
	}

	public void PlaySFXOneShot (AnimationAudio audioType) {
		anim.audioSource.PlayOneShot(anim.audioClip[(int) audioType]);
	}

	public void PlaySFX (AnimationAudio audioType) {
		if (!anim.audioSource.isPlaying) {
			anim.audioSource.clip = anim.audioClip[(int) audioType];
			anim.audioSource.Play();
		}
	}

	int CheckDirID (float dirX, float dirZ) {
		int dirIdx = 0;

		#region 4 Direction
		if (dirX == 0) {
			if (dirZ > 0) {
				dirIdx = 3;
			} else {
				dirIdx = 1;
			}
		} else if (dirX < 0) {
			dirIdx = 2;
		} else if (dirX > 0) {
			dirIdx = 4;
		}
		#endregion

#region 8 Direction
		// if (dirX == 0) {
		// 	if (dirY > 0) {
		// 		dirIdx = 5;
		// 	} else {
		// 		dirIdx = 1;
		// 	}
		// } else if (dirX < 0) {
		// 	if (dirY < 0) {
		// 		dirIdx = 2;
		// 	} else if (dirY > 0) {
		// 		dirIdx = 4;
		// 	} else {
		// 		dirIdx = 3;
		// 	}
		// } else if (dirX > 0) {
		// 	if (dirY < 0) {
		// 		dirIdx = 8;
		// 	} else if (dirY > 0) {
		// 		dirIdx = 6;
		// 	} else {
		// 		dirIdx = 7;
		// 	}
		// }
#endregion

		return dirIdx;
	}

	int CheckNewDirID (float dirX, float dirZ) {
		int dirIdx = 0;

		#region 4 Direction
		if (dirX == 0) {
			if (dirZ > 0) {
				dirIdx = 3;
			} else {
				dirIdx = 1;
			}
		} else if (dirX < 0) {
			dirIdx = 2;
		} else if (dirX > 0) {
			dirIdx = 4;
		}
		#endregion

		return dirIdx;
	}

#region OLD	
	// void ReverseDir () {
	// 	// animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, -currentDir.x);
	// 	// animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, -currentDir.z);
	// 	// SetFacingDirID (-currentDir.x, -currentDir.z);
	// 	input.moveDir = -currentMoveDir;
	// }

	// void SetAnimationMaterials (int animMatIndex) {
	// 	uvAnimationSystem.SetAnimationMaterials(animMatIndex);
	// }

	// public int AnimationMaterialIndex {
	// 	get {return currentAnimMatIndex;}
	// 	set {
	// 		if (currentAnimMatIndex == value) return;
			
	// 		currentAnimMatIndex = value;
	// 		SetAnimationMaterials (currentAnimMatIndex);
	// 	}
	// }

	// void StartCheckAnimation () {
		// if (!anim.isCheckBeforeAnimation) {
		// 	CheckBeforeAnimation ();
		// 	anim.isCheckBeforeAnimation = true;
		// } else if (!anim.isCheckAfterAnimation) {
		// 	CheckAfterAnimation ();
		// 	anim.isCheckAfterAnimation = true;
		// }
	// }

	// void CheckAttackList () {		
		// if (input.slashComboVal.Count == 0) {
			// animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, 0f);
			// player.isHitAnEnemy = false;
			// StopAttackAnimation ();
		// } else {
			// Debug.Log("Must Stop Attack Animation");
		// }
	// }

	// void CheckAfterAnimation (AnimationState animState) {
		// switch (animState) {
		// 	case AnimationState.AFTER_SLASH:
		// 		// if (input.slashComboVal.Count > 0) {
		// 		// 	int slashComboVal = input.slashComboVal[0];			
		// 		// 	// animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, slashComboVal);	
		// 		// 	Debug.Log("slashComboVal = " + slashComboVal);
		// 		// 	if (slashComboVal == 3) {					
		// 		// 		input.slashComboVal.Clear();
		// 		// 		Debug.Log("slashComboVal Clear");
		// 		// 	} else {
		// 		// 		input.slashComboVal.RemoveAt(0);
		// 		// 		Debug.Log("slashComboVal RemoveAt(0)");
		// 		// 	}
		// 		// 	CheckAttackList ();
		// 		// } else {
		// 			// CheckAttackList ();
		// 		// }
		// 		StopAttackAnimation ();
		// 		break;
		// 	case AnimationState.AFTER_CHARGE:
		// 		// animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
		// 		StopAttackAnimation ();
		// 		break;
		// 	case AnimationState.AFTER_DODGE:
		// 		player.SetPlayerIdle();
		// 		break;
		// 	case AnimationState.AFTER_COUNTER:
		// 		animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
		// 		player.isPlayerHit = false;
		// 		StopAttackAnimation ();
		// 		break;
		// 	case AnimationState.AFTER_RAPIDSLASH:
		// 		input.bulletTimeAttackQty--;
		// 		if (input.bulletTimeAttackQty == 0) {
		// 			// player.isRapidSlashing = false;
		// 			player.isHitAnEnemy = false;
		// 			animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, false);
		// 			StopAttackAnimation();
		// 		}
		// 		break;
		// 	case AnimationState.AFTER_BLOCK:
		// 		if (player.isGuarding) {
		// 			player.SetPlayerIdle();
		// 		}
		// 		break;
		// 	case AnimationState.AFTER_HURT:
		// 		player.SetPlayerIdle();
		// 		break;
		// 	case AnimationState.AFTER_LIFT:
		// 		input.liftingMode = -1;
		// 		break;
		// 	// case AnimationState.AFTER_DASH:
		// 	// 	//
		// 	// 	break;
		// 	// case AnimationState.AFTER_BRAKING:
		// 	// 	//
		// 	// 	break;
		// 	case AnimationState.AFTER_GRAB://case after steady power bracelet, input.interactvalue = 1
		// 		PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;
		// 		input.interactValue = 1;

		// 		if (powerBraceletState == PowerBraceletState.GRAB) {
		// 			powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
		// 		} else if (powerBraceletState == PowerBraceletState.CAN_LIFT) {
		// 			powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
		// 			powerBraceletSystem.SetLiftObjectParent();
		// 		}
		// 		break;
		// 	case AnimationState.AFTER_UNGRAB://case after using power bracelet, bool interact = false (optional)
		// 		input.interactValue = 0;
		// 		powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
		// 		player.SetPlayerIdle();
		// 		break;
		// 	case AnimationState.AFTER_THROW:
		// 		powerBraceletSystem.UnSetLiftObjectParent();
		// 		powerBraceletSystem.AddForceRigidbody(facing.DirID);
		// 		input.interactValue = 0;
		// 		player.SetPlayerIdle();
		// 		break;
		// 	case AnimationState.AFTER_FISHING:
		// 		input.interactValue = 0;
		// 		player.SetPlayerIdle();
		// 		break;
		// 	default:
		// 		Debug.LogWarning ("Unknown Animation played");
		// 		break;
		// }
	// } 
#endregion OLD
}
