using UnityEngine;
using Unity.Entities;

public class FishingRodSystem : ComponentSystem {
	public struct FishingData {
		public readonly int Length;
		public ComponentArray<FishingRod> FishingRod;
	}
	[InjectAttribute] FishingData fishingData;
	[InjectAttribute] PlayerMovementSystem playerMovementSystem;
	[InjectAttribute] FishCollectibleSystem fishCollectibleSystem;

	FishingRod fishingRod;
	Facing2D facing;

	FishingRodState state;
	FishCollectibleType type;

	float deltaTime;

	protected override void OnUpdate () {
		if (fishingData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishingData.Length; i++) {
			fishingRod = fishingData.FishingRod[i];
			state = fishingRod.state;

			if (fishingRod.state == FishingRodState.THROW && !fishingRod.isBaitLaunched) {
				fishingRod.isBaitLaunched = true;
				Throw ();
			} else if (state == FishingRodState.STAY) {
				Stay ();
			} else if (state == FishingRodState.RETURN) {
				Return ();
			}
		}
	}

	void Throw () {
		fishingRod.transform.localPosition = GetDestinationPos(Vector2.zero, playerMovementSystem.facing.DirID, fishingRod.fishingRange);
		// fishingRod.baitCol.enabled = true;
		fishingRod.isBaitFish = false;
		fishingRod.state = FishingRodState.STAY;
	}

	void Stay () {
		// fishingRod.state = FishingRodState.RETURN;
		if (fishingRod.fishCollectible != null) {
			if (fishingRod.fishCollectible.state == FishState.CATCH) {
				fishingRod.isCatchSomething = true;
				type = fishingRod.fishType;
			} else {
				fishingRod.isCatchSomething = false;
			}
		}
	}

	void Return () {
		//CHECK ITEM
		if (fishingRod.isCatchSomething) {
			Debug.Log("You Got Fish with type "+type);
			fishCollectibleSystem.CatchFish(fishingRod.fishCollectible);
		}

		fishingRod.isCatchSomething = false;
		fishingRod.fishObj = null;
		fishingRod.fishCollectible = null;
		fishingRod.state = FishingRodState.IDLE;
		// fishingRod.baitCol.enabled = false;
		fishingRod.isBaitLaunched = false;
		fishingRod.transform.localPosition = Vector2.zero;
	}

	Vector2 GetDestinationPos(Vector2 throwObjInitPos, int dirID, float range)
	{
		Vector3 destination = throwObjInitPos;
		float x = throwObjInitPos.x;
		float y = throwObjInitPos.y;

		if(dirID == 1){ //bottom
			y-=range;
		}else if(dirID == 2){ //bottom left
			x-=range;
			y-=range;
		}else if(dirID == 3){ //left
			x-=range;
		}else if(dirID == 4){ //top left
			x-=range;
			y+=range;
		}else if(dirID == 5){ //top
			y+=range;
		}else if(dirID == 6){ //top right
			x+=range;
			y+=range;
		}else if(dirID == 7){ //right
			x+=range;
		}else if(dirID == 8){ //bottom right
			x+=range;
			y-=range;
		}

		return new Vector2(x,y);
	}
}
