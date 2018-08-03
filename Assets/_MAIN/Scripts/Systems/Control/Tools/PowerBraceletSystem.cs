using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PowerBraceletSystem : ComponentSystem {
	public struct LiftData {
		public readonly int Length;
		public ComponentArray<PowerBracelet> powerBracelet;
	}
	[InjectAttribute] LiftData liftData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	public PowerBracelet powerBracelet;

	PlayerInput input;
	Player player;

	LiftState state;
	LiftableType type;

	bool isLiftResponse = false;

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		if (input == null || player == null) {
			input = playerInputSystem.input;
			player = playerInputSystem.player;

			return;
		}

		for (int i=0; i<liftData.Length; i++) {
			powerBracelet = liftData.powerBracelet[i];
			type = powerBracelet.type;

			if (powerBracelet.IsInteracting && !isLiftResponse && player.IsHitLiftableObject) {
				if (type == LiftableType.LIFTABLE) {
					SetPowerBraceletState(LiftState.CAN_LIFT);
				} else if (type == LiftableType.UNLIFTABLE) {
					SetPowerBraceletState(LiftState.CANNOT_LIFT);
				} else if (type == LiftableType.GRABABLE) {
					SetPowerBraceletState(LiftState.GRAB);
				} else {
					SetPowerBraceletState(LiftState.NONE);
				}

				isLiftResponse = true;
			}

			state = powerBracelet.state;

			if (isLiftResponse) {
				if (state == LiftState.NONE) {
					//
				} else if (state == LiftState.CAN_LIFT) {
					input.LiftingMode = -3;
					//
				} else if (state == LiftState.CANNOT_LIFT) {
					input.LiftingMode = 0;
				} else if (state == LiftState.GRAB) {
					input.LiftingMode = 1;
				}

				powerBracelet.IsInteracting = false;
				isLiftResponse = false;
				// powerBracelet.IsColliderOn = false;
			}
		}
	}

	void SetPowerBraceletState (LiftState state) {
		powerBracelet.state = state;
	}

	public void SetTargetRigidbody (RigidbodyType2D type) {
		powerBracelet.liftableRigidbody.bodyType = type;
	}

	public void AddForceRigidbody (int dirID, float force) {
		// powerBracelet.liftableRigidbody.AddForce (startPos * force);
		Vector2 initPos = powerBracelet.transform.position;
		Vector2 targetDirPos = GetDestinationPos(initPos, dirID, powerBracelet.throwRange);
		Debug.Log("targetDirPos : " + targetDirPos);

		// rb.AddForce((target.position - tr.position) * movement.attackMoveSpeed);
	}

	public void SetLiftObjectParent () {
		powerBracelet.liftableCollider.isTrigger = true;
		powerBracelet.liftableTransform.parent = powerBracelet.liftParent;
		powerBracelet.liftableTransform.localPosition = Vector2.zero;
	}

	public void UnSetLiftObjectParent () {
		powerBracelet.liftableCollider.isTrigger = false;
		powerBracelet.liftableTransform.parent = null;
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
