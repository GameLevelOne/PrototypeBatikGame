using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using System.Collections.Generic;

public class PlayerInputSystem : ComponentSystem {
	public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Health> Health;
	}
	[InjectAttribute] InputData inputData;
	[Inject] ToolSystem toolSystem;

	Vector2 currentDir = Vector2.zero;
	float parryTimer = 0f;
	float chargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	bool isAttackAway = true;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;
		
		for (int i=0; i<inputData.Length; i++) {
			PlayerInput input = inputData.PlayerInput[i];
			Health health = inputData.Health[i];
			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];
			float chargeAttackThreshold = input.chargeAttackThreshold;
			float beforeChargeDelay = input.beforeChargeDelay;
			float attackAwayDelay = input.attackAwayDelay;
<<<<<<< HEAD
			float guardParryDelay = input.guardParryDelay;
			// bool isGuarding = input.isGuarding;
			// bool isParrying = input.isParrying;
=======
			
>>>>>>> f89cd8658d5385c749265332371990ca52e3e552

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
				
				if (chargeAttackTimer >= beforeChargeDelay) {
					Debug.Log("Start charging");
					SetMovement(i, 1, false); //START CHARGE
				}
			} else {
				if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
					attackAwayTimer += Time.deltaTime;
				} else {
					input.slashComboVal.Clear();
					attackAwayTimer = 0f;
					isAttackAway = true;
				}
			}
			
			if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
				if ((chargeAttackTimer >= chargeAttackThreshold) && input.SteadyMode == 1) {
					Debug.Log("Charge Attack");
					input.AttackMode = -1; //CHARGE
				} else {
					Debug.Log("Slash Attack");
					if (input.AttackMode <= 2) {
						if (!Data.isEnemyHit){
							input.AttackMode = 1; //SLASH							
						} else {
							input.AttackMode += 1; //SLASH
						}
					}
				}
				
				SetMovement(i, 0, false);
				chargeAttackTimer = 0f;
				isAttackAway = false;				
			}
			#endregion

			#region Button Guard
			if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {
				if (parryTimer < guardParryDelay) {
					parryTimer += Time.deltaTime;
					input.isParrying = true;
					
					if (Data.isPlayerHit) {
						input.AttackMode = -2;
						Debug.Log("Input Counter");
					}
				} else {
					input.isParrying = false;
					Data.isPlayerHit = false;
				}
			}

			if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Debug.Log("Start Guard");
				SetMovement(i, 2, false); //START GUARD

				input.isGuarding = true;
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				Debug.Log("End Guard");
				SetMovement(i, 0, false);
				
				input.isGuarding = false;
				parryTimer = 0f;
				input.isParrying = false;
			}
			#endregion

			#region Button Dodge
			if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
				input.isDodging = true; //START DODGE
			}
			#endregion

			#region Button Tools
			if(Input.GetKeyDown(KeyCode.Space)){
				if(!input.IsUsingTool){
					input.IsUsingTool = true;
					UseTool();
				}
			}
			#endregion
		}
	}

	void SetMovement (int idx, int value, bool isMoveOnly) {
		PlayerInput input = inputData.PlayerInput[idx];

		input.MoveMode = value;
		
		if (!isMoveOnly) {
			input.SteadyMode = value;
		}
	}

	void ChangeDir (int idx, float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);
		PlayerInput input = inputData.PlayerInput[idx];
		
		if (currentDir != newDir) {
			currentDir = newDir;
			input.MoveDir = currentDir;
		}
	}

	void UseTool()
	{
		toolSystem.Enabled = true;
	}
}
