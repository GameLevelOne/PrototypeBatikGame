using Unity.Entities;
using UnityEngine;

public class BushSystem : ComponentSystem {

	public struct BushComponent{
		public readonly int Length;
		public ComponentArray<Bush> bush;
	}

	[InjectAttribute] BushComponent bushComponent;
	Bush currBush;

	protected override void OnUpdate()
	{
		for(int i = 0;i<bushComponent.Length;i++){
			currBush = bushComponent.bush[i];

			CheckBush();
		}
	}

	void CheckBush()
	{
		if(currBush.destroy) Destroy();
	}


	void Destroy()
	{
		 GameObject.Destroy(currBush.gameObject);
		UpdateInjectedComponentGroups();
	}
}
