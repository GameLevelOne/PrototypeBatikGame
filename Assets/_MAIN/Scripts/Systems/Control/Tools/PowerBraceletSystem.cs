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

	bool isDoneInitPowerBracelet;
	bool isDoneCheckState = false;

	float weight;
	float manaCost;
	float liftPower;
	float standLiftPower;
	float throwRange;
	float speed;

	protected override void OnUpdate () {
		if (powerBraceletData.Length == 0) return;

		if (input == null) {
			input = playerInputSystem.input;

			return;
		}

		for (int i=0; i<powerBraceletData.Length; i++) {
			powerBracelet = powerBraceletData.powerBracelet[i];

			if (!isDoneInitPowerBracelet) {
				InitPowerBracelet ();
			} else {
				if (powerBracelet.isInteracting && !isDoneCheckState) {
					CheckLiftableObject();

					state = powerBracelet.state;

					if (state != PowerBraceletState.NONE) {
						SetLiftableInput();
					}
				} 
			}
		}
	}

	void InitPowerBracelet () {
		manaCost = powerBracelet.manaCost;
		liftPower = powerBracelet.liftPower;
		standLiftPower = powerBracelet.standLiftPower;
		throwRange = powerBracelet.throwRange;
		speed = powerBracelet.speed;
		powerBracelet.isInitLiftToParent = false;
		
		isDoneInitPowerBracelet = true;
	}

	void CheckLiftableObject () {
		LiftableType type = powerBracelet.liftable.liftableType;
		weight = powerBracelet.liftable.weight;

		if (type == LiftableType.LIFTABLE) {
			if (liftPower >= weight) {
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
			if (liftPower >= weight) {
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
		// if (state == PowerBraceletState.NONE) {
			
		// } else 
		if (state == PowerBraceletState.CAN_LIFT) {
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
		if (manaSystem.isHaveEnoughMana(powerBracelet.manaCost, false, false)) {
			if (liftPower + standLiftPower >= weight) {
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

	void SetTargetRigidbodyKinematic (bool isKinematic) { // OLD Parameter RigidbodyType2D type
		powerBracelet.liftable.mainRigidbody.isKinematic = isKinematic;
#region OLD
		// powerBracelet.liftable.shadowRigidbody.bodyType = type;
		// // powerBracelet.liftable.mainObjRigidbody.bodyType = type; //SET TO DYNAMIC FOR CURVE THROW
#endregion
	}

	void SetTargetRigidbodyPositionConstraints (bool isStatic) { // OLD Parameter RigidbodyType2D type
		if (isStatic) {
			powerBracelet.liftable.mainRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		} else {
			powerBracelet.liftable.mainRigidbody.constraints = RigidbodyConstraints.None;
			powerBracelet.liftable.mainRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
	}

	/// <summary>
    /// <para>typeID: <br /></para>
	/// <para>0 Static<br /></para>
	/// <para>1 Dynamic<br /></para>
	/// <para>2 Kinematic<br /></para>
    /// </summary>
	public void SetTargetRigidbodyType (int typeID) {
		switch (typeID) {
			case 0: 
				// SetTargetRigidbodyKinematic(true); //NOT NECESSARY
				SetTargetRigidbodyPositionConstraints(true);
				break;
			case 1: 
				SetTargetRigidbodyKinematic(false);
				SetTargetRigidbodyPositionConstraints(false);
				break;
			case 2: 
				SetTargetRigidbodyKinematic(true);
				SetTargetRigidbodyPositionConstraints(true);
				break;
		}
	}

	public void AddForceRigidbody () {
		// SetTargetRigidbody(RigidbodyType2D.Dynamic);
		SetTargetRigidbodyType(1);
		// powerBracelet.liftable.dirID = dirID;
		// powerBracelet.liftable.throwRange = powerBracelet.throwRange;
		// powerBracelet.liftable.speed = powerBracelet.speed;
		powerBracelet.liftable.projectile.isStartLaunching = true;
		powerBracelet.liftable.mainRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		// powerBracelet.liftable.mainRigidbody.useGravity = true;

		powerBracelet.liftable.state = LiftableState.THROW;
	}

	public void SetLiftObjectParent () {
		if (!powerBracelet.isInitLiftToParent) {
			if (powerBracelet.liftable != null) {
				powerBracelet.liftable.mainCollider.isTrigger = true;
				powerBracelet.liftable.mainTransform.parent = powerBracelet.liftMainObjParent;
				powerBracelet.liftable.mainTransform.localPosition = Vector3.zero;
				powerBracelet.liftable.mainRigidbody.mass = powerBracelet.liftable.throwMass;
				powerBracelet.liftable.mainRigidbody.drag = 0f;
				powerBracelet.liftable.gravityAwakeTimer = powerBracelet.liftable.initGravityAwakeTime;
				// powerBracelet.liftable.shadowTransform.parent = powerBracelet.liftShadowParent;
				// powerBracelet.liftable.shadowTransform.localPosition = Vector2.zero;

				if (powerBracelet.liftable.GetComponent<Bush>() != null) {
					powerBracelet.liftable.GetComponent<Bush>().isLifted = true;
				}
			}

			powerBracelet.isInitLiftToParent = true;
		}
	}

	public void UnSetLiftObjectParent (int dirID) {
		if (powerBracelet.liftable != null) {
			powerBracelet.liftable.projectile.direction = GetDirPos (powerBracelet.liftable.mainTransform.localPosition, dirID);
			// powerBracelet.liftable.testDir.localPosition = powerBracelet.liftable.projectile.direction; // TEMP
			powerBracelet.liftable.mainCollider.isTrigger = false;
			// powerBracelet.liftable.shadowTransform.parent = null;
			powerBracelet.liftable.mainTransform.parent = null;
		}

		powerBracelet.isInitLiftToParent = false;
	}

	public void ResetPowerBracelet () {	
		powerBracelet.isInteracting = false;
		powerBracelet.isInitLiftToParent = false;
		powerBracelet.liftable = null;
		powerBracelet.SetState(PowerBraceletState.NONE);
	}

	Vector3 GetDirPos(Vector3 throwObjInitPos, int dirID)
	{
		// Vector3 destination = throwObjInitPos;
		float x = throwObjInitPos.x;
		float z = throwObjInitPos.z;

		if (dirID == 1){ //bottom
			return new Vector3(0f, 0f, z-1);
		} else if (dirID == 2){ //left
			return new Vector3(x-1, 0f, 0f);
		} else if (dirID == 3){ //top
			return new Vector3(0f, 0f, z+1);
		} else if (dirID == 4){ //right
			return new Vector3(x+1, 0f, 0f);
		} else {
			Debug.Log("Something wrong in Power Bracelet throw direction");
			return new Vector3(0f, 0f, 0f);
		}
	}
}
