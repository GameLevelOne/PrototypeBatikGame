using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ShovelSystem : ComponentSystem {
	public struct ShovelData {
		public readonly int Length;
		public ComponentArray<Shovel> shovel;
	}
	[InjectAttribute] ShovelData shovelData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	Shovel shovel;
	
	bool checkList = true;

	protected override void OnUpdate () {
		if (shovelData.Length == 0) return;

		for (int i=0; i<shovelData.Length; i++) { 
			shovel = shovelData.shovel[i];

			if (shovel.listDig.Count >= 1) {
				checkList = shovel.listDig.Contains(true) ? true : false;

				if (!checkList && playerInputSystem.player.isCanDigging) {
					GameObjectEntity.Instantiate(shovel.diggingObj, shovel.transform.position, Quaternion.identity);
					// shovel.IsNotCleanForDigging = true;
				}
			} 
		}
		
		// foreach (var e in GetEntities<ShovelData>()) {
		// 	Shovel shovel = e.shovel;
			
		// 	if (shovel.listDig.Count >= 1) {
		// 		checkList = shovel.listDig.Contains(true) ? true : false;

		// 		if (!checkList && playerInputSystem.player.IsCanDigging) {
		// 			GameObjectEntity.Instantiate(shovel.diggingObj, shovel.transform.position, Quaternion.identity);
		// 			// shovel.IsNotCleanForDigging = true;
		// 		}
		// 	} 
		// }
	}
}
