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
	NPCType currentType;

	protected override void OnUpdate () {
		if (npcData.Length == 0) return;

		for (int i=0; i<npcData.Length; i++) {
			currentNPC = npcData.NPC[i];
			
			currentState = currentNPC.state;

			if (!currentNPC.isDoneInitNPC) {
				InitNPC ();
			} else {
				currentType = currentNPC.npcType;

				if (currentType != NPCType.NONE) {
					CheckNPCInteraction ();
				}
			}
		}
	}

	void InitNPC () {
		//
		currentNPC.isDoneInitNPC = true;
	}

	void CheckNPCInteraction () {
		if (currentNPC.IsInteracting) return;
		
		if (currentState == NPCState.INTERACT && currentNPC.player != null) {
			currentNPC.IsInteracting = true;
			 // Debug.Log(currentNPC.gameObject.name + "is interacting");
		} else {
			if (currentState != NPCState.IDLE) {
				currentNPC.state = NPCState.IDLE;
			}
		}
	}
}
