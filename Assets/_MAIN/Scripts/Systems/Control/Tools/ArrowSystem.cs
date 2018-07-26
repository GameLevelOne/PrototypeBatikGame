using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ArrowSystem : ComponentSystem {

	public struct ArrowData {
		public readonly int Length;
		public ComponentArray<Arrow> Arrow;
		public  ComponentArray<Transform> Transform;
		public  ComponentArray<Rigidbody2D> Rigidbody2D;
	}
	[InjectAttribute] ArrowData arrowData;

	protected override void OnUpdate () {
		if (arrowData.Length == 0) return;

		for (int i=0; i<arrowData.Length; i++) {
			Arrow arrow = arrowData.Arrow[i];
			Rigidbody2D rb = arrowData.Rigidbody2D[i];
			Transform tr = arrowData.Transform[i];

			float speed = arrow.speed;

			if (!arrow.IsShot) {
				rb.AddForce (tr.right * speed * 50f);
				arrow.IsShot = true;
			}

			if (arrow.IsHit) {
				// GameObject.Destroy(arrow.gameObject);
				// return; //TEMP, Error without this
				
				GameObjectEntity.Destroy(arrow.gameObject);
				UpdateInjectedComponentGroups(); //TEMP, Error without this
			}
		}
	}
}
