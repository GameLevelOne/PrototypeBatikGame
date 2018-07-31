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

	public Facing2D facing;

	Player player;

	PlayerState state;

	float moveSpeed;
	bool isDodgeMove = false;
	bool isAttackMove = false;
	bool isStartDashing = false;

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		float dt = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			PlayerInput input = movementData.PlayerInput[i];
			player = movementData.Player[i];
			state = player.playerState;
			Transform tr = movementData.Transform[i];
			SpriteRenderer spriteRen = movementData.Sprite[i].spriteRen;
			Rigidbody2D rb = movementData.Rigidbody[i];
			Movement movement = movementData.Movement[i];
			facing = movementData.Facing[i];
			TeleportBulletTime teleportBulletTime = movementData.TeleportBulletTime[i];
			PlayerTool tool = toolSystem.tool;

			if (state == PlayerState.DIE) continue;
			
			int attackMode = input.AttackMode;
			int moveMode = input.MoveMode;
			
			switch (moveMode) {
            case 0:
                moveSpeed = movement.normalSpeed; //NORMAL
                break;
            // case 1:
            //     moveSpeed = movement.slowSpeed; //CHARGING
            //     break;
			}
			
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

			if (input.IsUsingTool) {
				if ((player.IsHooking) || ((int) tool.currentTool != 18)) {
					rb.velocity = Vector2.zero;
				} else if (player.IsDashing) {
					Debug.Log("Set dashDir");
					Transform target = facing.attackArea.transform;
					// isStartDashing = true;
					// rb.AddForce((target.position - tr.position) * tool.dashSpeed);
					rb.velocity = (target.position - tr.position).normalized * tool.dashSpeed * dt;
				}
				
				continue;
			} else {
				player.IsDashing = false;
			}

			if (attackMode == 0) {
				Vector2 moveDir = input.MoveDir;

				if (state == PlayerState.DODGE) {
					if (!isDodgeMove) {
						Transform target = facing.attackArea.transform;
						isDodgeMove = true;
						rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
					}
				} else {
					isDodgeMove = false;
					moveDir = moveDir.normalized * moveSpeed * dt;
					rb.velocity = moveDir;	
					
					if (moveDir == Vector2.zero) {
						// player.SetPlayerIdle();
					} else {
						player.SetPlayerState(PlayerState.MOVE);
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
		}
	}
}