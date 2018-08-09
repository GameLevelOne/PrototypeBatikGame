using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerMovementSystem : ComponentSystem {
	public struct MovementData {
		public readonly int Length;
		public ComponentArray<Transform> Transform;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Movement> Movement;
		public ComponentArray<Sprite2D> Sprite;
		public ComponentArray<Rigidbody2D> Rigidbody;
		public ComponentArray<Facing2D> Facing;
		public ComponentArray<TeleportBulletTime> TeleportBulletTime;
	}
	[InjectAttribute] MovementData movementData;
	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] SwimSystem SwimSystem;

	public Facing2D facing;

	Player player;
	PlayerInput input;
	Movement movement;

	PlayerState state;

	Transform tr;
	Rigidbody2D rb;

	float deltaTime;
	float moveSpeed;
	bool isDodgeMove = false;
	bool isAttackMove = false;
	// bool isStartDashing = false;
	float brakeTime = 0f;

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		deltaTime = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			input = movementData.PlayerInput[i];
			player = movementData.Player[i];
			state = player.state;
			tr = movementData.Transform[i];
			SpriteRenderer spriteRen = movementData.Sprite[i].spriteRen;
			rb = movementData.Rigidbody[i];
			movement = movementData.Movement[i];
			facing = movementData.Facing[i];
			TeleportBulletTime teleportBulletTime = movementData.TeleportBulletTime[i];
			PlayerTool tool = toolSystem.tool;

			if (state == PlayerState.DIE) continue;
			
			int attackMode = input.AttackMode;
			// int moveMode = input.MoveMode;
			
			// switch (moveMode) {
            // case 0:
			moveSpeed = movement.normalSpeed; //NORMAL
            //     break;
            // // case 1:
            // //     moveSpeed = movement.slowSpeed; //CHARGING
            // //     break;
			// }
			
			SetPlayerMove ();
			continue; //TEMP
			
			#region OLD
			if ((state == PlayerState.SLOW_MOTION) || (state == PlayerState.RAPID_SLASH)) {
				if (attackMode == -3) {
					tr.position = teleportBulletTime.Teleport();
					Time.timeScale = 0.1f;
					input.AttackMode = 0;
					rb.velocity = Vector2.zero;
					spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
				}

				continue;
			}

			if (state == PlayerState.USING_TOOL || state == PlayerState.HOOK || state == PlayerState.DASH || state == PlayerState.BOUNCE || state == PlayerState.BRAKE) {
				Transform target = facing.attackArea.transform;
				Vector2 dir = target.position - tr.position;
				
				if (state == PlayerState.HOOK) {
					rb.velocity = Vector2.zero;
				} else if (state == PlayerState.DASH) {
					// isStartDashing = true;
					// rb.AddForce(dir * tool.dashSpeed);
					rb.velocity = dir.normalized * tool.dashSpeed * deltaTime;
				} else if (state == PlayerState.BRAKE) {
					if (brakeTime > 0f) {
						brakeTime -= deltaTime;
						rb.velocity = dir.normalized * movement.bounceSpeed * deltaTime * brakeTime;
					} else {
						input.moveDir = Vector2.zero;
						player.SetPlayerIdle();
					}
				} else if (state == PlayerState.BOUNCE) {
					// rb.AddForce(-dir * movement.bounceSpeed);
					if (brakeTime > 0f) {
						brakeTime -= deltaTime;
						rb.velocity = -dir.normalized * movement.bounceSpeed * deltaTime * brakeTime;
					} else {
						input.moveDir = Vector2.zero;
						player.SetPlayerIdle();
					}
				} else {
					rb.velocity = Vector2.zero;
				}
				
				continue;
			} else if (state == PlayerState.POWER_BRACELET) {
				// rb.AddForce(-dir * movement.bounceSpeed);
				if (input.interactValue == 2 || input.interactValue == 0) {
					// Debug.Log("input.InteractValue : "+input.InteractValue);
					input.moveDir = Vector2.zero;
				}
			} else if (state == PlayerState.FISHING) {
				input.moveDir = Vector2.zero;
				rb.velocity = Vector2.zero;
			} else {
				brakeTime = movement.brakeTime;
				// player.IsDashing = false;
			}

			if (attackMode == 0) {
				Vector2 moveDir = input.moveDir;

				if (state == PlayerState.DODGE) {
					if (!isDodgeMove) {
						Transform target = facing.attackArea.transform;
						isDodgeMove = true;
						rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
					}
				} else {
					isDodgeMove = false;
					moveDir = moveDir.normalized * moveSpeed * deltaTime;
					rb.velocity = moveDir;	
					
					if (moveDir == Vector2.zero) {
						// player.SetPlayerIdle();
					} else {
						if (state != PlayerState.POWER_BRACELET && !SwimSystem.flippers.IsPlayerSwimming) {
							player.SetPlayerState(PlayerState.MOVE);
						}
					}
				}
			} else if ((attackMode == 2) || (attackMode == 3)) {
				if (!isAttackMove) {
					Transform target = facing.attackArea.transform;
					isAttackMove = true;
					rb.AddForce((target.position - tr.position) * movement.attackMoveSpeed);
				} else {
					isAttackMove = false;
					rb.velocity = Vector2.zero;
				}
			} else {
				rb.velocity = Vector2.zero;
			}

			if (rb.velocity.y != 0f) {
				spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			}
			#endregion OLD
		}
	}

	void SetPlayerMove () {
		Vector2 moveDir = input.moveDir;

		if (state == PlayerState.DODGE) {
			if (!isDodgeMove) {
				Transform target = facing.attackArea.transform;
				isDodgeMove = true;
				rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
			} 
		} else {
			isDodgeMove = false;
			moveDir = moveDir.normalized * moveSpeed * deltaTime;
			rb.velocity = moveDir;	
			
			if (moveDir == Vector2.zero) {
				// player.SetPlayerIdle();
			} else {
				player.SetPlayerState(PlayerState.MOVE);
			}
		}
	}
}