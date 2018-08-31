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

			if (!currentNPC.isDoneInitNPC) {
				InitNPC ();
				currentNPC.isDoneInitNPC = true;
			} else {
				CheckNPCInteraction ();
			}
		}
	}

	void InitNPC () {
		//
	}

	void CheckNPCInteraction () {
		if (currentNPC.IsInteracting) return;
		
		if (currentState == NPCState.INTERACT && currentNPC.player != null) {
			currentNPC.IsInteracting = true;
			Debug.Log(currentNPC.gameObject.name + "is interacting");
		} else {
			//
		}
	}
}
