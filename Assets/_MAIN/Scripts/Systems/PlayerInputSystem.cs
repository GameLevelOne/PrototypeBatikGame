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
		public ComponentArray<Player> Player;
		// public ComponentArray<PlayerTool> PlayerTool;
		public ComponentArray<Health> Health;
	}
	[InjectAttribute] InputData inputData;
	[InjectAttribute] ToolSystem toolSystem;

	public PlayerInput input;

	Vector2 currentDir = Vector2.zero;
	float parryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	bool isAttackAway = true;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;

		float deltaTime = Time.deltaTime;
		
		for (int i=0; i<inputData.Length; i++) {
			input = inputData.PlayerInput[i];
			Player player = inputData.Player[i];
			// PlayerTool playerTool = inputData.PlayerTool[i];
			Health health = inputData.Health[i];
			PlayerTool tool = toolSystem.tool;

			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];
			float chargeAttackThreshold = input.chargeAttackThreshold;
			float beforeChargeDelay = input.beforeChargeDelay;
			float attackAwayDelay = input.attackAwayDelay;
			float bulletTimeDuration = input.bulletTimeDuration;

			float guardParryDelay = input.guardParryDelay;
			float bulletTimeDelay = input.bulletTimeDelay;
			// bool isGuarding = input.IsGuarding;
			// bool isParrying = input.IsParrying;

			if (player.IsSlowMotion || player.IsRapidSlashing) {
				if (slowDownTimer < bulletTimeDuration) {
					slowDownTimer += deltaTime;

					if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
						input.AttackMode = 1;
						input.BulletTimeAttackQty++;
					}
				} else {
					player.IsSlowMotion = false;
					slowDownTimer = 0f;
					Time.timeScale = 1f;
					player.IsRapidSlashing = true;
					input.SteadyMode = 0;
				}

				continue;
			}

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

			// if (input.IsUsingTool) {
			// 	SetMovement(i, 0, false);
			// 	chargeAttackTimer = 0f;
			// 	isAttackAway = false;
				
			// 	player.IsGuarding = false;
			// 	parryTimer = 0f;
			// 	player.IsParrying = false;

			// 	continue;
			// }

			#region Button Attack
			if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) {
				chargeAttackTimer += deltaTime;
				
				if (chargeAttackTimer >= beforeChargeDelay) {
					Debug.Log("Start charging");
					SetMovement(i, 1, false); //START CHARGE
				}
			} else {
				if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
					attackAwayTimer += deltaTime;
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
						if (!player.IsHitAnEnemy){
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
			if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Debug.Log("Start Guard");
				SetMovement(i, 2, false); //START GUARD
				
				player.IsGuarding = true;
				player.IsParrying = true;
			}
			
			if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {
				if (parryTimer < guardParryDelay) {
					parryTimer += deltaTime;
				} else {
					player.IsParrying = false;
					player.IsPlayerHit = false;
				}
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				SetMovement(i, 0, false);
				
				player.IsGuarding = false;
				parryTimer = 0f;
				player.IsParrying = false;
			}
			#endregion

			#region Button Dodge			
			if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
				input.IsDodging = true; //START DODGE
				// player.IsBulletTiming = true;
			}

			if (input.IsDodging) {
				if (bulletTimeTimer < bulletTimeDelay) {
					bulletTimeTimer += deltaTime;
					player.IsBulletTiming = true;
				} else {
					player.IsBulletTiming = false;
					player.IsPlayerHit = false;
					bulletTimeTimer = 0f;
				}
			}	

			if (player.IsBulletTiming) {
				if (player.IsPlayerHit) {
					player.IsBulletTiming = false;
					input.IsDodging = false;
					ChangeDir(i, midValue, midValue);
					input.SteadyMode = 3; //STEADY FOR RAPID SLASH
					input.AttackMode = -3;
					player.IsSlowMotion = true;
					Debug.Log("Start BulletTime");
				}
			}

			// if (Input.GetKeyUp(KeyCode.KeypadPeriod)) {
				// SetMovement(i, 0, false);
				// input.IsDodging = false;
				// bulletTimeTimer = 0f;
				// player.IsBulletTiming = false;
			// }
			#endregion
			
			if (player.IsParrying) {
				if (player.IsPlayerHit) {
					input.AttackMode = -2;
					player.IsParrying = false;
					player.IsPlayerHit = false;
					Debug.Log("Start Counter");
				}
			} else {
				player.IsPlayerHit = false;
			}

			#region Button Tools
			if(Input.GetKeyDown(KeyCode.C)){
				if (!input.IsUsingTool) {
					Debug.Log("Input Change Tool");
					input.IsChangeTool = true;
					// input.ToolType++;
					// toolSystem.ChangeTool(playerTool);
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)){
				int toolType = (int)tool.currentTool;

				if (!input.IsUsingTool && (toolType != 0)) {
					Debug.Log("Input Use Tool");
					input.IsUsingTool = true;
					// toolSystem.UseTool(playerTool);
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

	// void UseTool()
	// {
	// 	// toolSystem.Enabled = true;
	// }
}
