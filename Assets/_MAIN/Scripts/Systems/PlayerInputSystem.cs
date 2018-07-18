﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerInputSystem : ComponentSystem {
	public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
	}
	[InjectAttribute] InputData inputData;

	Vector2 currentDir = Vector2.zero;
	float chargeAttackTimer = 0f;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;
		
		for (int i=0; i<inputData.Length; i++) {
			PlayerInput input = inputData.PlayerInput[i];
			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];
			float chargeAttackThreshold = input.chargeAttackThreshold;

			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				ChangeDir(i, currentDir.x, maxValue);
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				ChangeDir(i, currentDir.x, minValue);
			} 
			
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				ChangeDir(i, maxValue, currentDir.y);
			} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				ChangeDir(i, minValue, currentDir.y);
			} 
			
			if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
				ChangeDir(i, midValue, currentDir.y);
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
				ChangeDir(i, currentDir.x, midValue);
			}

			if (Input.GetButton("Fire1")) {
				chargeAttackTimer += Time.deltaTime;
				
				if (chargeAttackTimer >= 0.3f) {
					Debug.Log("Start charging");
					input.SteadyMode = 1; //START CHARGE
					input.MoveMode = 1; //START CHARGE
				}
			}
			
			if (Input.GetButtonUp("Fire1")) {
				if ((chargeAttackTimer >= chargeAttackThreshold) && input.SteadyMode == 1) {
					Debug.Log("Charge Attack");
					input.AttackMode = -1; //CHARGE
					input.SteadyMode = 0;
					input.MoveMode = 0; 
				} else {
					Debug.Log("Slash Attack");
					input.AttackMode += 1; //SLASH
				}

				chargeAttackTimer = 0f;				
			}
		}
	}

	void ChangeDir (int idx, float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);
		PlayerInput input = inputData.PlayerInput[idx];
		
		input.MoveDir = newDir;
		// if (currentDir != newDir) {
		// 	currentDir = newDir;
		// 	input.MoveDir = currentDir;
		// }
	}
}
