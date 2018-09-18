using Unity.Entities;
using UnityEngine;

public class BushSystem : ComponentSystem {

	public struct BushComponent{
		public readonly int Length;
		public ComponentArray<Bush> bush;
		public ComponentArray<Transform> bushTransform;
	}

	[InjectAttribute] BushComponent bushComponent;
	Bush currBush;
	Transform currBushTransform;

	protected override void OnUpdate()
	{
		for(int i = 0;i<bushComponent.Length;i++){
			currBush = bushComponent.bush[i];
			currBushTransform = bushComponent.bushTransform[i];
			CheckBush();
		}
	}

	void CheckBush()
	{
		if(currBush.destroy) Destroy();
	}


	void Destroy()
	{
		GameObject.Instantiate(currBush.bushCutFX,currBushTransform.position,Quaternion.Euler(40f,0f,0f));
		GameObject.Destroy(currBush.gameObject);
		UpdateInjectedComponentGroups();
	}
}
