using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class LanternSystem : ComponentSystem {
	public struct LanternData {
		public readonly int Length;
		public ComponentArray<Lantern> lantern;
	}
	[InjectAttribute] LanternData lanternData;

	Lantern lantern;

	protected override void OnUpdate () {
		if (lanternData.Length == 0) return;

		for (int i=0; i<lanternData.Length; i++) {
			lantern = lanternData.lantern[i];

			lantern.lanternLight.SetActive(lantern.IsLightOn);
		}
	}
}
