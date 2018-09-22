using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	// public bool isInteractingWithNPC
	public Player player;
	
	[HeaderAttribute("Current")]
	public NPC currentNPC;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == null) {
			currentNPC = col.GetComponent<NPC>();

			if (currentNPC.npcChar == NPCCharacteristic.TALKABLE) {
				// currentNPC.state = NPCState.INTERACT;
				player.isCanInteractWithNPC = true;
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == col.GetComponent<NPC>()) {
			player.isCanInteractWithNPC = false;
			player.isInteractingWithNPC = false;
			currentNPC.IsInteracting = false;
			currentNPC.player = null;
			currentNPC = null;
		}
	}
}
