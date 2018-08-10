using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class StandAnimationSystem : ComponentSystem {
	public struct StandData {
		public readonly int Length;
		// public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<PlayerTool> PlayerTool; // STAND / SUMMON / TOOL
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] StandData standData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] ToolSystem toolSystem;

	public PlayerTool tool;

	PlayerInput input;
	Player player;
	PlayerState state;
	Animation2D anim;
	Facing2D facing;

	ToolType standType;
	Animator animator;
	Vector2 currentMove;
	Vector2 currentDir;
	Vector2 movement;
	bool isFinishAnyStandAnim = true;

	public bool isFinishAnyStandAnimation {
		get {return isFinishAnyStandAnim;}
		set {
			isFinishAnyStandAnim = value;
			// Debug.Log(isFinishAnyAnim + " on state " + state);
		}
	}
	
	protected override void OnUpdate () {
		if (standData.Length == 0) return;
		
		if (input == null) {
			input = playerInputSystem.input;

			return;
		} 

		for (int i=0; i<standData.Length; i++) {
			player = playerInputSystem.player;
			state = player.state;
			tool = standData.PlayerTool[i];
			anim = standData.Animation[i];
			facing = standData.Facing[i];
		
			animator = anim.animator;
			standType = tool.currentTool;
			movement = input.moveDir;

			if (CheckIfPlayerUseStand()) {
				SetAnimationFaceDirection();
				CheckPlayerState ();
				CheckStandAnimation ();
			} else {
				animator.Play(Constants.BlendTreeName.STAND_INACTIVE);
			}

			continue; //TEMP

			#region OLD
			if(state == PlayerState.USING_TOOL || state == PlayerState.HOOK) {
			// 	int standType = (int)stand.currentTool;
			// 	SetStand(standType);
			// 	continue;
			// } else {
			// 	StopStandAnimation();
			}
			#endregion
		}
	}

	void CheckPlayerState () {
		if (!isFinishAnyStandAnimation) return;

		switch (state) {
			case PlayerState.DASH: 
				animator.Play(Constants.BlendTreeName.STAND_DASH);
				break;
			case PlayerState.BOW:
				if (input.interactValue == 0) {
					animator.Play(Constants.BlendTreeName.STAND_TAKE_AIM_BOW);
				} else if (input.interactValue == 1) {
					animator.Play(Constants.BlendTreeName.STAND_AIMING_BOW);
				} else if (input.interactValue == 2) {
					animator.Play(Constants.BlendTreeName.STAND_SHOT_BOW);
				}
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					animator.Play(Constants.BlendTreeName.STAND_GRABBING);
				} else if (input.interactValue == 1) {
					if (input.liftingMode == 0) {
						// animator.Play(Constants.BlendTreeName.SWEATING_GRAB);
					} else if (input.liftingMode == -1) {
						animator.Play(Constants.BlendTreeName.STAND_IDLE_LIFT);
					} else if (input.liftingMode == 1) {
						animator.Play(Constants.BlendTreeName.STAND_IDLE_PUSH);
					} else if (input.liftingMode == -2) {
						animator.Play(Constants.BlendTreeName.STAND_MOVE_LIFT);
					} else if (input.liftingMode == 2) {
						animator.Play(Constants.BlendTreeName.STAND_MOVE_PUSH);
					} else if (input.liftingMode == -3) {
						animator.Play(Constants.BlendTreeName.STAND_LIFTING);
					}
				} else if (input.interactValue == 2) {
					if (input.liftingMode == 0) {
						animator.Play(Constants.BlendTreeName.STAND_UNGRABBING);
					} else if (input.liftingMode == -1) {
						animator.Play(Constants.BlendTreeName.STAND_THROWING_LIFT);
					} else if (input.liftingMode == 1) {
						animator.Play(Constants.BlendTreeName.STAND_UNGRABBING);
					}
				}
				
				break;
			case PlayerState.USING_TOOL:
				if (tool.currentTool == ToolType.Bomb) {
					animator.Play(Constants.BlendTreeName.STAND_BOMB);
				} else if (tool.currentTool == ToolType.MagicMedallion) {
					animator.Play(Constants.BlendTreeName.STAND_MAGIC_MEDALLION);
				} 
				break;
		}
	}

	void SetAnimationFaceDirection () {
		if (currentMove != movement) {
			currentMove = movement;
			
			if (currentMove != Vector2.zero) {
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
				SetFaceDir (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
			}
		}
	}

	void CheckStandAnimation () {
		if (!anim.IsCheckBeforeStandAnimation) {
			CheckStartStandAnimation ();
			anim.IsCheckBeforeStandAnimation = true;
		} else if (!anim.IsCheckAfterStandAnimation) {
			CheckEndStandAnimation ();
			anim.IsCheckAfterStandAnimation = true;
		}
	}

	void CheckStartStandAnimation () {
		isFinishAnyStandAnimation = false;

		switch (state) {
			case PlayerState.DASH: 
			
				break;
			case PlayerState.BOW:
				if (input.interactValue == 0) {
					
				} else if (input.interactValue == 1) {
					
				} else if (input.interactValue == 2) {
					
				}
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					
				} else if (input.interactValue == 1) {

				} else if (input.interactValue == 2) {

				}
				
				break;
			case PlayerState.USING_TOOL:
			
				break;
		}
	}

	void SetStandIdle () {
		isFinishAnyStandAnimation = true;
		animator.Play(Constants.BlendTreeName.STAND_INACTIVE);
	}

	void CheckEndStandAnimation () {
		switch (state) {
			case PlayerState.DASH: 
				SetStandIdle();
				break;
			case PlayerState.BOW:
				Debug.Log("interactVal : "+input.interactValue);
				if (input.interactValue == 0) {
					isFinishAnyStandAnimation = true;
				} else if (input.interactValue == 1) {
					//
				} else if (input.interactValue == 2) {
					SetStandIdle();
				}
				break;
			case PlayerState.POWER_BRACELET:
				if (input.interactValue == 0) {
					isFinishAnyStandAnimation = true;
				} else if (input.interactValue == 1) {
					isFinishAnyStandAnimation = true;
				} else if (input.interactValue == 2) {
					SetStandIdle();
				}
				
				break;
			case PlayerState.USING_TOOL:
				SetStandIdle();
				break;
		}
	}

	void SetFaceDir (string animName, float animValue, bool isVertical) {
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

	bool CheckIfPlayerUseStand () {
		if (state == PlayerState.BOW || state == PlayerState.POWER_BRACELET || state == PlayerState.DASH) {
			if (player.isUsingStand) {
				return true;
			} else {
				return false;
			}
		} else if (state == PlayerState.USING_TOOL) {
			if (standType == ToolType.Bomb) {
				return true;
			} else if (standType == ToolType.MagicMedallion) {
				if (player.isUsingStand) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		} else {
			return false;
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

	// void StartCheckStandAnimation () {
	// 	// if (!anim.IsCheckBeforeStandAnimation) {
	// 	// 	CheckBeforeStandAnimation (anim.standAnimState);
	// 	// 	anim.IsCheckBeforeStandAnimation = true;
	// 	// } else if (!anim.IsCheckAfterStandAnimation) {
	// 	// 	CheckAfterStandAnimation (anim.standAnimState);
	// 	// 	anim.IsCheckAfterStandAnimation = true;
	// 	// }
	// }

	// void SetStand (float mode) { //
	// 	if ((mode >= 8) && (mode <=10)) {
	// 		mode = 7;
	// 	}

	// 	// animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, mode); 
	// 	// animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, true);
	// }

	// void CheckBeforeStandAnimation (StandAnimationState animState) {
	// 	switch (animState) {
	// 		case StandAnimationState.START_USING_TOOL:
	// 			tool.IsActToolReady = true;
	// 			break;
	// 		default:
	// 			Debug.LogWarning ("Unknown Stand Animation played");
	// 			break;
	// 	}
	// }

	// void CheckAfterStandAnimation (StandAnimationState animState) {
	// 	switch (animState) {
	// 		case StandAnimationState.AFTER_USING_TOOL:

	// 			if (state == PlayerState.HOOK) {
	// 				animator.enabled = false;
	// 			} else {
	// 				StopStandAnimation();
	// 				player.SetPlayerIdle();
	// 			}
	// 			break;
	// 		default:
	// 			Debug.LogWarning ("Unknown Stand Animation played");
	// 			break;
	// 	}
	// }

	// public void StopStandAnimation () {
	// 	if (!animator.enabled) {
	// 		animator.enabled = true;
	// 	}
		
	// 	// animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, 0f); 
	// 	// animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, false);
	// }
}
