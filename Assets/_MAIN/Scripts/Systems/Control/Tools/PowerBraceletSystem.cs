using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PowerBraceletSystem : ComponentSystem {
	public struct PowerBraceletData {
		public readonly int Length;
		public ComponentArray<PowerBracelet> powerBracelet;
	}
	[InjectAttribute] PowerBraceletData powerBraceletData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	public PowerBracelet powerBracelet;

	PlayerInput input;
	Player player;

	PowerBraceletState state;
	LiftableType type;

	bool isLiftResponse = false;

	protected override void OnUpdate () {
		if (powerBraceletData.Length == 0) return;

		if (input == null || player == null) {
			input = playerInputSystem.input;
			player = playerInputSystem.player;

			return;
		}

		for (int i=0; i<powerBraceletData.Length; i++) {
			powerBracelet = powerBraceletData.powerBracelet[i];
			type = powerBracelet.type;

			if (powerBracelet.IsInteracting && !isLiftResponse && player.IsHitLiftableObject) {
				if (type == LiftableType.LIFTABLE) {
					SetPowerBraceletState(PowerBraceletState.CAN_LIFT);
				} else if (type == LiftableType.UNLIFTABLE) {
					SetPowerBraceletState(PowerBraceletState.CANNOT_LIFT);
				} else if (type == LiftableType.GRABABLE) {
					SetPowerBraceletState(PowerBraceletState.GRAB);
				} else {
					SetPowerBraceletState(PowerBraceletState.NONE);
				}

				isLiftResponse = true;
			}

			state = powerBracelet.state;

			if (isLiftResponse) {
				if (state == PowerBraceletState.NONE) {
					//
				} else if (state == PowerBraceletState.CAN_LIFT) {
					input.LiftingMode = -3;
					//
				} else if (state == PowerBraceletState.CANNOT_LIFT) {
					input.LiftingMode = 0;
				} else if (state == PowerBraceletState.GRAB) {
					input.LiftingMode = 1;
				}

				powerBracelet.IsInteracting = false;
				isLiftResponse = false;
				// powerBracelet.IsColliderOn = false;
			}
		}
	}

	void SetPowerBraceletState (PowerBraceletState state) {
		powerBracelet.state = state;
	}

	public void SetTargetRigidbody (RigidbodyType2D type) {
		powerBracelet.liftable.shadowRigidbody.bodyType = type;
		// powerBracelet.liftable.mainObjRigidbody.bodyType = type; //STILL KINEMATIC
	}

	public void AddForceRigidbody (int dirID) {
		SetTargetRigidbody(RigidbodyType2D.Dynamic);
		powerBracelet.liftable.dirID = dirID;
		powerBracelet.liftable.throwRange = powerBracelet.throwRange;
		powerBracelet.liftable.speed = powerBracelet.speed;
		powerBracelet.liftable.state = LiftableState.THROW;
	}

	public void SetLiftObjectParent () {
		powerBracelet.liftable.collider.isTrigger = true;
		powerBracelet.liftable.shadowTransform.parent = powerBracelet.liftShadowParent;
		powerBracelet.liftable.mainObjTransform.parent = powerBracelet.liftMainObjParent;
		powerBracelet.liftable.shadowTransform.localPosition = Vector2.zero;
		powerBracelet.liftable.mainObjTransform.localPosition = Vector2.zero;
	}

	public void UnSetLiftObjectParent () {
		powerBracelet.liftable.collider.isTrigger = false;
		powerBracelet.liftable.shadowTransform.parent = null;
		powerBracelet.liftable.mainObjTransform.parent = null;
	}
}
