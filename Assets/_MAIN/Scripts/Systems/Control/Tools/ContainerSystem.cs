using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


public class ContainerSystem : ComponentSystem {
	public struct ContainerData {
		public readonly int Length;
		public ComponentArray<Container> Container;
	}
	[InjectAttribute] ContainerData containerData;

	Container container;

	LootableType[] lootableTypes;

	protected override void OnUpdate () {
		if (containerData.Length == 0) return;

		for (int i=0; i<containerData.Length; i++) {
			container = containerData.Container[i];

			lootableTypes = container.lootableTypes;
		}
	}

	public void SaveToContainer (Lootable lootable) {
		for (int i=0; i<lootableTypes.Length; i++) {
			if (container.CheckIfContainerIsEmpty(i)) {
				lootableTypes[i] = lootable.lootableType;

				break;
			} else {
				Debug.Log(lootableTypes[i] + " is failed to contain, there is no empty container");
			}
		}
	}

	public void UseCollectibleInContainer (int lootableTypeIdx) {
		if (container.CheckIfContainerIsEmpty(lootableTypeIdx)) {
			int idx = lootableTypeIdx + 1;
			Debug.Log("Container " + idx + " is empty");
		} else {
			switch(lootableTypes[lootableTypeIdx]) {
				case LootableType.NONE: 
					//
					ReportContainerIsEmpty();
					break;
				case LootableType.GOLD: 
					//
					UseGold();
					break;
				case LootableType.HP_POTION: 
					//
					UseHPPotion();
					break;
				case LootableType.MANA_POTION: 
					//
					UseManaPotion();
					break;
				default: 
					Debug.Log("Unknown item in container");
					break;
			}

			lootableTypes[lootableTypeIdx] = LootableType.NONE;
		}
	}

	void ReportContainerIsEmpty () {
		Debug.Log("This container is empty");
	}

	void UseGold () {
		Debug.Log("Use Gold");
	}

	void UseHPPotion () {
		Debug.Log("Use HP Potion");
	}

	void UseManaPotion () {
		Debug.Log("Use Mana Potion");
	}
}
