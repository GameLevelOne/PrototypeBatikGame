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
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	
	bool checkList = true;

	protected override void OnUpdate () {
		foreach (var e in GetEntities<ShovelData>()) {
			Shovel shovel = e.shovel;
			
			if (shovel.listDig.Count >= 1) {
				checkList = shovel.listDig.Contains(true) ? true : false;

				if (!checkList && playerInputSystem.player.IsCanDigging) {
					GameObjectEntity.Instantiate(shovel.diggingObj, shovel.transform.position, Quaternion.identity);
					// shovel.IsNotCleanForDigging = true;
				}
			} 
		}
	}
}
