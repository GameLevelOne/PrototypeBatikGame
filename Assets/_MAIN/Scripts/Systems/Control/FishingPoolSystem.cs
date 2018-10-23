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

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		// if (fishingPoolData.Length == 0) return;

		for (int i=0; i<fishingPoolData.Length; i++) {
			fishingPool = fishingPoolData.FishingPool[i];

			if (!fishingPool.isInitFishingPool) {
				InitFishingPool();
			} else {
				if (!fishingPool.isFinishSpawning) {
					SpawnFish();
				}
			}
		}
	}

	void InitFishingPool () {
		fishingPool.poolRadiusX = fishingPool.fishingPoolCol.bounds.size.x / 2;
		fishingPool.poolRadiusZ = fishingPool.fishingPoolCol.bounds.size.z / 2;
		fishingPool.poolPos = fishingPool.transform.position;

		fishingPool.isInitFishingPool = true;
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
			fish.poolRadiusX = fishingPool.poolRadiusX; 
			fish.poolRadiusZ = fishingPool.poolRadiusZ;//CIRCLE
			// fish.fishChar = (FishCharacteristic)Random.Range(0, System.Enum.GetValues(typeof(FishCharacteristic)).Length);
			fish.fishChar = FishCharacteristic.WILD; //TEMP
			//  // Debug.Log(fish.fishChar);

			newFish.SetActive(true);

			fishingPool.isSpawning = false;
			fishingPool.TimeSpawn = fishingPool.spawnInterval;

			if (fishingPool.fishList.Count >= fishingPool.maxSpawn) {
				fishingPool.isFinishSpawning = true;
			}
		}
	}

	Vector3 RandomPosition () {
		Vector3 poolPos = fishingPool.poolPos;
		float poolRadiusX = fishingPool.poolRadiusX;
		float poolRadiusZ = fishingPool.poolRadiusZ;
		float randomX = poolPos.x + Random.Range(-poolRadiusX, poolRadiusX);
		float randomZ = poolPos.z + Random.Range(-poolRadiusZ, poolRadiusZ);
		
		return new Vector3 (randomX, 0f, randomZ);
	}
}
