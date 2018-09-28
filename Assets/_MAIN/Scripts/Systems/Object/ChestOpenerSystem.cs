using UnityEngine;
using Unity.Entities;

public class ChestOpenerSystem : ComponentSystem {
	public struct ChestOpenerData {
		public readonly int Length;
		public ComponentArray<ChestOpener> ChestOpener;
	}
	[InjectAttribute] ChestOpenerData chestOpenerData;

	ChestOpener chestOpener;

	public struct ChestData {
		public readonly int Length;
		public ComponentArray<Chest> Chest;
	}
	[InjectAttribute] ChestData chestData;

	Chest chest;

	protected override void OnUpdate () {
		// if (chestOpenerData.Length == 0) return;
		for (int i=0; i<chestData.Length; i++) {
			chest = chestData.Chest[i];

			if (!chest.isInitChest) {
				InitChest ();
			}
		}

		for (int i=0; i<chestOpenerData.Length; i++) {
			chestOpener = chestOpenerData.ChestOpener[i];

			if (chestOpener.isInteracting) {
				if (chestOpener.chest.isInitChest && !chestOpener.chest.isOpened) {
					chestOpener.player.isCanOpenChest = true;
				} else {
					chestOpener.player.isCanOpenChest = false;
				}
			}
		}
	}

	void InitChest () {
		//LOAD CHEST STATE
		chest.isOpened = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + chest.chestID, 0) == 1 ? true : false;

		//SET CHEST SPRITE BY SAVED CHEST STATE
		chest.chestSpriteRen.sprite = chest.isOpened ? chest.openedChestSprite : chest.closedChestSprite;

		chest.isInitChest = true;
	}

	public void OpenChest () {
		//SAVE CHEST STATE
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + chest.chestID, 1);

		chestOpener.chest.isOpened = true;
		chestOpener.chest.chestSpriteRen.sprite = chestOpener.chest.openedChestSprite;
		// chestOpener.chest.chestAnimator.Play(Constants.AnimationName.CHEST_OPEN);
		chestOpener.chest.isSelected = false;
		// chestOpener.chest = null;
		chestOpener.isInteracting = false;
		chestOpener.player.isCanOpenChest = false;

		if (chestOpener.chest.questTrigger != null) {
			//SEND QUEST TRIGGER
			chestOpener.chest.questTrigger.isDoQuest = true;
		} else {
			Debug.Log("No Quest Triggered");
		}

		if(chestOpener.chest.cutsceneTrigger != null){
			chestOpener.chest.cutsceneTrigger.gameObject.SetActive(true);
		}else{
			Debug.Log("No CutScene");
		}

	}

	public void SpawnTreasure (Vector3 playerPos) {
		ChestType type = chestOpener.chest.chestType;
		
		if (type == ChestType.TREASURE) {
			SpawnTreasureObj(chestOpener.chest.treasurePrize, playerPos);
		}

		chestOpener.chest = null;
	}

    void SpawnTreasureObj (GameObject obj, Vector3 pos) {
		// Quaternion rot = Quaternion.Euler(40f, 0f, 0f);
		Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
        GameObject spawnedObj = GameObject.Instantiate(obj, pos, rot);
        spawnedObj.SetActive(true);
    }
}
