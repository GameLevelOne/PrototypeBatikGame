using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class LiftSystem : ComponentSystem {
	public struct LiftData {
		public readonly int Length;
		public ComponentArray<PowerBracelet> powerBracelet;
	}
	[InjectAttribute] LiftData liftData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	PowerBracelet powerBracelet;
	PlayerInput input;
	LiftState state;

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		if (playerInputSystem == null) return;

		for (int i=0; i<liftData.Length; i++) {
			input = playerInputSystem.input;
			powerBracelet = liftData.powerBracelet[i];
			state = powerBracelet.state;
			
			if (state == LiftState.NONE) {
				//
			} else if (state == LiftState.CAN_LIFT) {
				input.LiftingMode = -1;
			} else if (state == LiftState.CANNOT_LIFT) {
				input.LiftingMode = 0;
			} else if (state == LiftState.GRAB) {
				input.LiftingMode = 1;
			}
		}
	}
}
