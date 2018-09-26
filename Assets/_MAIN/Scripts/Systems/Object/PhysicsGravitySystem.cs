using UnityEngine;
using Unity.Entities;

public class PhysicsGravitySystem : ComponentSystem {
	public struct RigidbodyData {
		public readonly int Length;
		public ComponentArray<Rigidbody> Rigidbody;
	}
	[InjectAttribute] RigidbodyData rigidbodyData;

	// public struct PlayerRigidbodyData {
	// 	public readonly int Length;
	// 	public ComponentArray<Player> Player;
	// }
	// [InjectAttribute] PlayerRigidbodyData playerRigidbodyData;

	protected override void OnUpdate () {
		for (int i=0; i<rigidbodyData.Length; i++) {
			Rigidbody rigidbody = rigidbodyData.Rigidbody[i];

			if (rigidbody.useGravity) {
				float mass = rigidbody.mass;

				rigidbody.AddForce(Physics.gravity * (mass * mass), ForceMode.Acceleration);
				// rigidbody.useGravity = false;
			}
		}

		// for (int i=0; i<playerRigidbodyData.Length; i++) {
		// 	Player player = playerRigidbodyData.Player[i];

		// 	if (player.playerInteractionRB.IsSleeping()) {
		// 		player.playerInteractionRB.WakeUp();
		// 	}
		// }
	}
}
