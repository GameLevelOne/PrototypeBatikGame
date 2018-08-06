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
	// Rigidbody2D mainObjRb;
	Transform mainObjTransform;
	
	Vector2 shadowPos;
	// Vector2 mainObjPos;
	Vector2 targetPos;
	// Vector3 downCurve;

	float deltaTime;
	float speed;
	float dist;
	float throwRange;
	float moveTime;

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

		shadowRb = liftable.shadowRigidbody;
		mainObjTransform = liftable.mainObjTransform;
		speed = liftable.speed;
		shadowPos = liftable.shadowTransform.position;
		// mainObjPos = liftable.mainObjTransform.localPosition;
		targetPos = liftable.targetDirPos;
		throwRange = liftable.throwRange;
		
		dist = Vector2.Distance(shadowPos, targetPos) + 0.1f;
		moveTime = 0.0f;
		liftable.state = LiftableState.FLY;
	}

	void Fly () {
		if (liftable.attachedObject == null && IsNotFinish(liftable.transform.position, targetPos)) {
			Vector2 dir = liftable.targetDirPos - shadowPos;
			shadowRb.velocity = dir * speed * deltaTime;
		} else {
			shadowRb.velocity = Vector2.zero;
			mainObjTransform.localPosition = Vector2.zero;
			liftable.state = LiftableState.LAND;
		}
	}

	bool IsNotFinish (Vector2 a, Vector2 b) {
		float temp = Vector2.Distance(a,b);
		moveTime += deltaTime;

		if (dist > temp) {
			dist = temp;

			if (dist > throwRange / 1.5f) { // >2
				// Debug.Log("Up");
				// downCurve = new Vector3(0f,mainObjPos.y + 1f,-1f);
			} else if (dist > throwRange / 3) { // <1
				// Debug.Log("Steady");
				// downCurve = new Vector3(0f,mainObjPos.y,-1f);
			} else {
				// downCurve = new Vector3(0f,0f,-1f);
				// Debug.Log("Down");
			}
			Debug.Log(dist);

			// mainObjTransform.localPosition = Vector3.Lerp(mainObjPos, downCurve, dist * moveTime); Curve Move			
			return true;
		} else {
			return false;
		}
	}

	void Land () {
		Vector3 newObjPos = new Vector3 (0f, 0f, -1f);
		liftable.mainObjTransform.localPosition = newObjPos;
		liftable.shadowRigidbody.bodyType = RigidbodyType2D.Static;
		liftable.mainObjRigidbody.bodyType = RigidbodyType2D.Kinematic;
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
