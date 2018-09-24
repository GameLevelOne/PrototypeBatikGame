using UnityEngine;
using Unity.Entities;

public class GainTreasureSystem : ComponentSystem {
	public struct GainTreasureData {
		public readonly int Length;
		public ComponentArray<GainTreasure> GainTreasure;
	}
	[InjectAttribute] GainTreasureData gainTreasureData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	GainTreasure gainTreasure;
	PlayerInput input;
	PlayerState state;
	Player player;
	Lootable lootable;

	protected override void OnUpdate () {
		if (gainTreasureData.Length == 0) return;

		if (input == null) {
			input = playerInputSystem.input;

			return;
		}

		for (int i=0; i<gainTreasureData.Length; i++) {
			gainTreasure = gainTreasureData.GainTreasure[i];
			player = gainTreasure.player;
			state = player.state;

			if (gainTreasure.isLooting && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
				lootable = gainTreasure.lootable;

				switch (lootable.treasureType) {
					case TreasureType.NONE: 
						UseAndDestroyTreasure();
						break;
					default: //TEMP
						SetLiftObjectParent (gainTreasure.lootableTransform);
						// input.interactValue = 0;
						input.interactValue = 1; //NO LIFTING TREASURE
						input.interactMode = 6;
						player.SetPlayerState(PlayerState.GET_TREASURE);
						break;
				}

				gainTreasure.isLooting = false;
			}
		}
	}

	void SetLiftObjectParent (Transform treasureTransform) {
		treasureTransform.parent = gainTreasure.liftingTreasureParent;
		treasureTransform.localPosition = Vector2.zero;
	}

	public void UseTreasure () {
		switch (lootable.treasureType) { //TEMP
			case TreasureType.FISH: 
				lootable.initSprite.SetActive(false);
				lootable.mainSprite.SetActive(true);
				break;
			case TreasureType.POWERARROW: 
				if (gainTreasure.questTrigger != null) {
					//SEND QUEST TRIGGER
					gainTreasure.questTrigger.isDoQuest = true;
				} else {
					Debug.Log("No Quest Triggered");
				}

				toolSystem.tool.Bow = 1;
				toolSystem.tool.isInitCurrentTool = false;
				uiToolsSelectionSystem.uiToolsSelection.isInitToolImage = false;
				Debug.Log("RESET TOOL"); 
				break;
			case TreasureType.KEY: 
				PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + lootable.keyID, 1);
				break;
			default:
				//
				break;
		}
	}

	public void UseAndDestroyTreasure () {
		//PROCESS LOOTABLE ITEM
		int lootQTY = 0;
		LootableType lootableType = lootable.lootableType;

		if (lootableType != LootableType.NONE) {
			lootQTY = lootable.lootQuantity;
			Debug.Log("You got "+lootQTY+" "+lootableType);
			switch (lootableType) { //TEMP
				case LootableType.GOLD: 
					GameStorage.Instance.PlayerCoin += lootQTY;
					break;
				case LootableType.HP_POTION: 
					player.health.PlayerHP += lootQTY;
					break;
				case LootableType.MANA_POTION: 
					player.mana.PlayerMP += lootQTY;
					break;
				default:
					Debug.Log("Unknown LootableType : "+lootableType);
					break;
			}
		}

		//DESTROY LOOTABLE ITEM
		GameObject.Destroy(gainTreasure.lootableTransform.gameObject);
		UpdateInjectedComponentGroups();
	}
}
