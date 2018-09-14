﻿using UnityEngine;
using Unity.Entities;

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

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		deltaTime = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			input = movementData.PlayerInput[i];
			player = movementData.Player[i];
			state = player.state;
			tr = movementData.Transform[i];
			// spriteRen = movementData.Sprite[i].spriteRen;
			rb = movementData.Rigidbody[i];
			movement = movementData.Movement[i];
			facing = movementData.Facing[i];
			teleportBulletTime = movementData.TeleportBulletTime[i];
			tool = toolSystem.tool;

			if (state == PlayerState.DIE) continue;
			
			attackMode = input.AttackMode;
			// int moveMode = input.MoveMode;
			
			// switch (moveMode) {
            // case 0:
			moveSpeed = movement.normalSpeed; //NORMAL
            //     break;
            // // case 1:
            // //     moveSpeed = movement.slowSpeed; //CHARGING
            // //     break;
			// }

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
			}

			SetPlayerStandardMove();

			continue; //TEMP
			
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

	void SetPlayerStandardMove () {
		if (attackMode == 0) {
			// moveDir = input.moveDir;

			if (state == PlayerState.DODGE) {
				Transform target = facing.attackArea.transform;
				
				//=====SPEED GOING SLOWER=====//
				// if (!isDodgeMove) { 
				// 	isDodgeMove = true;
				// 	rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
				// } 
				
				//=====SPEED CONSTANT=====//
				if (!isDodgeMove) { 
					isDodgeMove = true;
					rb.velocity = (target.position - tr.position).normalized * movement.dodgeSpeed * deltaTime;

					input.moveDir = -moveDir; //REVERSE
				}
			} else {
				isDodgeMove = false;
				moveDir = moveDir.normalized * moveSpeed * deltaTime;
				rb.velocity = moveDir;	
				
				if (moveDir == Vector3.zero) {
					// player.SetPlayerIdle();
				} else {
					if (state != PlayerState.POWER_BRACELET && state != PlayerState.SWIM && state != PlayerState.OPEN_CHEST && state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
						player.SetPlayerState(PlayerState.MOVE);
					} 
				}
			}
		} else if (attackMode >= -1 && attackMode <= 3 && input.moveDir != Vector3.zero) {
			if (player.isMoveAttack) {
				Transform target = facing.attackArea.transform;
				rb.AddForce((target.position - tr.position) * movement.attackMoveForce);
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
		Transform target = facing.attackArea.transform;
		Vector3 dir = target.position - tr.position;
		float dashSpeed = tool.GetObj((int) ToolType.Boots).GetComponent<Boots>().bootsSpeed;
		// if (state == PlayerState.HOOK) {
		// 	rb.velocity = Vector2.zero;
		// }

		if (state == PlayerState.DASH) {
			if (input.interactValue == 0) {
				player.isUsingStand = false;
				if (dashDelay > 0f) {
					dashDelay -= deltaTime;
					rb.velocity = Vector3.zero;
				} else {
					if (isHaveEnoughMana((int) ToolType.Boots, true, true)) {
						input.interactValue = 1;
						// UseMana((int) ToolType.Boots);
					}
				}
			} else if (input.interactValue == 1) {
				if (player.isBouncing) {
					input.interactValue = 2;
				} else {
					if (isHaveEnoughMana((int) ToolType.Boots, false, false)) {
						rb.velocity = dir.normalized * dashSpeed * deltaTime;

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
						rb.velocity = -dir.normalized * movement.bounceSpeed * deltaTime * brakeTime;
					} else {
						rb.velocity = dir.normalized * movement.bounceSpeed * deltaTime * brakeTime;
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
			moveDir = Vector3.zero;
			rb.velocity = moveDir;
		}
	}

	bool CheckIfAllowedToMove () {
		if ((state == PlayerState.SLOW_MOTION) || (state == PlayerState.RAPID_SLASH)) {
			if (attackMode == -3) {
				Vector3 teleportPos = teleportBulletTime.Teleport();
				rb.position = new Vector3 (teleportPos.x, rb.position.y, teleportPos.z);
				Time.timeScale = 0.1f;
				input.AttackMode = 0;
				rb.velocity = Vector3.zero;
				// spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			}

			return false;
		} else if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH || state == PlayerState.BOW || state == PlayerState.FISHING || state == PlayerState.GET_TREASURE) {
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