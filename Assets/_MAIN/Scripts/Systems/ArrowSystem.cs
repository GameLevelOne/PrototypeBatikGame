using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ArrowSystem : ComponentSystem {

	public struct ArrowData {
		public Arrow arrow;
		public Transform transform;
		public Rigidbody2D rigidbody;
	}

	protected override void OnUpdate () {
		foreach (var e in GetEntities<ArrowData>()) {
			Arrow arrow = e.arrow;
			Rigidbody2D rb = e.rigidbody;
			Transform tr = e.transform;

			float speed = arrow.speed;

			if (!arrow.IsShot) {
				rb.AddForce (tr.right * speed * 50f);
				arrow.IsShot = true;
			}

			if (arrow.IsHit) {
				GameObject.Destroy(e.arrow.gameObject);
			}
		}
	}
}
