using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class EnemyAttackSystem : ComponentSystem {
	public struct AttackData {
		public readonly int Length;
		public ComponentArray<EnemyInput> EnemyInput;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Attack> Attack;
	}
	[InjectAttribute] AttackData attackData;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (attackData.Length == 0) return;
		
		// if (!isLocalVarInit) {
		// 	objTransform = new Transform[attackData.Length];
		// 	bulletSpawnPos = new Transform[attackData.Length];
		// 	bullet = new GameObject[attackData.Length];
		// 	isAttacking = new bool[attackData.Length];
		// 	isLocalVarInit = true;			
		// }

		for (int i=0; i<attackData.Length; i++) {
			EnemyInput input = attackData.EnemyInput[i];
			Animation2D anim = attackData.Animation[i];
			Attack attack = attackData.Attack[i];
			
			int attackMode = input.AttackMode;
			AnimationState animState = anim.animState;
			bool isAttacking = attack.isAttacking;

			Debug.Log("Enemy Attack entity : " + i);

			if (attackMode == 0) continue;

			//Attack
        	// attack.isAttacking = true;
			if (!isAttacking) {
				continue;
			} else {
				if ((animState == AnimationState.START_SLASH) || (animState == AnimationState.START_CHARGE) || 
				(animState == AnimationState.START_DODGE) || (animState == AnimationState.START_COUNTER)) {
					attack.SpawnSlashEffect(attackMode);
					attack.isAttacking = false;
				}
			}
		}
	}
}
