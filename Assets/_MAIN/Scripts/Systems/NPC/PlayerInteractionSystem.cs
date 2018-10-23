using UnityEngine;
using Unity.Entities;

public class PlayerInteractionSystem : ComponentSystem {
public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
	}
	[InjectAttribute] InputData inputData;
		public struct PlayerInteractData {
		public readonly int Length;
		public ComponentArray<PlayerInteract> PlayerInteract;
	}
	[InjectAttribute] PlayerInteractData playerInteractData;

	PlayerInteract playerInteract;
	Player player;
	NPC currentNPC;
	PlayerInput playerInput;

	// bool isCanInteractWithNPC;

	protected override void OnUpdate () {
		if (playerInteractData.Length == 0) return;

		for (int i=0; i<playerInteractData.Length; i++) {
			playerInteract = playerInteractData.PlayerInteract[i];
			for (int j=0;j<inputData.Length;j++) {
				playerInput = inputData.PlayerInput[j];
			}
			
			player = playerInteract.player;			
			currentNPC = playerInteract.currentNPC;
			// isCanInteractWithNPC = playerInteract.isCanInteractWithNPC;

			CheckIfPlayerIsCanInteract ();
		}
	}

	void CheckIfPlayerIsCanInteract () {
		if (!player.isCanInteractWithNPC) return;
		
		if (GameInput.IsActionPressed && !playerInput.isUIOpen) {
			if (!player.isInteractingWithNPC) {
				player.isInteractingWithNPC = true;
				currentNPC.player = playerInteract.player;
				currentNPC.state = NPCState.INTERACT;
				 // Debug.Log("Set "+currentNPC.gameObject.name + " to interact");
			} else {
				//
			}
		}
	}
}
