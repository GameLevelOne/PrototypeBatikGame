using UnityEngine;
using Unity.Entities;

public class FishCollectibleSystem : ComponentSystem {
	public struct FishCollectibleData {
		public readonly int Length;
		public ComponentArray<FishCollectible> FishCollectible;
		public ComponentArray<Rigidbody2D> Rigidbody;
	}
	[InjectAttribute] FishCollectibleData fishCollectibleData;

	FishCollectible fishCollectible;

	Rigidbody2D rigidbody;
	
	float timer;
	float deltaTime;

	protected override void OnUpdate () {
		if (fishCollectibleData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishCollectibleData.Length; i++) {
			fishCollectible = fishCollectibleData.FishCollectible[i];
			rigidbody = fishCollectibleData.Rigidbody[i];
			
			if (fishCollectible.state == FishState.IDLE) {
				Idle ();
			} else if (fishCollectible.state == FishState.PATROL) {
				Patrol ();
			} else if (fishCollectible.state == FishState.CHASE) {
				ChaseBait ();
			} else if (fishCollectible.state == FishState.CATCH) {
				WaitingToCatch ();
			} else if (fishCollectible.state == FishState.FLEE) {
				Flee ();
			}
		}
	}

	void Idle () {
		if (fishCollectible.TimeIdle <= 0f) {
			fishCollectible.targetPos = RandomPosition ();
			fishCollectible.state = FishState.PATROL;
		} else {
			fishCollectible.TimeIdle -= deltaTime;
		}
	}

	void Patrol () {
		if (Vector2.Distance(fishCollectible.targetPos, rigidbody.position) < 0.1f) {
			fishCollectible.TimeIdle = fishCollectible.idleDuration;
			fishCollectible.state = FishState.IDLE;
		} else {
			Move (rigidbody.position, fishCollectible.targetPos, fishCollectible.moveSpeed);
		}
	}

	void ChaseBait () {
		if (Vector2.Distance(fishCollectible.targetPos, rigidbody.position) < 0.1f) {
			fishCollectible.state = FishState.CATCH;
		} else {
			Move (rigidbody.position, fishCollectible.targetPos, fishCollectible.chaseSpeed);
		}
	}

	void WaitingToCatch () {
		if (timer < fishCollectible.timeToCatch) {
			timer += deltaTime;
		} else {
			timer = 0f;
			fishCollectible.targetPos = RandomPosition ();
			fishCollectible.state = FishState.FLEE;
		}
	}

	void Flee () {
		if (Vector2.Distance(fishCollectible.targetPos, rigidbody.position) < 0.1f) {
			fishCollectible.TimeIdle = fishCollectible.idleDuration;
			fishCollectible.state = FishState.IDLE;
		} else {
			Move (rigidbody.position, fishCollectible.targetPos, fishCollectible.fleeSpeed);
		}
	}

	public void CatchFish (FishCollectible fish) {
		GameObject.Destroy (fish.gameObject);
		UpdateInjectedComponentGroups();
	}

	void Move (Vector2 initPos, Vector2 targetPos, float speed) {
		rigidbody.position = Vector2.MoveTowards(initPos, targetPos, speed * deltaTime);
	}

	Vector2 RandomPosition () {
		float poolRadius = fishCollectible.parentPoolCol.bounds.size.x / 2; //CIRCLE
		Vector2 poolPos = fishCollectible.parentPoolCol.transform.position;
		float randomX = poolPos.x + Random.Range(-poolRadius, poolRadius);
		float randomY = poolPos.y + Random.Range(-poolRadius, poolRadius);
		
		return new Vector2 (randomX, randomY);
	}
}
