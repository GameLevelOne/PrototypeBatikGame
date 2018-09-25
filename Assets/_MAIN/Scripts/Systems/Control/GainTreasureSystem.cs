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

	protected override void OnUpdate () {
		if (gainTreasureData.Length == 0) return;

		if (input == null) {
			input = playerInputSystem.input;

			return;
		}

		for (int i=0; i<gainTreasureData.Length; i++) {
			gainTreasure = gainTreasureData.GainTreasure[i];
			
			if (!gainTreasure.isInitGainTreasure) {
				InitGainTreasureSystem();
			} else {
				player = gainTreasure.player;
				state = player.state;

				CheckLootedItem();
			}
		}
	}

	void InitGainTreasureSystem () {
		//
		gainTreasure.isInitGainTreasure = true;
	}

	void CheckLootedItem () {
		if (gainTreasure.isLootingStandard) {
			UseAndDestroyTreasure();
			
			gainTreasure.isLootingStandard = false;
		} else if (gainTreasure.isLootingTreasure) {
			SetLiftObjectParent (gainTreasure.lootableTransform);
			// input.interactValue = 0;
			input.interactValue = 1; //NO LIFTING TREASURE
			input.interactMode = 6;
			player.SetPlayerState(PlayerState.GET_TREASURE);
			
			gainTreasure.isLootingTreasure = false;
		}
	}

	void SetLiftObjectParent (Transform treasureTransform) {
		treasureTransform.parent = gainTreasure.liftingTreasureParent;
		treasureTransform.localPosition = Vector2.zero;
	}

	public void UseTreasure () {
		Lootable lootable = gainTreasure.lootable;
		
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
					Debug.Log("No Quest Triggered on GainTreasureSystem.UseTreasure POWERARROW");
				}

				toolSystem.tool.Bow = 1;
				ResetTool();
				Debug.Log("RESET TOOL after got FIREARROW"); 
				break;
			case TreasureType.FISHINGROD: 
				if (gainTreasure.questTrigger != null) {
					//SEND QUEST TRIGGER
					gainTreasure.questTrigger.isDoQuest = true;
				} else {
					Debug.Log("No Quest Triggered on GainTreasureSystem.UseTreasure FISHINGROD");
				}

				toolSystem.tool.FishingRod = 1;
				ResetTool();
				Debug.Log("RESET TOOL after got FISHINGROD");
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
		gainTreasure.lootable.isLooted = true;
	}

	void ResetTool () {
		toolSystem.tool.isInitCurrentTool = false;
		uiToolsSelectionSystem.uiToolsSelection.isInitToolImage = false;
	}
}
