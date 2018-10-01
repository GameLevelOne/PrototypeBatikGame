using UnityEngine;
using Unity.Entities;
public class ChestSpawnerSystem : ComponentSystem {
	public struct ChestSpawnerData {
		public readonly int Length;
		public ComponentArray<ChestSpawner> ChestSpawner;
	}
	[InjectAttribute] ChestSpawnerData chestSpawnerData;
	[InjectAttribute] ToolSystem toolSystem;
	ChestSpawner chestSpawner;

	// public struct ChestSpawnerTriggerData {
	// 	public readonly int Length;
	// 	public ComponentArray<ChestSpawnerTrigger> ChestSpawnerTrigger;
	// }
	// [InjectAttribute] ChestSpawnerTriggerData chestSpawnerTriggerData;
	// ChestSpawnerTrigger chestSpawnerTrigger;

	protected override void OnUpdate () {
		for (int i=0; i<chestSpawnerData.Length; i++) {
			chestSpawner = chestSpawnerData.ChestSpawner[i];

			if (!chestSpawner.isInitChestSpawner) {
				InitChestSpawner ();
			} else {
				if (!chestSpawner.isSpawned) {
					CheckChestSpawner ();
				}
			}
		}
	}

	void InitChestSpawner () {
		//LOAD CHEST STATE
		chestSpawner.isSpawned = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.CHEST_SPAWNER_STATE + chestSpawner.chestSpawnerID, 0) == 1 ? true : false;

		SetSpawnedChest (chestSpawner.isSpawned);

		chestSpawner.isInitChestSpawner = true;
	}

	void CheckChestSpawner () {
		// for (int i=0; i<chestSpawnerTriggerData.Length; i++) {
		// 	chestSpawnerTrigger = chestSpawnerTriggerData.ChestSpawnerTrigger[i];

		// 	if (chestSpawner.chestSpawnerID == chestSpawnerTrigger.targetSpawnChestID) {
		// 		// if (!chestSpawnerTrigger.isAlreadyTrigger) {
		// 			if (chestSpawnerTrigger.isTriggerSpawn) {
		// 				chestSpawner.currentTotalSpawnTrigger++;
		// 				CheckRequiredSpawnTrigger ();

		// 				chestSpawnerTrigger.isTriggerSpawn = false;
		// 				// chestSpawnerTrigger.isAlreadyTrigger = true;
		// 			}
		// 		// }
		// 	}
		// }
		if (chestSpawner.isTriggerSpawn) {
			chestSpawner.currentTotalSpawnTrigger++;
			CheckRequiredSpawnTrigger ();

			chestSpawner.isTriggerSpawn = false;
			// chestSpawnerTrigger.isAlreadyTrigger = true;
		}
	}

	void CheckRequiredSpawnTrigger () {
		if (chestSpawner.currentTotalSpawnTrigger == chestSpawner.requiredSpawnTrigger) {
			PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_SPAWNER_STATE + chestSpawner.chestSpawnerID, 1);

			SetSpawnedChest (true);

			chestSpawner.isSpawned = true;
		}
	}

	void SetSpawnedChest (bool activeState) {
		// chestSpawner.chestCol.enabled = activeState;
		// chestSpawner.chestSpriteRen.enabled = activeState;
		// chestSpawner.chestEntity.enabled = activeState;

		if (chestSpawner.chestSpawnerType == ChestSpawnerType.POWERARROW) {
			if (toolSystem.tool.Bow == 1){
				chestSpawner.chestObj.SetActive(false);
			} else {
				chestSpawner.chestObj.SetActive(activeState);
			}
		} else {
			chestSpawner.chestObj.SetActive(activeState);
		}
	}
}
