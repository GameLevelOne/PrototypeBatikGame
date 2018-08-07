using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class FishingRodSystem : ComponentSystem {
	public struct FishingData {
		public readonly int Length;
		public ComponentArray<FishingRod> FishingRod;
	}
	[InjectAttribute] FishingData fishingData;
	[InjectAttribute] PlayerMovementSystem playerMovementSystem;

	FishingRod fishingRod;
	Facing2D facing;

	FishingRodState state;

	float deltaTime;

	protected override void OnUpdate () {
		if (fishingData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishingData.Length; i++) {
			fishingRod = fishingData.FishingRod[i];
			state = fishingRod.state;

			if (fishingRod.state == FishingRodState.THROW && !fishingRod.IsBaitLaunched) {
				fishingRod.IsBaitLaunched = true;
				Throw ();
			} else if (state == FishingRodState.STAY) {
				Stay ();
			} else if (state == FishingRodState.RETURN) {
				Return ();
			}
		}
	}

	void Throw () {
		Debug.Log("Fishing Rod Throw");
		fishingRod.transform.localPosition = GetDestinationPos(Vector2.zero, playerMovementSystem.facing.DirID, fishingRod.fishingRange);
		fishingRod.baitCol.enabled = true;
		fishingRod.state = FishingRodState.STAY;
	}

	void Stay () {
		Debug.Log("Fishing Rod Stay");
		// fishingRod.state = FishingRodState.RETURN;
	}

	void Return () {
		Debug.Log("Fishing Rod Return");
		fishingRod.state = FishingRodState.IDLE;
		fishingRod.baitCol.enabled = false;
		fishingRod.IsBaitLaunched = false;
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
