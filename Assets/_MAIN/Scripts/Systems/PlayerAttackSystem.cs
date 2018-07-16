using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerAttackSystem : ComponentSystem {
	public struct AttackData {
		public readonly int Length;
		public ComponentArray<Transform> Transform;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Attack> Attack;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] AttackData attackData;

	protected override void OnUpdate () {
		for (int i=0; i<attackData.Length; i++) {
			if (attackData.Length == 0) return;

			Transform tr = attackData.Transform[i];
			PlayerInput input = attackData.PlayerInput[i];
			Attack attack = attackData.Attack[i];
			int attackMode = input.Attack;

			if (input.Attack == 0) return;

			//Attack
			switch(attackMode) {
				case -1:
					attack.Attacking(AttackType.Slash);
					break;
				case 1:
					attack.Attacking(AttackType.Shot);
					break;
			}
		}
	}
}
