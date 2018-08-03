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
		// public ComponentArray<Sprite2D> Sprite;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] StandData standData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] ToolSystem toolSystem;

	public PlayerTool stand;

	PlayerInput input;
	Player player;
	PlayerState state;
	Animation2D anim;
	Facing2D facing;

	Animator animator;
	Vector2 currentMove;
	Vector2 currentDir;
	
	protected override void OnUpdate () {
		if (standData.Length == 0) return;
		
		if (input == null) {
			input = playerInputSystem.input;

			return;
		} 

		for (int i=0; i<standData.Length; i++) {
			player = playerInputSystem.player;
			state = player.state;
			stand = standData.PlayerTool[i];
			// Sprite2D sprite = standData.Sprite[i];
			anim = standData.Animation[i];
			facing = standData.Facing[i];
		
			animator = anim.animator;

			StartCheckStandAnimation();
			
			Vector2 movement = input.MoveDir;

			if(state == PlayerState.USING_TOOL || state == PlayerState.HOOK) {
				int standType = (int)stand.currentTool;
				SetStand(standType);
				continue;
			} else {
				StopStandAnimation();
			}

			if (currentMove == movement) {
				continue;
			} else {
				currentMove = movement;
				
				if (currentMove != Vector2.zero) {
					SetAnimation (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
					SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
				}
			}
		}
	}

	void StartCheckStandAnimation () {
		if (!anim.IsCheckBeforeStandAnimation) {
			CheckBeforeStandAnimation (anim.standAnimState);
			anim.IsCheckBeforeStandAnimation = true;
		} else if (!anim.IsCheckAfterStandAnimation) {
			CheckAfterStandAnimation (anim.standAnimState);
			anim.IsCheckAfterStandAnimation = true;
		}
	}

	void SetStand (float mode) { //
		if ((mode >= 8) && (mode <=10)) {
			mode = 7;
		}

		animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, mode); 
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, true);
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

	void CheckBeforeStandAnimation (StandAnimationState animState) {
		switch (animState) {
			case StandAnimationState.START_USING_TOOL:
				stand.IsActToolReady = true;
				break;
			default:
				Debug.LogWarning ("Unknown Stand Animation played");
				break;
		}
	}

	void CheckAfterStandAnimation (StandAnimationState animState) {
		switch (animState) {
			case StandAnimationState.AFTER_USING_TOOL:

				if (state == PlayerState.HOOK) {
					animator.enabled = false;
				} else {
					StopStandAnimation();
					player.SetPlayerIdle();
				}
				break;
			default:
				Debug.LogWarning ("Unknown Stand Animation played");
				break;
		}
	}

	public void StopStandAnimation () {
		if (!animator.enabled) {
			animator.enabled = true;
		}
		
		animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, 0f); 
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, false);
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
