using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ThrowSystem : ComponentSystem {
	public struct LiftData {
		public readonly int Length;
		public ComponentArray<Liftable> Liftable;
	}
	[InjectAttribute] LiftData liftData;
	
	Liftable liftable;
	LiftableState state;

	Rigidbody2D shadowRb;
	
	Vector2 objPos;
	Vector2 targetPos;

	float deltaTime;
	float speed;
	float dist;

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<liftData.Length; i++) {
			liftable = liftData.Liftable[i];
			state = liftable.state;
			
			if (state == LiftableState.THROW) {
				Throw ();
			} else if (state == LiftableState.FLY) {
				Fly ();
			} else if (state == LiftableState.LAND) {
				Land ();
			}
		}
	}

	void Throw () {
		liftable.mainObjTransform.parent = liftable.shadowTransform;
		liftable.initPos = liftable.transform.position;
		liftable.targetDirPos = GetDestinationPos(liftable.initPos, liftable.dirID, liftable.throwRange);
		liftable.state = LiftableState.FLY;

		shadowRb = liftable.shadowRigidbody;
		speed = liftable.speed;
		objPos = liftable.transform.position;
		targetPos = liftable.targetDirPos;
		
		dist = Vector2.Distance(objPos, targetPos) + 1;
	}

	void Fly () {
		// IsFinish(objPos, targetPos);

		if (liftable.attachedObject == null && IsNotFinish(liftable.transform.position, targetPos)) {
			Vector2 dir = liftable.targetDirPos - objPos;
			shadowRb.velocity = dir * speed * deltaTime;
		} else {
			shadowRb.velocity = Vector2.zero;
			liftable.state = LiftableState.LAND;
		}
	}

	bool IsNotFinish (Vector2 a, Vector2 b) {
		float temp = Vector2.Distance(a,b);

		if (dist > temp) {
			dist = temp;
			return true;
		} else {
			return false;
		}
	}

	void Land () {
		Vector3 newObjPos = new Vector3 (0f, 0f, -1f);
		liftable.mainObjTransform.localPosition = newObjPos;
		liftable.shadowRigidbody.bodyType = RigidbodyType2D.Static;
		liftable.state = LiftableState.IDLE;
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
