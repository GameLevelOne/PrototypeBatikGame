﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerAnimationSystem : ComponentSystem {
	public struct AnimationData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] AnimationData animationData;
	
	[InjectAttribute] PlayerAttackSystem playerAttackSystem;
	[InjectAttribute] StandAnimationSystem standAnimationSystem;
	[InjectAttribute] PowerBraceletSystem powerBraceletSystem;
	
	public Facing2D facing;
	public Animator animator;
	
	PlayerInput input;
	Player player;
	Attack attack;
	PlayerTool tool;
	Animation2D anim;

	PlayerState state;
	
	Vector2 moveDir;
	Vector2 currentMove;
	Vector2 currentDir;
	bool isFinishAnyAnim = true;

	public bool isFinishAnyAnimation {
		get {return isFinishAnyAnim;}
		set {
			isFinishAnyAnim = value;
			// Debug.Log(isFinishAnyAnim + " on state " + state);
		}
	}

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (tool == null || attack == null) {
			tool = standAnimationSystem.tool;
			attack = playerAttackSystem.attack;

			return;
		}

		for (int i=0; i<animationData.Length; i++) {
			input = animationData.PlayerInput[i];
			player = animationData.Player[i];
			anim = animationData.Animation[i];
			facing = animationData.Facing[i];
			
			state = player.state;
			animator = anim.animator; 
			int attackMode = input.AttackMode;
			moveDir = input.moveDir;
			

			CheckPlayerState ();
			CheckAnimation ();

			if (CheckIfAllowedToChangeDir()) {
				SetAnimationFaceDirection ();
			} 

			continue; //TEMP

			#region OLD		
			if (state == PlayerState.SLOW_MOTION) {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				// animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.steadyMode);
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_X, -currentMove.x, false);
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_Y, -currentMove.y, true);

				continue;
			} else if (state == PlayerState.RAPID_SLASH) {
				if (attackMode == 1) {
					// SetRapidAttack(0f); //BULLET TIME RAPID SLASH
				} else {
					player.SetPlayerIdle();
				}
				
				StartCheckAnimation();
				continue;
			}

			if (attackMode >= 1) {
				// SetAttack(0f); //SLASH
			} else if (attackMode == -1) {
				// SetAttack(1f); //CHARGE
			} else if (attackMode == -2) {
				// SetAttack(2f); //COUNTER
				Debug.Log("Animation Counter");
			} else if (attackMode == -3) {
				Debug.Log("Steady for crazy standing");
			}

			StartCheckAnimation();
			#endregion OLD
		}
	}

	void CheckPlayerState () {
		if (!isFinishAnyAnimation) return;

		switch (state) {
			case PlayerState.IDLE:
				switch (input.moveMode) {
					case 0: 
						animator.Play(Constants.BlendTreeName.IDLE_STAND);
						break;
					case 1: 
						animator.Play(Constants.BlendTreeName.IDLE_CHARGE);
						break;
					case 2: 
						animator.Play(Constants.BlendTreeName.IDLE_GUARD);
						break;
					case 3: 
						animator.Play(Constants.BlendTreeName.IDLE_SWIM);
						break;
				}
				break;
			case PlayerState.MOVE:
				switch (input.moveMode) {
					case 0: 
						animator.Play(Constants.BlendTreeName.MOVE_RUN);
						break;
					case 1: 
						animator.Play(Constants.BlendTreeName.MOVE_CHARGE);
						break;
					case 2: 
						animator.Play(Constants.BlendTreeName.MOVE_GUARD);
						break;
				}
				// Debug.Log("MOVE");
				break;
			case PlayerState.SWIM: 
				if (moveDir != Vector2.zero) {
					animator.Play(Constants.BlendTreeName.MOVE_SWIM);						
				} else {
					animator.Play(Constants.BlendTreeName.IDLE_SWIM);						
				}
				break;
			case PlayerState.DODGE: 
				animator.Play(Constants.BlendTreeName.MOVE_DODGE);
				break;
			case PlayerState.ATTACK:
				// if (input.slashComboVal.Count == 0) break;
				if (attack.isAttacking) {
					animator.Play(Constants.BlendTreeName.IDLE_STAND); break;
				} else if (input.AttackMode == 1) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_1);						
				} else if (input.AttackMode == 2) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_2);						
				} else if (input.AttackMode == 3) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_3);						
				}
				// Debug.Log("ATTACK");
				break;
			case PlayerState.CHARGE: 
				animator.Play(Constants.BlendTreeName.CHARGE_ATTACK);
				break;
			case PlayerState.DASH: 
				animator.Play(Constants.BlendTreeName.MOVE_DASH);
				break;
			case PlayerState.BOUNCE: 
				animator.Play(Constants.BlendTreeName.IDLE_BRAKE);
				break;
			case PlayerState.BRAKE: 
				animator.Play(Constants.BlendTreeName.IDLE_BRAKE);
				break;
			case PlayerState.USING_TOOL: 
				if (tool.currentTool == ToolType.Bomb) {
					animator.Play(Constants.BlendTreeName.USE_BOMB);
				} else if (tool.currentTool == ToolType.Hammer) {
					animator.Play(Constants.BlendTreeName.USE_HAMMER);
				} else if (tool.currentTool == ToolType.Shovel) {
					animator.Play(Constants.BlendTreeName.USE_SHOVEL);
				} else if (tool.currentTool == ToolType.MagicMedallion) {
					animator.Play(Constants.BlendTreeName.USE_MAGIC_MEDALLION);
				} else if (tool.currentTool == ToolType.Container1 || tool.currentTool == ToolType.Container2 || tool.currentTool == ToolType.Container3 || tool.currentTool == ToolType.Container4) {
					animator.Play(Constants.BlendTreeName.USE_CONTAINER);
				}
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					animator.Play(Constants.BlendTreeName.GRABBING);
				} else if (input.interactValue == 1) {
					if (input.liftingMode == 0) {
						animator.Play(Constants.BlendTreeName.SWEATING_GRAB);
					} else if (input.liftingMode == -1) {
						animator.Play(Constants.BlendTreeName.IDLE_LIFT);
					} else if (input.liftingMode == 1) {
						animator.Play(Constants.BlendTreeName.IDLE_PUSH);
					} else if (input.liftingMode == -2) {
						animator.Play(Constants.BlendTreeName.MOVE_LIFT);
					} else if (input.liftingMode == 2) {
						animator.Play(Constants.BlendTreeName.MOVE_PUSH);
					} else if (input.liftingMode == -3) {
						animator.Play(Constants.BlendTreeName.LIFTING);
					}
				} else if (input.interactValue == 2) {
					if (input.liftingMode == 0) {
						animator.Play(Constants.BlendTreeName.UNGRABBING);
					} else if (input.liftingMode == -1) {
						animator.Play(Constants.BlendTreeName.THROWING_LIFT);
					} else if (input.liftingMode == 1) {
						animator.Play(Constants.BlendTreeName.UNGRABBING);
					}
				}

				break;
			case PlayerState.FISHING:
				if (input.interactValue == 0) {
					animator.Play(Constants.BlendTreeName.THROW_FISH_BAIT);
				} else if (input.interactValue == 1) {
					animator.Play(Constants.BlendTreeName.IDLE_FISHING);
				} else if (input.interactValue == 2) {
					animator.Play(Constants.BlendTreeName.RETURN_FISH_BAIT);
				}
				break;
			case PlayerState.BOW:
				if (input.interactValue == 0) {
					animator.Play(Constants.BlendTreeName.TAKE_AIM_BOW);
				} else if (input.interactValue == 1) {
					animator.Play(Constants.BlendTreeName.AIMING_BOW);
				} else if (input.interactValue == 2) {
					animator.Play(Constants.BlendTreeName.SHOT_BOW);
				}
				break;
			case PlayerState.DIE: 
				animator.Play(Constants.BlendTreeName.IDLE_DIE);
				break;
			case PlayerState.GET_HURT: 
				animator.Play(Constants.BlendTreeName.GET_HURT);
				break;
			case PlayerState.BLOCK_ATTACK: 
				animator.Play(Constants.BlendTreeName.BLOCK_ATTACK);
				break;
		}
	}

	void SetAnimationFaceDirection () {			
		if (currentMove != moveDir) {
			currentMove = moveDir;
			
			if (currentMove == Vector2.zero) {

				if (input.liftingMode == -2) {
					input.liftingMode = -1;
				} else if (input.liftingMode == 2) {
					input.liftingMode = 1;
				}
			} else {
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
				
				if (input.liftingMode == -1) {
					input.liftingMode = -2;
				} else if (input.liftingMode == 1) {
					input.liftingMode = 2;
				}
			}
		}
	}

	void CheckAnimation () {
		if (!anim.IsCheckBeforeAnimation) {
			CheckStartAnimation ();
			anim.IsCheckBeforeAnimation = true;
		} else if (!anim.IsCheckAfterAnimation) {
			CheckEndAnimation ();
			anim.IsCheckAfterAnimation = true;
		}
	}

	void CheckStartAnimation () {
		isFinishAnyAnimation = false;
		
		switch(state) {
			case PlayerState.ATTACK: 
				attack.isAttacking = true;
				break;
			case PlayerState.CHARGE: 
				attack.isAttacking = true;
				break;
			case PlayerState.DODGE:
				//
				break;
			case PlayerState.RAPID_SLASH:
				attack.isAttacking  = true;
				break;
			case PlayerState.BLOCK_ATTACK:
				attack.isAttacking  = true;
				break;
			case PlayerState.GET_HURT:
				//
				break;
			case PlayerState.DASH:
				//
				break;
			case PlayerState.BRAKE:
				//
				break;
			case PlayerState.POWER_BRACELET:
				//
				break;
			case PlayerState.USING_TOOL:
				tool.IsActToolReady = true;
				break;
			case PlayerState.FISHING:
				//
				break;
			case PlayerState.BOW:
				if (input.interactValue == 2) { 
					tool.IsActToolReady = true;
				}

				break;
			case PlayerState.DIE: 
				//
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void StopAttackAnimation () {
		player.IsHitAnEnemy = false;
		input.AttackMode = 0;
		StopAnyAnimation();
	}

	void StopAnyAnimation () {
		isFinishAnyAnimation = true;
		player.SetPlayerIdle();
	}

	void CheckEndAnimation () {
		switch(state) {
			case PlayerState.ATTACK: 
				StopAttackAnimation();
				break;
			case PlayerState.CHARGE: 
				StopAttackAnimation();
				break;
			case PlayerState.DODGE:
				StopAnyAnimation();
				break;
			case PlayerState.RAPID_SLASH:
				input.bulletTimeAttackQty--;
				if (input.bulletTimeAttackQty == 0) {
					player.IsHitAnEnemy = false;
					StopAttackAnimation();
				}
				break;
			case PlayerState.GET_HURT:
				StopAnyAnimation();
				break;
			case PlayerState.DASH:
				//
				StopAnyAnimation();
				break;
			case PlayerState.BRAKE:
				StopAnyAnimation();
				break;
			case PlayerState.USING_TOOL:
				StopAnyAnimation();
				break;
			case PlayerState.FISHING:
				if (input.interactValue == 0) { 
					input.interactValue = 1;
					tool.IsActToolReady = true;
					
					isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) { 
					//
				} else if (input.interactValue == 2) { 
					input.interactValue = 0;
					
					StopAnyAnimation();
				}

				break;
			case PlayerState.BLOCK_ATTACK:
				player.IsPlayerHit = false;
				StopAttackAnimation();
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					input.interactValue = 1;

					PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;

					if (powerBraceletState == PowerBraceletState.GRAB) {
						powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
					} else if (powerBraceletState == PowerBraceletState.CAN_LIFT) {
						powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
						powerBraceletSystem.SetLiftObjectParent();
					}

					isFinishAnyAnimation = true;
				} else if (input.interactValue == 1) {
					if (input.liftingMode == -3) {
						input.liftingMode = -1;
					}
					isFinishAnyAnimation = true;
				} else if (input.interactValue == 2) {
					input.interactValue = 0;

					if (input.liftingMode == -1) {
						powerBraceletSystem.UnSetLiftObjectParent();
						powerBraceletSystem.AddForceRigidbody(facing.DirID);
					} else if (input.liftingMode == 1) {
						powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
					}
					
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
					input.interactValue = 0;
					
					StopAnyAnimation();
				}

				break;
			case PlayerState.DIE: 
					//
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	bool CheckIfAllowedToChangeDir () {
		if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH || state == PlayerState.BOUNCE || state == PlayerState.BRAKE) {
			return false;
		} else {
			return true;
		}
	}

	void StartCheckAnimation () {
	// 	if (!anim.IsCheckBeforeAnimation) {
	// 		CheckBeforeAnimation ();
	// 		anim.IsCheckBeforeAnimation = true;
	// 	} else if (!anim.IsCheckAfterAnimation) {
	// 		CheckAfterAnimation ();
	// 		anim.IsCheckAfterAnimation = true;
	// 	}
	}

	void CheckAttackList () {		
		if (input.slashComboVal.Count == 0) {
			// animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, 0f);
			player.IsHitAnEnemy = false;
			Debug.Log("Stop Attack Animation");
			StopAttackAnimation ();
		} else {
			Debug.Log("Must Stop Attack Animation");
		}
	}

	void CheckBeforeAnimation () {
		// switch (animState) {
		// 	case AnimationState.START_SLASH:
		// 		attack.isAttacking  = true;
		// 		break;
		// 	case AnimationState.START_CHARGE:
		// 		attack.isAttacking  = true;
		// 		break;
		// 	case AnimationState.START_DODGE:
		// 		//
		// 		break;
		// 	case AnimationState.START_COUNTER:
		// 		attack.isAttacking  = true;
		// 		break;
		// 	case AnimationState.START_RAPIDSLASH:
		// 		attack.isAttacking  = true;
		// 		break;
		// 	case AnimationState.START_BLOCK:
		// 		//
		// 		break;
		// 	case AnimationState.START_HURT:
		// 		//
		// 		break;
		// 	// case AnimationState.START_DASH:
		// 	// 	//
		// 	// 	break;
		// 	// case AnimationState.START_BRAKING:
		// 	// 	//
		// 	// 	break;
		// 	case AnimationState.START_GRAB:
		// 		//
		// 		break;
		// 	case AnimationState.START_UNGRAB:
		// 		//
		// 		break;
		// 	case AnimationState.START_LIFT:
		// 		//
		// 		break;
		// 	case AnimationState.START_THROW:
		// 		//
		// 		break;
		// 	case AnimationState.START_FISHING:
		// 		input.interactValue = 1;
		// 		tool.IsActToolReady = true;
		// 		break;
		// 	default:
		// 		Debug.LogWarning ("Unknown Animation played");
		// 		break;
		// }
	}

	void CheckAfterAnimation (AnimationState animState) {
	// 	switch (animState) {
	// 		case AnimationState.AFTER_SLASH:
	// 			// if (input.slashComboVal.Count > 0) {
	// 			// 	int slashComboVal = input.slashComboVal[0];
					
	// 			// 	// animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, slashComboVal);	
		
	// 			// 	Debug.Log("slashComboVal = " + slashComboVal);
	// 			// 	if (slashComboVal == 3) {					
	// 			// 		input.slashComboVal.Clear();
	// 			// 		Debug.Log("slashComboVal Clear");
	// 			// 	} else {
	// 			// 		input.slashComboVal.RemoveAt(0);
	// 			// 		Debug.Log("slashComboVal RemoveAt(0)");
	// 			// 	}

	// 			// 	CheckAttackList ();
	// 			// } else {
	// 				// CheckAttackList ();
	// 			// }
	// 			StopAttackAnimation ();
	// 			break;
	// 		case AnimationState.AFTER_CHARGE:
	// 			// animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
	// 			StopAttackAnimation ();
	// 			break;
	// 		case AnimationState.AFTER_DODGE:
	// 			player.SetPlayerIdle();
	// 			break;
	// 		case AnimationState.AFTER_COUNTER:
	// 			animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
	// 			player.IsPlayerHit = false;
	// 			StopAttackAnimation ();
	// 			break;
	// 		case AnimationState.AFTER_RAPIDSLASH:
	// 			input.bulletTimeAttackQty--;
	// 			if (input.bulletTimeAttackQty == 0) {
	// 				// player.IsRapidSlashing = false;
	// 				player.IsHitAnEnemy = false;
	// 				animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, false);
	// 				StopAttackAnimation();
	// 			}
	// 			break;
	// 		case AnimationState.AFTER_BLOCK:
	// 			if (player.isGuarding) {
	// 				player.SetPlayerIdle();
	// 			}
	// 			break;
	// 		case AnimationState.AFTER_HURT:
	// 			player.SetPlayerIdle();
	// 			break;
	// 		case AnimationState.AFTER_LIFT:
	// 			input.liftingMode = -1;
	// 			break;
	// 		// case AnimationState.AFTER_DASH:
	// 		// 	//
	// 		// 	break;
	// 		// case AnimationState.AFTER_BRAKING:
	// 		// 	//
	// 		// 	break;
	// 		case AnimationState.AFTER_GRAB://case after steady power bracelet, input.interactvalue = 1
	// 			PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;
	// 			input.interactValue = 1;

	// 			if (powerBraceletState == PowerBraceletState.GRAB) {
	// 				powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
	// 			} else if (powerBraceletState == PowerBraceletState.CAN_LIFT) {
	// 				powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
	// 				powerBraceletSystem.SetLiftObjectParent();
	// 			}
	// 			break;
	// 		case AnimationState.AFTER_UNGRAB://case after using power bracelet, bool interact = false (optional)
	// 			input.interactValue = 0;
	// 			powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
	// 			player.SetPlayerIdle();
	// 			break;
	// 		case AnimationState.AFTER_THROW:
	// 			powerBraceletSystem.UnSetLiftObjectParent();
	// 			powerBraceletSystem.AddForceRigidbody(facing.DirID);
	// 			input.interactValue = 0;
	// 			player.SetPlayerIdle();
	// 			break;
	// 		case AnimationState.AFTER_FISHING:
	// 			input.interactValue = 0;
	// 			player.SetPlayerIdle();
	// 			break;
	// 		default:
	// 			Debug.LogWarning ("Unknown Animation played");
	// 			break;
	// 	}
	}

	void SetFaceDir (string animName, float animValue, bool isVertical) {
		Vector2 movement = input.moveDir;
		animator.SetFloat(animName, animValue);
		
		if (isVertical) {
			movement.y = Mathf.RoundToInt(animValue);
		} else {
			movement.x = Mathf.RoundToInt(animValue);
		}

		if (currentDir != movement) {
			currentDir = movement;
			facing.DirID = CheckDirID(currentDir.x, currentDir.y);
		}
	}

	int CheckDirID (float dirX, float dirY) {
		int dirIdx = 0;

		if (dirX == 0) {
			if (dirY > 0) {
				dirIdx = 5;
			} else {
				dirIdx = 1;
			}
		} else if (dirX < 0) {
			if (dirY < 0) {
				dirIdx = 2;
			} else if (dirY > 0) {
				dirIdx = 4;
			} else {
				dirIdx = 3;
			}
		} else if (dirX > 0) {
			if (dirY < 0) {
				dirIdx = 8;
			} else if (dirY > 0) {
				dirIdx = 6;
			} else {
				dirIdx = 7;
			}
		}

		return dirIdx;
	}
}
