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

	float moveSpeed;
	bool isDodgeMove = false;
	bool isAttackMove = false;

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		float dt = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			PlayerInput input = movementData.PlayerInput[i];
			Player player = movementData.Player[i];
			Transform tr = movementData.Transform[i];
			SpriteRenderer spriteRen = movementData.Sprite[i].spriteRen;
			Rigidbody2D rb = movementData.Rigidbody[i];
			Movement movement = movementData.Movement[i];
			Facing2D facing = movementData.Facing[i];
			TeleportBulletTime teleportBulletTime = movementData.TeleportBulletTime[i];
			
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
			
			if (player.IsSlowMotion || player.IsRapidSlashing) {
				if (attackMode == -3) {
					teleportBulletTime.Teleport();
					input.AttackMode = 0;
					rb.velocity = Vector2.zero;
					spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
				}

				return;
			}

			if (attackMode == 0) {
				Vector2 moveDir = input.MoveDir;

				if (input.IsDodging) {
					if (!isDodgeMove) {
						Transform target = facing.attackArea.transform;
						isDodgeMove = true;
						rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
					}
				} else {
					isDodgeMove = false;
					moveDir = moveDir.normalized * moveSpeed * dt;
					rb.velocity = moveDir;	
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