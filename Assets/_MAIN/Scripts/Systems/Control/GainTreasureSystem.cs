using UnityEngine;
using Unity.Entities;

public class GainTreasureSystem : ComponentSystem {
	public struct GainTreasureData {
		public readonly int Length;
		public ComponentArray<GainTreasure> GainTreasure;
	}
	public struct UINotifData {
		public readonly int Length;
		public ComponentArray<UINotif> uiNotif;

	}
	[InjectAttribute] UINotifData uiNotifData;
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
			GameStorage.Instance.PlayBGM(BGMType.GAIN_TREASURE,false);

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
		UINotif uiNotif = null;
		for (int i=0;i<uiNotifData.Length;i++) {
			uiNotif = uiNotifData.uiNotif[i];
		}
		if (uiNotif==null) {
			Debug.LogWarning("Missing Notification Window");
		}
		
		GameStorage.Instance.PlayBGM(BGMType.MAIN,false);

		switch (lootable.treasureType) { //TEMP
			case TreasureType.FISH: 
				lootable.initSprite.SetActive(false);
				lootable.mainSprite.SetActive(true);
				string fishSize = "SMALL";
				if (lootable.lootQuantity==5) 
					fishSize = "MEDIUM";
				else if (lootable.lootQuantity==25) 
					fishSize = "LARGE";

				//SHOW NOTIFICATION
				uiNotif.TextToShow = "YOU GOT "+fishSize+" FISH!";
				uiNotif.call = true;
				break;
			case TreasureType.POWERARROW: 
				if (gainTreasure.questTrigger != null) {
					//SEND QUEST TRIGGER
					gainTreasure.questTrigger.isDoQuest = true;
				} else {
					Debug.Log("No Quest Triggered on GainTreasureSystem.UseTreasure POWERARROW");
				}

				//SHOW NOTIFICATION
				uiNotif.TextToShow = "YOU GOT FIRE ARROW!";
				uiNotif.call = true;

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

				//SHOW NOTIFICATION
				uiNotif.TextToShow = "YOU GOT FISHING ROD!";
				uiNotif.call = true;


				toolSystem.tool.FishingRod = 1;
				ResetTool();
				Debug.Log("RESET TOOL after got FISHINGROD");
				break;
			case TreasureType.KEY: 
				//SHOW NOTIFICATION
				uiNotif.TextToShow = "YOU GOT A	KEY!";
				uiNotif.call = true;

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
