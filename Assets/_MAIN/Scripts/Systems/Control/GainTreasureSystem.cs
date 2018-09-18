using UnityEngine;
using Unity.Entities;

public class GainTreasureSystem : ComponentSystem {
	public struct GainTreasureData {
		public readonly int Length;
		public ComponentArray<GainTreasure> GainTreasure;
	}
	[InjectAttribute] GainTreasureData gainTreasureData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

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
			default:
				//
				break;
		}
	}

	public void UseAndDestroyTreasure () {
		//PROCESS LOOTABLE ITEM

		GameObject.Destroy(gainTreasure.lootableTransform.gameObject);
	}
}
