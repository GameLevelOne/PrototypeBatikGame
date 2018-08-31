using UnityEngine;
using Unity.Entities;

public class SwimSystem : ComponentSystem {
	public struct SwimData {
		public readonly int Length;
		public ComponentArray<Flippers> Flippers;
	}
	[InjectAttribute] SwimData swimData; 

	public Flippers flippers;

	Player player;
	PlayerInput input;

	public bool isAlreadySwimming = false;

	protected override void OnUpdate () {
		if (swimData.Length == 0) return;

		for (int i=0; i<swimData.Length; i++) {
			flippers = swimData.Flippers[i];
			
            if (flippers.isPlayerOnWater) {
                Collider waterBoundariesCol = flippers.waterBoundariesCol;
                player = flippers.player;
				input = flippers.input;

                if (flippers.isEquipped) {
					Physics.IgnoreCollision(player.playerCol, waterBoundariesCol, true);

                    if (flippers.isPlayerSwimming && input.interactValue == 0) {
				        player.SetPlayerState(PlayerState.SWIM);
                    }
				} else {
                    if (waterBoundariesCol != null) {
					    Physics.IgnoreCollision(player.playerCol, waterBoundariesCol, false);
                    }
                }
            }
		}
	}
}
