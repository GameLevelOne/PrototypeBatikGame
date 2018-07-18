using UnityEngine;
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

			#region Button Movement
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
				ChangeDir(i, currentDir.x, maxValue);
			} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
				ChangeDir(i, currentDir.x, minValue);
			} 
			
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
				ChangeDir(i, maxValue, currentDir.y);
			} else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
				ChangeDir(i, minValue, currentDir.y);
			} 
			
			if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) {
				ChangeDir(i, midValue, currentDir.y);
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
				ChangeDir(i, currentDir.x, midValue);
			}
			#endregion

			#region Button Attack
			if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) {
				chargeAttackTimer += Time.deltaTime;
				
				if (chargeAttackTimer >= 0.3f) {
					Debug.Log("Start charging");
					SetMovement(i, 1); //START CHARGE
				}
			}
			
			if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
				if ((chargeAttackTimer >= chargeAttackThreshold) && input.SteadyMode == 1) {
					Debug.Log("Charge Attack");
					input.AttackMode = -1; //CHARGE
				} else {
					Debug.Log("Slash Attack");
					input.AttackMode += 1; //SLASH
				}
				
				SetMovement(i, 0); 
				chargeAttackTimer = 0f;				
			}
			#endregion

			#region Button Guard
			if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Debug.Log("Guard");
				SetMovement(i, 2); //START GUARD
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				Debug.Log("Guard");
				SetMovement(i, 0);
			}
			#endregion
		}
	}

	void SetMovement (int idx, int value) {
		PlayerInput input = inputData.PlayerInput[idx];

		input.SteadyMode = value;
		input.MoveMode = value;
	}

	void ChangeDir (int idx, float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);
		PlayerInput input = inputData.PlayerInput[idx];
		
		if (currentDir != newDir) {
			currentDir = newDir;
			input.MoveDir = currentDir;
		}
	}
}
