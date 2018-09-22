using UnityEngine;
using Unity.Entities;

public class ChestOpenerSystem : ComponentSystem {
	public struct ChestOpenerData {
		public readonly int Length;
		public ComponentArray<ChestOpener> ChestOpener;
	}
	[InjectAttribute] ChestOpenerData chestOpenerData;

	ChestOpener chestOpener;

	// bool isOpenerResponse = false;

	protected override void OnUpdate () {
		if (chestOpenerData.Length == 0) return;

		for (int i=0; i<chestOpenerData.Length; i++) {
			chestOpener = chestOpenerData.ChestOpener[i];

			if (chestOpener.isInteracting && !chestOpener.chest.isOpened) {
				chestOpener.player.isCanOpenChest = true;
			} else {
				chestOpener.player.isCanOpenChest = false;
			}
		}
	}

	public void OpenChest () {
		chestOpener.chest.isOpened = true;
		chestOpener.player.isCanOpenChest = false;
		chestOpener.chest.chestAnimator.Play(Constants.AnimationName.CHEST_OPEN);
	}

	public void SpawnTreasure (Vector3 playerPos) {
		ChestType type = chestOpener.chest.chestType;
		
		if (type == ChestType.TREASURE) {
			SpawnTreasureObj(chestOpener.chest.treasurePrize, playerPos);
		}
	}

    void SpawnTreasureObj (GameObject obj, Vector3 pos) {
		// Quaternion rot = Quaternion.Euler(40f, 0f, 0f);
		Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
        GameObject spawnedObj = GameObject.Instantiate(obj, pos, rot);
        spawnedObj.SetActive(true);
    }
}
