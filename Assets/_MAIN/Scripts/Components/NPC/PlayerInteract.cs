using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	// public bool isInteractingWithNPC
	public Player player;
	
	[HeaderAttribute("Current")]
	public NPC currentNPC;
	
	[SerializeField] bool isCanInteractWithNPC;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == null) {
			currentNPC = col.GetComponent<NPC>();
			Debug.Log("NPC "+col.name+" is "+currentNPC.npcChar);

			if (currentNPC.npcChar == NPCCharacteristic.TALKABLE) {
				// currentNPC.state = NPCState.INTERACT;
				isCanInteractWithNPC = true;
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.NPC && currentNPC == col.GetComponent<NPC>()) {
			currentNPC = null;
			isCanInteractWithNPC = false;
		}
	}
}
