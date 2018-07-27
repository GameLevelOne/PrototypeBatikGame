using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class NetSystem : ComponentSystem {
	public struct NetData {
		public Net net;
	}

	[InjectAttribute] ContainerSystem containerSystem;
	
	protected override void OnUpdate () {
		foreach (var e in GetEntities<NetData>()) {
			Net net = e.net;

			if (net.IsGotSomething) {
				containerSystem.SaveToContainer(net.collectibleObject);

				GameObjectEntity.Destroy(net.collectibleObject.gameObject);
				net.IsGotSomething = false;
			}
		}
	}
}
