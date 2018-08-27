using UnityEngine;
using Unity.Entities;

public class FishSystem : ComponentSystem {
	public struct FishCollectibleData {
		public readonly int Length;
		public ComponentArray<Fish> Fish;
		// public ComponentArray<Rigidbody2D> Rigidbody;

	}
	[InjectAttribute] FishCollectibleData fishCollectibleData;

	Fish fish;
	
	FishState state;
	Rigidbody2D rigidbody;
	Animator anim;
	
	float timer;
	float deltaTime;

	protected override void OnUpdate () {
		if (fishCollectibleData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishCollectibleData.Length; i++) {
			fish = fishCollectibleData.Fish[i];
			
			rigidbody = fish.rigidbody;
			state = fish.state;
			anim = fish.anim;
			
			if (state == FishState.IDLE) {
				Idle ();
			} else if (state == FishState.PATROL) {
				Patrol ();
			} else if (state == FishState.CHASE) {
				ChaseBait ();
			} else if (state == FishState.CATCH) {
				WaitingToCatch ();
			} else if (state == FishState.FLEE) {
				Flee ();
			}
		}
	}

	void Idle () {
		if (fish.TimeIdle <= 0f) {
			fish.targetPos = RandomPosition ();
			fish.state = FishState.PATROL;
			anim.Play(FishState.CHASE.ToString());
		} else {
			fish.TimeIdle -= deltaTime;
		}
	}

	void Patrol () {
		if (Vector2.Distance(fish.targetPos, rigidbody.position) < 0.1f) {
			fish.TimeIdle = fish.idleDuration;
			fish.state = FishState.IDLE;
			anim.Play(FishState.IDLE.ToString());
		} else {
			Move (rigidbody.position, fish.targetPos, fish.moveSpeed);
		}
	}

	void ChaseBait () {
		if (Vector2.Distance(fish.targetPos, rigidbody.position) < 0.1f) {
			fish.state = FishState.CATCH;
			anim.Play(FishState.IDLE.ToString());
		} else {
			Move (rigidbody.position, fish.targetPos, fish.chaseSpeed);
		}
	}

	void WaitingToCatch () {
		if (timer < fish.timeToCatch) {
			timer += deltaTime;
		} else {
			timer = 0f;
			fish.selfCol.enabled = false;
			fish.targetPos = RandomPosition ();
			fish.state = FishState.FLEE;
			anim.Play(FishState.CHASE.ToString());
		}
	}

	void Flee () {
		if (Vector2.Distance(fish.targetPos, rigidbody.position) < 0.1f) {
			fish.TimeIdle = fish.idleDuration;
			fish.selfCol.enabled = true;
			fish.state = FishState.IDLE;
		} else {
			Move (rigidbody.position, fish.targetPos, fish.fleeSpeed);
		}
	}

	public void CatchFish (Fish fish, Transform playerTransform) {
		//INSTANTIATE LOOTABLE
		GameObject.Instantiate (fish.lootableObjs[Random.Range(0,fish.lootableObjs.Length)], playerTransform.position, Quaternion.identity);

		GameObject.Destroy (fish.gameObject);
		UpdateInjectedComponentGroups();
	}

	void Move (Vector2 initPos, Vector2 targetPos, float speed) {
		rigidbody.position = Vector2.MoveTowards(initPos, targetPos, speed * deltaTime);
		
		Vector2 dir = GetDirection(rigidbody.position,fish.targetPos);
		anim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,dir.x);
		anim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,dir.y);
	}

	Vector2 RandomPosition () {
		float poolRadius = fish.parentPoolRadius;
		Vector2 poolPos = fish.parentPoolCol.position;
		float randomX = poolPos.x + Random.Range(-poolRadius, poolRadius);
		float randomY = poolPos.y + Random.Range(-poolRadius, poolRadius);
		
		return new Vector2 (randomX, randomY);
	}

	/// <summary>
    /// <para>4 Directions: <br /></para>
	/// <para>UL = (-1, 1) <br /></para>
	/// <para>UR = ( 1, 1) <br /></para>
	/// <para>DL = (-1,-1) <br /></para>
	/// <para>DR = ( 1,-1) <br /></para>
    /// </summary>
	Vector2 GetDirection(Vector2 self, Vector2 target)
	{
		Vector3 distance = target - self;
		float magnitude = distance.magnitude;
		Vector3 direction = distance / magnitude;

		float x = direction.x;
		float y = direction.y;
		
		if(x < 0f) x = -1f;
		else x = 1f;

		if(y < 0f) y = -1f;
		else y = 1f;

		return new Vector2(x,y);
	}
}
