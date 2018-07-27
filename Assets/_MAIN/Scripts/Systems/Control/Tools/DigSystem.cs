using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class DigSystem : ComponentSystem {
	public struct DigData {
		public DigChecker digChecker;
	}

	// [InjectAttribute] ContainerSystem containerSystem;
	
	protected override void OnUpdate () {
		foreach (var e in GetEntities<DigData>()) {
			DigChecker digChecker = e.digChecker;

			if (digChecker.IsCleanForDigging) {
				GameObjectEntity.Instantiate(digChecker.diggingResultObj, digChecker.transform.position, Quaternion.identity);
				digChecker.IsCleanForDigging = false;

				GameObjectEntity.Destroy(digChecker.gameObject);
				UpdateInjectedComponentGroups();
			} else {
				// GameObjectEntity.Destroy(digChecker.gameObject);
				// UpdateInjectedComponentGroups();
			}
		}
	}
}
