﻿using UnityEngine;
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

	LiftState state;
	PlayerInput input;
	LiftableType type;

	bool isLiftResponse = false;

	protected override void OnUpdate () {
		if (liftData.Length == 0) return;

		if (input == null) {
			input = playerInputSystem.input;

			return;
		}

		for (int i=0; i<liftData.Length; i++) {
			powerBracelet = liftData.powerBracelet[i];
			type = powerBracelet.type;

			if (powerBracelet.IsInteracting && !isLiftResponse) {
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
					input.LiftingMode = -1;
				} else if (state == LiftState.CANNOT_LIFT) {
					input.LiftingMode = 0;
				} else if (state == LiftState.GRAB) {
					input.LiftingMode = 1;
				}

				// powerBracelet.IsInteracting = false;
				isLiftResponse = false;
				// powerBracelet.IsColliderOn = false;
			}
		}
	}

	void SetPowerBraceletState (LiftState state) {
		powerBracelet.state = state;
	}
}
