using UnityEngine;
using Unity.Entities;


public class ContainerSystem : ComponentSystem {
	public struct ContainerData {
		public readonly int Length;
		public ComponentArray<Container> Container;
	}
	[InjectAttribute] ContainerData containerData;
	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	public Container container;

	LootableType[] lootableTypes;

	protected override void OnUpdate () {
		if (containerData.Length == 0) return;

		for (int i=0; i<containerData.Length; i++) {
			container = containerData.Container[i];
			
			if (!container.isInitContainer) {
				InitContainer();
			}

			// if (container.isProcessingBuyItem) {
			// 	ProcessCoin (container.boughtItemLootabelType, container.boughtItemPrice);		
			// }
		}
	}

	void InitContainer () {
		lootableTypes = container.lootableTypes;

		//LOAD CONTAINER : 0 NONE, 1 HP, 2 MP
		for (int i=0; i<lootableTypes.Length; i++) {
			int containerAvailability = PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 0);

			switch (containerAvailability) {
				case 1:
					container.lootableTypes[i] = LootableType.HP_POTION;
					break;
				case 2:
					container.lootableTypes[i] = LootableType.MANA_POTION;
					break;
				default:
					container.lootableTypes[i] = LootableType.NONE;
					break;
			}
		}

		container.isInitContainer = true;
	}

	// void ProcessCoin (LootableType type, int price) {
	// 	Debug.Log("Your have "+GameStorage.Instance.PlayerCoin+" coins");
	// 	if (price <= GameStorage.Instance.PlayerCoin) {
	// 		if (SaveToContainer (type)) {
	// 			Debug.Log("You have buy item "+type+" for "+price+" coins");
	// 			GameStorage.Instance.PlayerCoin -= price;
	// 			Debug.Log("Your remaining coins "+GameStorage.Instance.PlayerCoin);
	// 		} else {
	// 			//No Empty Container
	// 		}
	// 	} else {
	// 		Debug.Log("You have not enough coins");
	// 	}

	// 	container.isProcessingBuyItem = false;
	// }

	public bool SaveToContainer (LootableType currentLootableType) {
		for (int i=0; i<lootableTypes.Length; i++) {
			if (container.CheckIfContainerIsEmpty(i)) {
				lootableTypes[i] = currentLootableType;

				//SAVE CONTAINER
				// switch (currentLootableType) {
				// 	case LootableType.HP_POTION:
				// 		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 1);
				// 		break;
				// 	case LootableType.MANA_POTION:
				// 		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 2);
				// 		break;
				// 	default:
				// 		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 0);
				// 		break;
				// }

				Debug.Log(lootableTypes[i] + " is contained in container "+i);
				ResetTool();

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
				// case LootableType.GOLD: 
				// 	//
				// 	UseGold();
				// 	break;
				case LootableType.HP_POTION: 
					//
					UseHPPotion(lootableTypeIdx);
					ResetTool();
					break;
				case LootableType.MANA_POTION: 
					//
					UseManaPotion(lootableTypeIdx);
					ResetTool();
					break;
				default: 
					Debug.Log("Unknown item in container");
					break;
			}
		}
	}

	void ReportContainerIsEmpty () {
		Debug.Log("This container is empty");
	}

	// void UseGold () {
	// 	Debug.Log("Use Gold");
	// }

	void UseHPPotion (int lootableTypeIdx) {
		Debug.Log("Use HP Potion");
		container.player.health.PlayerHP += container.player.MaxHP;
		
		lootableTypes[lootableTypeIdx] = LootableType.NONE;
		// PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + lootableTypeIdx, 0);
	}

	void UseManaPotion (int lootableTypeIdx) {
		Debug.Log("Use Mana Potion");
		container.player.mana.PlayerMP += container.player.MaxMP;
		
		lootableTypes[lootableTypeIdx] = LootableType.NONE;
		// PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + lootableTypeIdx, 0);
	}

	void ResetTool() {
		// toolSystem.tool.isInitCurrentTool = false;
		uiToolsSelectionSystem.InitImages(true);
		Debug.Log("RESET TOOL"); 
	}
}
