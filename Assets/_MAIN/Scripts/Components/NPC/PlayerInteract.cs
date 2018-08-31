using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	// public bool isInteractingWithNPC
	public Player player;
	
	[HeaderAttribute("Current")]
	public NPC currentNPC;
	
	public bool isCanInteractWithNPC;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == null) {
			currentNPC = col.GetComponent<NPC>();

			if (currentNPC.npcChar == NPCCharacteristic.TALKABLE) {
				// currentNPC.state = NPCState.INTERACT;
				isCanInteractWithNPC = true;
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == col.GetComponent<NPC>()) {
			isCanInteractWithNPC = false;
			player.isInteractingWithNPC = false;
			currentNPC.IsInteracting = false;
			currentNPC.player = null;
			currentNPC = null;
		}
	}
}
