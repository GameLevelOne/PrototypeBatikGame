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
	}
	[InjectAttribute] MovementData movementData;

	float moveSpeed;

	protected override void OnUpdate () {
		if (movementData.Length == 0) return;
		
		float dt = Time.deltaTime;

		for (int i=0; i<movementData.Length; i++) {
			PlayerInput input = movementData.PlayerInput[i];
			Transform tr = movementData.Transform[i];
			SpriteRenderer spriteRen = movementData.Sprite[i].spriteRen;
			Rigidbody2D rb = movementData.Rigidbody[i];
			Movement movement = movementData.Movement[i];
			
			switch (input.MoveMode) {
            case 0:
                moveSpeed = movement.normalValue; //NORMAL
                break;
            // case 1:
            //     moveSpeed = movement.slowValue; //CHARGING
            //     break;
			}
			
			if (input.AttackMode == 0) {
				Vector2 moveDir = input.MoveDir;
				moveDir = moveDir.normalized * moveSpeed * dt;
				rb.velocity = moveDir;
			} else {
				rb.velocity = Vector2.zero;
			}

			if (rb.velocity.y != 0f) {
				spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
			}
		}
	}
}
