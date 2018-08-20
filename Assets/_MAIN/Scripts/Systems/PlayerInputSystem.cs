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

	public PlayerInput input;
	public Player player;
	public ToolType toolType;

	PlayerTool tool;
	Facing2D facing;
	PowerBracelet powerBracelet;

	PlayerState state;

	Vector2 currentDir = Vector2.zero;
	Vector2 playerDir = Vector2.zero;
	float deltaTime;
	float parryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float attackAwayTimer = 0f;
	float dodgeCooldownTimer = 0f;
	bool isAttackAway = true;
	bool isReadyForDodging = true;

	bool isDodging = false;
	bool isButtonToolHold = false;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;

		deltaTime = Time.deltaTime;
		
		for (int i=0; i<inputData.Length; i++) {
			input = inputData.PlayerInput[i];
			player = inputData.Player[i];
			state = player.state;
			Health health = inputData.Health[i];
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
			continue; //TEMP

			#region OLD
			// if (state == PlayerState.SLOW_MOTION) {
			// 	if (slowDownTimer < bulletTimeDuration) {
			// 		slowDownTimer += deltaTime;

			// 		if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
			// 			input.AttackMode = 1;
			// 			input.bulletTimeAttackQty++;
			// 		}
			// 	} else {
			// 		slowDownTimer = 0f;
			// 		Time.timeScale = 1f;
			// 		input.moveMode = 0;
			// 		player.SetPlayerState(PlayerState.RAPID_SLASH);
			// 	}

			// 	continue;
			// } else if (state == PlayerState.RAPID_SLASH) {
			// 	continue;
			// } else if (state == PlayerState.POWER_BRACELET && input.interactValue == 0) {
			// 	currentDir = Vector2.zero;

			// 	if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button4)) {
			// 		isButtonToolHold = false;
			// 	}

			// 	continue;
			// } else if (state == PlayerState.FISHING) { 				
			// 	if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
			// 		input.interactValue = 2;
			// 		toolSystem.UseTool();
			// 	}
				
			// 	continue;
			// } 

			#region Button Movement
			CheckMovementInput ();
			#endregion

			#region Button Tools
			//
			#endregion

			#region Button Attack
			// if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			// 	chargeAttackTimer += deltaTime;
				
			// 	if (chargeAttackTimer >= beforeChargeDelay) {
			// 		Debug.Log("Start charging");
			// 		SetMovement(1, false); //START CHARGE
			// 	}
			// } else {
			// 	if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
			// 		attackAwayTimer += deltaTime;
			// 	} else {
			// 		// input.slashComboVal.Clear();
			// 		attackAwayTimer = 0f;
			// 		isAttackAway = true;
			// 	}
			// }
			
			// if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
			// 	if ((chargeAttackTimer >= chargeAttackThreshold) && input.moveMode == 1) {
			// 		input.AttackMode = -1; //CHARGE
			// 		player.SetPlayerState(PlayerState.CHARGE);
			// 	} else {
			// 		if (input.AttackMode <= 2) {
			// 			if (!player.isHitAnEnemy){
			// 				input.AttackMode = 1; //SLASH							
			// 			} else {
			// 				input.AttackMode += 1; //SLASH
			// 			}
			// 		}
			// 		if (state != PlayerState.ATTACK) {
			// 			player.SetPlayerState(PlayerState.ATTACK);
			// 		}	
			// 	}
				
			//	SetMovement(0, false);
			// 	chargeAttackTimer = 0f;
			// 	isAttackAway = false;			
			// }
			#endregion

			#region Button Guard
			// if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) { //JOYSTICK AUTOMATIC BUTTON B ("Fire2")
			// 	// SetMovement(2, false); //START GUARD
				
			// 	// player.isGuarding = true;
			// }
			
			// if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {

			// 	if (state == PlayerState.BLOCK_ATTACK) {
			// 		input.interactMode = -1;
			// 	}

			// 	if (parryTimer < guardParryDelay) {
			// 		parryTimer += deltaTime;	
			// 		player.isParrying = true;
			// 	} else {
			// 		player.isParrying = false;
			// 		player.isPlayerHit = false;	
			// 	}
			// }

			// if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
			// 	// SetMovement(0, false);
				
			// 	// player.isGuarding = false;
			// 	parryTimer = 0f;
			// 	player.isParrying = false;
			// }
			#endregion

			#region Button Dodge			
			// if (Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetKeyDown(KeyCode.Joystick1Button4)) {
			// 	if (!isDodging && isReadyForDodging && (currentDir != Vector2.zero)) {
			// 		player.SetPlayerState(PlayerState.DODGE);
			// 		bulletTimeTimer = 0f;	
			// 		dodgeCooldownTimer = 0f;
			// 		isDodging = true;
			// 		isReadyForDodging = false;
			// 		input.interactMode = 0;
			// 	}
			// }	

			// if (isDodging) {
			// 	if (dodgeCooldownTimer < dodgeCooldown) {
			// 		dodgeCooldownTimer += deltaTime;
			// 	} else {
			// 		isDodging = false;
			// 		isReadyForDodging = true;
			// 	}

			// 	if (state == PlayerState.DODGE) {
			// 		if (bulletTimeTimer < bulletTimeDelay) {
			// 			bulletTimeTimer += deltaTime;
			// 			player.isBulletTiming = true;
			// 		} else {
			// 			player.isBulletTiming = false;
			// 			player.isPlayerHit = false;
			// 		}
			// 	}
			// }

			// if (player.isBulletTiming) {
			// 	if (player.isPlayerHit) {	
			// 		player.isBulletTiming = false;
			// 		ChangeDir(0f, 0f);
			// 		input.moveMode = 3; //STEADY FOR RAPID SLASH
			// 		input.AttackMode = -3;
			// 		Debug.Log("Start BulletTime");
			// 		player.SetPlayerState(PlayerState.SLOW_MOTION);
			// 	}
			// }
			#endregion
			
			// if (player.isParrying) {
			// 	if (player.isPlayerHit) {
			// 		input.AttackMode = -2;
			// 		player.isParrying = false;
			// 		player.isPlayerHit = false;
			// 		Debug.Log("Start Counter");
			// 		player.SetPlayerState(PlayerState.COUNTER);
			// 	}
			// } else {
			// 	player.isPlayerHit = false;
			// }
			#endregion OLD
		}
	}

	void CheckMovementInput () {
		#region JOYSTICK
		if (Input.GetJoystickNames().Length >= 1) {
			if (Input.GetJoystickNames()[0] != "") {
				float inputX = Input.GetAxis("Horizontal Javatale");
				float inputY = Input.GetAxis("Vertical Javatale");
				ChangeDir (inputX, inputY);

				if (inputX == 0 || inputY == 0) {
					CheckEndMove();
				}
			}
		} 
		#endregion
		
		#region MOUSE & KEYBOARD
		else {
			int maxValue = input.moveAnimValue[2];
			// int midValue = input.moveAnimValue[1];
			int minValue = input.moveAnimValue[0];

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
				ChangeDir(0f, currentDir.y);
				CheckEndMove();
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
				ChangeDir(currentDir.x, 0f);
				CheckEndMove();
			}
		}
		#endregion
	}

	void CheckAttackInput () {
		#region Arrow
		if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton9)) {
			toolType = tool.currentTool;
			input.interactValue = 0;

			if (toolType == ToolType.Bow) {
				CheckMana((int) ToolType.Bow);

				if (player.isUsingStand) {
					input.interactMode = 5;
				} else {
					input.AttackMode = -4;
				}
			} else {
				input.AttackMode = -4;
			}

			player.SetPlayerState(PlayerState.BOW);
			isButtonToolHold = true;
		} else if (Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Joystick1Button9)) {
			isButtonToolHold = false;
		} else {
			if (!isButtonToolHold && input.interactValue == 1) {
				input.interactValue = 2;
			}
		}
		#endregion

		#region Open Chest
		if (player.isCanOpenChest) {
			playerDir = input.moveDir == Vector2.zero ? playerDir : input.moveDir;

			if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) && playerDir == Vector2.up) {
				input.interactValue = 0;
				input.interactMode = -4;
				player.SetPlayerState(PlayerState.OPEN_CHEST);
				isButtonToolHold = true;
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
			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
				input.interactValue = 0;
				input.interactMode = 3;
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

		if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			if (input.AttackMode <= 2) {
				if (!player.isHitAnEnemy){
					input.AttackMode = 1; //SLASH	
					player.SetPlayerState(PlayerState.ATTACK);
				} else {
					input.AttackMode += 1; //SLASH COMBO
				}
			}
			
			attackAwayTimer = 0f;
			isAttackAway = false;	
		} else if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			chargeAttackTimer += deltaTime;
			
			// if (chargeAttackTimer >= beforeChargeDelay) {
			if (chargeAttackTimer >= chargeAttackThreshold) {
				SetMovement(1); //START CHARGE
			}
		} else if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
			if (input.moveMode == 1) {
				input.AttackMode = -1; //CHARGE
				player.SetPlayerState(PlayerState.CHARGE);
			}
			
			SetMovement(0); //RUN / STAND
			chargeAttackTimer = 0f;
		} 
		
		if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
			attackAwayTimer += deltaTime;
		} else {
			input.slashComboVal.Clear();
			// attackAwayTimer = 0f;
			isAttackAway = true;
		}

		// if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
		// 	chargeAttackTimer += deltaTime;
			
		// 	if (chargeAttackTimer >= beforeChargeDelay) {
		// 		SetMovement(1); //START CHARGE
		// 	}
		// } else if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
		// 	if ((chargeAttackTimer >= chargeAttackThreshold) && input.moveMode == 1) {
		// 		input.AttackMode = -1; //CHARGE
		// 		player.SetPlayerState(PlayerState.CHARGE);
		// 	} else {
		// 		if (input.AttackMode <= 2) {
		// 			if (!player.isHitAnEnemy){
		// 				input.AttackMode = 1; //SLASH							
		// 			} else {
		// 				input.AttackMode += 1; //SLASH
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
	}

	void CheckGuardInput () {
		float guardParryDelay = input.guardParryDelay;

		#region Button Guard
		if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) { //JOYSTICK AUTOMATIC BUTTON B ("Fire2")
			SetMovement(2); //START GUARD
			
			player.isGuarding = true;
		} else if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {
			
			if (state == PlayerState.BLOCK_ATTACK) {
				input.interactMode = -1;
			}

			if (parryTimer < guardParryDelay) {
				parryTimer += deltaTime;	
				player.isParrying = true;
			} else {
				player.isParrying = false;
				// player.isPlayerHit = false;	
			}
		} else if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
			SetMovement(0);
			
			player.isGuarding = false;
			parryTimer = 0f;
			player.isParrying = false;
		}

		if (player.isParrying) {
			if (player.isPlayerHit) {
				input.AttackMode = -2;
				player.isParrying = false;
				// player.isPlayerHit = false;
				Debug.Log("Start Counter");
				player.SetPlayerState(PlayerState.COUNTER);
			}
		} else {
			// player.isPlayerHit = false;
		}
		#endregion
	}

	void CheckDodgeInput () {
		float dodgeCooldown = input.dodgeCooldown;
		float bulletTimeDelay = input.bulletTimeDelay;

		#region Button Dodge
		if (Input.GetKeyDown(KeyCode.KeypadPeriod) || Input.GetKeyDown(KeyCode.Joystick1Button4)) {
			if (!isDodging && isReadyForDodging && (currentDir != Vector2.zero)) {
				player.SetPlayerState(PlayerState.DODGE);
				bulletTimeTimer = 0f;	
				dodgeCooldownTimer = 0f;
				isDodging = true;
				isReadyForDodging = false;
				input.interactMode = 0;
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
					player.isBulletTiming = true;
				} else {
					player.isBulletTiming = false;
					// player.isPlayerHit = false;
				}
			}
		}

		if (player.isBulletTiming) {
			if (player.isPlayerHit) {	
				player.isBulletTiming = false;
				ChangeDir(0f, 0f);
				input.moveMode = 3; //STEADY FOR RAPID SLASH
				input.AttackMode = -3;
				// Debug.Log("Start BulletTime");
				player.SetPlayerState(PlayerState.SLOW_MOTION);
			}
		}
		#endregion
	}

	void CheckToolInput () {
		#region Button Tools
		if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.SWIM) && (state != PlayerState.FISHING) && state != PlayerState.BOW) {
			if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.Joystick1Button7)){
				toolSystem.NextTool();
			}
			
			if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.Joystick1Button6)){
				toolSystem.PrevTool();
			}

			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)) {
				toolType = tool.currentTool;

				if (toolType != ToolType.None && toolType != ToolType.Bow) {
					Debug.Log("Input Use Tool : " + toolType);
					input.interactValue = 0;

					if (toolType == ToolType.Hook) {
						player.SetPlayerState(PlayerState.HOOK);
					} else if (toolType == ToolType.Boots) {
						input.interactMode = 1;
						player.SetPlayerState(PlayerState.DASH);
					} else if (toolType == ToolType.FishingRod) {
						if (player.isCanFishing) {
							input.interactMode = 4;
							player.SetPlayerState(PlayerState.FISHING);
						}
					} else {
						player.SetPlayerState(PlayerState.USING_TOOL);

						if (toolType == ToolType.Bomb) {
							tool.isActToolReady = true;
						}
					}
				}
			} 
		} else if (state == PlayerState.POWER_BRACELET) { 				
			if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) && input.liftingMode < 0){ //THROW
				input.interactValue = 2;
			}

			if ((Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) && input.liftingMode >= 0){
				isButtonToolHold = false;
			}
			
			if (!isButtonToolHold) {
				input.interactValue = 2;
			} 
		} 
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
		
		input.slashComboVal.Clear();
		attackAwayTimer = 0f;
		isAttackAway = true;

		player.isGuarding = false;
		parryTimer = 0f;
		player.isParrying = false;
	}

	bool CheckIfUsingAnyTool () {
		if (state == PlayerState.SLOW_MOTION) {
			float bulletTimeDuration = input.bulletTimeDuration;

			if (slowDownTimer < bulletTimeDuration) {
				slowDownTimer += deltaTime;

				if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
					input.bulletTimeAttackQty++;
				}
			} else {
				slowDownTimer = 0f;
				Time.timeScale = 1f;
				input.moveMode = 0;
				input.AttackMode = 1;
				player.SetPlayerState(PlayerState.RAPID_SLASH);
			}

			return true;
		} else if (state == PlayerState.RAPID_SLASH) {
			return true;
		} else  if (state == PlayerState.POWER_BRACELET && input.interactValue == 0) {
			currentDir = Vector2.zero;

			if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
				isButtonToolHold = false;
			}

			return true;
		} 
		// else if (state == PlayerState.BOW && input.interactValue == 0 && input.interactValue == 1) {
		// 	currentDir = Vector2.zero;

		// 	if (Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Joystick1Button9)) {
		// 		isButtonToolHold = false;
		// 	}

		// 	return true;
		// } 
		else if (state == PlayerState.FISHING) { 	
			currentDir = Vector2.zero;
						
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
				input.interactValue = 2;
				toolSystem.UseTool();
			}
			
			return true;
		} else if (state == PlayerState.GET_TREASURE) { 
			currentDir = Vector2.zero;

			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)){ //ANY BUTTON
				gainTreasureSystem.UseTreasure();
				input.interactValue = 2;
			}
			
			return true;
		} else {
			return false;
		}
	}

	bool CheckIfInSpecificState () {
		if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.POWER_BRACELET) {	

			return true;
		} else if (state == PlayerState.DASH) {
			if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button3))){
				if (input.interactValue == 1) {
					input.interactValue = 2;				
				} else {
					player.SetPlayerIdle();
				}
			}

			return true;
		} else if (state == PlayerState.GET_HURT) {
			input.interactMode = -2;

			return true;
		} else if (state == PlayerState.SWIM) {
			SetButtonUp ();
			return true;
		} else {
			return false;
		}
	}

	bool CheckIfPlayerIsAttacking () {
		if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.COUNTER || state == PlayerState.DODGE || state == PlayerState.SLOW_MOTION || state == PlayerState.RAPID_SLASH) {
			return true;
		} else {
			return false;
		}
	}

	public void SetMovement (int value) {
		input.moveMode = value;
		
		// if (!isMoveOnly) {
		// 	input.steadyMode = value;
		// }
	}

	void ChangeDir (float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);

		if (state == PlayerState.POWER_BRACELET) {
			if (input.liftingMode == 1 || input.liftingMode == 2) {
				facing = playerAnimationSystem.facing;
				// Debug.Log("==========Grabbing==========");
				// Debug.Log("Before " + facing.DirID);
				switch (facing.DirID) {
					case 1: 
						if (newDir.x == 0 && newDir.y <= 0) SetDir (0f, newDir.y);
						// Debug.Log("Bottom");
						break;
					case 2: 
						if (newDir.x <= 0 && newDir.y <= 0) SetDir (newDir.x, newDir.y);
						// Debug.Log("Bottom left");
						break;
					case 3: 
						if (newDir.x <= 0 && newDir.y <= 0) SetDir (newDir.x, 0f);
						// Debug.Log("Left");
						break;
					case 4: 
						if (newDir.x <= 0 && newDir.y >= 0) SetDir (newDir.x, newDir.y);
						// Debug.Log("Top left");
						break;
					case 5: 
						if (newDir.x == 0 && newDir.y >= 0) SetDir (0f, newDir.y);
						// Debug.Log("Top");
						break;
					case 6: 
						if (newDir.x >= 0 && newDir.y >= 0) SetDir (newDir.x, newDir.y);
						// Debug.Log("Top right");
						break;
					case 7: 
						if (newDir.x >= 0 && newDir.y == 0) SetDir (newDir.x, 0f);
						// Debug.Log("right");
						break;
					case 8: 
						if (newDir.x >= 0 && newDir.y <= 0) SetDir (newDir.x, newDir.y);
						// Debug.Log("Bottom right");
						break;
				}
				// Debug.Log("After " + facing.DirID);
				// Debug.Log("==========End Grabbing==========");
			} else if (input.liftingMode == -1 || input.liftingMode == -2){
				SetDir (newDir.x, newDir.y);
			} 
		} else {
			// player.SetPlayerState(PlayerState.MOVE);
			SetDir (newDir.x, newDir.y);
		}
	}

	void SetDir (float dirX, float dirY) {
		Vector2 fixDir = new Vector2(dirX, dirY);

		if (currentDir != fixDir) {
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

	void CheckMana (int toolIdx) {
		// Debug.Log("mana cost for tool " + toolIdx + " is " + tool.GetToolManaCost(toolIdx));
		manaSystem.CheckMana(tool.GetToolManaCost(toolIdx));
	}
}
