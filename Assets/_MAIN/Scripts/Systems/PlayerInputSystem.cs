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
		public ComponentArray<Health> Health;
	}
	[InjectAttribute] InputData inputData;
	[Inject] ToolSystem toolSystem;

	Vector2 currentDir = Vector2.zero;
	float parryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	bool isAttackAway = true;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;
		
		for (int i=0; i<inputData.Length; i++) {
			PlayerInput input = inputData.PlayerInput[i];
			Player player = inputData.Player[i];
			Health health = inputData.Health[i];
			int maxValue = input.moveAnimValue[2];
			int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];
			float chargeAttackThreshold = input.chargeAttackThreshold;
			float beforeChargeDelay = input.beforeChargeDelay;
			float attackAwayDelay = input.attackAwayDelay;
			float bulletTimeDuration = input.bulletTimeDuration;

			float guardParryDelay = input.guardParryDelay;
			float bulletTimeDelay = input.bulletTimeDelay;
			// bool isGuarding = input.isGuarding;
			// bool isParrying = input.isParrying;

			if (player.isSlowMotion || player.isRapidSlashing) {
				if (slowDownTimer < bulletTimeDuration) {
					slowDownTimer += Time.deltaTime;

					if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
						input.AttackMode = 1;
						input.BulletTimeAttackQty++;
					}
				} else {
					player.isSlowMotion = false;
					slowDownTimer = 0f;
					Time.timeScale = 1f;
					player.isRapidSlashing = true;
				}

				return;
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
						if (!player.isHitAnEnemy){
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
				
				player.isGuarding = true;
				player.isParrying = true;
			}
			
			if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {
				if (parryTimer < guardParryDelay) {
					parryTimer += Time.deltaTime;
				} else {
					player.isParrying = false;
					player.isPlayerHit = false;
				}
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				SetMovement(i, 0, false);
				
				player.isGuarding = false;
				parryTimer = 0f;
				player.isParrying = false;
			}
			#endregion

			#region Button Dodge			
			if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
				input.isDodging = true; //START DODGE
				// player.isBulletTiming = true;
			}

			if (input.isDodging) {
				if (bulletTimeTimer < bulletTimeDelay) {
					bulletTimeTimer += Time.deltaTime;
					player.isBulletTiming = true;
				} else {
					player.isBulletTiming = false;
					player.isPlayerHit = false;
					bulletTimeTimer = 0f;
				}
			}	

			if (player.isBulletTiming) {
				if (player.isPlayerHit) {
					player.isBulletTiming = false;
					input.isDodging = false;
					ChangeDir(i, midValue, midValue);
					input.AttackMode = -3;
					player.isSlowMotion = true;
					Debug.Log("Start BulletTime");
				}
			}

			// if (Input.GetKeyUp(KeyCode.KeypadPeriod)) {
				// SetMovement(i, 0, false);
				// input.isDodging = false;
				// bulletTimeTimer = 0f;
				// player.isBulletTiming = false;
			// }
			#endregion
			
			if (player.isParrying) {
				if (player.isPlayerHit) {
					input.AttackMode = -2;
					player.isParrying = false;
					player.isPlayerHit = false;
					Debug.Log("Start Counter");
				}
			} else {
				player.isPlayerHit = false;
			}

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
