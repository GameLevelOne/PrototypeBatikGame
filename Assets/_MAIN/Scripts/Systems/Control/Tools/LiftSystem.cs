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

	PowerBracelet powerBracelet;
	LiftState state;

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		for (int i=0; i<liftData.Length; i++) {
			powerBracelet = liftData.powerBracelet[i];
			state = powerBracelet.state;
			
			if (state == LiftState.NONE) {
				//
			} else if (state == LiftState.CAN_LIFT) {
				powerBracelet.LiftingMode = -1;
			} else if (state == LiftState.CANNOT_LIFT) {
				powerBracelet.LiftingMode = 0;
			} else if (state == LiftState.GRAB) {
				powerBracelet.LiftingMode = 1;
			}
		}
	}
}
