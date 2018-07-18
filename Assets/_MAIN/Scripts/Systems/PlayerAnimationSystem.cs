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
	
	PlayerInput input;
	Animation2D anim;
	Animator animator;
	Vector2 currentMoves;
	Vector2 currentDirs;
	Facing2D allFacings;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (animationData.Length == 0) return;
		
		// if (!isLocalVarInit) {
		// 	currentMoves = new Vector2[animationData.Length];
		// 	currentDirs = new Vector2[animationData.Length];
		// 	allFacings = new Facing2D[animationData.Length];
		// 	isLocalVarInit = true;			
		// }

		for (int i=0; i<animationData.Length; i++) {
			anim = animationData.Animation[i];
			input = animationData.PlayerInput[i];
			allFacings = animationData.Facing[i];
			animator = anim.animator; 

			int attackMode = input.AttackMode;
			
			// if (attackMode != 0) {
			// 	if (attackMode)
			// 	animator.SetFloat("AttackMode", 0f); //SLASH 0, CHARGE 1, SHOT -1
			// 	animator.SetBool("IsAttacking", true);
				
			// 	return;
			// }
			
			if (attackMode >= 1) {
				SetAttack(0f); //SLASH
			} else if (attackMode == -1) {
				SetAttack(1f); //CHARGE
			} else if (attackMode == -2) {
				SetAttack(-1f); //SHOT
			}

			animator.SetFloat("IdleMode", CheckSteadyMode(input.SteadyMode));
			animator.SetFloat("MoveMode", CheckSteadyMode(input.MoveMode));

			Vector2 movement = input.MoveDir;
			
			if (currentMoves == movement) {
				return;
			} else {
				currentMoves = movement;
			}

			if (currentMoves == Vector2.zero) {
				animator.SetBool("IsMoving", false);
			} else {
				SetAnimation ("FaceX", currentMoves.x, false);
				SetAnimation ("FaceY", currentMoves.y, true);
				
				animator.SetBool("IsMoving", true);
			}
		}
	}

	void SetAttack (float mode) { //SLASH 0, CHARGE 1, SHOT -1
		animator.SetFloat("AttackMode", mode); 
		animator.SetBool("IsAttacking", true);
	}

	void SetAnimation (string animName, float animValue, bool isVertical) {
		Vector2 movement = input.MoveDir;
		animator.SetFloat(animName, animValue);
		
		if (isVertical) {
			movement.y = Mathf.RoundToInt(animValue);
		} else {
			movement.x = Mathf.RoundToInt(animValue);
		}

		if (currentDirs == movement) return;

		currentDirs = movement;
		allFacings.DirID = CheckDirID(currentDirs.x, currentDirs.y);
	}

	float CheckSteadyMode (int mode) {
		switch (mode) {
			case 0: 
				return 0f; //STAND / MOVE
				break;
			case 1: 
				return 1f; //CHARGE
				break;
			case 2:
				return 2f; //GUARD
				break;
			case -1: 
				return -1f; //DIE
				break;
		}

		return 0f;
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
