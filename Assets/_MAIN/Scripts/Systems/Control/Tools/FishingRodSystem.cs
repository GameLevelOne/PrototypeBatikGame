using UnityEngine;
using Unity.Entities;

public class FishingRodSystem : ComponentSystem {
	public struct FishingData {
		public readonly int Length;
		public ComponentArray<FishingRod> FishingRod;
	}
	[InjectAttribute] FishingData fishingData;
	[InjectAttribute] PlayerMovementSystem playerMovementSystem;
	[InjectAttribute] FishSystem fishSystem;

	public FishingRod fishingRod;
	Facing2D facing;

	FishingRodState state;

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
		fishingRod.transform.localPosition = GetDestinationPos(Vector3.zero, playerMovementSystem.facing.DirID, fishingRod.fishingRange);
		// fishingRod.baitCol.enabled = true;
		fishingRod.isBaitFish = false;
		fishingRod.state = FishingRodState.STAY;
	}

	void Stay () {
		// fishingRod.state = FishingRodState.RETURN;
		if (fishingRod.fish != null) {
			if (fishingRod.fish.state == FishState.CATCH) {
				fishingRod.isCatchSomething = true;
			} else {
				fishingRod.isCatchSomething = false;
			}
		}
	}

	void Return () {
		//CHECK ITEM
		if (fishingRod.isCatchSomething) {
			fishingRod.fish.transform.parent = fishingRod.transform;
			// playerMovementSystem.input.interactMode = -4;
		} else {
			if (fishingRod.fish != null) {
				if (fishingRod.fish.state == FishState.CHASE || fishingRod.fish.state == FishState.TRYINGGRAB || fishingRod.fish.state == FishState.WAIT) {
					fishingRod.fish.state = FishState.IDLE;
				} else if (fishingRod.fish.state == FishState.FLEE) {
					playerMovementSystem.input.interactMode = -3;
				}
			}
		}
		
		// fishingRod.baitCol.enabled = false;
	}

	public void ResetFishingRod () {
		fishingRod.isCatchSomething = false;
		fishingRod.fishObj = null;
		fishingRod.fish = null;
		fishingRod.isBaitLaunched = false;
		fishingRod.transform.localPosition = Vector3.zero;
		fishingRod.state = FishingRodState.IDLE;
	}

	public void ProcessFish () {
		fishSystem.CatchFish(fishingRod.fish, playerMovementSystem.player.transform);
	}

	Vector3 GetDestinationPos(Vector3 throwObjInitPos, int dirID, float range)
	{
		Vector3 destination = throwObjInitPos;
		float x = throwObjInitPos.x;
		float z = throwObjInitPos.z;

		#region 4 Direction
		if(dirID == 1){ //bottom
			z-=range;
		}else if(dirID == 3){ //left
			x-=range;
		}else if(dirID == 5){ //top
			z+=range;
		}else if(dirID == 7){ //right
			x+=range;
		}
		#endregion

#region 8 Direction
		// if(dirID == 1){ //bottom
		// 	z-=range;
		// }else if(dirID == 2){ //bottom left
		// 	x-=range;
		// 	z-=range;
		// }else if(dirID == 3){ //left
		// 	x-=range;
		// }else if(dirID == 4){ //top left
		// 	x-=range;
		// 	z+=range;
		// }else if(dirID == 5){ //top
		// 	z+=range;
		// }else if(dirID == 6){ //top right
		// 	x+=range;
		// 	z+=range;
		// }else if(dirID == 7){ //right
		// 	x+=range;
		// }else if(dirID == 8){ //bottom right
		// 	x+=range;
		// 	z-=range;
		// }
#endregion

		return new Vector3(x, 0f, z);
	}
}
