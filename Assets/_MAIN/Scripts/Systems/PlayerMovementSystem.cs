using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PlayerMovementSystem : ComponentSystem {
	public struct MovementData {
		public readonly int Length;
		public ComponentArray<Transform> Transform;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Movement> Movement;
		public ComponentArray<Sprite2D> Sprite;
		public ComponentArray<Rigidbody> Rigidbody;
		public ComponentArray<Facing2D> Facing;
		public ComponentArray<TeleportBulletTime> TeleportBulletTime;
	}
	[InjectAttribute] MovementData movementData;
	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] SwimSystem SwimSystem;
	[InjectAttribute] ManaSystem manaSystem;

	public PlayerInput input;
	public Facing2D facing;
	public Player player;

	Movement movement;
	PlayerTool tool;
	PlayerState state;
	TeleportBulletTime teleportBulletTime;

	Transform tr;
	Rigidbody rb;
	// SpriteRenderer spriteRen;

	float deltaTime;
	float moveSpeed;
	bool isDodgeMove = false;
	// bool isAttackMove = false;
	// bool isStartDashing = false;
	float brakeTime = 0f;
	float dashDelay = 0f;
	float dashTime = 0f;
	int attackMode;
	Vector3 moveDir;
	// Vector3 currentMoveDir;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		// if (movementData.Length == 0) return;
		
		for (int i=0; i<movementData.Length; i++) {
			input = movementData.PlayerInput[i];
			player = movementData.Player[i];
			tr = movementData.Transform[i];
			// spriteRen = movementData.Sprite[i].spriteRen;
			rb = movementData.Rigidbody[i];
			movement = movementData.Movement[i];
			facing = movementData.Facing[i];
			teleportBulletTime = movementData.TeleportBulletTime[i];

			if (!movement.isInitMovement) {
				InitMovement();

				continue;
			}

			state = player.state;
			tool = toolSystem.tool;

			if (state == PlayerState.DIE) {
				rb.velocity = Vector3.zero;
				input.moveDir = Vector3.zero;

				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0")){
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.LEVEL_PLAYER_START_POS,0);
					SceneManager.LoadScene(Constants.SceneName.SCENE_LEVEL_1);
				}	

				continue;
			}

#region TESTING 
			// if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0")){
			// 	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			// }	
#endregion
			
			attackMode = input.attackMode;
			// int moveMode = input.MoveMode;
			
			// switch (moveMode) {
            // case 0:
			moveSpeed = movement.normalSpeed; //NORMAL
            //     break;
            // // case 1:
            // //     moveSpeed = movement.slowSpeed; //CHARGING
            // //     break;
			// }

			
			// Debug.Log("Velocity A "+rb.velocity +" | "+moveSpeed);

			if (!CheckIfAllowedToMove()) {
				SetPlayerSpecificMove ();
				continue;
			} else if (state == PlayerState.POWER_BRACELET) {
				if (input.interactValue == 2 || input.interactValue == 0) {
					moveDir = Vector3.zero;
				} else {
					moveDir = input.moveDir;
				}
			} else if (state == PlayerState.SWIM) {
				if (input.interactValue == 2 || input.interactValue == 0) {
					moveDir = Vector3.zero;
				} else {
					moveDir = input.moveDir;
				}
			}
			// else if (state == PlayerState.FISHING || state == PlayerState.GET_TREASURE) {
			// 	moveDir = Vector2.zero;
			// 	rb.velocity = moveDir;
			// } 
			// else if (state == PlayerState.BOW) {
			// 	moveDir = Vector2.zero;
			// } 
			else {
				dashDelay = movement.dashDelay;
				brakeTime = movement.brakeTime;
				player.isBouncing = false;
				moveDir = input.moveDir;

				// Debug.Log(moveDir +"\n"+ moveDir.normalized);
			}

			SetPlayerStandardMove();
			// Debug.Log(currentMoveDir);
			// continue; //TEMP
			
#region OLD
			// if ((state == PlayerState.SLOW_MOTION) || (state == PlayerState.RAPID_SLASH)) {
			// 	if (attackMode == -3) {
			// 		tr.position = teleportBulletTime.Teleport();
			// 		Time.timeScale = 0.1f;
			// 		input.AttackMode = 0;
			// 		rb.velocity = Vector2.zero;
			// 		spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			// 	}

			// 	continue;
			// }

			// if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH) {
			// 	Transform target = facing.attackArea.transform;
			// 	Vector2 dir = target.position - tr.position;
				
			// 	if (state == PlayerState.HOOK) {
			// 		rb.velocity = Vector2.zero;
			// 	} else if (state == PlayerState.DASH) {
			// 		// isStartDashing = true;
			// 		// rb.AddForce(dir * tool.dashSpeed);
			// 		// rb.velocity = dir.normalized * tool.dashSpeed * deltaTime;
			// 	} else {
			// 		rb.velocity = Vector2.zero;
			// 	}
				
			// 	continue;
			// } else if (state == PlayerState.POWER_BRACELET) {
			// 	// rb.AddForce(-dir * movement.bounceSpeed);
			// 	if (input.interactValue == 2 || input.interactValue == 0) {
			// 		// Debug.Log("input.InteractValue : "+input.InteractValue);
			// 		input.moveDir = Vector2.zero;
			// 	}
			// } else if (state == PlayerState.FISHING) {
			// 	input.moveDir = Vector2.zero;
			// 	rb.velocity = Vector2.zero;
			// } else {
			// 	brakeTime = movement.brakeTime;
			// 	// player.IsDashing = false;
			// }

			// if (attackMode == 0) {
			// 	moveDir = input.moveDir;

			// 	if (state == PlayerState.DODGE) {
			// 		if (!isDodgeMove) {
			// 			Transform target = facing.attackArea.transform;
			// 			isDodgeMove = true;
			// 			rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
			// 		}
			// 	} else {
			// 		isDodgeMove = false;
			// 		moveDir = moveDir.normalized * moveSpeed * deltaTime;
			// 		rb.velocity = moveDir;	
					
			// 		if (moveDir == Vector2.zero) {
			// 			// player.SetPlayerIdle();
			// 		} else {
			// 			if (state != PlayerState.POWER_BRACELET && !SwimSystem.flippers.isPlayerSwimming) {
			// 				player.SetPlayerState(PlayerState.MOVE);
			// 			}
			// 		}
			// 	}
			// } else if ((attackMode == 2) || (attackMode == 3)) {
			// 	// if (!isAttackMove) {
			// 		Transform target = facing.attackArea.transform;
			// 		// isAttackMove = true;
			// 		rb.AddForce((target.position - tr.position) * movement.attackMoveForce);
			// 	// } else {
			// 	// 	isAttackMove = false;
			// 		rb.velocity = Vector2.zero;
			// 	// }
			// } else {
			// 	rb.velocity = Vector2.zero;
			// }

			// if (rb.velocity.y != 0f) {
			// 	spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			// }
#endregion OLD
		}
	}

	void InitMovement () {
		brakeTime = 0f;
		dashTime = 0f;
		dashDelay = 0f;
		isDodgeMove = false;
		moveDir = Vector3.zero;

		movement.isInitMovement = true;
	}

	void SetPlayerStandardMove () {
		if (attackMode == 0) {
			// moveDir = input.moveDir;

			if (state == PlayerState.DODGE) {
				// Transform target = facing.attackArea.transform;
				
				//=====SPEED GOING SLOWER=====//
				// if (!isDodgeMove) { 
				// 	isDodgeMove = true;
				// 	rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
				// } 
				
				//=====SPEED CONSTANT=====//
				if (!isDodgeMove) { 
					isDodgeMove = true;
					rb.velocity = -moveDir * movement.dodgeSpeed * deltaTime;

					// input.moveDir = -moveDir; //REVERSE
					input.moveDir = Vector3.zero;
					input.dirButtons =  new List<int>(4){0,0,0,0};
				} else {
					// input.dirButtons =  new List<int>(4){0,0,0,0};
					// moveDir = Vector3.zero;
					// input.moveDir = Vector3.zero;
				}
			} else {
				isDodgeMove = false;
				//SET VELOCITY
				// moveDir = moveDir.normalized * moveSpeed * deltaTime;
				rb.velocity = moveDir * moveSpeed * deltaTime;
				// Debug.Log("Velocity : "+rb.velocity+"\n MoveDir : "+moveDir+" | Normalized : "+moveDir.normalized);
				// Debug.Log("Velocity: "+rb.velocity+"= \n normalized: "+moveDir.normalized+ "x moveSpeed: "+moveSpeed+"x deltaTime: "+deltaTime);

				if (moveDir != Vector3.zero) {
					// currentMoveDir = moveDir;
					
					if (state != PlayerState.POWER_BRACELET && state != PlayerState.SWIM && state != PlayerState.OPEN_CHEST && state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
						player.SetPlayerState(PlayerState.MOVE);
					} 
				}
			}
		} else if (attackMode >= -1 && attackMode <= 3 && input.moveDir != Vector3.zero) {
			// currentMoveDir = moveDir;

			if (player.isMoveAttack) {
				// Transform target = facing.attackArea.transform;
				rb.AddForce(moveDir * movement.attackMoveForce);
				// player.isMoveAttack = false;
			} else {
				rb.velocity = Vector3.zero;
			}
		} else {
			rb.velocity = Vector3.zero;
		}

		// if (rb.velocity.y != 0f) {
		// 	spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
		// }
	}

	void SetPlayerSpecificMove () {
		// Transform target = facing.attackArea.transform;
		// Vector3 dir = target.position - tr.position;
		float dashSpeed = tool.GetObj((int) ToolType.Boots).GetComponent<Boots>().bootsSpeed;
		// if (state == PlayerState.HOOK) {
		// 	rb.velocity = Vector2.zero;
		// }

		if (state == PlayerState.DASH) {
			if (input.interactValue == 0) {
				// player.isUsingStand = false;
				// if (dashDelay > 0f) {
				// 	dashDelay -= deltaTime;
					rb.velocity = Vector3.zero;
				// } else {
				// 	if (isHaveEnoughMana((int) ToolType.Boots, true, true)) {
				// 		input.interactValue = 1;
				// 		// UseMana((int) ToolType.Boots);
				// 	}
				// }
			} else if (input.interactValue == 1) {
				if (player.isBouncing) {
					input.interactValue = 2;
				} else {
					if (isHaveEnoughMana((int) ToolType.Boots, false, false)) {
						rb.velocity = moveDir * dashSpeed * deltaTime;

						if (dashTime <= 0.2f) {
							dashTime += deltaTime;
						} else {
							dashTime = 0f;
							// Debug.Log("Use mana dash");
							UseMana((int) ToolType.Boots, true);
						}
					}
				}
			} else if (input.interactValue == 2) {
				if (brakeTime > 0f) {
					brakeTime -= deltaTime;
					
					if (player.isBouncing) {
						rb.velocity = -moveDir * movement.bounceSpeed * deltaTime * brakeTime;
					} else {
						rb.velocity = moveDir * movement.bounceSpeed * deltaTime * brakeTime;
					}
				} else {
					input.moveDir = Vector3.zero;
					player.isBouncing = false;
					dashTime = 0f;
					player.SetPlayerIdle();
				}
			} else {
				rb.velocity = Vector3.zero;
			}
		} else if (state == PlayerState.OPEN_CHEST) {
			rb.velocity = Vector3.zero;
		} else {
			input.moveDir = Vector3.zero;
			rb.velocity = Vector3.zero;
		}
	}

	bool CheckIfAllowedToMove () {
		if ((state == PlayerState.SLOW_MOTION) || (state == PlayerState.RAPID_SLASH)) {
			if (attackMode == 0) {
				Vector3 teleportPos = teleportBulletTime.Teleport();
				rb.position = new Vector3 (teleportPos.x, rb.position.y, teleportPos.z);
				Time.timeScale = 0.1f;
				input.attackMode = -3; //Set counterslash first
				// Debug.Log("Reset AttackMode - CheckIfAllowedToMove");
				rb.velocity = Vector3.zero;
				// spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			}

			return false;
		} else if (
			state == PlayerState.USING_TOOL || 
			state == PlayerState.HOOK || 
			state == PlayerState.DASH || 
			state == PlayerState.BOW || 
			state == PlayerState.FISHING || 
			state == PlayerState.GET_TREASURE || 
			state == PlayerState.DIE
			) {
			return false;
		} else {
			return true;
		}
	}

	bool isHaveEnoughMana (int toolIdx, bool isUseMana, bool isUsingStand) {
		// Debug.Log("mana cost for tool " + toolIdx + " is " + tool.GetToolManaCost(toolIdx));
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