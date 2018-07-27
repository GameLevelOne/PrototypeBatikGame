using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class CollectibleObjectSystem : ComponentSystem {
	public struct CollectibleData {
		public Collectible collectible;
	}

	bool isInstantiating = false;
	
	protected override void OnUpdate () {
		foreach (var e in GetEntities<CollectibleData>()) {
			Collectible collectible = e.collectible;

			if (!isInstantiating) {
				// GameObjectEntity.Instantiate ()
			}
		}
	}
}
