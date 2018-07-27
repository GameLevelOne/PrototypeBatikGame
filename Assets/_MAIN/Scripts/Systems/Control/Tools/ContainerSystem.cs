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

	CollectibleType[] collectibleTypes;

	protected override void OnUpdate () {
		if (containerData.Length == 0) return;

		for (int i=0; i<containerData.Length; i++) {
			container = containerData.Container[i];

			collectibleTypes = container.collectibleTypes;
		}
	}

	public void SaveToContainer (Collectible collectible) {
		for (int i=0; i<collectibleTypes.Length; i++) {
			if (container.CheckIfContainerIsEmpty(i)) {
				collectibleTypes[i] = collectible.collectibleType;

				break;
			} else {
				Debug.Log(collectibleTypes[i] + " is failed to contain, there is no empty container");
			}
		}
	}

	public void UseCollectibleInContainer (int collectibleTypeIdx) {
		if (container.CheckIfContainerIsEmpty(collectibleTypeIdx)) {
			int idx = collectibleTypeIdx + 1;
			Debug.Log("Container " + idx + " is empty");
		} else {
			switch(collectibleTypes[collectibleTypeIdx]) {
				case CollectibleType.NONE: 
					//
					ReportContainerIsEmpty();
					break;
				case CollectibleType.GEM_STONE: 
					//
					UseGemStone();
					break;
				case CollectibleType.SACRED_STONE: 
					//
					UseSacredStone();
					break;
				case CollectibleType.MYSTICAL_STONE: 
					//
					UseMysticalStone();
					break;
				case CollectibleType.HP_POTION: 
					//
					UseHPPotion();
					break;
				case CollectibleType.MANA_POTION: 
					//
					UseManaPotion();
					break;
			}

			collectibleTypes[collectibleTypeIdx] = CollectibleType.NONE;
		}
	}

	void ReportContainerIsEmpty () {
		Debug.Log("This container is empty");
	}

	void UseGemStone () {
		Debug.Log("Use Gem Stone");
	}

	void UseSacredStone () {
		Debug.Log("Use Sacred Stone");
	}

	void UseMysticalStone () {
		Debug.Log("Use Mystical Stone");
	}

	void UseHPPotion () {
		Debug.Log("Use HP Potion");
	}

	void UseManaPotion () {
		Debug.Log("Use Mana Potion");
	}
}
