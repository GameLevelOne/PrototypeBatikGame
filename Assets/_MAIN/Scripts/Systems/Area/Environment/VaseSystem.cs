using Unity.Entities;
using UnityEngine;

public class VaseSystem : ComponentSystem {

	public struct VaseComponent{
		public readonly int Length;
		public ComponentArray<Vase> vase;
		public ComponentArray<Transform> vaseTransform;
	}

	[InjectAttribute] VaseComponent vaseComponent;
	Vase vase;
	Transform vaseTransform;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	protected override void OnUpdate()
	{
		for(int i = 0;i<vaseComponent.Length;i++){
			vase = vaseComponent.vase[i];
			vaseTransform = vaseComponent.vaseTransform[i];
		}
	}

	void CheckVase()
	{
		if(vase.destroy){
			vase.destroy = false;
			vase.vaseIdle.SetActive(false);
			if(vase.vaseGreyIdle.activeSelf) vase.vaseGreyIdle.SetActive(false);
			vase.vaseBroken.SetActive(true);
			// vase.particle.SetActive(true);
			lootableSpawnerSystem.CheckPlayerLuck(vase.lootDropProbability,vaseTransform.position);
		}
	}
}
