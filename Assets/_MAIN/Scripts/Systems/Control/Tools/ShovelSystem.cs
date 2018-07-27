using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ShovelSystem : ComponentSystem {
	public struct ShovelData {
		public Shovel shovel;
	}

	// [InjectAttribute] ContainerSystem containerSystem;
	
	protected override void OnUpdate () {
		foreach (var e in GetEntities<ShovelData>()) {
			Shovel shovel = e.shovel;

			if (shovel.IsDiggingOnDigArea) {
				GameObjectEntity.Instantiate(shovel.diggingCheckerObj, shovel.transform.position, Quaternion.identity);
				shovel.IsDiggingOnDigArea = false;
			}
		}
	}
}
