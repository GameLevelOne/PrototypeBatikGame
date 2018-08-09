using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class SwimSystem : ComponentSystem {
	public struct SwimData {
		public readonly int Length;
		public ComponentArray<Flippers> Flippers;
	}
	[InjectAttribute] SwimData swimData; 
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	public Flippers flippers;

	Player player;


	protected override void OnUpdate () {
		if (swimData.Length == 0) return;

		for (int i=0; i<swimData.Length; i++) {
			flippers = swimData.Flippers[i];
			
            if (flippers.IsPlayerOnWater) {
                Collider2D waterBoundariesCol = flippers.waterBoundariesCol;
                player = flippers.player;

                if (flippers.IsEquipped) {
					Physics2D.IgnoreCollision(player.playerCol, waterBoundariesCol, true);

                    if (flippers.IsPlayerSwimming) {
				        player.SetPlayerState(PlayerState.SWIM);
                    } else {
                        // 
                    }
				} else {
                    if (waterBoundariesCol != null) {
					    Physics2D.IgnoreCollision(player.playerCol, waterBoundariesCol, false);
                    }
                }
            }
		}
	}
}
