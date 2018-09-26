using UnityEngine;
using Unity.Entities;

public class NPCOpening : MonoBehaviour {
	public delegate void NPCOpeningEvent();
	public event NPCOpeningEvent OnNPCEndDialogue;

	public TimelineEventTrigger openingDialogueTrigger;
	public GameObjectEntity entity;
	public NPC npc;
	public Animator animator;

	void OnEnable () {
		openingDialogueTrigger.OnNPCStartDialogue += OnNPCStartDialogue;
	}

	void OnDisable () {
		openingDialogueTrigger.OnNPCStartDialogue -= OnNPCStartDialogue;
	}

	void OnNPCStartDialogue () {
		entity.enabled = true;
		npc.state = NPCState.INTERACT;
		npc.IsInteracting = true;

		// Debug.Log("Finish set opening");
	}

	public void EndOpeningDialogue () {
		npc.state = NPCState.IDLE;
		npc.IsInteracting = false;
		npc.npcType = NPCType.SHOP;
		npc.dialog.dialogIndex = 0;
		npc.dialog.letterIndex = 0;
		npc.dialog.dialogDeltaTime = 0f;
		npc.dialog.dialogState = DialogState.IDLE;

		npc.InteractIndex = 0;
		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, 1);
		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, 0);

		if (OnNPCEndDialogue != null) {
			OnNPCEndDialogue ();
		}
	}
}
