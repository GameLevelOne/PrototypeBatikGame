using UnityEngine;
using Unity.Entities;

public class LootableSpawnerSystem : ComponentSystem {
	public struct LootableSpawnerData {
		public readonly int Length;
		public ComponentArray<LootableSpawner> LootableSpawner;
	}
	[InjectAttribute] LootableSpawnerData lootableSpawnerData;

	LootableSpawner lootableSpawner;

	int lootablesQty;

	protected override void OnUpdate () {
		for (int i=0; i<lootableSpawnerData.Length; i++) {
			lootableSpawner = lootableSpawnerData.LootableSpawner[i];

			if (!lootableSpawner.isInitLootableSpawner) {
				InitLootableSpawner();
			}

			// if (Input.GetButtonDown("Fire1")) {
			// 	Testing();
			// }
		}
	}

	void InitLootableSpawner () {
		lootablesQty = lootableSpawner.lootables.Length;

		lootableSpawner.isInitLootableSpawner = true;
	}

	public void CheckPlayerLuck (float probability, Vector3 spawnPos) {
		float luckyNumber = Random.value;
		// Debug.Log("luckyNumber : "+luckyNumber+"\n probability : "+probability);
		if (luckyNumber <= probability) {	
			int randomItemIdx = Random.Range(0, lootablesQty);
			Lootable lootable = lootableSpawner.lootables[randomItemIdx];
			
			Vector3 targetPos = new Vector3 (spawnPos.x, lootable.initPosY, spawnPos.z);
			SpawnLootableObj (lootable.gameObject, targetPos);
		}
	}

	void SpawnLootableObj (GameObject obj, Vector3 spawnPos) {
        GameObject spawnedObj = GameObject.Instantiate(obj, spawnPos, Quaternion.Euler(0f, 0f, 0f));

        spawnedObj.SetActive(true);
    }

	void Testing () {
		CheckPlayerLuck (0.5f, Vector3.zero);
	}
}
