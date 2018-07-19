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
		public ComponentArray<Movement> Movement;
		public ComponentArray<Sprite2D> Sprite;
		public ComponentArray<Rigidbody2D> Rigidbody;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] MovementData movementData;

	float moveSpeed;
	bool isDodgeMove = false;

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		float dt = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			PlayerInput input = movementData.PlayerInput[i];
			Transform tr = movementData.Transform[i];
			SpriteRenderer spriteRen = movementData.Sprite[i].spriteRen;
			Rigidbody2D rb = movementData.Rigidbody[i];
			Movement movement = movementData.Movement[i];
			Facing2D facing = movementData.Facing[i];
			
			switch (input.MoveMode) {
            case 0:
                moveSpeed = movement.normalSpeed; //NORMAL
                break;
            // case 1:
            //     moveSpeed = movement.slowSpeed; //CHARGING
            //     break;
			}
			
			if (input.AttackMode == 0) {
				Vector2 moveDir = input.MoveDir;
				int moveMode = input.MoveMode;

				if (input.isDodging) {
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
			} else {
				rb.velocity = Vector2.zero;
			}

			if (rb.velocity.y != 0f) {
				spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			}
		}
	}
}
