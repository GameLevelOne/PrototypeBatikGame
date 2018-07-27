using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using System.Collections.Generic;

public class EnemyInputSystem : ComponentSystem {
	public struct InputData {
		public readonly int Length;
		public ComponentArray<EnemyInput> EnemyInput;
		public ComponentArray<Health> Health;
	}
	[InjectAttribute] InputData inputData;

	Vector2 currentDir = Vector2.zero;
	float attackInterval = 0;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;

		for (int i=0; i<inputData.Length; i++) {
			EnemyInput input = inputData.EnemyInput[i];
			Health health = inputData.Health[i];
			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];

			#region AI Movement
			//
			#endregion

			#region AI Attack
			float attackIntervalMin = input.attackInterval[0];
			float attackIntervalMax = input.attackInterval[1];

			float randomAttackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
			
			if (attackInterval >= randomAttackInterval) {
				input.AttackMode = 1;
				attackInterval = 0f;
			} else {
				attackInterval += Time.deltaTime;				
			}
			#endregion
		}
	}

	void SetMovement (int idx, int value, bool isMoveOnly) {
		EnemyInput input = inputData.EnemyInput[idx];

		input.MoveMode = value;
		
		if (!isMoveOnly) {
			input.SteadyMode = value;
		}
	}

	void ChangeDir (int idx, float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);
		EnemyInput input = inputData.EnemyInput[idx];
		
		if (currentDir != newDir) {
			currentDir = newDir;
			input.MoveDir = currentDir;
		}
	}
}
