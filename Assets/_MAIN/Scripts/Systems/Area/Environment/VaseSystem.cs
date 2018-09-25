using Unity.Entities;
using UnityEngine;

public class VaseSystem : ComponentSystem {

	public struct VaseComponent{
		public readonly int Length;
		public ComponentArray<Vase> vase;
	}

	[InjectAttribute] VaseComponent vaseComponent;
	Vase vase;

	protected override void OnUpdate()
	{
		for(int i = 0;i<vaseComponent.Length;i++){
			vase = vaseComponent.vase[i];
		}
	}

	void CheckVase()
	{
		if(vase.destroy){
			vase.destroy = false;
		}
	}
}
