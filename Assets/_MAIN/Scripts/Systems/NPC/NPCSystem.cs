using UnityEngine;
using Unity.Entities;

public class NPCSystem : ComponentSystem {
	public struct NPCData {
		public readonly int Length;
		public ComponentArray<NPC> NPC;
	}
	[InjectAttribute] NPCData npcData;

	NPC currentNPC;
	
	NPCState currentState;

	protected override void OnUpdate () {
		if (npcData.Length == 0) return;

		for (int i=0; i<npcData.Length; i++) {
			currentNPC = npcData.NPC[i];

			currentState = currentNPC.state;

			CheckNPCInteraction ();
		}
	}

	void CheckNPCInteraction () {
		if (currentState == NPCState.INTERACT) {
			if (!currentNPC.IsInteracting) {
				Debug.Log(currentNPC.gameObject.name + "is interacting");
				currentNPC.IsInteracting = true;
			}
		}
	}
}
