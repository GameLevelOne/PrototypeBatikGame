using UnityEngine;
using Unity.Entities;

public class PowerBraceletSystem : ComponentSystem {
	public struct PowerBraceletData {
		public readonly int Length;
		public ComponentArray<PowerBracelet> powerBracelet;
	}
	[InjectAttribute] PowerBraceletData powerBraceletData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] ManaSystem manaSystem;

	public PowerBracelet powerBracelet;
	
	public bool withStand = false;

	PlayerInput input;

	PowerBraceletState state;

	bool isDoneCheckState = false;

	protected override void OnUpdate () {
		if (powerBraceletData.Length == 0) return;

		if (input == null) {
			input = playerInputSystem.input;

			return;
		}

		for (int i=0; i<powerBraceletData.Length; i++) {
			powerBracelet = powerBraceletData.powerBracelet[i];

			if (powerBracelet.isInteracting && !isDoneCheckState) {
				CheckLiftableObject();
				
				state = powerBracelet.state;

				if (state != PowerBraceletState.NONE) {
					SetLiftableInput();
				}
			} 
		}
	}

	void CheckLiftableObject () {
		LiftableType type = powerBracelet.liftable.liftableType;

		if (type == LiftableType.LIFTABLE) {
			if (powerBracelet.liftPower >= powerBracelet.liftable.weight) {
				powerBracelet.SetState(PowerBraceletState.CAN_LIFT);
				withStand = false;
			} else {
				if (isHaveEnoughMana()) {
					powerBracelet.SetState(PowerBraceletState.CAN_LIFT);
				} else {
					powerBracelet.SetState(PowerBraceletState.CANNOT_LIFT);
				}
			}
		} else if (type == LiftableType.UNLIFTABLE) {
			powerBracelet.SetState(PowerBraceletState.CANNOT_LIFT);
		} else if (type == LiftableType.GRABABLE) {
			if (powerBracelet.liftPower >= powerBracelet.liftable.weight) {
				powerBracelet.SetState(PowerBraceletState.GRAB);
				withStand = false;
			} else {
				if (isHaveEnoughMana()) {
					powerBracelet.SetState(PowerBraceletState.GRAB);
				} else {
					powerBracelet.SetState(PowerBraceletState.CANNOT_LIFT);
				}
			}
		} else {
			powerBracelet.SetState(PowerBraceletState.NONE);
		}
		// Debug.Log("CheckLiftableObject");
		isDoneCheckState = true;
	}

	void SetLiftableInput () {
		if (state == PowerBraceletState.NONE) {
			//
		} else if (state == PowerBraceletState.CAN_LIFT) {
			input.liftingMode = -3;
		} else if (state == PowerBraceletState.CANNOT_LIFT) {
			input.liftingMode = 0;
		} else if (state == PowerBraceletState.GRAB) {
			input.liftingMode = 1;
		}
		// Debug.Log("SetLiftableInput");
		powerBracelet.isInteracting = false;
		isDoneCheckState = false;
		// isCanSetLiftableInput = false;
		// powerBracelet.IsColliderOn = false;
	}

	bool isHaveEnoughMana () {
		if (manaSystem.isHaveEnoughMana(powerBracelet.manaCost, false)) {
			if (powerBracelet.liftPower + powerBracelet.standLiftPower >= powerBracelet.liftable.weight) {
				withStand = true;
				return true;
			} else {
				withStand = false;
				return false;
			}
		} else {
			withStand = false;
			return false;
		}
	}

	public void SetTargetRigidbody (RigidbodyType2D type) {
		powerBracelet.liftable.shadowRigidbody.bodyType = type;
		// powerBracelet.liftable.mainObjRigidbody.bodyType = type; //SET TO DYNAMIC FOR CURVE THROW
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
