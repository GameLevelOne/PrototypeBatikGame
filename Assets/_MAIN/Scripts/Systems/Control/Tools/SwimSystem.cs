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
                        flippers.input.MoveMode = 4;
                        flippers.input.SteadyMode = 4;
				        player.SetPlayerState(PlayerState.SWIM);
                    } else {
                        flippers.input.MoveMode = 0;
                        flippers.input.SteadyMode = 0;
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
