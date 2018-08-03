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
	Vector2 currentMove;
	Vector2 currentDir;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (tool == null || attack == null) {
			tool = standAnimationSystem.stand;
			attack = playerAttackSystem.attack;

			return;
		}
		
		// if (!isLocalVarInit) {
		// 	currentMoves = new Vector2[animationData.Length];
		// 	currentDirs = new Vector2[animationData.Length];
		// 	allFacings = new Facing2D[animationData.Length];
		// 	isLocalVarInit = true;			
		// }

		for (int i=0; i<animationData.Length; i++) {
			input = animationData.PlayerInput[i];
			player = animationData.Player[i];
			state = player.state;
			anim = animationData.Animation[i];
			facing = animationData.Facing[i];

			animator = anim.animator; 
			int attackMode = input.AttackMode;

			if (state == PlayerState.DIE) {
				Debug.Log("Player Die Animation");
				input.SteadyMode = -1;
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, false);
				animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.SteadyMode);
				continue;
			}

			#region ACTION
			if (state == PlayerState.BLOCK_ATTACK || state == PlayerState.DODGE || state == PlayerState.DASH || state == PlayerState.BOUNCE || state == PlayerState.BRAKE || state == PlayerState.HURT_MOVE || state == PlayerState.POWER_BRACELET) {
				// Debug.Log("Check IS_INTERACT " + Constants.AnimatorParameter.Bool.IS_INTERACT + ", INTERACT_VALUE " + Constants.AnimatorParameter.Int.INTERACT_VALUE);
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, true);
				animator.SetInteger(Constants.AnimatorParameter.Int.INTERACT_VALUE, input.InteractValue);
			} else {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_INTERACT, false);
			}
			
			if (state == PlayerState.SLOW_MOTION) {
				animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
				animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, input.SteadyMode);
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
			animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE,input.SteadyMode);
			animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, input.MoveMode);
			animator.SetFloat(Constants.AnimatorParameter.Float.INTERACT_MODE, input.InteractMode);
			animator.SetFloat(Constants.AnimatorParameter.Float.LIFTING_MODE, input.LiftingMode);

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
			
			Vector2 movement = input.MoveDir;
			
			if (currentMove == movement) {
				continue;
			} else {
				currentMove = movement;
				
				if (currentMove == Vector2.zero) {
					animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
					if (input.LiftingMode == -2) {
						input.LiftingMode = -1;
					} else if (input.LiftingMode == 2) {
						input.LiftingMode = 1;
					}

				} else {
					SetAnimation (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
					SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
					
					animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, true);
					if (input.LiftingMode == -1) {
						input.LiftingMode = -2;
					} else if (input.LiftingMode == 1) {
						input.LiftingMode = 2;
					}
				}
			}
			#endregion
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
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, true);
	}

	void SetRapidAttack (float mode) { //RAPID SLASH 0
		animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, mode); 
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, true);
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, true);
	}

	void SetAnimation (string animName, float animValue, bool isVertical) {
		Vector2 movement = input.MoveDir;
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
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void CheckAfterAnimation (AnimationState animState) {
		switch (animState) {
			case AnimationState.AFTER_SLASH:
				if (input.slashComboVal.Count > 0) {
					int slashComboVal = input.slashComboVal[0];
					
					animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, slashComboVal);	
					
					if (slashComboVal == 3) {					
						input.slashComboVal.Clear();
					} else {
						input.slashComboVal.RemoveAt(0);
					}

					CheckAttackList ();
				} else {
					CheckAttackList ();
				}
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
				input.BulletTimeAttackQty--;
				if (input.BulletTimeAttackQty == 0) {
					// player.IsRapidSlashing = false;
					player.IsHitAnEnemy = false;
					animator.SetBool(Constants.AnimatorParameter.Bool.IS_RAPID_SLASHING, false);
					StopAttackAnimation();
				}
				break;
			case AnimationState.AFTER_BLOCK:
				if (player.IsGuarding) {
					player.SetPlayerIdle();
				}
				break;
			case AnimationState.AFTER_HURT:
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_LIFT:
				input.LiftingMode = -1;
				break;
			// case AnimationState.AFTER_DASH:
			// 	//
			// 	break;
			// case AnimationState.AFTER_BRAKING:
			// 	//
			// 	break;
			case AnimationState.AFTER_GRAB://case after steady power bracelet, input.interactvalue = 1
				LiftState liftState = powerBraceletSystem.powerBracelet.state;
				input.InteractValue = 1;

				if (liftState == LiftState.GRAB) {
					powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
				} else if (liftState == LiftState.CAN_LIFT) {
					powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Kinematic);
					powerBraceletSystem.SetLiftObjectParent();
				}
				break;
			case AnimationState.AFTER_UNGRAB://case after using power bracelet, bool interact = false (optional)
				input.InteractValue = 0;
				powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Static);
				player.SetPlayerIdle();
				break;
			case AnimationState.AFTER_THROW:
				powerBraceletSystem.UnSetLiftObjectParent();
				powerBraceletSystem.AddForceRigidbody(facing.DirID, 50f);
				input.InteractValue = 0;
				player.SetPlayerIdle();
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void CheckAttackList () {		
		if (input.slashComboVal.Count == 0) {
			animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, 0f);
			player.IsHitAnEnemy = false;
			StopAttackAnimation ();
		}
	}

	void StopAttackAnimation () {
		// if (role.gameRole == GameRole.Player) {
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
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
