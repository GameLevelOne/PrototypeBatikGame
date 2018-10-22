using UnityEngine;
using Unity.Entities;

public class ShovelSystem : ComponentSystem {
	public struct ShovelData {
		public readonly int Length;
		public ComponentArray<Shovel> shovel;
	}
	[InjectAttribute] ShovelData shovelData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	Shovel shovel;
	
	bool checkList = true;

	protected override void OnUpdate () {
		// if (shovelData.Length == 0) return;

		for (int i=0; i<shovelData.Length; i++) { 
			shovel = shovelData.shovel[i];

			if (shovel.listDig.Count >= 1) {
				checkList = shovel.listDig.Contains(true) ? true : false;

				if (!checkList && playerInputSystem.player.isCanDigging) {
					SpawnDigResult (shovel.diggingObj);
					// shovel.IsNotCleanForDigging = true
				}
			} 
		}
	}

    void SpawnDigResult (GameObject obj) {
		Vector3 shovelPos = shovel.transform.position;
		// Vector3 pos = new Vector3 (shovelPos.x, shovel.digResultPosY, shovelPos.z);
		Vector3 pos = new Vector3 (shovelPos.x, 0.02f, shovelPos.z);
		// Quaternion rot = Quaternion.Euler(40f, 0f, 0f);
		Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
        GameObject spawnedObj = GameObject.Instantiate(obj, pos, rot);
        spawnedObj.SetActive(true);

		//SPAWN ITEM
		lootableSpawnerSystem.CheckPlayerLuck(shovel.spawnItemProbability, shovelPos);
    }
}
