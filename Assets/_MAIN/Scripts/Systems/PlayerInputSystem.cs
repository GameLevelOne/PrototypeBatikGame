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
		
	}
	void CheckAttackInput () {
		#region Arrow
		if (GameInput.IsBowPressed) {
			toolType = tool.currentTool;
			input.interactValue = 0;

			if (toolType == ToolType.Bow) {

				if (isHaveEnoughMana((int) ToolType.Bow, true, true)) {
					PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
					input.interactMode = 5;
				} else {
					PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
					input.attackMode = -4;
				}
			} else {
				PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
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

		if (powerBracelet.state != PowerBraceletState.NONE && player.isHitLiftableObject && !CheckIfPlayerIsAttacking()) {
			if (GameInput.IsAttackPressed) {
				PlaySFX(PlayerInputAudio.PICK_UP);
				input.interactValue = 0;
				input.interactMode = 3;
				
				if (powerBraceletSystem.withStand) {
					player.isUsingStand = true;
					powerBraceletSystem.withStand = false;
					UseMana((int) ToolType.PowerBracelet, true);
				}

				player.SetPlayerState(PlayerState.POWER_BRACELET);
				// isButtonToolHold = true;
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
							} else {
								PlaySFX(PlayerInputAudio.NO_MANA);
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
							PlaySFX(PlayerInputAudio.NO_MANA);							
							player.SetPlayerIdle();
						} else {
							if (toolType == ToolType.Bomb || 
							tool.currentTool == ToolType.Container1 || 
							tool.currentTool == ToolType.Container2 || 
							tool.currentTool == ToolType.Container3 || 
							tool.currentTool == ToolType.Container4) {
								tool.isActToolReady = true;
							}
						}
					}
				}
			} 
		} 
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
			if (playerAnimationSystem.anim.isFinishAnyAnimation) {
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

					CheckMovementInput ();
					return false; 
				} else return true;
			} else return true;
		}
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
						player.SetPlayerIdle(); //
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
		if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.PARRY || state == PlayerState.DODGE || state == PlayerState.SLOW_MOTION || state == PlayerState.RAPID_SLASH || state == PlayerState.DASH || 
		player.isHitChestObject || player.isBouncing || player.isHitGateObject ||  
		input.moveMode != 0) {
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

	public void PlaySFXOneShot(PlayerInputAudio audioType)	{
		input.audioSource.PlayOneShot(input.audioClip[(int) audioType]);
	}

	public void PlaySFX(PlayerInputAudio audioType)	{
		if (!input.audioSource.isPlaying) {
			input.audioSource.clip = input.audioClip[(int) audioType];
			input.audioSource.Play();
		}
	}
}
