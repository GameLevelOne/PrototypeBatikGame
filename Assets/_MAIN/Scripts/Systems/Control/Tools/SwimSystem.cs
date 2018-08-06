using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class SwimSystem : ComponentSystem {
	public struct SwimData {
		public readonly int Length;
		public ComponentArray<Water> Water;
	}
	[InjectAttribute] SwimData swimData; 
	[InjectAttribute] ToolSystem toolSystem; 

	Water water;
	Player player;

	ToolType type;

	protected override void OnUpdate () {
		if (swimData.Length == 0) return;

		if (type == null) return;

		for (int i=0; i<swimData.Length; i++) {
			water = swimData.Water[i];
			type = toolSystem.tool.currentTool;
			
			if (water.player != null && type == ToolType.Flippers) {
				player = water.player;

				if (player.IsCanSwim) {
					// Debug.Log("Player Can Swim : " + player.playerCol.name + " & " + water.waterBoundariesCol.name);
					Physics2D.IgnoreCollision(player.playerCol, water.waterBoundariesCol);
				}
			}
		}
	}
}
