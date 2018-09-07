using UnityEngine;
using Unity.Entities;

public class NPCOpening : MonoBehaviour {
	public delegate void NPCOpeningEvent();
	public event NPCOpeningEvent OnNPCEndDialogueTrigger;

	public TimelineEventTrigger openingDialogueTrigger;
	public GameObjectEntity entity;
	public NPC npc;

	void OnEnable () {
		openingDialogueTrigger.OnNPCDialogueTrigger += OnNPCDialogueTrigger;
	}

	void OnDisable () {
		openingDialogueTrigger.OnNPCDialogueTrigger -= OnNPCDialogueTrigger;
	}

	void OnNPCDialogueTrigger () {
		entity.enabled = true;
		npc.state = NPCState.INTERACT;
		npc.IsInteracting = true;

		Debug.Log("Finish set opening");
	}

	public void EndOpeningDialogue () {
		npc.state = NPCState.IDLE;
		npc.IsInteracting = false;

		if (OnNPCEndDialogueTrigger != null) {
			OnNPCEndDialogueTrigger ();
		}
	}
}
