using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	// public bool isInteractingWithNPC

	NPC currentNPC;

	void OnTriggerEnter (Collider col) {
		Debug.Log(col.name);

		if (col.tag == Constants.Tag.NPC) {
			currentNPC = col.GetComponent<NPC>();

			Debug.Log("NPC "+col.name+" is "+currentNPC.npcChar);
			if (currentNPC.npcChar == NPCCharacteristic.TALKABLE) {
				currentNPC.state = NPCState.INTERACT;
			}
		}
	}
}
