using Unity.Entities;
using UnityEngine;

public class BushSystem : ComponentSystem {

	public struct BushComponent{
		public readonly int Length;
		public ComponentArray<Bush> bush;
		public ComponentArray<Transform> bushTransform;
	}

	[InjectAttribute] BushComponent bushComponent;
	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	Bush currBush;
	Transform currBushTransform;

	protected override void OnUpdate()
	{
		for(int i = 0;i<bushComponent.Length;i++){
			currBush = bushComponent.bush[i];
			currBushTransform = bushComponent.bushTransform[i];

			if (!currBush.isInitBush) {
				InitBush();
			} else {
				CheckBush();
			}
		}
	}

	void InitBush () {
		currBush.initBushPos = currBushTransform.position;
		//
		currBush.isInitBush = true;
	}

	void CheckBush()
	{
		if (currBush.isLifted) {
			SpawnBushRoot();

			currBush.isLifted = false;
		} else if (currBush.destroy) Destroy();
	}

	void SpawnBushRoot () {
		GameObject.Instantiate(currBush.rootObj,currBush.initBushPos,Quaternion.Euler(40f,0f,0f));
		currBush.isRootSpawned = true;
	}

	void Destroy()
	{
		GameObject.Instantiate(currBush.bushCutFX,currBushTransform.position,Quaternion.Euler(40f,0f,0f));

		if (!currBush.isRootSpawned) {
			SpawnBushRoot();
		}

		//SPAWN ITEM
		lootableSpawnerSystem.CheckPlayerLuck(currBush.spawnItemProbability, currBush.transform.position);

		GameObject.Destroy(currBush.gameObject);
		UpdateInjectedComponentGroups();
	}
}
