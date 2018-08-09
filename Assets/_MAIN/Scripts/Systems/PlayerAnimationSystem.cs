using UnityEngine;
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
	
	PlayerInput input;
	Player player;
	Attack attack;
	PlayerTool tool;
	Animation2D anim;

	PlayerState state;
	
	Animator animator;
	Vector2 moveDir;
	Vector2 currentMove;
	Vector2 currentDir;

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (tool == null || attack == null) {
			tool = standAnimationSystem.stand;
			attack = playerAttackSystem.attack;

			return;
		}

		for (int i=0; i<animationData.Length; i++) {
			input = animationData.PlayerInput[i];
			player = animationData.Player[i];
			anim = animationData.Animation[i];
			facing = animationData.Facing[i];

			animator = anim.animator; 
			int attackMode = input.AttackMode;
			moveDir = input.moveDir;
			

			CheckPlayerState ();
			StartCheckAnimation();
			
			// StartCheckAnimation();

			SetAnimationFaceDirection ();
			continue; //TEMP

			#region OLD

			if (state == PlayerState.DIE) {
				Debug.Log("Player Die Animation");
				// input.steadyMode = -1;
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, false);
				// animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.steadyMode);
				continue;
			}

			#region ACTION
			if (state == PlayerState.BLOCK_ATTACK || state == PlayerState.DODGE || state == PlayerState.DASH || state == PlayerState.BOUNCE || state == PlayerState.BRAKE || state == PlayerState.HURT_MOVE || state == PlayerState.POWER_BRACELET || state == PlayerState.FISHING) {
				// Debug.Log("Check IS_INTERACT " + Constants.AnimatorParameter.Bool.IS_INTERACT + ", INTERACT_VALUE " + Constants.AnimatorParameter.Int.INTERACT_VALUE);

				// animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, true);
				animator.Play("INTERACT");

				animator.SetInteger(Constants.AnimatorParameter.Int.INTERACT_VALUE, input.interactValue);
			} else {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, false);
			}
			
			if (state == PlayerState.SLOW_MOTION) {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				// animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.steadyMode);
				SetAnimation (Constants.AnimatorParameter.Float.FACE_X, -currentMove.x, false);
				SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, -currentMove.y, true);

				continue;
			} else if (state == PlayerState.RAPID_SLASH) {
				if (attackMode == 1) {
					SetRapidAttack(0f); //BULLET TIME RAPID SLASH
				} else {
					player.SetPlayerIdle();
				}
				
				StartCheckAnimation();
				continue;
			}

			if (attackMode >= 1) {
				SetAttack(0f); //SLASH
			} else if (attackMode == -1) {
				SetAttack(1f); //CHARGE
			} else if (attackMode == -2) {
				SetAttack(2f); //COUNTER
				Debug.Log("Animation Counter");
			} else if (attackMode == -3) {
				Debug.Log("Steady for crazy standing");
			}

			StartCheckAnimation();
			#endregion

			#region MOVEMENT
			// animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE,input.SteadyMode);
			// animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, input.MoveMode);
			// animator.SetFloat(Constants.AnimatorParameter.Float.INTERACT_MODE, input.InteractMode);
			// animator.SetFloat(Constants.AnimatorParameter.Float.LIFTING_MODE, input.LiftingMode);

			if ((state == PlayerState.USING_TOOL) || (state == PlayerState.HOOK)) {	
				int toolType = (int)tool.currentTool;
				
				if ((toolType >= 8) && (toolType <=10)) {
					toolType = 7;
				}
				
				animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, toolType); 
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, true);
				continue;
			} else if (state == PlayerState.DASH || state == PlayerState.BOUNCE || state == PlayerState.BRAKE) {
				//
				continue;
			} else {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, false);
			}
			
			// SetAnimationFaceDirection ();
			#endregion
			#endregion OLD
		}
	}

	void CheckPlayerState () {
		switch (player.state) {
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
				// Debug.Log("IDLE");
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
				if (input.AttackMode == 1) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_1);						
				} else if (input.AttackMode == 2) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_2);						
				} else if (input.AttackMode == 3) {
					animator.Play(Constants.BlendTreeName.NORMAL_ATTACK_3);						
				}
				// Debug.Log("ATTACK");
				break;
		}
	}

	void SetAnimationFaceDirection () {			
		if (currentMove == moveDir) {
			return;
		} else {
			currentMove = moveDir;
			
			if (currentMove == Vector2.zero) {
				// animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				// player.SetPlayerIdle();

				// if (input.LiftingMode == -2) {
				// 	input.LiftingMode = -1;
				// } else if (input.LiftingMode == 2) {
				// 	input.LiftingMode = 1;
				// }
			} else {
				SetAnimation (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
				SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
				
				// animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, true);
				// if (input.LiftingMode == -1) {
				// 	input.LiftingMode = -2;
				// } else if (input.LiftingMode == 1) {
				// 	input.LiftingMode = 2;
				// }
			}
		}
	}

	void StartCheckAnimation () {
		if (!anim.IsCheckBeforeAnimation) {
			CheckBeforeAnimation (anim.animState);
			anim.IsCheckBeforeAnimation = true;
		} else if (!anim.IsCheckAfterAnimation) {
			CheckAfterAnimation (anim.animState);
			anim.IsCheckAfterAnimation = true;
		}
	}

	void SetAttack (float mode) { //SLASH 0, CHARGE 1, SHOT -1
		animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, mode); 
		// animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, true);
	}

	void SetRapidAttack (float mode) { //RAPID SLASH 0
		animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, mode); 
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, true);
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, true);
	}

	void SetAnimation (string animName, float animValue, bool isVertical) {
		Vector2 movement = input.moveDir;
		animator.SetFloat(animName, animValue);
		
		if (isVertical) {
			movement.y = Mathf.RoundToInt(animValue);
		} else {
			movement.x = Mathf.RoundToInt(animValue);
		}

		if (currentDir == movement) return;

		currentDir = movement;
		
		facing.DirID = CheckDirID(currentDir.x, currentDir.y);
	}

	void CheckBeforeAnimation (AnimationState animState) {
		switch (animState) {
			case AnimationState.START_SLASH:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_CHARGE:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_DODGE:
				//
				break;
			case AnimationState.START_COUNTER:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_RAPIDSLASH:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_BLOCK:
				//
				break;
			case AnimationState.START_HURT:
				//
				break;
			// case AnimationState.START_DASH:
			// 	//
			// 	break;
			// case AnimationState.START_BRAKING:
			// 	//
			// 	break;
			case AnimationState.START_GRAB:
				//
				break;
			case AnimationState.START_UNGRAB:
				//
				break;
			case AnimationState.START_LIFT:
				//
				break;
			case AnimationState.START_THROW:
				//
				break;
			case AnimationState.START_FISHING:
				input.interactValue = 1;
				tool.IsActToolReady = true;
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void CheckAfterAnimation (AnimationState animState) {
		switch (animState) {
			case AnimationState.AFTER_SLASH:
				// if (input.slashComboVal.Count > 0) {
				// 	int slashComboVal = input.slashComboVal[0];
					
				// 	animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, slashComboVal);	
					
				// 	if (slashComboVal == 3) {					
				// 		input.slashComboVal.Clear();
				// 	} else {
				// 		input.slashComboVal.RemoveAt(0);
				// 	}

				// 	CheckAttackList ();
				// } else {
				// 	CheckAttackList ();
				// }
				StopAttackAnimation ();
				break;
			case AnimationState.AFTER_CHARGE:
				animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
				StopAttackAnimation ();
				break;
			case AnimationState.AFTER_DODGE:
				// animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
				// input.IsDodging = false;
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_COUNTER:
				animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
				player.IsPlayerHit = false;
				StopAttackAnimation ();
				break;
			case AnimationState.AFTER_RAPIDSLASH:
				input.bulletTimeAttackQty--;
				if (input.bulletTimeAttackQty == 0) {
					// player.IsRapidSlashing = false;
					player.IsHitAnEnemy = false;
					animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, false);
					StopAttackAnimation();
				}
				break;
			case AnimationState.AFTER_BLOCK:
				if (player.isGuarding) {
					player.SetPlayerIdle();
				}
				break;
			case AnimationState.AFTER_HURT:
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_LIFT:
				input.liftingMode = -1;
				break;
			// case AnimationState.AFTER_DASH:
			// 	//
			// 	break;
			// case AnimationState.AFTER_BRAKING:
			// 	//
			// 	break;
			case AnimationState.AFTER_GRAB://case after steady power bracelet, input.interactvalue = 1
				PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;
				input.interactValue = 1;

				if (powerBraceletState == PowerBraceletState.GRAB) {
					powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
				} else if (powerBraceletState == PowerBraceletState.CAN_LIFT) {
					powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
					powerBraceletSystem.SetLiftObjectParent();
				}
				break;
			case AnimationState.AFTER_UNGRAB://case after using power bracelet, bool interact = false (optional)
				input.interactValue = 0;
				powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_THROW:
				powerBraceletSystem.UnSetLiftObjectParent();
				powerBraceletSystem.AddForceRigidbody(facing.DirID);
				input.interactValue = 0;
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_FISHING:
				input.interactValue = 0;
				player.SetPlayerIdle();
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	// void CheckAttackList () {		
	// 	if (input.slashComboVal.Count == 0) {
	// 		animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, 0f);
	// 		player.IsHitAnEnemy = false;
	// 		Debug.Log("Stop Attack Animation");
	// 		StopAttackAnimation ();
	// 	} else {
			
	// 		Debug.Log("Must Stop Attack Animation");
	// 	}
	// }

	void StopAttackAnimation () {
		// if (role.gameRole == GameRole.Player) {
		// animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
		player.IsHitAnEnemy = false;
		input.AttackMode = 0;
		// attack.IsAttacking = false;
		// }
		player.SetPlayerIdle();
	}

	// float CheckMode (int mode) {
	// 	switch (mode) {
	// 		case 0: 
	// 			return 0f; //STAND / MOVE / DODGE
	// 			// break;
	// 		case 1: 
	// 			return 1f; //CHARGE / DASH
	// 			// break;
	// 		case 2:
	// 			return 2f; //GUARD
	// 			// break;
	// 		case 3:
	// 			return 3f; //STEADY FOR RAPID SLASH
	// 			// break;
	// 		case -1: 
	// 			return -1f; //DIE / BLOCK
	// 			// break;
	// 		case -2: 
	// 			return -2f; //HURT
	// 			// break;
	// 		default:
	// 			Debug.Log("Unknown Mode in Animation System");
	// 			return 0f;
	// 	}
	// }

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
