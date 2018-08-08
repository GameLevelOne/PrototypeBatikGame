using UnityEngine;
using Unity.Entities;

public class FishCollectibleSystem : ComponentSystem {
	public struct FishCollectibleData {
		public readonly int Length;
		public ComponentArray<FishCollectible> FishCollectible;
	}
	[InjectAttribute] FishCollectibleData fishCollectibleData;

	FishCollectible fishCollectible;
	
	float timer;
	float deltaTime;

	protected override void OnUpdate () {
		if (fishCollectibleData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishCollectibleData.Length; i++) {
			fishCollectible = fishCollectibleData.FishCollectible[i];

			//CHASE first before CATCH
			if (fishCollectible.state == FishState.CATCH) {
				WaitingToCatch ();
			}
			
		}
	}

	void WaitingToCatch () {
		if (fishCollectible.fishingRod.state == FishingRodState.STAY) {
			if (timer < fishCollectible.timeToCatch) {
				timer += deltaTime;
				Debug.Log(timer);
			} else {
				timer = 0f;
				Debug.Log("Flee");
				fishCollectible.state = FishState.FLEE;
				timer = 0;
			}
		} 
		// else if (fishCollectible.fishingRod.state == FishingRodState.RETURN) {
		// 	if (fishCollectible.state == FishState.CATCH) {
				
		// 	}
		// } 
	}

	public void CatchFish (FishCollectible fish) {
		GameObject.Destroy (fish.gameObject);
		UpdateInjectedComponentGroups();
	}
}
