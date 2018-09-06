using UnityEngine;
using Unity.Entities;

public class PhysicsGravitySystem : ComponentSystem {
	public struct RigidbodyData {
		public readonly int Length;
		public ComponentArray<Rigidbody> Rigidbody;
	}
	[InjectAttribute] RigidbodyData rigidbodyData;

	Rigidbody rigidbody;

	protected override void OnUpdate () {
		if (rigidbodyData.Length == 0) return;

		for (int i=0; i<rigidbodyData.Length; i++) {
			rigidbody = rigidbodyData.Rigidbody[i];

			if (rigidbody.useGravity) {
				float mass = rigidbody.mass;

				rigidbody.AddForce(Physics.gravity * (mass * mass), ForceMode.Acceleration);
				// rigidbody.useGravity = false;
			}
		}
	}
}
