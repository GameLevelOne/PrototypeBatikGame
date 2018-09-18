using Unity.Entities;
using UnityEngine;

public class GrassCutSystem : ComponentSystem {

	public struct GrassCutComponent{
		public readonly int Length;
		public ComponentArray<GrassCut> grassCut;
	}

	[InjectAttribute] GrassCutComponent grassCutComponent;
	GrassCut grassCut;

	protected override void OnUpdate()
	{
		for(int i = 0;i<grassCutComponent.Length;i++){
			grassCut = grassCutComponent.grassCut[i];

			CheckDestroy();
		}
	}

	void CheckDestroy()
	{
		if(grassCut.destroy) GameObject.Destroy(grassCut.gameObject);
		UpdateInjectedComponentGroups();
	}

}
