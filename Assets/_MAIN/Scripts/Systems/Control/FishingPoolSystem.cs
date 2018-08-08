using UnityEngine;
using Unity.Entities;

public class FishingPoolSystem : ComponentSystem {
	public struct FishingPoolData {
		public readonly int Length;
		public ComponentArray<FishingPool> FishingPool;
	}
	[InjectAttribute] FishingPoolData fishingPoolData;

	FishingPool fishingPool;

	protected override void OnUpdate () {
		if (fishingPoolData.Length == 0) return;

		for (int i=0; i<fishingPoolData.Length; i++) {
			fishingPool = fishingPoolData.FishingPool[i];

			float size = fishingPool.fishingPoolCol.bounds.size.x / 2; //CIRCLE
			Vector2 fishingPoolPos = fishingPool.transform.position;

			if (fishingPool.fishCollectibleList.Count < fishingPool.maxSpawn) {
				Vector2 randomPos = new Vector2 (fishingPoolPos.x + Random.Range(-size, size), fishingPoolPos.y + Random.Range(-size, size));
				GameObject newFish = GameObject.Instantiate(fishingPool.fishCollectibleObj, randomPos, Quaternion.identity);
				fishingPool.fishCollectibleList.Add(newFish.GetComponent<FishCollectible>());
				newFish.transform.parent = fishingPool.transform;
			}
		}
	}
}
