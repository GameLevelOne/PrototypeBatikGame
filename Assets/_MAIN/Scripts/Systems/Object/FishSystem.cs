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
	
	FishCharacteristic fishChar;
	FishState state;
	Rigidbody2D rigidbody;
	Animator anim;
	
	float waitingTimer = 0f;
	float randomWaitingDuration;
	int randomTryingGrabTime;
	float catchTimer = 0f;
	float deltaTime;

	Vector2 readyPos;

	bool isInitThinking = false;

	protected override void OnUpdate () {
		if (fishCollectibleData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<fishCollectibleData.Length; i++) {
			fish = fishCollectibleData.Fish[i];
			
			fishChar = fish.fishChar;
			rigidbody = fish.rigidbody;
			state = fish.state;
			anim = fish.anim;
			
			if (state == FishState.IDLE) {
				Idle ();
			} else if (state == FishState.PATROL) {
				Patrol ();
			} else if (state == FishState.THINK) {
				Think ();
			} else if (state == FishState.WAIT) {
				Wait ();
			} else if (state == FishState.TRYINGGRAB) {
				TryingGrab ();
			} else if (state == FishState.CHASE) {
				ChaseBait ();
			} else if (state == FishState.CATCH) {
				Catching ();
			} else if (state == FishState.FLEE) {
				Flee ();
			}
		}
	}

	void Idle () {
		if (!fish.initIdle) {
			anim.Play(Constants.BlendTreeName.ENEMY_IDLE);
			randomWaitingDuration = 0f;
			// randomTryingGrabTime = 0;
			readyPos = Vector2.zero;
			isInitThinking = false;
			fish.initIdle = true;
		} else {
			if (fish.TimeIdle <= 0f) {
				fish.targetPos = RandomPosition ();
				fish.state = FishState.PATROL;
				anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			} else {
				fish.TimeIdle -= deltaTime;
			}
		}
	}

	void Patrol () {
		if (isObjReachDestination(fish.targetPos, rigidbody.position)) {
			fish.TimeIdle = fish.idleDuration;
			fish.state = FishState.IDLE;
			fish.initIdle = false;
		} else {
			Move (rigidbody.position, fish.targetPos, fish.moveSpeed);
		}
	}

	void Think () {
		if (!isInitThinking) {
			randomWaitingDuration = Random.Range(0f, fish.maxWaitingDuration);
			isInitThinking = true;
		} else {
			switch (fishChar) {
				case FishCharacteristic.CALM: 
					fish.state = FishState.WAIT;
					anim.Play(Constants.BlendTreeName.ENEMY_IDLE);
					isInitThinking = false;
					break;
				case FishCharacteristic.WILD:
					int tempRandom = Random.Range(0, fish.maxTryingGrabTimes);
					randomTryingGrabTime = tempRandom % 2 == 0 ? tempRandom : tempRandom++;
					// randomTryingGrabTime = fish.randomTryingGrabTime;
					readyPos = rigidbody.position;
					fish.state = FishState.TRYINGGRAB;
					anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
					isInitThinking = false;
					break;
				default:
					fish.state = FishState.CHASE;
					anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
					isInitThinking = false;
					break;
			}
		}
	}

	void Wait () {
		if (waitingTimer < randomWaitingDuration) {
			waitingTimer += Time.deltaTime;
		} else {
			fish.state = FishState.CHASE;
			anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
			waitingTimer = 0f;
		}
	}

	void TryingGrab () {
		if (waitingTimer < randomWaitingDuration) {
			waitingTimer += Time.deltaTime;
		} else {
			Debug.Log("end waiting");
			// if (randomTryingGrabTime > 0) {
			// 	if (randomTryingGrabTime % 2 == 0) {
			// 		if (isObjReachDestination(fish.targetPos, rigidbody.position)) {
			// 			randomTryingGrabTime--;
			// 			Debug.Log("go to : "+readyPos);
			// 			return;
			// 		} else {
			// 			Move (rigidbody.position, fish.targetPos, fish.moveSpeed);
			// 		}
			// 	} else {
			// 		if (isObjReachDestination(readyPos, rigidbody.position)) {
			// 			fish.randomTryingGrabTime--;
			// 			Debug.Log("go to : "+fish.targetPos);
			// 			return;
			// 		} else {
			// 			Move (rigidbody.position, readyPos, fish.moveSpeed);
			// 		}
			// 	}
			// } else {
				fish.state = FishState.CHASE;
				anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
				waitingTimer = 0f;
			// }
		}
	}

	void ChaseBait () {
		if (isObjReachDestination(fish.targetPos, rigidbody.position)) {
			fish.state = FishState.CATCH;
			anim.Play(Constants.BlendTreeName.ENEMY_IDLE);
		} else {
			Move (rigidbody.position, fish.targetPos, fish.chaseSpeed);
		}
	}

	void Catching () {
		if (catchTimer < fish.timeToCatch) {
			catchTimer += deltaTime;
		} else {
			catchTimer = 0f;
			fish.selfCol.enabled = false;
			fish.targetPos = RandomPosition ();
			fish.state = FishState.FLEE;
			anim.Play(Constants.BlendTreeName.ENEMY_PATROL);
		}
	}

	void Flee () {
		if (isObjReachDestination(fish.targetPos, rigidbody.position)) {
			fish.TimeIdle = fish.idleDuration;
			fish.selfCol.enabled = true;
			fish.state = FishState.IDLE;
			fish.initIdle = false;
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

	bool isObjReachDestination (Vector2 destinationPos, Vector2 currentPos) {
		if (Vector2.Distance(destinationPos, currentPos) < 0.1f) {
			return true;
		} else {
			return false;
		}
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
