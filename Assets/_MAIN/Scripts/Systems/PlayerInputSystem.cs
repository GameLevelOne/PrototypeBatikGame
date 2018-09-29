using UnityEngine;
using Unity.Entities;

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
	[InjectAttribute] PlayerAnimationSystem playerAnimationSystem;
	[InjectAttribute] GainTreasureSystem gainTreasureSystem;
	[InjectAttribute] ManaSystem manaSystem;
	[InjectAttribute] GameFXSystem gameFXSystem;
	[InjectAttribute] GateOpenerSystem gateOpenerSystem;

	public PlayerInput input;
	public Player player;
	public ToolType toolType;

	PlayerTool tool;
	// Facing2D facing;
	PowerBracelet powerBracelet;

	PlayerState state;

	/// <summary>
    /// <para>Current Move Direction (X, Z)<br /></para>
	/// </summary>
	Vector3 currentDir = Vector3.zero;
	// Vector3 playerDir = Vector3.zero;
	float deltaTime;
	float parryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float startChargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	float dodgeCooldownTimer = 0f;
	float dodgeCooldown = 0f;
	float bulletTimeDelay = 0f;
	bool isAttackAway = true;
	bool isReadyForDodging = true;

	bool isDodging = false;
	bool isBulletTimePeriod = false;
	bool isParryPeriod = false;
	bool isButtonToolHold = false;
	bool isChargingAttack = false;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		// if (inputData.Length == 0) return;
		
		for (int i=0; i<inputData.Length; i++) {
			input = inputData.PlayerInput[i];
			player = inputData.Player[i];
			Health health = inputData.Health[i];

			if (!input.isInitPlayerInput) {
				InitPlayerInput();

				continue;
			}

			state = player.state;
			tool = toolSystem.tool;

			if (CheckIfUsingAnyTool ()) {
				continue;
			}
			
			CheckMovementInput ();
			CheckToolInput ();

			if (CheckIfInSpecificState ()) {
				continue;
			}

			CheckAttackInput ();
			CheckGuardInput ();
			CheckDodgeInput ();
		}
	}

	void InitPlayerInput () {
		currentDir = Vector3.zero;
		// playerDir = Vector3.zero;
		parryTimer = 0f;
		bulletTimeTimer = 0f;
		slowDownTimer = 0f;
		chargeAttackTimer = 0f;
		startChargeAttackTimer = 0f;
		attackAwayTimer = 0f;
		dodgeCooldownTimer = 0f;
		isAttackAway = true;
		isReadyForDodging = true;

		isDodging = false;
		isBulletTimePeriod = false;
		isParryPeriod = false;
		isButtonToolHold = false;
		isChargingAttack = false;
		// input.moveDir = input.initMoveDir;
		dodgeCooldown = input.dodgeCooldown;
		bulletTimeDelay = input.bulletTimeDelay;

		input.isInitPlayerInput = true;
	}

	void CheckMovementInput () {
		//NEW GAME INPUT
		float dirX = 0f;
		float dirZ = 0f;

		if (GameInput.IsUpDirectionHeld)
			dirZ += 1f;
		if (GameInput.IsDownDirectionHeld)
			dirZ -= 1f;
		if (GameInput.IsRightDirectionHeld)
			dirX += 1f;
		if (GameInput.IsLeftDirectionHeld)
			dirX -= 1f;

		// Debug.Log("Input Dir: "+dirX+","+dirZ);

		SetDir(dirX,dirZ);


#region JOYSTICK OLD
		// if (Input.GetJoystickNames().Length >= 1) {
		// 	if (Input.GetJoystickNames()[0] != "") {
		// 		float inputX = Input.GetAxis("Horizontal Javatale");
		// 		float inputY = Input.GetAxis("Vertical Javatale");
		// 		// ChangeDir (inputX, inputY);

		// 		// if (inputX == 0 || inputY == 0) {
		// 		// 	CheckEndMove();
		// 		// }

		// 		//KEY DOWN
			
		// 		if (inputY < 0f) {
		// 			SetJoystickAndKeyboardInput(true, 0);
		// 		} 
				
		// 		if (inputX < 0f) {
		// 			SetJoystickAndKeyboardInput(true, 1);
		// 		}
				
		// 		if (inputY > 0f) {
		// 		Debug.Log("Halooooooo");
		// 			SetJoystickAndKeyboardInput(true, 2);
		// 		}
				
		// 		if (inputX > 0f) {
		// 			SetJoystickAndKeyboardInput(true, 3);
		// 		}
				
		// 		if (inputY == 0f) {
		// 			if (input.dirButtons[0] == 1) {
		// 				SetJoystickAndKeyboardInput(false, 0);						
		// 			} else if (input.dirButtons[2] == 1) {
		// 				SetJoystickAndKeyboardInput(false, 2);
		// 			}
		// 		}
				

		// 		if (inputX == 0f) {
		// 			if (input.dirButtons[1] == 1) {
		// 				SetJoystickAndKeyboardInput(false, 1);						
		// 			} else if (input.dirButtons[3] == 1) {
		// 				SetJoystickAndKeyboardInput(false, 3);
		// 			}
		// 		}
		// 	}
		// } 
#endregion
		
#region MOUSE & KEYBOARD OLD
		// else {
		// 	// int maxValue = input.moveAnimValue[2];
		// 	// int midValue = input.moveAnimValue[1];
		// 	// int minValue = input.moveAnimValue[0];

		// 	//KEY DOWN
			
		// 	if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
		// 		SetJoystickAndKeyboardInput(true, 0);
		// 	}  
			
		// 	if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
		// 		SetJoystickAndKeyboardInput(true, 1);
		// 	} 

		// 	if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
		// 		Debug.Log("Halooooooo");
		// 		SetJoystickAndKeyboardInput(true, 2);
		// 	} 
			
		// 	if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
		// 		SetJoystickAndKeyboardInput(true, 3);
		// 	}

		// 	//KEY UP

		// 	if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) {
		// 		SetJoystickAndKeyboardInput(false, 0);
		// 	}
			
		// 	if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
		// 		SetJoystickAndKeyboardInput(false, 1);
		// 	}
			
		// 	if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) {
		// 		SetJoystickAndKeyboardInput(false, 2);
		// 	}
			
		// 	if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
		// 		SetJoystickAndKeyboardInput(false, 3);
		// 	}
		// }
#endregion
	}

#region SET JOYSTICK & KEYBOARD
	/// <summary>
    /// <para>Direction Index : <br /></para>
	/// <para>0 Down<br /></para>
	/// <para>1 Left<br /></para>
	/// <para>2 Up<br /></para>
	/// <para>3 Right<br /></para>
	/// </summary>
	// void SetJoystickAndKeyboardInput (bool isButtonDown, int dirIdx) {
	// 	if (isButtonDown) { //Key Down
	// 		input.dirButtons[dirIdx] = 1;

	// 		switch (dirIdx) {
	// 			case 0:
	// 				// Debug.Log("DOWN");
	// 				if (input.dirButtons[2] == 0) {
	// 					ChangeDir(currentDir.x, -1f);
	// 					CheckLockDir(0, 1, 3);
	// 				}
	// 				break;
	// 			case 1:
	// 				// Debug.Log("LEFT");
	// 				if (input.dirButtons[3] == 0) {
	// 					ChangeDir(-1f, currentDir.z);
	// 					CheckLockDir(1, 0, 2);
	// 				}
	// 				break;
	// 			case 2:
	// 				// Debug.Log("UP");
	// 				if (input.dirButtons[0] == 0) {
	// 					ChangeDir(currentDir.x, 1f);
	// 					CheckLockDir(2, 1, 3);
	// 				}
	// 				break;
	// 			case 3:
	// 				// Debug.Log("RIGHT");
	// 				if (input.dirButtons[1] == 0) {
	// 					ChangeDir(1f, currentDir.z);
	// 					CheckLockDir(3, 0, 2);
	// 				}
	// 				break;
	// 		}
	// 	} else { //Key Up
	// 		input.dirButtons[dirIdx] = 0;

	// 		switch (dirIdx) {
	// 			case 0:
	// 				if (input.dirButtons[2] == 0) {
	// 					ChangeDir(currentDir.x, 0f);
	// 					CheckEndMove();
	// 					CheckRareCaseLockDir(1, 3);
	// 				} else {
	// 					ChangeDir(currentDir.x, 1f);
	// 					CheckLockDir(2, 1, 3);
	// 				}
	// 				break;
	// 			case 1:
	// 				if (input.dirButtons[3] == 0) {
	// 					ChangeDir(0f, currentDir.z);
	// 					CheckEndMove();
	// 					CheckRareCaseLockDir(0, 2);
	// 				} else {
	// 					ChangeDir(1f, currentDir.z);
	// 					CheckLockDir(3, 0, 2);
	// 				}
	// 				break;
	// 			case 2:
	// 				if (input.dirButtons[0] == 0) {
	// 					ChangeDir(currentDir.x, 0f);
	// 					CheckEndMove();
	// 					CheckRareCaseLockDir(1, 3);
	// 				} else {
	// 					ChangeDir(currentDir.x, 0f);
	// 					CheckLockDir(0, 1, 3);
	// 				}
	// 				break;
	// 			case 3:
	// 				if (input.dirButtons[1] == 0) {
	// 					ChangeDir(0f, currentDir.z);
	// 					CheckEndMove();
	// 					CheckRareCaseLockDir(0, 2);
	// 				} else {
	// 					ChangeDir(0f, currentDir.z);
	// 					CheckLockDir(1, 0, 2);
	// 				}
	// 				break;
	// 		}
	// 	}
	// }

	// public void CheckLockDir (int dirIndex, int positiveDir, int negativeDir) {
	// 	if (input.dirButtons[positiveDir] == 0 && input.dirButtons[negativeDir] == 0) {
	// 		input.direction = dirIndex;
	// 		input.isLockDir = true;
	// 	}
	// } 

	// void CheckRareCaseLockDir (int positiveDir, int negativeDir) {
	// 	if (input.dirButtons[positiveDir] == 1) {
	// 		input.direction = positiveDir;
	// 		input.isLockDir = true;
	// 	} else if (input.dirButtons[negativeDir] == 1) {
	// 		input.direction = negativeDir;
	// 		input.isLockDir = true;
	// 	}
	// } 
#endregion

	void CheckAttackInput () {
		#region Arrow
		if (GameInput.IsBowPressed) {
			toolType = tool.currentTool;
			input.interactValue = 0;

			if (toolType == ToolType.Bow) {

				if (isHaveEnoughMana((int) ToolType.Bow, true, true)) {
					input.interactMode = 5;
				} else {
					input.attackMode = -4;
				}
			} else {
				input.attackMode = -4;
			}

			player.SetPlayerState(PlayerState.BOW);
			isButtonToolHold = true;
		} else if (GameInput.IsBowReleased) {
			isButtonToolHold = false;
		} else {
			if (!isButtonToolHold && input.interactValue == 1) {
				input.interactValue = 2;
			}
		}
		#endregion

		#region Open Chest
		if (player.isCanOpenChest) {
			if (GameInput.IsAttackPressed && playerAnimationSystem.facing.DirID == 3) {
				input.interactValue = 0;
				input.interactMode = -4;
				player.SetPlayerState(PlayerState.OPEN_CHEST);
				isButtonToolHold = true;
			}

			return;
		}
		#endregion

		#region Open Gate
		if (player.isCanOpenGate) {
			if (GameInput.IsAttackPressed) {
				gateOpenerSystem.CheckAvailabilityGateKey();
			}

			return;
		}
		#endregion

		#region Power Bracelet
		if (powerBracelet == null) {
			powerBracelet = powerBraceletSystem.powerBracelet;
			
			return;
		}

		if (powerBracelet.state != PowerBraceletState.NONE && !CheckIfPlayerIsAttacking()) {
			if (GameInput.IsAttackPressed) {
				input.interactValue = 0;
				input.interactMode = 3;
				
				if (powerBraceletSystem.withStand) {
					player.isUsingStand = true;
					powerBraceletSystem.withStand = false;
					UseMana((int) ToolType.PowerBracelet, true);
				}

				player.SetPlayerState(PlayerState.POWER_BRACELET);
				isButtonToolHold = true;
			} 

			return;
		}
		#endregion

		#region Attack
		float chargeAttackThreshold = input.chargeAttackThreshold;
		// float beforeChargeDelay = input.beforeChargeDelay;
		float attackAwayDelay = input.attackAwayDelay;
		int attackMode = input.attackMode;

		if (GameInput.IsAttackPressed) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			if(state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.ATTACK){
#region Dennis
				// if (input.attackMode <= 2) {
				// 	if (!player.isHitAnEnemy){
				// 		input.attackMode = 1; //SLASH	
				// 		player.SetPlayerState(PlayerState.ATTACK);
				// 		Debug.Log("First Hit, AM = "+ input.attackMode);
				// 	} else {
				// 		Debug.Log("Next Hit Before increment, AM = "+input.attackMode);
				// 		input.attackMode += 1; //SLASH COMBO
				// 		// player.isHitAnEnemy = false;
				// 		player.SetPlayerState(PlayerState.ATTACK);
				// 		// player.isHitAnEnemy = false;
				// 		Debug.Log("Next Hit After Increment, AM = "+input.attackMode);
				// 	}
				// }else{
				// 	//player.isHitAnEnemy = false;
				// }
#endregion
				if (playerAnimationSystem.anim.isFinishAttackAnimation) {
					// Debug.Log("NEXT ATTACK");
					if (attackMode <= 0 || attackMode >= 3) {
						input.attackMode = 1;	
						// Debug.Log("Set attackMode 1");
					} else {
						input.attackMode++;
						// Debug.Log("Set attackMode++");
					}
					
					playerAnimationSystem.anim.isFinishAttackAnimation = false;
					player.SetPlayerState(PlayerState.ATTACK);
				}
				
				attackAwayTimer = 0f;
				isAttackAway = false;	
				isChargingAttack = false;
				input.isInitChargeAttack = false;
			}
		} else if (GameInput.IsAttackHeld) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			if (!input.isInitChargeAttack) {
				if (startChargeAttackTimer >= 0.3f) {
					input.isInitChargeAttack = true;
				} else {
					startChargeAttackTimer += deltaTime;
				}
			} else {
				if (!isChargingAttack) {
					if (chargeAttackTimer >= chargeAttackThreshold) {
						isChargingAttack = true;
					} else {
						chargeAttackTimer += deltaTime;
					}
				}
			}
		} else if (GameInput.IsAttackReleased) {
			if (input.moveMode == 1 && isChargingAttack) {
				input.attackMode = -1; //CHARGE
				isChargingAttack = false;
				player.SetPlayerState(PlayerState.CHARGE);
			} else {
				SetMovement(0);
			}
			
			SetMovement(0); //RUN / STAND
			chargeAttackTimer = 0f;
			startChargeAttackTimer = 0f;
			input.isInitChargeAttack = false;
		} 
		
		// if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
		// 	attackAwayTimer += deltaTime;
		// } else {
		// 	// input.slashComboVal.Clear();
		// 	// Debug.Log("CHECK ATTACK INPUT AttackList CLEAR");
		// 	// attackAwayTimer = 0f;
		// 	isAttackAway = true;
		// }

#region OLD
		// if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
		// 	chargeAttackTimer += deltaTime;
			
		// 	if (chargeAttackTimer >= beforeChargeDelay) {
		// 		SetMovement(1); //START CHARGE
		// 	}
		// } else if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
		// 	if ((chargeAttackTimer >= chargeAttackThreshold) && input.moveMode == 1) {
		// 		input.attackMode = -1; //CHARGE
		// 		player.SetPlayerState(PlayerState.CHARGE);
		// 	} else {
		// 		if (input.attackMode <= 2) {
		// 			if (!player.isHitAnEnemy){
		// 				input.attackMode = 1; //SLASH							
		// 			} else {
		// 				input.attackMode += 1; //SLASH
		// 			}
		// 		}

		// 		player.SetPlayerState(PlayerState.ATTACK);	
		// 	}
			
		// 	SetMovement(0); //RUN / STAND
		// 	chargeAttackTimer = 0f;
		// 	isAttackAway = false;			
		// } else {
		// 	if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
		// 		attackAwayTimer += deltaTime;
		// 	} else {
		// 		input.slashComboVal.Clear();
		// 		attackAwayTimer = 0f;
		// 		isAttackAway = true;
		// 	}
		// }
#endregion
		
		#endregion
	}

	void CheckGuardInput () {
		float guardParryDelay = input.guardParryDelay;

		#region Button Guard
		if (GameInput.IsGuardPressed) { //JOYSTICK AUTOMATIC BUTTON B ("Fire2")
			SetMovement(2); //START GUARD
			
			player.isGuarding = true;	
			isParryPeriod = true;
		} else if (GameInput.IsGuardHeld) {
			
			if (state == PlayerState.BLOCK_ATTACK) {
				input.interactMode = -1;
			}

			if (parryTimer < guardParryDelay) {
				parryTimer += deltaTime;
			} else {
				isParryPeriod = false;
				player.isCanParry = false;
				// player.isPlayerHit = false;	
			}
		} else if (GameInput.IsGuardReleased) {
			SetMovement(0);
			
			player.isGuarding = false;
			parryTimer = 0f;
			isParryPeriod = false;
			player.isCanParry = false;
		}

		if (isParryPeriod) {
			if (player.isPlayerHit && player.isCanParry) {
				// input.attackMode = -2;
				isParryPeriod = false;
				player.isCanParry = false;
				// player.isPlayerHit = false;
				Debug.Log("Start Counter");
				player.SetPlayerState(PlayerState.PARRY);
				gameFXSystem.SpawnObj(gameFXSystem.gameFX.parryEffect, player.transform.position);
			}
		} else {
			// player.isPlayerHit = false;
		}
		#endregion
	}

	void CheckDodgeInput () {
		#region Button Dodge
		if (GameInput.IsDodgePressed) {
			if (!isDodging && isReadyForDodging && currentDir != Vector3.zero) {
				// gameFXSystem.ToggleDodgeFlag(true);
				gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dodgeEffect, true);
				player.SetPlayerState(PlayerState.DODGE);
				input.moveDir = -currentDir; //REVERSE
				currentDir = Vector3.zero;
				bulletTimeTimer = 0f;	
				dodgeCooldownTimer = 0f;
				isDodging = true;
				isReadyForDodging = false;
				input.interactMode = 0;
			}
		}	

		// Debug.Log("isDodging : "+isDodging);

		if (isDodging) {
			// Debug.Log("dodgeCooldownTimer : "+dodgeCooldownTimer);
			if (dodgeCooldownTimer < dodgeCooldown) {
				dodgeCooldownTimer += deltaTime;

				if (bulletTimeTimer < bulletTimeDelay) {
					bulletTimeTimer += deltaTime;
					isBulletTimePeriod = true;
				} else {
					isBulletTimePeriod = false;
					player.isCanBulletTime = false;
					// player.isPlayerHit = false;
				}
			} else {
				isDodging = false;
				isReadyForDodging = true;
			}

			// if (state == PlayerState.DODGE) {
			// 	if (bulletTimeTimer < bulletTimeDelay) {
			// 		bulletTimeTimer += deltaTime;
			// 		isBulletTimePeriod = true;
			// 	} else {
			// 		isBulletTimePeriod = false;
			// 		player.isCanBulletTime = false;
			// 		// player.isPlayerHit = false;
			// 	}
			// }
		}
		// Debug.Log("isBulletTimePeriod : "+isBulletTimePeriod+"\n bulletTimeTimer : "+bulletTimeTimer);
		// Debug.Log("isPlayerHit : "+player.isPlayerHit+"\n isCanBulletTime : "+player.isCanBulletTime);

		if (isBulletTimePeriod) {
			if (player.isPlayerHit && player.isCanBulletTime && player.somethingThatHitsPlayer.GetComponent<Enemy>() != null) {	
				isBulletTimePeriod = false;
				player.isCanBulletTime = false;
				// ChangeDir(0f, 0f);
				// ChangeDir(-currentDir.x, -currentDir.y);
				input.moveMode = 3; //STEADY FOR RAPID SLASH
				input.attackMode = 0;
				player.SetPlayerState(PlayerState.SLOW_MOTION);
			}
		}
		#endregion
	}

	void CheckToolInput () {
		#region Button Tools
		if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.SWIM) && (state != PlayerState.FISHING) && state != PlayerState.BOW) {
			if(GameInput.IsQuickRPressed){
				// player.isUsingStand = false;
				toolSystem.NextTool();
			}
			
			if(GameInput.IsQuickLPressed){
				// player.isUsingStand = false;
				toolSystem.PrevTool();
			}

			if (GameInput.IsToolsPressed) {
				toolType = tool.currentTool;

				if (toolType != ToolType.None && toolType != ToolType.Bow) {
					// Debug.Log("Input Use Tool : " + toolType);
					input.interactValue = 0;

					// if (toolType == ToolType.Hook) {
					// 	player.SetPlayerState(PlayerState.HOOK);
					// } else 
					if (toolType == ToolType.Boots) {
						if (currentDir != Vector3.zero) {
							if (isHaveEnoughMana((int) ToolType.Boots, true, false)) {
								input.interactMode = 1;
								input.interactValue = 0;
								player.isUsingStand = false;
								player.SetPlayerState(PlayerState.DASH);
								gameFXSystem.ToggleRunFX(false);
							}
						}
					} else if (toolType == ToolType.FishingRod) {
						if (player.isCanFishing) {
							input.interactMode = 4;
							player.SetPlayerState(PlayerState.FISHING);
						}
					} else {
						player.SetPlayerState(PlayerState.USING_TOOL);
						
						if (!isHaveEnoughMana((int) toolType, true, true)) {
							player.SetPlayerIdle();
						} else {
							if (toolType == ToolType.Bomb || 
							tool.currentTool == ToolType.Container1 || 
							tool.currentTool == ToolType.Container2 || 
							tool.currentTool == ToolType.Container3 || 
							tool.currentTool == ToolType.Container4) {
								tool.isActToolReady = true;
							}
							// else if (toolType == ToolType.MagicMedallion) {
							// 	if (!isHaveEnoughMana((int) ToolType.MagicMedallion, true, true)) {
							// 		player.SetPlayerIdle();
							// 	}
							// }
						}
					}
				}
			} 
		} 
		// else if (state == PlayerState.POWER_BRACELET) { 				
		// 	if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) && input.liftingMode < 0){ //THROW
		// 		input.interactValue = 2;
		// 	}

		// 	if ((Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) && input.liftingMode >= 0){
		// 		isButtonToolHold = false;
		// 	}
			
		// 	if (!isButtonToolHold) {
		// 		input.interactValue = 2;
		// 	} 
		// } 
		// else if (state == PlayerState.BOW) { 				
		// 	// if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Joystick1Button9)){ //SHOT
		// 	// 	input.interactValue = 2;
		// 	// }

		// 	if (Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Joystick1Button9)) {
		// 		isButtonToolHold = false;
		// 	}
			
		// 	if (!isButtonToolHold && input.interactValue == 1) {
		// 		input.interactValue = 2;
		// 	} else {
		// 		input.interactValue = 1;
		// 	}
		// } 		
		#endregion
	}

	void SetButtonUp () {
		SetMovement(0); //RUN / STAND
		chargeAttackTimer = 0f;
		isAttackAway = false;
		
		// input.slashComboVal.Clear();
		// Debug.Log("SET BUTTON UP AttackList CLEAR");
		attackAwayTimer = 0f;
		isAttackAway = true;

		player.isGuarding = false;
		parryTimer = 0f;
		isParryPeriod = false;
	}

	bool CheckIfUsingAnyTool () {
		if (state == PlayerState.SLOW_MOTION) {
			float bulletTimeDuration = input.bulletTimeDuration;

			if (slowDownTimer < bulletTimeDuration) {
				slowDownTimer += deltaTime;

				if (GameInput.IsAttackPressed) {
					input.bulletTimeAttackQty++;
				}
			} else {
				slowDownTimer = 0f;
				Time.timeScale = 1f;
				input.moveMode = 0;
				// input.attackMode = -3; //Set counterslash first
				player.SetPlayerState(PlayerState.RAPID_SLASH);
				// ChangeDir(0f, 0f);
				SetDir(0f,0f);
			}

			return true;
		} else if (state == PlayerState.RAPID_SLASH) {
			return true;
		} else  if (state == PlayerState.POWER_BRACELET) {
			if (input.interactValue == 0) {
				currentDir = Vector3.zero;

				if (GameInput.IsAttackReleased) {
					// isButtonToolHold = false;
					CheckEndMove();
					input.interactValue = 2;
				}

				return true;
			} else if (input.interactValue == 1) { 	
				if (input.liftingMode < 0) { //LIFTING
					if (GameInput.IsAttackPressed){
						CheckEndMove();
						input.interactValue = 2;
					}
				} else { //PUSHING / SWEATING
					if (GameInput.IsAttackReleased) {
						CheckEndMove();
						// isButtonToolHold = false;
						input.interactValue = 2;
					}
				}

				return false; 
			} else { 	
				CheckMovementInput ();
				return true;
			}
		}
		// else if (state == PlayerState.BOW && input.interactValue == 0 && input.interactValue == 1) {
		// 	currentDir = Vector2.zero;

		// 	if (Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Joystick1Button9)) {
		// 		isButtonToolHold = false;
		// 	}

		// 	return true;
		// } 
		else if (state == PlayerState.FISHING) { 	
			currentDir = Vector3.zero;
						
			if (GameInput.IsToolsPressed || GameInput.IsAttackPressed){
				input.interactValue = 2;
				toolSystem.UseTool();
			}
			
			return true;
		} else if (state == PlayerState.GET_TREASURE) { 
			currentDir = Vector3.zero;

			if (GameInput.AnyButtonPressed){ //ANY BUTTON
				gainTreasureSystem.UseTreasure();
				input.interactValue = 2;
			}
			
			return true;
		} else if (state == PlayerState.DODGE) {
			CheckDodgeInput ();
			return true;
		} else {
			return false;
		}
	}

	bool CheckIfInSpecificState () {
		if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.POWER_BRACELET || player.isCanInteractWithNPC) {	

			return true;
		} else if (state == PlayerState.DASH) {
			if (GameInput.IsToolsReleased){
				if (input.interactValue == 1) {
					input.interactValue = 2;
					player.isUsingStand = false;				
				} else {
					if (!player.isBouncing) {
						player.SetPlayerIdle();
					}
				}
			}

			return true;
		} else if (state == PlayerState.GET_HURT) {
			// input.interactMode = -2;

			return true;
		} else if (state == PlayerState.SWIM) {
			SetButtonUp ();
			
			// if (input.interactValue == 0 || input.interactValue == 2) {
			// 	Debug.Log("Pause swim");
			// }

			return true;
		} else {
			return false;
		}
	}

	bool CheckIfPlayerIsAttacking () {
		if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.PARRY || state == PlayerState.DODGE || state == PlayerState.SLOW_MOTION || state == PlayerState.RAPID_SLASH) {
			return true;
		} else {
			return false;
		}
	}

	public void SetMovement (int value) {
		// Debug.Log("SetMovement : "+value);
		input.moveMode = value;
		
		#region CHARGE ATTACK EFFECT
		if (value == 1 && state == PlayerState.ATTACK) {
			gameFXSystem.ToggleObjectEffect(gameFXSystem.gameFX.chargingEffect, true);
		} else {
			gameFXSystem.ToggleObjectEffect(gameFXSystem.gameFX.chargingEffect, false);
		}
		#endregion
		
		// if (!isMoveOnly) {
		// 	input.steadyMode = value;
		// }
	}

#region OLD ChangeDir
	// public void ChangeDir (float dirX, float dirZ) {
	// 	Vector3 newDir = new Vector3(dirX, 0f, dirZ);
	// 	// Debug.Log(newDir);

	// 	#region RUN EFFECT
	// 	if (newDir != Vector3.zero && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
	// 		gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.runEffect, true);
	// 	} else {
	// 		gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.runEffect, false);
	// 	}
	// 	#endregion

	// 	if (state == PlayerState.POWER_BRACELET) {
	// 		if (input.liftingMode == 1 || input.liftingMode == 2) {
	// 			Facing2D facing = playerAnimationSystem.facing;
	// 			// Debug.Log(facing.DirID);
	// 			// Debug.Log("==========Grabbing==========");
	// 			// Debug.Log("Before " + facing.DirID);
	// 			switch (facing.DirID) {
	// 				case 1: 
	// 					if (newDir.x == 0 && newDir.z <= 0) SetDir (0f, 0f, newDir.z);
	// 					// Debug.Log("Bottom");
	// 					break;
	// 				case 2: 
	// 					if (newDir.x <= 0 && newDir.z == 0) SetDir (newDir.x, 0f, 0f);
	// 					// Debug.Log("Left");
	// 					break;
	// 				case 3: 
	// 					if (newDir.x == 0 && newDir.z >= 0) SetDir (0f, 0f, newDir.z);
	// 					// Debug.Log("Top");
	// 					break;
	// 				case 4: 
	// 					if (newDir.x >= 0 && newDir.z == 0) SetDir (newDir.x, 0f, 0f);
	// 					// Debug.Log("right");
	// 					break;

	// #region OLD 8 Direction
	// 				// case 1: 
	// 				// 	if (newDir.x == 0 && newDir.z <= 0) SetDir (0f, 0f, newDir.z);
	// 				// 	// Debug.Log("Bottom");
	// 				// 	break;
	// 				// case 2: 
	// 				// 	if (newDir.x <= 0 && newDir.z <= 0) SetDir (newDir.x, 0f, newDir.z);
	// 				// 	// Debug.Log("Bottom left");
	// 				// 	break;
	// 				// case 3: 
	// 				// 	if (newDir.x <= 0 && newDir.z == 0) SetDir (newDir.x, 0f, 0f);
	// 				// 	// Debug.Log("Left");
	// 				// 	break;
	// 				// case 4: 
	// 				// 	if (newDir.x <= 0 && newDir.z >= 0) SetDir (newDir.x, 0f, newDir.z);
	// 				// 	// Debug.Log("Top left");
	// 				// 	break;
	// 				// case 5: 
	// 				// 	if (newDir.x == 0 && newDir.z >= 0) SetDir (0f, 0f, newDir.z);
	// 				// 	// Debug.Log("Top");
	// 				// 	break;
	// 				// case 6: 
	// 				// 	if (newDir.x >= 0 && newDir.z >= 0) SetDir (newDir.x, 0f, newDir.z);
	// 				// 	// Debug.Log("Top right");
	// 				// 	break;
	// 				// case 7: 
	// 				// 	if (newDir.x >= 0 && newDir.z == 0) SetDir (newDir.x, 0f, 0f);
	// 				// 	// Debug.Log("right");
	// 				// 	break;
	// 				// case 8: 
	// 				// 	if (newDir.x >= 0 && newDir.z <= 0) SetDir (newDir.x, 0f, newDir.z);
	// 				// 	// Debug.Log("Bottom right");
	// 				// 	break;
	// #endregion
	// 			}
	// 			// Debug.Log("After " + facing.DirID);
	// 			// Debug.Log("==========End Grabbing==========");
	// 		} else if (input.liftingMode == -1 || input.liftingMode == -2){
	// 			SetDir (newDir.x, 0f, newDir.z);
	// 		} 
	// 	} else {
	// 		// player.SetPlayerState(PlayerState.MOVE);
	// 		SetDir (newDir.x, 0f, newDir.z);
	// 	}
	// }
#endregion

	public void SetDir (float dirX, float dirZ) {
		Vector3 fixDir = new Vector3(dirX, 0f, dirZ);

		#region RUN EFFECT
		if (fixDir != Vector3.zero && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
			gameFXSystem.ToggleRunFX(true);
		} else {
			gameFXSystem.ToggleRunFX(false);
		}
		#endregion


		if (currentDir != fixDir) {
			if (dirX!=0f && dirZ!=0f) {
				//DIAGONAL FACING				
				if (currentDir.x==0f) {//PREVIOUS MOVEMENT IS VERTICAL
					if (dirZ == -1f)
						input.direction = 1;//FACE DOWN
					else 
						input.direction = 3;//FACE UP
				} else {//PREVIOUS MOVEMENT IS HORIZONTAL
					if (dirX == -1f)
						input.direction = 2;//FACE LEFT
					else 
						input.direction = 4;//FACE RIGHT

				}
			} else if (dirZ == -1f) {//FACE DOWN
				input.direction = 1;
			} else if (dirZ == 1f) {//FACE UP
				input.direction = 3;
			} else if (dirX == -1f) {//FACE LEFT
				input.direction = 2;
			} else if (dirX == 1f) {//FACE RIGHT
				input.direction = 4;
			}

			currentDir = fixDir;
			input.moveDir = currentDir;
		}
	}

	void CheckEndMove () {
		if (state == PlayerState.MOVE) {
			player.SetPlayerIdle();
		} else if (state == PlayerState.POWER_BRACELET && input.liftingMode == -2) {
			input.liftingMode = -1;
		} else if (state == PlayerState.POWER_BRACELET && input.liftingMode == 2) {
			input.liftingMode = 1;
		}
	}

	bool isHaveEnoughMana (int toolIdx, bool isUseMana, bool isUsingStand) {
		Debug.Log("mana cost for tool " + toolIdx + " is " + tool.GetToolManaCost(toolIdx));
		if(manaSystem.isHaveEnoughMana(tool.GetToolManaCost(toolIdx), isUseMana, isUsingStand)) {
			return true;
		} else {
			return false;
		}
	}

	void UseMana (int toolIdx, bool isUsingStand) {
		manaSystem.UseMana(tool.GetToolManaCost(toolIdx), isUsingStand);
	}
}
