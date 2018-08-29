using UnityEngine;

public enum NPCCharacteristic {
	UNTALKABLE,
	TALKABLE
}

public enum NPCState {
	IDLE,
	INTERACTING
}

public class NPC : MonoBehaviour {
	public NPCCharacteristic npcChar;
	public NPCState state;

	// public bool isInitInteraction;
	public bool isInteracting;
}
