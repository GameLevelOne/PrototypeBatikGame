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

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		for (int i=0; i<liftData.Length; i++) {
			powerBracelet = liftData.powerBracelet[i];

			if (powerBracelet.IsLiftSomething) {
				//
			}
		}

	}
}
