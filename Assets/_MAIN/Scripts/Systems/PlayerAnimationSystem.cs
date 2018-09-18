using UnityEngine;
using Unity.Entities;

public class PlayerAnimationSystem : ComponentSystem {
	public struct AnimationData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] AnimationData animationData;
	
	[InjectAttribute] PlayerAttackSystem playerAttackSystem;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] StandAnimationSystem standAnimationSystem;
	[InjectAttribute] PowerBraceletSystem powerBraceletSystem;
	[InjectAttribute] FishingRodSystem fishingRodSystem;
	[InjectAttribute] GainTreasureSystem gainTreasureSystem;
	[InjectAttribute] ChestOpenerSystem chestOpenerSystem;
	// [InjectAttribute] UVAnimationSystem uvAnimationSystem;
	[InjectAttribute] GameFXSystem gameFXSystem;
	
	public Facing2D facing;
	public Animator animator;
	public Animation2D anim;
	
	PlayerInput input;
	Player player;
	Attack attack;
	PlayerTool tool;
	Transform playerTransform;

	PlayerState state;

	public bool isFinishAttackAnimation = true;
	public bool isFinishAnyAnimation = true;
	
	Vector3 moveDir;
	Vector3 currentMoveDir;
	// Vector3 currentDir;
	
	// int attackCombo = 0;
	int currentDirID;
	// int currentAnimMatIndex = 0;

	// bool isFinishAnyAnim = true;
	bool isEnableChargeEffect = false;

	// public bool isFinishAnyAnimation {
	// 	get {return isFinishAnyAnim;}
	// 	set {
	// 		if (!value && state == PlayerState.IDLE) {
	// 			isFinishAnyAnim = true;
	// 		} else {
	// 			isFinishAnyAnim = value;
	// 		}
			
	// 		// Debug.Log(isFinishAnyAnim + " on state " + state);
	// 	}
	// }

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (tool == null || attack == null) {
			tool = standAnimationSystem.tool;
			attack = playerAttackSystem.attack;
			Debug.Log("Player animation won't running");
			return;
		}

		for (int i=0; i<animationData.Length; i++) {
			input = animationData.PlayerInput[i];
			player = animationData.Player[i];
			anim = animationData.Animation[i];
			facing = animationData.Facing[i];
			playerTransform = animationData.Transform[i];
			
			if (!anim.isInitAnimation) {
				InitAnimation();

				continue;
			}

			state = player.state;
			animator = anim.animator; 
			moveDir = input.moveDir;
			
			CheckPlayerState ();
			CheckAnimation ();
			CheckSpawnOnAnimation ();

			if (CheckIfAllowedToChangeDir()) {
				SetAnimationFaceDirection ();
			} 

			continue; //TEMP

#region OLD		
			// if (state == PlayerState.SLOW_MOTION) {
			// 	// animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
			// 	// animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.steadyMode);
			// 	SetFaceDir (Constants.AnimatorParameter.Float.FACE_X, -currentMove.x, false);
			// 	SetFaceDir (Constants.AnimatorParameter.Float.FACE_Y, -currentMove.z, true);

			// 	continue;
			// } else if (state == PlayerState.RAPID_SLASH) {
			// 	if (attackMode == 1) {
			// 		// SetRapidAttack(0f); //BULLET TIME RAPID SLASH
			// 	} else {
			// 		player.SetPlayerIdle();
			// 	}
				
			// 	StartCheckAnimation();
			// 	continue;
			// }

			// if (attackMode >= 1) {
			// 	// SetAttack(0f); //SLASH
			// } else if (attackMode == -1) {
			// 	// SetAttack(1f); //CHARGE
			// } else if (attackMode == -2) {
			// 	// SetAttack(2f); //COUNTER
			// 	Debug.Log("Animation Counter");
			// } else if (attackMode == -3) {
			// 	Debug.Log("Steady for crazy standing");
			// }

			// StartCheckAnimation();
#endregion OLD
		}
	}

	void InitAnimation () {
		isFinishAttackAnimation = true;
		isFinishAnyAnimation = true;
	
		moveDir = Vector3.zero;
		currentMoveDir = Vector3.zero;

		isEnableChargeEffect = false;
		currentDirID = facing.initFacingDirID;
		SetFacingDirID (currentDirID);

		anim.isInitAnimation = true;
	}

	void PlayLoopAnimation (string animName) {
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName)) {
			animator.Play(animName);
		}
	}

	void PlayOneShotAnimation (string animName) {
		animator.Play(animName);
	}

	void CheckPlayerState () {
		if (!isFinishAnyAnimation && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
			Debug.Log("TEST");
			StopAnyAnimation ();
			return;
		}
		
		if (state == PlayerState.DODGE) {
			isFinishAnyAnimation = true;
			PlayOneShotAnimation(Constants.BlendTreeName.MOVE_DODGE);
		} else {
			gameFXSystem.ToggleDodgeFlag(false);
			int attackMode = input.attackMode;

			switch (state) {
				case PlayerState.IDLE:
					isFinishAnyAnimation = true;
					switch (input.moveMode) {
						case 0: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_STAND);
							// AnimationMaterialIndex = 0;
							break;
						case 1: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_CHARGE);
							// AnimationMaterialIndex = 1;
							break;
						case 2: 
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_GUARD);;
							// AnimationMaterialIndex = 2;
							break;
					}
					break;
				case PlayerState.MOVE:
					isFinishAnyAnimation = true;
					switch (input.moveMode) {
						case 0: 
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_RUN);
							break;
						case 1: 
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_CHARGE);
							break;
						case 2: 
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_GUARD);
							break;
					}

					// Debug.Log("Player Move "+input.moveMode.ToString());
					break;
				case PlayerState.SWIM: 
					isFinishAnyAnimation = true;
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.GRABBING); //TEMP
					} else if (input.interactValue == 1) {
						if (moveDir != Vector3.zero) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_SWIM);						
						} else {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_SWIM);						
						}	
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING); //TEMP
					}
					
					break;
				// case PlayerState.DODGE: 
				// 	isFinishAnyAnimation = true;
				// 	PlayOneShotAnimation(Constants.BlendTreeName.MOVE_DODGE);
				// 	break;
				// case PlayerState.COUNTER: 
				// 	if (input.AttackMode == -2) {
				// 		PlayOneShotAnimation(Constants.BlendTreeName.COUNTER_ATTACK);
				// 	}
				// 	break;
				case PlayerState.PARRY: 
					PlayOneShotAnimation(Constants.BlendTreeName.PARRY);
					break;
				case PlayerState.SLOW_MOTION: 
					if (input.attackMode == -3) {
						// facing.DirID = CheckDirID(-currentDir.x, -currentDir.z); //OLD
						// ReverseDir();

						PlayOneShotAnimation(Constants.BlendTreeName.IDLE_BULLET_TIME);
					}

					break;
				case PlayerState.RAPID_SLASH: 
					if (attackMode == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_1);
					} else if (attackMode == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_2);	
					} else if (attackMode == 3) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_3);	
					} else if (attackMode == -3) {
						PlayOneShotAnimation(Constants.BlendTreeName.RAPID_SLASH_BULLET_TIME);	
					}

					break;
				case PlayerState.ATTACK:
					if (attackMode == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_1);
					} else if (attackMode == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_2);	
					} else if (attackMode == 3) {
						PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_3);	
					} 
					
					isFinishAnyAnimation = true;
					
					// if (input.slashComboVal.Count > 0) {
					// 	attackCombo = input.slashComboVal[0];
					// 	// Debug.Log("ATTACK COMBO = "+attackCombo);
					// 	if (attackCombo == 1) {
					// 		PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_1);
					// 	} else if (attackCombo == 2) {
					// 		PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_2);
					// 	} else if (attackCombo == 3) {
					// 		PlayOneShotAnimation(Constants.BlendTreeName.NORMAL_ATTACK_3);	
					// 	}
					// } else { //TEMP SOLUTION FOR STUCK
					// 	if (!attack.isAttacking) {
					// 		StopAttackAnimation();
					// 	}
					// } 
					
					break;
				case PlayerState.CHARGE:
					// player.isMoveAttack = true;
					PlayOneShotAnimation(Constants.BlendTreeName.CHARGE_ATTACK);
					break;
				case PlayerState.DASH: 
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.IDLE_DASH);
						attack.isDashing = false;
					} else if (input.interactValue == 1) {
						PlayOneShotAnimation(Constants.BlendTreeName.MOVE_DASH);
						attack.isDashing = true;
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.IDLE_BRAKE);
						attack.isDashing = false;
					}
					break;
				case PlayerState.USING_TOOL: 
					// if (tool.currentTool == ToolType.Bomb) {
					// 	PlayOneShotAnimation(Constants.BlendTreeName.USE_BOMB);
					// }
					
					if (tool.currentTool == ToolType.Hammer) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_HAMMER);
					} else if (tool.currentTool == ToolType.Shovel) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_SHOVEL);
					} else if (tool.currentTool == ToolType.MagicMedallion) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_MAGIC_MEDALLION);
					} else if (tool.currentTool == ToolType.Container1 || tool.currentTool == ToolType.Container2 || tool.currentTool == ToolType.Container3 || tool.currentTool == ToolType.Container4) {
						PlayOneShotAnimation(Constants.BlendTreeName.USE_CONTAINER);
					}
					break;
				case PlayerState.POWER_BRACELET:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.GRABBING);
					} else if (input.interactValue == 1) {
						if (input.liftingMode == 0) {
							PlayLoopAnimation(Constants.BlendTreeName.SWEATING_GRAB);
						} else if (input.liftingMode == -1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_LIFT);
						} else if (input.liftingMode == 1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_PUSH);
						} else if (input.liftingMode == -2) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_LIFT);
						} else if (input.liftingMode == 2) {
							PlayLoopAnimation(Constants.BlendTreeName.MOVE_PUSH);
						} else if (input.liftingMode == -3) {
							PlayOneShotAnimation(Constants.BlendTreeName.LIFTING);
						}
					} else if (input.interactValue == 2) {
						// Debug.Log("Throw anim");
						if (input.liftingMode == 0) {
							PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING);
						} else if (input.liftingMode == -1) {
							PlayOneShotAnimation(Constants.BlendTreeName.THROWING_LIFT);
						} else if (input.liftingMode == 1) {
							PlayOneShotAnimation(Constants.BlendTreeName.UNGRABBING);
						}
					}

					break;
				case PlayerState.BOW:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.TAKE_AIM_BOW);
					} else if (input.interactValue == 1) {
						PlayLoopAnimation(Constants.BlendTreeName.AIMING_BOW);
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.SHOT_BOW);
					}
					break;
				case PlayerState.DIE: 
					PlayOneShotAnimation(Constants.BlendTreeName.IDLE_DIE);
					break;
				case PlayerState.GET_HURT: 
					isFinishAnyAnimation = true;
					input.attackMode = 0;
					Debug.Log("Reset AttackMode - GET HURT  - CheckPlayerState");
					PlayOneShotAnimation(Constants.BlendTreeName.GET_HURT);
					break;
				case PlayerState.BLOCK_ATTACK:
					isFinishAnyAnimation = true; 
					PlayOneShotAnimation(Constants.BlendTreeName.BLOCK_ATTACK);
					break;
				case PlayerState.FISHING:
					if (input.interactValue == 0) {
						PlayOneShotAnimation(Constants.BlendTreeName.THROW_FISH_BAIT);
					} else if (input.interactValue == 1) {
						PlayLoopAnimation(Constants.BlendTreeName.IDLE_FISHING);
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.RETURN_FISH_BAIT);
					} else if (input.interactValue == 3) {
						PlayOneShotAnimation(Constants.BlendTreeName.FISHING_FAIL);
					}
					
					break;
				case PlayerState.GET_TREASURE:
					if (input.interactMode == 6) { //GET TREASURE
						if (input.interactValue == 0) { 
							// PlayOneShotAnimation(Constants.BlendTreeName.LIFTING_TREASURE);
						} else if (input.interactValue == 1) {
							PlayLoopAnimation(Constants.BlendTreeName.IDLE_LIFT_TREASURE);
						} else if (input.interactValue == 2) {
							// PlayOneShotAnimation(Constants.BlendTreeName.END_LIFT_TREASURE);
							StopAnyAnimation();

							gainTreasureSystem.UseAndDestroyTreasure();
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
						PlayOneShotAnimation(Constants.BlendTreeName.OPENING_CHEST);
					} else if (input.interactValue == 1) {
						//
					} else if (input.interactValue == 2) {
						PlayOneShotAnimation(Constants.BlendTreeName.AFTER_OPEN_CHEST);
					}
					break;
			}
		}
	}

	void SetAnimationFaceDirection () {			
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
					SetFacingDirID (currentMoveDir.x, currentMoveDir.z);
				} else {
					SetFacingDirection ();
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

	void CheckAnimation () {
		if (!anim.isCheckBeforeAnimation) {
			CheckStartAnimation ();
			anim.isCheckBeforeAnimation = true;
		} else if (!anim.isCheckAfterAnimation) {
			CheckEndAnimation ();
			anim.isCheckAfterAnimation = true;
		}
	}

	void CheckSpawnOnAnimation () {
		if (!anim.isSpawnSomethingOnAnimation) {
			switch(state) {
				case PlayerState.ATTACK: 
					player.isMoveAttack = false;
					attack.isAttacking = true;	
					isFinishAttackAnimation	= true;	
					break;
				case PlayerState.CHARGE: 
					player.isMoveAttack = false;
					attack.isAttacking = true;
					break;
				case PlayerState.RAPID_SLASH:
					attack.isAttacking  = true;
					// isFinishAnyAnimation = true;
					break;
				case PlayerState.BLOCK_ATTACK:
					// attack.isAttacking  = true;
					break;
				// case PlayerState.COUNTER:
				// 	attack.isAttacking  = true;
				// 	break;
				case PlayerState.USING_TOOL:
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

					break;
				default:
					Debug.LogWarning ("Unknown Animation played");
					break;
			}
			
			anim.isSpawnSomethingOnAnimation = true;
		}
	}

	void CheckStartAnimation () {
		isFinishAnyAnimation = false;
		
		switch(state) {
			case PlayerState.ATTACK: 
				player.isMoveAttack = true;
				// attack.isAttacking = true;				
				break;
			case PlayerState.CHARGE: 
				player.isMoveAttack = true;
				// attack.isAttacking = true;
				// isFinishAnyAnimation = true;
				// gameFXSystem.SpawnObj(gameFXSystem.gameFX.chargingEffect, player.playerWeaponPos.position);
				break;
			case PlayerState.DODGE:
				// isFinishAnyAnimation = true;
				// gameFXSystem.SpawnObj(gameFXSystem.gameFX.dodgeEffect, playerTransform.position);
				break;
			case PlayerState.SLOW_MOTION:
				//
				break;
			case PlayerState.RAPID_SLASH:
				// attack.isAttacking  = true;
				break;
			case PlayerState.BLOCK_ATTACK:
				break;
			// case PlayerState.COUNTER:
				// attack.isAttacking  = true;
				// break;
			case PlayerState.PARRY:
				//
				break;
			case PlayerState.GET_HURT:
				// isFinishAnyAnimation = true;
				input.attackMode = 0;
				Debug.Log("Reset AttackMode - GET HURT - CheckStartAnimation");
				gameFXSystem.ToggleEffect(gameFXSystem.gameFX.chargingEffect, false);
				break;
			case PlayerState.DASH:
				//
				break;
			case PlayerState.POWER_BRACELET:
				//
				break;
			case PlayerState.USING_TOOL:
				// tool.isActToolReady = true;
				break;
			case PlayerState.FISHING:
				//
				break;
			case PlayerState.BOW:
				// if (input.interactValue == 2) { 
				// 	if (!player.isUsingStand) {
				// 		attack.isAttacking = true;
				// 	} else {
				// 		tool.isActToolReady = true;
				// 	}
				// }

				break;
			case PlayerState.DIE: 
				//
				break;
			case PlayerState.GET_TREASURE: 
				//
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void StopAttackAnimation () {
		// attack.isAttacking = false;
		StopAnyAnimation();
		// input.attackMode = 0;
		// Debug.Log("Reset AttackMode - StopAttacAnimation");
		player.isHitAnEnemy = false;
	}

	void StopAnyAnimation () {
		player.SetPlayerIdle();
		isFinishAnyAnimation = true;
		isFinishAttackAnimation	= true;	
		input.attackMode = 0;
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
				if (input.isChargingAttack) {
					playerInputSystem.SetMovement(1);
				}

				StopAttackAnimation();
				break;
			case PlayerState.CHARGE: 
				StopAttackAnimation();
				break;
			case PlayerState.DODGE:
				gameFXSystem.gameFX.isEnableDodgeEffect = false;
				// ReverseDir ();
								
				StopAnyAnimation();
				break;
			case PlayerState.SLOW_MOTION:
				isFinishAnyAnimation = true;
				break;
			case PlayerState.RAPID_SLASH:
				if (input.attackMode == -3) {
					input.attackMode = 1;
					animator.speed = 3f;
				} else {
					if (input.attackMode < 3) {
						input.attackMode++;
					} else {
						input.attackMode = 1;
					}

					input.bulletTimeAttackQty--;
				}

				if (input.bulletTimeAttackQty <= 0) {
					player.isHitAnEnemy = false;
					player.enemyThatHitsPlayer = null;
					StopAttackAnimation();
					// input.attackMode = 0;
					
					animator.speed = 1f;
				} else {
					isFinishAnyAnimation = true;
				}

				break;
			case PlayerState.GET_HURT:
				StopAnyAnimation();
				break;
			case PlayerState.DASH:
				if (input.interactValue == 0) { 
					input.interactValue = 1;
					
					isFinishAnyAnimation = true;
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
			// case PlayerState.COUNTER:
			// 	// player.isPlayerHit = false;
			// 	StopAttackAnimation();
			// 	break;
			case PlayerState.PARRY:
				// player.isPlayerHit = false;
				StopAnyAnimation();
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					input.interactValue = 1;

					PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;

					if (powerBraceletState == PowerBraceletState.GRAB) {
						// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
						powerBraceletSystem.SetTargetRigidbodyType(1);
					} else if (powerBraceletState == PowerBraceletState.CAN_LIFT) {
						// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
						powerBraceletSystem.SetTargetRigidbodyType(2);
						powerBraceletSystem.SetLiftObjectParent();
					}

					isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) {
					if (input.liftingMode == -3) {
						input.liftingMode = -1;
					}
					isFinishAnyAnimation = true;
				} else if (input.interactValue == 2) {
					if (input.liftingMode == -1 || input.liftingMode == -2) {
						powerBraceletSystem.UnSetLiftObjectParent(currentDirID);
						powerBraceletSystem.AddForceRigidbody();
						powerBraceletSystem.ResetPowerBracelet();
					} else if (input.liftingMode == 1) {
						// powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
						powerBraceletSystem.SetTargetRigidbodyType(0);
					}
					
					// player.isUsingStand = false;
					StopAnyAnimation();
				}

				break;
			case PlayerState.BOW:
				if (input.interactValue == 0) { 
					input.interactValue = 1;
					
					isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) { 
					//
				} else if (input.interactValue == 2) { 
					if (!player.isUsingStand) {
						StopAttackAnimation();
					} else {
						StopAnyAnimation();
					}
				}

				break;
			case PlayerState.SWIM:
				if (input.interactValue == 0) { 
					input.interactValue = 1;
					
					isFinishAnyAnimation = true;
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
					
					isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) { 
					//
				} else if (input.interactValue == 2) {
					if (input.interactMode == -3) { //AFTER FISHING FAIL
						input.interactValue = 3;
					} else {
						StopAnyAnimation ();
						
						if (fishingRodSystem.fishingRod.isCatchSomething) {
							fishingRodSystem.ProcessFish();
						}
						
						fishingRodSystem.ResetFishingRod();
					}
				} else if (input.interactValue == 3) {
					StopAnyAnimation ();
					fishingRodSystem.ResetFishingRod();
				}

				break;
			case PlayerState.GET_TREASURE: 
				if (input.interactValue == 0) { 
					// input.interactValue = 1;
					
					// isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) { 
					//
				} else if (input.interactValue == 2) { 
					// StopAnyAnimation();

					// gainTreasureSystem.UseAndDestroyTreasure();
				}

				break;
			case PlayerState.OPEN_CHEST: 
				if (input.interactValue == 0) { 
					input.interactValue = 2;
					chestOpenerSystem.OpenChest();
					
					isFinishAnyAnimation = true;
				} 
				// else if (input.interactValue == 1) { 
				// 	//
				// } 
				else if (input.interactValue == 2) { 
					chestOpenerSystem.SpawnTreasure(player.transform.position);
					StopAnyAnimation();
				}

				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	bool CheckIfAllowedToChangeDir () {
		if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH) {
			return false;
		} else {
			return true;
		}
	}

	void SetFacingDirection () {
		if (input.isLockDir) {
			int dirID = input.direction + 1;
			
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

			SetFacingDirID(dirID);
			input.isLockDir = false;
		}
	}

	// void SetFaceDir (string animName, float animValue, bool isVertical) {
	// 	// Vector2 movement = input.moveDir;
	// 	animator.SetFloat(animName, animValue);
		
	// 	if (isVertical) {
	// 		moveDir.z = Mathf.RoundToInt(animValue);
	// 	} else {
	// 		moveDir.x = Mathf.RoundToInt(animValue);
	// 	}

	// 	// if (currentDir != moveDir) {
	// 		currentDir = moveDir;
	// 		SetFacingDirID (currentDir.x, currentDir.z);
	// 	// }
	// }

	void SetFacingDirID (float x, float z) {
		currentDirID = CheckDirID(x, z);
		facing.DirID = currentDirID;

		// uvAnimationSystem.SetMaterial(currentDirID-1);
	}

	void SetFacingDirID (int dirID) {
		currentDirID = dirID;
		facing.DirID = currentDirID;
	}

	void ReverseDir () {
		// animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, -currentDir.x);
		// animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, -currentDir.z);
		// SetFacingDirID (-currentDir.x, -currentDir.z);
		input.moveDir = -currentMoveDir;
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
