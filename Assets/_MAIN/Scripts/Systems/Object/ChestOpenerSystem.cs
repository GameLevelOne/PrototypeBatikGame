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
				//PLAYER.ISCANOPENCHEST
			}
		}
	}

	public void OpenChest () { //CALL BY PLAYERINPUTSYSTEM
		//SET CHEST TO BE OPENED
		//PLAY CHEST OPEN ANIMATION
	}
}
