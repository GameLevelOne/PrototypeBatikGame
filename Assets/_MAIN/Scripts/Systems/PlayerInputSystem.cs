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

	public PlayerInput input;
	public Player player;

	PlayerTool tool;
	Facing2D facing;

	PlayerState state;
	ToolType toolType;

	Vector2 currentDir = Vector2.zero;
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
			
			float chargeAttackThreshold = input.chargeAttackThreshold;
			float beforeChargeDelay = input.beforeChargeDelay;
			float attackAwayDelay = input.attackAwayDelay;
			float bulletTimeDuration = input.bulletTimeDuration;
			float dodgeCooldown = input.dodgeCooldown;

			float guardParryDelay = input.guardParryDelay;
			float bulletTimeDelay = input.bulletTimeDelay;

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
			if (state == PlayerState.SLOW_MOTION) {
				if (slowDownTimer < bulletTimeDuration) {
					slowDownTimer += deltaTime;

					if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
						input.AttackMode = 1;
						input.bulletTimeAttackQty++;
					}
				} else {
					slowDownTimer = 0f;
					Time.timeScale = 1f;
					input.moveMode = 0;
					player.SetPlayerState(PlayerState.RAPID_SLASH);
				}

				continue;
			} else if (state == PlayerState.RAPID_SLASH) {
				continue;
			} else if (state == PlayerState.POWER_BRACELET && input.interactValue == 0) {
				currentDir = Vector2.zero;

				if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button4)) {
					isButtonToolHold = false;
				}

				continue;
			} else if (state == PlayerState.FISHING) { 				
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
					input.interactValue = 2;
					toolSystem.UseTool();
				}
				
				continue;
			} 

			#region Button Movement
			CheckMovementInput ();
			#endregion

			#region Button Tools
			if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.SWIM) && (state != PlayerState.FISHING) && (state != PlayerState.BOW)) {
				if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.Joystick1Button7)){
					toolSystem.NextTool();
				}
				
				if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.Joystick1Button6)){
					toolSystem.PrevTool();
				}

				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
					toolType = tool.currentTool;

					if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.FISHING) && (toolType != ToolType.None)) {
						Debug.Log("Input Use Tool : " + toolType);

						if (toolType == ToolType.Hook) {
							player.SetPlayerState(PlayerState.HOOK);
						} else if (toolType == ToolType.Boots) {
							input.interactMode = 1;
							player.SetPlayerState(PlayerState.DASH);
						} else if (toolType == ToolType.PowerBracelet) {
							PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;

							if (powerBraceletState != PowerBraceletState.NONE) {
								input.interactMode = 3;
								player.SetPlayerState(PlayerState.POWER_BRACELET);
								isButtonToolHold = true;

								// if (liftState == LiftState.GRAB) {
								// 	powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
								// }
							} else {
								continue;
							}
						} else if (toolType == ToolType.Flippers) {
							//
						} else if (toolType == ToolType.FishingRod) {
							if (player.IsCanFishing) {
								input.interactMode = 4;
								player.SetPlayerState(PlayerState.FISHING);
							}
						} else {
							player.SetPlayerState(PlayerState.USING_TOOL);
						}
					}
				}
			} else if (state == PlayerState.POWER_BRACELET) { 				
				if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)) && input.liftingMode < 0){ //THROW
					input.interactValue = 2;
				}

				if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button3)) && input.liftingMode >= 0){
					isButtonToolHold = false;
				}
				
				if (!isButtonToolHold) {
					input.interactValue = 2;
				} 
			}			

			if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.POWER_BRACELET || state == PlayerState.SWIM) {	

				continue;
			} else if (state == PlayerState.DASH) {
				if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button3)){
					input.interactMode = 2;
					// player.SetPlayerState (PlayerState.BRAKE);
				}

				continue;
			} else if (state == PlayerState.GET_HURT) {
				input.interactMode = -2;

				continue;
			}
			#endregion

			#region Button Attack
			if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
				chargeAttackTimer += deltaTime;
				
				if (chargeAttackTimer >= beforeChargeDelay) {
					Debug.Log("Start charging");
					// SetMovement(1, false); //START CHARGE
				}
			} else {
				if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
					attackAwayTimer += deltaTime;
				} else {
					// input.slashComboVal.Clear();
					attackAwayTimer = 0f;
					isAttackAway = true;
				}
			}
			
			if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
				if ((chargeAttackTimer >= chargeAttackThreshold) && input.moveMode == 1) {
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
					if (state != PlayerState.ATTACK) {
						player.SetPlayerState(PlayerState.ATTACK);
					}	
				}
				
				// SetMovement(0, false);
				chargeAttackTimer = 0f;
				isAttackAway = false;			
			}
			#endregion

			#region Button Guard
			if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.KeypadEnter)) { //JOYSTICK AUTOMATIC BUTTON B ("Fire2")
				// SetMovement(2, false); //START GUARD
				
				// player.IsGuarding = true;
			}
			
			if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.KeypadEnter)) {

				if (state == PlayerState.BLOCK_ATTACK) {
					input.interactMode = -1;
				}

				if (parryTimer < guardParryDelay) {
					parryTimer += deltaTime;	
					player.isParrying = true;
				} else {
					player.isParrying = false;
					player.IsPlayerHit = false;	
				}
			}

			if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
				// SetMovement(0, false);
				
				// player.IsGuarding = false;
				parryTimer = 0f;
				player.isParrying = false;
			}
			#endregion

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
					ChangeDir(0f, 0f);
					input.moveMode = 3; //STEADY FOR RAPID SLASH
					input.AttackMode = -3;
					Debug.Log("Start BulletTime");
					player.SetPlayerState(PlayerState.SLOW_MOTION);
				}
			}
			#endregion
			
			if (player.isParrying) {
				if (player.IsPlayerHit) {
					input.AttackMode = -2;
					player.isParrying = false;
					player.IsPlayerHit = false;
					Debug.Log("Start Counter");
					player.SetPlayerState(PlayerState.COUNTER);
				}
			} else {
				player.IsPlayerHit = false;
			}
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
		float chargeAttackThreshold = input.chargeAttackThreshold;
		float beforeChargeDelay = input.beforeChargeDelay;
		float attackAwayDelay = input.attackAwayDelay;

		#region Button Attack
		if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Keypad0)) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
			chargeAttackTimer += deltaTime;
			
			if (chargeAttackTimer >= beforeChargeDelay) {
				SetMovement(1); //START CHARGE
			}
		} else if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Keypad0)) {
			if ((chargeAttackTimer >= chargeAttackThreshold) && input.moveMode == 1) {
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
				if (state != PlayerState.ATTACK) {
					player.SetPlayerState(PlayerState.ATTACK);
				}	
			}
			
			SetMovement(0); //RUN / STAND
			chargeAttackTimer = 0f;
			isAttackAway = false;			
		} else {
			if ((attackAwayTimer <= attackAwayDelay) && !isAttackAway) {
				attackAwayTimer += deltaTime;
			} else {
				input.slashComboVal.Clear();
				attackAwayTimer = 0f;
				isAttackAway = true;
			}
		}
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
				player.IsPlayerHit = false;	
			}
		} else if (Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.KeypadEnter)) {
			SetMovement(0);
			
			player.isGuarding = false;
			parryTimer = 0f;
			player.isParrying = false;
		}

		if (player.isParrying) {
			if (player.IsPlayerHit) {
				input.AttackMode = -2;
				player.isParrying = false;
				player.IsPlayerHit = false;
				Debug.Log("Start Counter");
				player.SetPlayerState(PlayerState.COUNTER);
			}
		} else {
			player.IsPlayerHit = false;
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
				ChangeDir(0f, 0f);
				input.moveMode = 3; //STEADY FOR RAPID SLASH
				input.AttackMode = -3;
				Debug.Log("Start BulletTime");
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

			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
				toolType = tool.currentTool;

				if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.FISHING) && (toolType != ToolType.None)) {
					Debug.Log("Input Use Tool : " + toolType);
					input.interactValue = 0;

					if (toolType == ToolType.Hook) {
						player.SetPlayerState(PlayerState.HOOK);
					} else if (toolType == ToolType.Boots) {
						input.interactMode = 1;
						player.SetPlayerState(PlayerState.DASH);
					} else if (toolType == ToolType.PowerBracelet) {
						PowerBraceletState powerBraceletState = powerBraceletSystem.powerBracelet.state;

						if (powerBraceletState != PowerBraceletState.NONE) {
							input.interactMode = 3;
							player.SetPlayerState(PlayerState.POWER_BRACELET);
							isButtonToolHold = true;

							// if (liftState == LiftState.GRAB) {
							// 	powerBraceletSystem.SetTargetRigidbody (RigidbodyType2D.Dynamic);
							// }
						} else {
							// continue;
						}
					} else if (toolType == ToolType.Flippers) {
						//
					} else if (toolType == ToolType.FishingRod) {
						if (player.IsCanFishing) {
							input.interactMode = 4;
							player.SetPlayerState(PlayerState.FISHING);
						}
					} else if (toolType == ToolType.Bow) {
						input.interactMode = 5;
						player.SetPlayerState(PlayerState.BOW);
						isButtonToolHold = true;
					} else {
						player.SetPlayerState(PlayerState.USING_TOOL);
					}
				}
			}
		} else if (state == PlayerState.POWER_BRACELET || state == PlayerState.BOW) { 				
			if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)) && input.liftingMode < 0){ //THROW
				input.interactValue = 2;
			}

			if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button3)) && input.liftingMode >= 0){
				isButtonToolHold = false;
			}
			
			if (!isButtonToolHold) {
				input.interactValue = 2;
			} 
		}		
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
		if ((state == PlayerState.POWER_BRACELET || state == PlayerState.BOW) && input.interactValue == 0) {
			currentDir = Vector2.zero;

			if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button4)) {
				isButtonToolHold = false;
			}

			return true;
		} else if (state == PlayerState.FISHING) { 				
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
				input.interactValue = 2;
				Debug.Log("input.interactValue = 2");
				toolSystem.UseTool();
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
}
