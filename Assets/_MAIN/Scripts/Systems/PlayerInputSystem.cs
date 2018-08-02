﻿using UnityEngine;
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
	[InjectAttribute] PowerBraceletSystem powerBraceletSystem;

	public PlayerInput input;
	public Player player;

	PlayerState state;
	ToolType toolType;
	LiftState liftState;

	Vector2 currentDir = Vector2.zero;
	float parryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	float dodgeCooldownTimer = 0f;
	bool isAttackAway = true;
	bool isReadyForDodging = true;

	bool isDodging = false;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;
		
		if (liftState == null) {
			liftState = powerBraceletSystem.powerBracelet.state;

			return;
		} 

		float deltaTime = Time.deltaTime;
		
		for (int i=0; i<inputData.Length; i++) {
			input = inputData.PlayerInput[i];
			player = inputData.Player[i];
			state = player.state;
			Health health = inputData.Health[i];
			PlayerTool tool = toolSystem.tool;

			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];
			float chargeAttackThreshold = input.chargeAttackThreshold;
			float beforeChargeDelay = input.beforeChargeDelay;
			float attackAwayDelay = input.attackAwayDelay;
			float bulletTimeDuration = input.bulletTimeDuration;
			float dodgeCooldown = input.dodgeCooldown;

			float guardParryDelay = input.guardParryDelay;
			float bulletTimeDelay = input.bulletTimeDelay;

			if (state == PlayerState.SLOW_MOTION) {
				if (slowDownTimer < bulletTimeDuration) {
					slowDownTimer += deltaTime;

					if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
						input.AttackMode = 1;
						input.BulletTimeAttackQty++;
					}
				} else {
					slowDownTimer = 0f;
					Time.timeScale = 1f;
					input.SteadyMode = 0;
					player.SetPlayerState(PlayerState.RAPID_SLASH);
				}

				continue;
			} else if (state == PlayerState.RAPID_SLASH) {
				continue;
			}

			#region Button Movement
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
				ChangeDir(currentDir.x, maxValue);
			} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
				ChangeDir(currentDir.x, minValue);
			} 
			
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
				ChangeDir(maxValue, currentDir.y);
			} else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
				ChangeDir(minValue, currentDir.y);
			} 
			
			if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) {
				ChangeDir(midValue, currentDir.y);
				CheckEndMove();
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
				ChangeDir(currentDir.x, midValue);
				CheckEndMove();
			}
			#endregion

			if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK) {	

				continue;
			} else if (state == PlayerState.DASH) {
				if (Input.GetKeyUp(KeyCode.Space)){
					input.InteractMode = 2;
					player.SetPlayerState (PlayerState.BRAKE);
				}

				continue;
			} else if (state == PlayerState.BOUNCE) {
				input.InteractMode = 2;

				continue;
			} else if (state == PlayerState.GET_HURT) {
				input.InteractMode = -2;

				continue;
			}

			#region Button Attack
			if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) {
				chargeAttackTimer += deltaTime;
				
				if (chargeAttackTimer >= beforeChargeDelay) {
					Debug.Log("Start charging");
					SetMovement(1, false); //START CHARGE
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
					input.AttackMode = -1; //CHARGE
					player.SetPlayerState(PlayerState.CHARGE);
				} else {
					if (input.AttackMode <= 2) {
						if (!player.IsHitAnEnemy){
							input.AttackMode = 1; //SLASH							
						} else {
							input.AttackMode += 1; //SLASH
						}
					}
					player.SetPlayerState(PlayerState.ATTACK);	
				}
				
				SetMovement(0, false);
				chargeAttackTimer = 0f;
				isAttackAway = false;			
			}
			#endregion

			#region Button Guard
			if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				SetMovement(2, false); //START GUARD
				
				player.IsGuarding = true;
			}
			
			if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {

				if (state == PlayerState.BLOCK_ATTACK) {
					input.InteractMode = -1;
				}

				if (parryTimer < guardParryDelay) {
					parryTimer += deltaTime;	
					player.IsParrying = true;
				} else {
					player.IsParrying = false;
					player.IsPlayerHit = false;	
				}
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				SetMovement(0, false);
				
				player.IsGuarding = false;
				parryTimer = 0f;
				player.IsParrying = false;
			}
			#endregion

			#region Button Dodge			
			if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
				if (!isDodging && isReadyForDodging && (currentDir != Vector2.zero)) {
					player.SetPlayerState(PlayerState.DODGE);
					bulletTimeTimer = 0f;	
					dodgeCooldownTimer = 0f;
					isDodging = true;
					isReadyForDodging = false;
					input.InteractMode = 0;
				}
			}	

			if (isDodging) {
				if (dodgeCooldownTimer < dodgeCooldown) {
					dodgeCooldownTimer += deltaTime;
				} else {
					isDodging = false;
					isReadyForDodging = true;
				}

				if (state == PlayerState.DODGE) {
					if (bulletTimeTimer < bulletTimeDelay) {
						bulletTimeTimer += deltaTime;
						player.IsBulletTiming = true;
					} else {
						player.IsBulletTiming = false;
						player.IsPlayerHit = false;
					}
				}
			}

			if (player.IsBulletTiming) {
				if (player.IsPlayerHit) {	
					player.IsBulletTiming = false;
					ChangeDir(midValue, midValue);
					input.SteadyMode = 3; //STEADY FOR RAPID SLASH
					input.AttackMode = -3;
					Debug.Log("Start BulletTime");
					player.SetPlayerState(PlayerState.SLOW_MOTION);
				}
			}
			#endregion
			
			if (player.IsParrying) {
				if (player.IsPlayerHit) {
					input.AttackMode = -2;
					player.IsParrying = false;
					player.IsPlayerHit = false;
					Debug.Log("Start Counter");
					player.SetPlayerState(PlayerState.COUNTER);
				}
			} else {
				player.IsPlayerHit = false;
			}

			#region Button Tools
			if(Input.GetKeyDown(KeyCode.X)){
				if (state != PlayerState.USING_TOOL) {
					Debug.Log("Input Next Tool");
					toolSystem.NextTool();
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Z)){
				if (state != PlayerState.USING_TOOL) {
					Debug.Log("Input Prev Tool");
					toolSystem.PrevTool();
				}
			}
			
			if (Input.GetKeyDown(KeyCode.Space)){
				toolType = tool.currentTool;

				if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (toolType != ToolType.None)) {
					Debug.Log("Input Use Tool : " + toolType);

					if (toolType == ToolType.Hook) {
						player.SetPlayerState(PlayerState.HOOK);
					} else if (toolType == ToolType.Boots) {
						input.InteractMode = 1;
						player.SetPlayerState(PlayerState.DASH);
					} else if (toolType == ToolType.PowerBracelet) {
						if (liftState != LiftState.NONE) {
							input.InteractMode = 3;
							player.SetPlayerState(PlayerState.POWER_BRACELET);
						} else {
							continue;
						}
					} else {
						player.SetPlayerState(PlayerState.USING_TOOL);
					}
				} else if (state == PlayerState.POWER_BRACELET && input.LiftingMode < 0) { 
					input.InteractValue = 2;
					input.LiftingMode = -1;
				}
			}

			if (Input.GetKeyUp(KeyCode.Space)){
				if (state == PlayerState.POWER_BRACELET && input.LiftingMode >= 0) {
					input.InteractValue = 2;
					input.LiftingMode = 0;
				}
			}
			#endregion
		}
	}

	void SetMovement (int value, bool isMoveOnly) {
		input.MoveMode = value;
		
		if (!isMoveOnly) {
			input.SteadyMode = value;
		}
	}

	void ChangeDir (float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);

		if (currentDir != newDir) {
			currentDir = newDir;
			input.MoveDir = currentDir;

			if (state == PlayerState.POWER_BRACELET && input.LiftingMode == -1) {
				input.LiftingMode = -2;
			} else if (state == PlayerState.POWER_BRACELET && input.LiftingMode == 1) {
				input.LiftingMode = 2;
			}
		}
	}

	void CheckEndMove () {
		if (state == PlayerState.MOVE) {
			player.SetPlayerIdle();
		} else if (state == PlayerState.POWER_BRACELET && input.LiftingMode == -2) {
			input.LiftingMode = -1;
		} else if (state == PlayerState.POWER_BRACELET && input.LiftingMode == 2) {
			input.LiftingMode = 1;
		}
	}
}
