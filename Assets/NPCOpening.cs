using UnityEngine;
using Unity.Entities;

public class NPCOpening : MonoBehaviour {
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
}
