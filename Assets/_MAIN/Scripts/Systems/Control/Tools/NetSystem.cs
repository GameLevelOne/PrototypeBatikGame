using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class NetSystem : ComponentSystem {
	public struct NetData {
		public readonly int Length;
		public ComponentArray<Net> net;
	}
	[InjectAttribute] NetData netData;

	[InjectAttribute] ContainerSystem containerSystem;

	Net net;
	
	protected override void OnUpdate () {
		if (netData.Length == 0) return;

		for (int i=0; i<netData.Length; i++) { 
			net = netData.net[i];

			if (net.IsGotSomething) {
				containerSystem.SaveToContainer(net.collectibleObject);

				GameObjectEntity.Destroy(net.collectibleObject.gameObject);
				net.IsGotSomething = false;
			}
		}

		// foreach (var e in GetEntities<NetData>()) {
		// 	Net net = e.net;

		// 	if (net.IsGotSomething) {
		// 		containerSystem.SaveToContainer(net.collectibleObject);

		// 		GameObjectEntity.Destroy(net.collectibleObject.gameObject);
		// 		net.IsGotSomething = false;
		// 	}
		// }
	}
}
