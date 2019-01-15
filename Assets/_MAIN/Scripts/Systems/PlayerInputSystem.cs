using UnityEngine;
using Unity.Entities;
using UnityEngine.PostProcessing;

public class PlayerInputSystem : ComponentSystem {
	public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		// public ComponentArray<PlayerTool> PlayerTool;
		// public ComponentArray<Health> Health;
		public ComponentArray<Facing2D> Facing;
		public ComponentArray<Animation2D> Animation;
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

	PostProcessingBehaviour postProcCamera;
	PlayerTool tool;
	Facing2D facing;
	PowerBracelet powerBracelet;
	Animation2D animation;

	PlayerState state;

	/// <summary>
    /// <para>Current Move Direction (X, Z)<br /></para>
	/// </summary>
	Vector3 currentDir = Vector3.zero;
	// Vector3 playerDir = Vector3.zero;
	float deltaTime;
	float timeScale;
	float parryTimer = 0f;
	float slowDownParryTimer = 0f;
	float bulletTimeTimer = 0f;
	float slowDownTimer = 0f;
	float chargeAttackTimer = 0f;
	float startChargeAttackTimer = 0f;
	// float attackAwayTimer = 0f;
	float dodgeCooldownTimer = 0f;
	float dodgeCooldown = 0f;
	float bulletTimeDelay = 0f;
	// bool isAttackAway = true;
	bool isReadyForDodging = true;

	bool isDodging = false;
	bool isChargingAttack = false;
	bool isFinishAttackAnimation = true;
	bool isFinishAnyAnimation = true;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		timeScale = Time.timeScale;
		// if (inputData.Length == 0) return;

		// if (playerAnimationSystem.anim == null) return;

		postProcCamera = Camera.main.GetComponent<PostProcessingBehaviour>();
		
		for (int i=0; i<inputData.Length; i++) {
			input = inputData.PlayerInput[i];
			player = inputData.Player[i];
			// Health health = inputData.Health[i];
			facing = inputData.Facing[i];
			animation = inputData.Animation[i];

			if (!input.isInitPlayerInput) {
				InitPlayerInput();

				continue;
			}

			state = player.state;
			tool = toolSystem.tool;
			isFinishAttackAnimation = animation.isFinishAttackAnimation;
			isFinishAnyAnimation = animation.isFinishAnyAnimation;

			if (CheckIfUsingAnyTool ()) {
				continue;
			}
			
			CheckMovementInput ();
			CheckToolInput ();

			if (CheckIfInSpecificState ()) {
				continue;
			}

			// if (playerAnimationSystem.anim.isFinishAttackAnimation) {
				CheckAttackInput();
				CheckArrowInput();
				CheckDodgeInput();
				CheckGuardInput();

				if ((state == PlayerState.IDLE || state == PlayerState.MOVE) && !CheckIfPlayerIsAttacking()) {
					CheckActionInput();
				}
			// }

			CheckParryTimeScale();
		}
	}

	void InitPlayerInput () {
		currentDir = Vector3.zero;
		// playerDir = Vector3.zero;
		parryTimer = 0f;
		slowDownParryTimer = 0f;
		bulletTimeTimer = 0f;
		slowDownTimer = 0f;
		chargeAttackTimer = 0f;
		startChargeAttackTimer = 0f;
		// attackAwayTimer = 0f;
		dodgeCooldownTimer = 0f;
		// isAttackAway = true;
		isReadyForDodging = true;

		isDodging = false;
		isChargingAttack = false;
		isFinishAttackAnimation = true;

		input.isUIOpen = false;
		input.isInitAddRapidSlashQty = false;
		input.imagePressAttack.SetActive(false);
		input.imageRapidSlashHit.SetActive(false);
		// input.moveDir = input.initMoveDir;
		dodgeCooldown = input.dodgeCooldown;
		bulletTimeDelay = input.bulletTimeDelay;
		// input.isButtonHold = false;

		input.isInitPlayerInput = true;
	}

	void CheckParryTimeScale () {
		if (timeScale == input.slowTimeScale && state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
			if (slowDownParryTimer < input.slowParryDuration) {
				slowDownParryTimer += deltaTime;

				if (state != PlayerState.PARRY) {
					slowDownParryTimer = input.slowParryDuration;
				}
			} else {
				slowDownParryTimer = 0f;
				Time.timeScale = 1;
				postProcCamera.enabled = false;
			}
		}
	}

	void CheckMovementInput () {
		//NEW GAME INPUT
		float dirX = 0f;
		float dirZ = 0f;

		if (GameInput.IsUpDirectionHeld && !input.isUIOpen)
			dirZ += 1f;
		if (GameInput.IsDownDirectionHeld && !input.isUIOpen)
			dirZ -= 1f;
		if (GameInput.IsRightDirectionHeld && !input.isUIOpen)
			dirX += 1f;
		if (GameInput.IsLeftDirectionHeld && !input.isUIOpen)
			dirX -= 1f;

		//  // Debug.Log("Input Dir: "+dirX+","+dirZ);

		SetDir(dirX,dirZ);
	}

	void CheckArrowInput () {
		#region Arrow
		if ((state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.BOW) && input.moveMode == 0) {
			if (GameInput.IsBowPressed && !input.isUIOpen && isFinishAttackAnimation) {
				// if (IsHaveEnoughMana(requiredBowMana, false, false)) {
				toolType = tool.currentTool;
				input.interactValue = 0;

				if (toolType == ToolType.Bow) {
					// if (IsHaveEnoughManaForTool((int) ToolType.Bow, true, true)) {
						PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
						input.interactMode = 5;
					// } else {
					// 	PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
					// 	input.attackMode = -4;
					// }
				} else {
					PlaySFXOneShot(PlayerInputAudio.BOW_AIM);
					input.attackMode = -4;
				}
				
				animation.isFinishAttackAnimation = false;
				player.SetPlayerState(PlayerState.BOW);
				// input.isButtonHold = true;
				// }
			} else if (GameInput.IsBowReleased) {
				// input.isButtonHold = false;
				if (input.interactValue == 1) {
					if (input.interactMode == 5 && IsHaveEnoughManaForTool((int) ToolType.Bow, true, true)) {
						input.interactValue = 2;
					} else if (input.attackMode == -4 && IsHaveEnoughMana(player.requiredBowMana, true, false)) {
						input.interactValue = 2;
					} else {
						input.interactValue = -1;
					}
				} else {
					input.interactValue = -1;
				}
			} else {
				// if (!input.isButtonHold && input.interactValue == 1) {
				// 	if (input.interactMode == 5 && IsHaveEnoughManaForTool((int) ToolType.Bow, true, true)) {
				// 		input.interactValue = 2;
				// 	} else if (input.attackMode == -4 && IsHaveEnoughMana(player.requiredBowMana, true, false)) {
				// 		input.interactValue = 2;
				// 	} else {
				// 		input.interactValue = -1;
				// 	}
				// }
			}
		}
		#endregion
	}

	void CheckActionInput () {
		
		#region Open Chest
		if (player.isCanOpenChest) {
			if (GameInput.IsActionPressed && facing.DirID == 3 && !input.isUIOpen) {
				input.interactValue = 0;
				input.interactMode = -4;

				player.SetPlayerState(PlayerState.OPEN_CHEST);
				// isButtonToolHold = true;
			}

			return;
		}
		#endregion

		#region Open Gate
		if (player.isCanOpenGate) {
			if (GameInput.IsActionPressed && !input.isUIOpen) {
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

		if (powerBracelet.state != PowerBraceletState.NONE) {
			if (player.isHitLiftableObject) {
				if (GameInput.IsActionPressed && !input.isUIOpen) {
					PlaySFXOneShot(PlayerInputAudio.PICK_UP);
					input.interactValue = 0;
					input.interactMode = 3;
					
					if (powerBraceletSystem.withStand) {
						player.isUsingStand = true;
						powerBraceletSystem.withStand = false;
						UseMana(tool.GetToolManaCost((int) ToolType.PowerBracelet), true);
					}

					player.SetPlayerState(PlayerState.POWER_BRACELET);
					// isButtonToolHold = true;
				}

				//SET UI INTERACTION HINT
				powerBracelet.player.ShowInteractionHint(HintMessage.LIFT);
			} else {
				//SET UI INTERACTION HINT
				player.HideHint();
			}
		}
		#endregion
	}

	void CheckAttackInput () {
		#region Attack
		if(state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.BLOCK_ATTACK || state == PlayerState.PARRY || state == PlayerState.ATTACK) {
			// float beforeChargeDelay = input.beforeChargeDelay;
			// float attackAwayDelay = input.attackAwayDelay;
			int attackMode = input.attackMode;

			if (GameInput.IsAttackPressed && !input.isUIOpen && isFinishAttackAnimation) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
				if (attackMode == 0 && state != PlayerState.ATTACK) {
					input.attackMode = 1;
					player.SetPlayerState(PlayerState.ATTACK);
					animation.isFinishAttackAnimation = false;
					playerAnimationSystem.SetAnimationFaceDirection();
				} else if (attackMode > 0 && state == PlayerState.ATTACK) {
					if (attackMode >= 3) {
						input.attackMode = 1;	
					} else {
						input.attackMode++;
					}
					
					animation.isFinishAttackAnimation = false;
					playerAnimationSystem.SetAnimationFaceDirection();
				}

				// if (attackMode <= 0 || attackMode >= 3) {
				// 	input.attackMode = 1;	
				// } else {
				// 	input.attackMode++;
				// }
				
				// attackAwayTimer = 0f;
				// isAttackAway = false;	
				isChargingAttack = false;
				input.isInitChargeAttack = false;
			} else if (GameInput.IsAttackHeld && !input.isUIOpen) { //JOYSTICK AUTOMATIC BUTTON A ("Fire1")
				if (!input.isInitChargeAttack) {
					if (startChargeAttackTimer >= 0.3f) {
						input.isInitChargeAttack = true;
					} else {
						startChargeAttackTimer += deltaTime;
					}
				} else {
					if (!isChargingAttack) {
						if (chargeAttackTimer >= input.chargeAttackThreshold) {
							isChargingAttack = true;
							PlaySFX(PlayerInputAudio.CHARGE_LOOP, true);
						} else {
							chargeAttackTimer += deltaTime;
						}
					}
				}
			} else if (GameInput.IsAttackReleased) {
				if (input.moveMode == 1 && isChargingAttack) {
					input.attackMode = -1; //CHARGE
					isChargingAttack = false;
					input.audioSource.Stop();
					player.SetPlayerState(PlayerState.CHARGE);
				} else {
					SetMovement(0);
				}
				
				SetMovement(0); //RUN / STAND
				chargeAttackTimer = 0f;
				startChargeAttackTimer = 0f;
				input.isInitChargeAttack = false;
			} 	
		} 
		#endregion
	}

	void CheckGuardInput () {
		#region Button Guard
		if (state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.BLOCK_ATTACK || state == PlayerState.PARRY || state == PlayerState.ATTACK) {
			if (GameInput.IsGuardPressed && !input.isUIOpen && isFinishAttackAnimation) { //JOYSTICK AUTOMATIC BUTTON B ("Fire2")
				SetMovement(2); //START GUARD
				
				player.isGuarding = true;
				parryTimer = 0f;	
				player.SetPlayerIdle();
				// player.isOnParryPeriod = true;

				player.parryPos = player.transform.position;
				player.parryDir = facing.DirID;
				GameObject parryTrigger = GameObject.Instantiate(player.playerParryTrigger, player.parryPos, Quaternion.identity);
				PlayerParryTrigger playerParryTrigger = parryTrigger.GetComponent<PlayerParryTrigger>();
				playerParryTrigger.player = player;
				player.currentParryTrigger = playerParryTrigger;
				player.ReferenceParryTrigger();
				parryTrigger.SetActive(true);
			// } else if (GameInput.IsGuardHeld && !input.isUIOpen) {
				//	
			} else if (GameInput.IsGuardReleased) {
				if (input.moveMode != 1) SetMovement(0);
				
				if (state == PlayerState.BLOCK_ATTACK) {
					player.SetPlayerIdle();
				}

				player.isGuarding = false;
				parryTimer = 0f;
				// player.isOnParryPeriod = false;

				if (player.currentParryTrigger != null) {
					GameObject.Destroy(player.currentParryTrigger.gameObject);
					player.currentParryTrigger = null;
				}

				player.isCanParry = false;
			}
		}

		if (player.isGuarding) {
			if (state == PlayerState.BLOCK_ATTACK) {
				input.interactMode = -1;
			}

			if (parryTimer < input.guardParryDelay) {
				parryTimer += deltaTime;

				if (player.isCanParry) {
					// input.attackMode = -2;
					// player.isOnParryPeriod = false;
					player.isGuarding = false;
					// player.isPlayerHit = false;
						// Debug.Log("Start Counter");
					player.SetPlayerState(PlayerState.PARRY);
					// gameFXSystem.SpawnObj(gameFXSystem.gameFX.parryEffect, player.transform.position);
					Time.timeScale = input.slowTimeScale;

					//SET POST PROCESSING
					postProcCamera.profile = input.postProcProfileCounterParry;
					postProcCamera.enabled = true;

					if (player.currentParryTrigger != null) {
						GameObject.Destroy(player.currentParryTrigger.gameObject);
						player.currentParryTrigger = null;
					}
					
					player.isCanParry = false;
				}
			} else {
				if (player.currentParryTrigger != null) {
					GameObject.Destroy(player.currentParryTrigger.gameObject);
					player.currentParryTrigger = null;
				}

				// player.isOnParryPeriod = false;
				player.isCanParry = false;
				// player.isPlayerHit = false;
			}
		} else {
			if (player.currentParryTrigger != null) {
				GameObject.Destroy(player.currentParryTrigger.gameObject);
				player.currentParryTrigger = null;
			}
			
			player.isCanParry = false;
		}
		#endregion
	}

	void CheckDodgeInput () {
		#region Button Dodge
		if (state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.ATTACK) {
			if (GameInput.IsDodgePressed && !input.isUIOpen) {
				if (!isDodging && isReadyForDodging && currentDir != Vector3.zero) {
					input.moveDir = -currentDir; //REVERSE
					// gameFXSystem.ToggleDodgeFlag(true);
					gameFXSystem.ToggleParticleEffect(gameFXSystem.gameFX.dodgeEffect, true);
					player.SetPlayerState(PlayerState.DODGE);
					currentDir = Vector3.zero;
					bulletTimeTimer = 0f;	
					dodgeCooldownTimer = 0f;
					isDodging = true;
					isReadyForDodging = false;
					input.interactMode = 0;

					player.counterPos = player.transform.position;
					player.counterDir = facing.DirID;
					GameObject counterTrigger = GameObject.Instantiate(player.playerCounterTrigger, player.counterPos, Quaternion.identity);
					PlayerCounterTrigger playerCounterTrigger = counterTrigger.GetComponent<PlayerCounterTrigger>();
					playerCounterTrigger.player = player;
					player.currentCounterTrigger = playerCounterTrigger;
					player.ReferenceCounterTrigger();
					counterTrigger.SetActive(true);
				}
			}	
		}

		if (isDodging) {
			if (dodgeCooldownTimer < dodgeCooldown) {
				dodgeCooldownTimer += deltaTime;

				if (bulletTimeTimer < bulletTimeDelay) {
					bulletTimeTimer += deltaTime;
					// player.isOnBulletTimePeriod = true;
				} else {
					if (player.currentCounterTrigger != null) {
						GameObject.Destroy(player.currentCounterTrigger.gameObject);
						player.currentCounterTrigger = null;
					}

					player.isCanBulletTime = false;
					// player.isCanBulletTime = false;
					// player.isPlayerHit = false;
				}
			} else {
				isDodging = false;
				isReadyForDodging = true;
			}
		}

		if (state == PlayerState.IDLE || state == PlayerState.MOVE || state == PlayerState.DODGE) {
			if (player.isCanBulletTime) {
				input.moveMode = 3; //STEADY FOR RAPID SLASH
				input.attackMode = 0;
				input.bulletTimeAttackQty = 0;
				input.isInitAddRapidSlashQty = false;
				input.imagePressAttack.SetActive(true);
				input.imageRapidSlashHit.SetActive(false);
				input.textRapidSlashHit.text = input.bulletTimeAttackQty.ToString();
				player.SetPlayerState(PlayerState.SLOW_MOTION);
				PlaySFXOneShot(PlayerInputAudio.BULLET_TIME);
				
				if (player.currentCounterTrigger != null) {
					GameObject.Destroy(player.currentCounterTrigger.gameObject);
					player.currentCounterTrigger = null;
				}
				
				player.isCanBulletTime = false;

				// if (player.isPlayerHit && player.isCanBulletTime && player.somethingThatHitsPlayer.GetComponent<Enemy>() != null) {	
				// 	player.isOnBulletTimePeriod = false;
				// 	player.isCanBulletTime = false;
				// 	// ChangeDir(0f, 0f);
				// 	// ChangeDir(-currentDir.x, -currentDir.y);
				// 	input.moveMode = 3; //STEADY FOR RAPID SLASH
				// 	input.attackMode = 0;
				// 	player.SetPlayerState(PlayerState.SLOW_MOTION);
				// }
			}
		}
		#endregion
	}

	void CheckToolInput () {
		#region Button Tools
		if ((state != PlayerState.USING_TOOL) && (state != PlayerState.HOOK) && (state != PlayerState.DASH)  && (state != PlayerState.POWER_BRACELET) && (state != PlayerState.SWIM) && (state != PlayerState.FISHING) && state != PlayerState.BOW && input.moveMode == 0) {
			if(GameInput.IsQuickRPressed && !input.isUIOpen){
				// player.isUsingStand = false;
				toolSystem.NextTool();
				PlaySFXOneShot(PlayerInputAudio.BUTTON_CLICK);
			}
			
			if(GameInput.IsQuickLPressed && !input.isUIOpen){
				// player.isUsingStand = false;
				toolSystem.PrevTool();
				PlaySFXOneShot(PlayerInputAudio.BUTTON_CLICK);
			}

			if (GameInput.IsToolsPressed && !input.isUIOpen && isFinishAnyAnimation) {
				toolType = tool.currentTool;

				if (toolType != ToolType.None && toolType != ToolType.Bow) {
					//  // Debug.Log("Input Use Tool : " + toolType);
					input.interactValue = 0;

					// if (toolType == ToolType.Hook) {
					// 	player.SetPlayerState(PlayerState.HOOK);
					// } else 
					if (toolType == ToolType.Boots) {
						// if (currentDir != Vector3.zero) {
							if (IsHaveEnoughManaForTool((int) ToolType.Boots, true, false)) {
								input.interactMode = 1;
								input.interactValue = 0;
								player.isUsingStand = false;
								player.SetPlayerState(PlayerState.DASH);
								gameFXSystem.ToggleRunFX(false);
							} 
							// else {
							// 	PlaySFX(PlayerInputAudio.NO_MANA);
							// }
						// }
					} else if (toolType == ToolType.FishingRod) {
						if (player.isCanFishing) {
							input.interactMode = 4;
							input.interactValue = 0;
							player.SetPlayerState(PlayerState.FISHING);
						}
					} else {
						player.SetPlayerState(PlayerState.USING_TOOL);
						
						if (!IsHaveEnoughManaForTool((int) toolType, true, true)) {
							// PlaySFX(PlayerInputAudio.NO_MANA);							
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
		// isAttackAway = false;
		
		// input.slashComboVal.Clear();
		//  // Debug.Log("SET BUTTON UP AttackList CLEAR");
		// attackAwayTimer = 0f;
		// isAttackAway = true;

		player.isGuarding = false;
		parryTimer = 0f;
		// player.isOnParryPeriod = false;
		// player.isOnParryPeriod = false;
	}

	bool CheckIfUsingAnyTool () {
		if (state == PlayerState.SLOW_MOTION) {
			// float bulletTimeDuration = input.bulletTimeDuration;
			
			// if (slowDownTimer < bulletTimeDuration) {
			// 	slowDownTimer += deltaTime;
			if (!player.isInitRapidSlash) {

				if (GameInput.IsAttackPressed) {
					if (!input.isInitAddRapidSlashQty) {
						input.isInitAddRapidSlashQty = true;
						input.imagePressAttack.SetActive(false);
						input.imageRapidSlashHit.SetActive(true);
					}
					
					input.bulletTimeAttackQty++;
					input.uiRapidSlashPressedFX.Play();
					PlaySFXOneShot(PlayerInputAudio.BUTTON_CLICK);
					input.textRapidSlashHit.text = input.bulletTimeAttackQty.ToString();
				}
			} else {
				// Time.timeScale = 1f;
				input.moveMode = 0;
				slowDownTimer = 0f;
				// SetDir(0f,0f);
				// playerAnimationSystem.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
				player.SetPlayerState(PlayerState.ENGAGE);
				PlaySFXOneShot(PlayerInputAudio.ENGAGE_MOVE);
				player.isInitRapidSlash = false;
				// player.SetPlayerState(PlayerState.RAPID_SLASH);
			}

			return true;
		} else if (state == PlayerState.ENGAGE) {
			return true;
		} else if (state == PlayerState.RAPID_SLASH) {
			return true;
		} else  if (state == PlayerState.POWER_BRACELET) {
			if (animation.isFinishAnyAnimation) {
				if (input.interactValue == 0) {
					currentDir = Vector3.zero;

					if (GameInput.IsActionReleased) {
						// isButtonToolHold = false;
						CheckEndMove();
						input.interactValue = 2;
					}

					return true;
				} else if (input.interactValue == 1) { 	
					if (input.liftingMode < 0) { //LIFTING
						if (GameInput.IsActionPressed){
							CheckEndMove();
							input.interactValue = 2;
						}
					} else { //PUSHING / SWEATING
						if (GameInput.IsActionReleased) {
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
						
			if ((GameInput.IsToolsPressed || GameInput.IsActionPressed) && input.interactValue == 1){
				input.interactValue = 2;
				toolSystem.UseTool();
				PlaySFXOneShot(PlayerInputAudio.FISHING_RETURN);
			}
			
			return true;
		} else if (state == PlayerState.GET_TREASURE) { 
			currentDir = Vector3.zero;

			if (GameInput.AnyButtonPressed){ //ANY BUTTON
				gainTreasureSystem.ChangeLootableSprite();
				input.interactValue = 2;
			}
			
			return true;
		} else if (state == PlayerState.DODGE) {
		// 	CheckDodgeInput ();
			return true;
		} else if (state == PlayerState.OPEN_CHEST) {
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
		} else {
			return false;
		}
	}

	bool CheckIfInSpecificState () {
		if (state == PlayerState.USING_TOOL) {	
			return true;
		// } else if (state == PlayerState.HOOK) {
		// 	return true;
		} else if (player.isCanInteractWithNPC) {
			return true;
		} else if (state == PlayerState.GET_HURT) {
			// input.interactMode = -2;
			return true;
		} else if (state == PlayerState.SWIM) {
			SetButtonUp ();
			
			// if (input.interactValue == 0 || input.interactValue == 2) {
			// 	 // Debug.Log("Pause swim");
			// }

			return true;
		} else {
			return false;
		}
	}

	bool CheckIfPlayerIsAttacking () {
		if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.PARRY || state == PlayerState.DODGE || state == PlayerState.SLOW_MOTION || state == PlayerState.RAPID_SLASH || state == PlayerState.DASH || state == PlayerState.BOW ||
		player.isBouncing || input.moveMode != 0) {
			return true;
		} else {
			return false;
		}
	}

	public void SetMovement (int value) {
		//  // Debug.Log("SetMovement : "+value);
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
		if (fixDir != Vector3.zero && !player.isGuarding && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
			gameFXSystem.ToggleRunFX(true);
			//  // Debug.Log("TRUE");
		} else {
			gameFXSystem.ToggleRunFX(false);
			//  // Debug.Log("FALSE");
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

	bool IsHaveEnoughManaForTool (int toolIdx, bool isUseMana, bool isUsingStand) {
		 // Debug.Log("Mana cost for tool " + toolIdx + " is " + tool.GetToolManaCost(toolIdx));
		return IsHaveEnoughMana(tool.GetToolManaCost(toolIdx), isUseMana, isUsingStand);
	}

	bool IsHaveEnoughMana (float requiredMana, bool isUseMana, bool isUsingStand) {
		if(manaSystem.isHaveEnoughMana(requiredMana, isUseMana, isUsingStand)) {
			return true;
		} else {
			PlaySFXOneShot(PlayerInputAudio.NO_MANA);
			return false;
		}
	}

	void UseMana (float requiredMana, bool isUsingStand) {
		// manaSystem.UseMana(tool.GetToolManaCost(toolIdx), isUsingStand);
		manaSystem.UseMana(requiredMana, isUsingStand);
	}

	public void PlaySFXOneShot(PlayerInputAudio audioType)	{
		input.audioSource.PlayOneShot(input.audioClip[(int) audioType]);
	}

	public void PlaySFX (PlayerInputAudio audioType, bool isLoop) {
		if (isLoop) {
			input.audioSource.loop = true;
		} else {
			input.audioSource.loop = false;
		}

		// if (!input.audioSource.isPlaying) {
			input.audioSource.clip = input.audioClip[(int) audioType];
			input.audioSource.Play();
		// }
	}

	public void ResetAllTimer () {
		parryTimer = 0f;
		// slowDownParryTimer = 0f;
		bulletTimeTimer = 0f;
		slowDownTimer = 0f;
		chargeAttackTimer = 0f;
		startChargeAttackTimer = 0f;
	}
}
