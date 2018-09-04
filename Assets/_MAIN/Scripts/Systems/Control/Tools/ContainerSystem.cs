using UnityEngine;
using Unity.Entities;


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

			if (container.isProcessingBuyItem) {
				ProcessCoin (container.boughtItemLootabelType, container.boughtItemPrice);		
			}
		}
	}

	void ProcessCoin (LootableType type, int price) {
		Debug.Log("Your have "+GameStorage.Instance.PlayerCoin+" coins");
		if (price <= GameStorage.Instance.PlayerCoin) {
			if (SaveToContainer (type)) {
				Debug.Log("You have buy item "+type+" for "+price+" coins");
				GameStorage.Instance.PlayerCoin -= price;
				Debug.Log("Your remaining coins "+GameStorage.Instance.PlayerCoin);
			} else {
				//No Empty Container
			}
		} else {
			Debug.Log("You have not enough coins");
		}

		container.isProcessingBuyItem = false;
	}

	public bool SaveToContainer (LootableType currentLootableType) {
		for (int i=0; i<lootableTypes.Length; i++) {
			if (container.CheckIfContainerIsEmpty(i)) {
				lootableTypes[i] = currentLootableType;

				Debug.Log(lootableTypes[i] + " is contained in container "+i);
				return true;
			} else {
				// return false;
			}
		}

		Debug.Log("There is no empty container");
		return false; //
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
