using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class CloakSystem : ComponentSystem {
	public struct CloakData {
		public readonly int Length;
		public ComponentArray<Cloak> cloak;
	}
	[InjectAttribute] CloakData cloakData;

	Cloak cloak;

	protected override void OnUpdate () {
		if (cloakData.Length == 0) return;

		for (int i=0; i<cloakData.Length; i++) {
			cloak = cloakData.cloak[i];

			// cloak.is.SetActive(cloak.IsLightOn);
            Color temp = cloak.spriteRen.color;

            if (cloak.isInvisible) {
                temp.a = cloak.invisibleAlpha;
            } else {
                temp.a = 1f;
            }

            cloak.spriteRen.color = temp;
		}
	}
}
