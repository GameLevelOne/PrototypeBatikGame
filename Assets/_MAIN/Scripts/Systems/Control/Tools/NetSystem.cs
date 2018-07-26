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
	
	protected override void OnUpdate () {
		foreach (var e in GetEntities<NetData>()) {
			Net net = e.net;

			if (net.IsGotSomething) {
				//Do Something
				
				//Save net.gottenGameobject to inventory
				GameObjectEntity.Destroy(net.gottenObject.gameObject);
				UpdateInjectedComponentGroups(); //TEMP, Error without this

				net.IsGotSomething = false;
			}
		}
	}
}
