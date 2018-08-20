using UnityEngine;
using Unity.Entities;

public class StandAnimationSystem : ComponentSystem {
	public struct StandData {
		public readonly int Length;
		public ComponentArray<PlayerTool> PlayerTool; // STAND / SUMMON / TOOL
		public ComponentArray<Animation2D> Animation;
	}
	[InjectAttribute] StandData standData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] PlayerAnimationSystem playerAnimationSystem;

	public PlayerTool tool;

	PlayerInput input;
	Player player;
	PlayerState state;
	Animation2D anim;
	Facing2D facing;

	ToolType standType;
	Animator animator;
	Animator playerAnimator;
	bool isFinishAnyStandAnim = true;

	public bool isFinishAnyStandAnimation {
		get {return isFinishAnyStandAnim;}
		set {
			isFinishAnyStandAnim = value;
			// Debug.Log(isFinishAnyStandAnim + " on state " + state);
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
			facing = playerAnimationSystem.facing;
		
			animator = anim.animator;
			playerAnimator = playerAnimationSystem.animator;
			standType = tool.currentTool;

			if (CheckIfPlayerUseStand()) {
				SetAnimationFaceDirection();
				CheckPlayerState ();
			} else {
				SetStandIdle ();
			}
			
			CheckStandAnimation ();

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
				// if (standType == ToolType.Bow && player.isUsingStand) {
					if (input.interactValue == 0) {
						animator.Play(Constants.BlendTreeName.STAND_TAKE_AIM_BOW);
					} else if (input.interactValue == 1) {
						animator.Play(Constants.BlendTreeName.STAND_AIMING_BOW);
					} else if (input.interactValue == 2) {
						animator.Play(Constants.BlendTreeName.STAND_SHOT_BOW);
					}
				// }
				
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
				// if (tool.currentTool == ToolType.Bomb) {
				// 	animator.Play(Constants.BlendTreeName.STAND_BOMB);
				// } 

				if (tool.currentTool == ToolType.MagicMedallion) {
					animator.Play(Constants.BlendTreeName.STAND_MAGIC_MEDALLION);
				}

				break;
		}
	}

	void SetAnimationFaceDirection () {
		float playerDirX = playerAnimator.GetFloat (Constants.AnimatorParameter.Float.FACE_X);
		float playerDirY = playerAnimator.GetFloat (Constants.AnimatorParameter.Float.FACE_Y);

		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, playerDirX);
		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, playerDirY);
	}

	void CheckStandAnimation () {
		if (!anim.isCheckBeforeStandAnimation) {
			CheckStartStandAnimation ();
			anim.isCheckBeforeStandAnimation = true;
		} else if (!anim.isCheckAfterStandAnimation) {
			CheckEndStandAnimation ();
			anim.isCheckAfterStandAnimation = true;
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
		player.isUsingStand = false;
		isFinishAnyStandAnimation = true;
		// Debug.Log(isFinishAnyStandAnimation);
		animator.Play(Constants.BlendTreeName.STAND_INACTIVE);
	}

	void CheckEndStandAnimation () {
		switch (state) {
			case PlayerState.DASH: 
				SetStandIdle();
				break;
			case PlayerState.BOW:
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

	bool CheckIfPlayerUseStand () {
		if (state == PlayerState.BOW || state == PlayerState.POWER_BRACELET || state == PlayerState.DASH) {
			if (player.isUsingStand) {
				return true;
			} else {
				return false;
			}
		} else if (state == PlayerState.USING_TOOL) {
			// if (standType == ToolType.Bomb) {
			// 	return true;
			// } 
			
			if (standType == ToolType.MagicMedallion) {
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
	// 			tool.isActToolReady = true;
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
