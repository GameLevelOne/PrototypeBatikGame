using UnityEngine;
using Unity.Entities;

public class EndGameTriggerSystem : ComponentSystem {
	public struct EndGameTriggerData {
		public readonly int Length;
		public ComponentArray<EndGameTrigger> EndGameTrigger;
	}
	[InjectAttribute] EndGameTriggerData endgGameTriggerData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	EndGameTrigger endGameTrigger;

	protected override void OnUpdate () {
		for (int i=0; i<endgGameTriggerData.Length; i++) {
			endGameTrigger = endgGameTriggerData.EndGameTrigger[i];

			if (!endGameTrigger.isInitEndGame) {
				InitEndGame();
			} else if (endGameTrigger.isTriggered) {
				//PLAY END GAME
				endGameTrigger.UIEndGame.call = true;
				playerInputSystem.SetDir(0f,1f);
				// playerInputSystem.ChangeDir(0f, 1f);
				// playerInputSystem.CheckLockDir(2, 1, 3);
		
				endGameTrigger.triggerCol.enabled = false;

				endGameTrigger.isTriggered = false;
			}
		}
	}

	void InitEndGame () {

	}

}
