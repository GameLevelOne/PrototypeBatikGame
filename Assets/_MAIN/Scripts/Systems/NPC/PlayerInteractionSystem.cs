using UnityEngine;
using Unity.Entities;

public class PlayerInteractionSystem : ComponentSystem {
	public struct PlayerInteractData {
		public readonly int Length;
		public ComponentArray<PlayerInteract> PlayerInteract;
	}
	[InjectAttribute] PlayerInteractData playerInteractData;

	PlayerInteract playerInteract;
	Player player;
	NPC currentNPC;

	// bool isCanInteractWithNPC;

	protected override void OnUpdate () {
		if (playerInteractData.Length == 0) return;

		for (int i=0; i<playerInteractData.Length; i++) {
			playerInteract = playerInteractData.PlayerInteract[i];
			
			player = playerInteract.player;			
			currentNPC = playerInteract.currentNPC;
			// isCanInteractWithNPC = playerInteract.isCanInteractWithNPC;

			CheckIfPlayerIsCanInteract ();
		}
	}

	void CheckIfPlayerIsCanInteract () {
		if (!playerInteract.isCanInteractWithNPC) return;

		if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
			if (!player.isInteractingWithNPC) {
				player.isInteractingWithNPC = true;
				currentNPC.player = playerInteract.player;
				currentNPC.state = NPCState.INTERACT;
				Debug.Log("Set "+currentNPC.gameObject.name + " to interact");
			} else {
				//
			}
		}
	}
}
