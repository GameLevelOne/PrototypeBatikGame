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

	PlayerInput input;
	PlayerTool stand;
	Animation2D anim;
	Facing2D facing;

	Animator animator;
	Vector2 currentMove;
	Vector2 currentDir;
	
	protected override void OnUpdate () {
		if (standData.Length == 0) return;

		for (int i=0; i<standData.Length; i++) {
			input = playerInputSystem.input;
			stand = standData.PlayerTool[i];
			// Sprite2D sprite = standData.Sprite[i];
			anim = standData.Animation[i];
			facing = standData.Facing[i];
		
			animator = anim.animator;
			int standType = (int)stand.currentTool;
			
			if (!anim.IsCheckAfterAnimation) {
				CheckAfterStandAnimation (anim.standAnimState);
				anim.IsCheckAfterAnimation = true;
			}

			if(input.IsUsingTool) {
				SetStand(standType);
			}
		}
	}

	void SetStand (float mode) { //
		SetAnimation (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
		SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
		
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

	void CheckAfterStandAnimation (StandAnimationState state) {
		Debug.Log("Start Checking : " + state);
		if (state == StandAnimationState.AFTER_USING_TOOL) {
			Debug.Log("Checked");
			animator.SetFloat(Constants.AnimatorParameter.Float.TOOL_TYPE, 0f); 
			animator.SetBool(Constants.AnimatorParameter.Bool.IS_USING_TOOL, false);
			input.IsUsingTool = false;
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
