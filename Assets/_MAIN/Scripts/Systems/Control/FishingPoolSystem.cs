using UnityEngine;
using Unity.Entities;

public class FishingPoolSystem : ComponentSystem {
	public struct FishingPoolData {
		public readonly int Length;
		public ComponentArray<FishingPool> FishingPool;
	}
	[InjectAttribute] FishingPoolData fishingPoolData;

	FishingPool fishingPool;

	float deltaTime;
	float poolRadius;
	Vector3 poolPos;

	protected override void OnUpdate () {
		if (fishingPoolData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishingPoolData.Length; i++) {
			fishingPool = fishingPoolData.FishingPool[i];

			poolRadius = fishingPool.fishingPoolCol.bounds.size.x / 2; //CIRCLE
			poolPos = fishingPool.transform.position;

			if (!fishingPool.isFinishSpawning) {
				SpawnFish();
			}
		}
	}

	void SpawnFish() {
		if (fishingPool.maxSpawn == 0) {
			fishingPool.isFinishSpawning = true;
			return;
		}

		if (!fishingPool.isSpawning) {
			fishingPool.TimeSpawn -= deltaTime;
			
			if (fishingPool.TimeSpawn <= 0f) {
				fishingPool.isSpawning = true;
			}
		} else {
			GameObject newFish = GameObject.Instantiate(fishingPool.fishCollectibleObj, RandomPosition(), Quaternion.identity);
			newFish.transform.parent = fishingPool.transform;

			Fish fish = newFish.GetComponent<Fish>();
			fishingPool.fishList.Add(fish);
			fish.parentPoolCol = fishingPool.fishingPoolCol.GetComponent<Transform>();
			fish.parentPoolRadius = fishingPool.fishingPoolCol.bounds.size.x / 2; //CIRCLE
			// fish.fishChar = (FishCharacteristic)Random.Range(0, System.Enum.GetValues(typeof(FishCharacteristic)).Length);
			fish.fishChar = FishCharacteristic.WILD; //TEMP
			// Debug.Log(fish.fishChar);

			newFish.SetActive(true);

			fishingPool.isSpawning = false;
			fishingPool.TimeSpawn = fishingPool.spawnInterval;

			if (fishingPool.fishList.Count >= fishingPool.maxSpawn) {
				fishingPool.isFinishSpawning = true;
			}
		}
	}

	Vector3 RandomPosition () {
		float randomX = poolPos.x + Random.Range(-poolRadius, poolRadius);
		float randomZ = poolPos.z + Random.Range(-poolRadius, poolRadius);
		
		return new Vector3 (randomX, 0f, randomZ);
	}
}
