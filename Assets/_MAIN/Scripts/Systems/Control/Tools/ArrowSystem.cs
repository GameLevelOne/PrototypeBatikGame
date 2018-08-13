using UnityEngine;
using Unity.Entities;

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

			if (!arrow.isShot) {
				rb.AddForce (tr.right * speed * 50f);
				arrow.isShot = true;
			}

			if (arrow.isHit) {				
				GameObjectEntity.Destroy(arrow.gameObject);
				UpdateInjectedComponentGroups(); //TEMP, Error without this
			}
		}
	}
}
