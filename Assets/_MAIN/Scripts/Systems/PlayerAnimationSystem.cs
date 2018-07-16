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
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] AnimationData animationData;

	Animator animator;
	Vector2[] currentMoves = new Vector2[0];
	Vector2[] currentDirs = new Vector2[0];
	Facing2D[] allFacings = new Facing2D[0];

	bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		if (!isLocalVarInit) {
			currentMoves = new Vector2[animationData.Length];
			currentDirs = new Vector2[animationData.Length];
			allFacings = new Facing2D[animationData.Length];
			isLocalVarInit = true;			
		}

		for (int i=0; i<animationData.Length; i++) {
			Animation2D anim = animationData.Animation[i];
			PlayerInput input = animationData.PlayerInput[i];
			int attackMode = input.Attack;

			animator = anim.animator; 
			allFacings[i] = animationData.Facing[i];
			
			if (attackMode != 0) {
				animator.SetFloat("Move Mode", attackMode);
				animator.SetBool("IsAttacking", true);
				
				return;
			}
			
			Vector2 movement = input.Move;
			
			if (currentMoves[i] == movement) return;

			currentMoves[i] = movement;
			if (currentMoves[i] == Vector2.zero) {
				animator.SetBool("IsMoving", false);
			} else {
				SetAnimation (i, "FaceX", currentMoves[i].x, false);
				SetAnimation (i, "FaceY", currentMoves[i].y, true);
				
				animator.SetBool("IsMoving", true);
			}
		}
	}

	void SetAnimation (int idx, string animName, float animValue, bool isVertical) {
		Vector2 movement = animationData.PlayerInput[idx].Move;
		animator.SetFloat(animName, animValue);
		
		if (isVertical) {
			movement.y = Mathf.RoundToInt(animValue);
		} else {
			movement.x = Mathf.RoundToInt(animValue);
		}

		if (currentDirs[idx] == movement) return;

		currentDirs[idx] = movement;
		allFacings[idx].DirID = CheckDirID(currentDirs[idx].x, currentDirs[idx].y);
	}

	public int CheckDirID (float dirX, float dirY) {
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
