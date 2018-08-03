using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class EnemyAnimationSystem : ComponentSystem {
	public struct AnimationData {
		public readonly int Length;
		public ComponentArray<EnemyInput> EnemyInput;
		public ComponentArray<Enemy> Enemy;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Facing2D> Facing;
		// public ComponentArray<Attack> Attack;
	}
	[InjectAttribute] AnimationData animationData;
	
	EnemyInput input;
	Enemy enemy;
	Animation2D anim;
	Facing2D facing;
	// Attack attack;
	// Role role;
	Animator animator;
	
	EnemyState state;
	Vector2 currentMove;
	Vector2 currentDir;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		// if (animationData.Length == 0) return;
		
		// // if (!isLocalVarInit) {
		// // 	currentMoves = new Vector2[animationData.Length];
		// // 	currentDirs = new Vector2[animationData.Length];
		// // 	allFacings = new Facing2D[animationData.Length];
		// // 	isLocalVarInit = true;			
		// // }

		// for (int i=0; i<animationData.Length; i++) {
		// 	input = animationData.EnemyInput[i];
		// 	enemy = animationData.Enemy[i];
		// 	state = enemy.state;
		// 	anim = animationData.Animation[i];
		// 	facing = animationData.Facing[i];
		// 	// attack = animationData.Attack[i];

		// 	animator = anim.animator; 
		// 	int attackMode = input.AttackMode;
		// 	// role = anim.role;
			
		// 	// if (enemy.IsEnemyDie) {
		// 	if (state == EnemyState.Die) {
		// 		input.SteadyMode = -1;
		// 		animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, CheckMode(input.SteadyMode));
		// 		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
		// 		animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
		// 		// animator.SetBool(Constants.AnimatorParameter.Bool.IS_DODGING, false);
		// 		continue;
		// 	}

		// 	#region ACTION
		// 	if (attackMode >= 1) {
		// 		SetAttack(0f); //SLASH
		// 	} else if (attackMode == -1) {
		// 		SetAttack(1f); //CHARGE
		// 	// } else if (attackMode == -2) {
		// 	// 	SetAttack(2f); //COUNTER
		// 	// 	Debug.Log("Counter");
		// 	} else if (attackMode == -3) {
		// 		SetAttack(-1f); //SHOT
		// 	}

		// 	if (!anim.IsCheckAfterAnimation) {
		// 		CheckAfterAnimation (anim.animState);
		// 		anim.IsCheckAfterAnimation = true;
		// 	}
		// 	#endregion

		// 	#region MOVEMENT
		// 	animator.SetFloat(Constants.AnimatorParameter.Float.IDLE_MODE, CheckMode(input.SteadyMode));
		// 	animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, CheckMode(input.MoveMode));

		// 	Vector2 movement = input.MoveDir;
			
		// 	if (currentMove == movement) {
		// 		continue;
		// 	} else {
		// 		currentMove = movement;

		// 		if (currentMove == Vector2.zero) {
		// 			animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, false);
		// 		} else {
		// 			SetAnimation (Constants.AnimatorParameter.Float.FACE_X, currentMove.x, false);
		// 			SetAnimation (Constants.AnimatorParameter.Float.FACE_Y, currentMove.y, true);
					
		// 			animator.SetBool(Constants.AnimatorParameter.Bool.IS_MOVING, true);
		// 		}
		// 	}
		// 	#endregion
		// }
	}

	void SetAttack (float mode) { //SLASH 0, CHARGE 1, SHOT -1
		// attack.isAttacking = true;
		animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, mode); 
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, true);
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
				animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
				input.isDodging = false;
				break;
			case AnimationState.AFTER_COUNTER:
				animator.SetFloat(Constants.AnimatorParameter.Float.ATTACK_MODE, 0f);
				input.isParrying = false;
				StopAttackAnimation ();
				break;
		}
	}

	void CheckAttackList () {		
		if (input.slashComboVal.Count == 0) {
			animator.SetFloat(Constants.AnimatorParameter.Float.SLASH_COMBO, 0f);
			StopAttackAnimation ();
		}
	}

	void StopAttackAnimation () {
		// if (role.gameRole == GameRole.Enemy) {
		animator.SetBool(Constants.AnimatorParameter.Bool.IS_ATTACKING, false);
		input.AttackMode = 0;
			// attack.isAttacking = false;
		// }
	}

	float CheckMode (int mode) {
		switch (mode) {
			case 0: 
				return 0f; //STAND / MOVE
				// break;
			case 1: 
				return 1f; //CHARGE
				// break;
			case 2:
				return 2f; //GUARD
				// break;
			case 3:
				return 3f; //DODGE
				// break;
			case -1: 
				return -1f; //DIE
				// break;
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
