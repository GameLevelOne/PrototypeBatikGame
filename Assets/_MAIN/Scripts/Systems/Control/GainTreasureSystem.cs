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
				SetLiftObjectParent (gainTreasure.lootableTransform);
				input.interactValue = 0;
				input.interactMode = 6;
				player.SetPlayerState(PlayerState.GET_TREASURE);

				gainTreasure.isLooting = false;
			}
		}
	}

	void SetLiftObjectParent (Transform treasureTransform) {
		treasureTransform.parent = gainTreasure.liftingTreasureParent;
		treasureTransform.localPosition = Vector2.zero;
	}

	public void UseAndDestroyTreasure () {
		GameObject.Destroy(gainTreasure.lootableTransform.gameObject);
	}
}
