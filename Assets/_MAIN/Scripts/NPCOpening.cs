using UnityEngine;
using Unity.Entities;

public class NPCOpening : MonoBehaviour {
	public delegate void NPCOpeningEvent();
	public event NPCOpeningEvent OnNPCEndDialogue;

	public TimelineEventTrigger[] openingDialogueTrigger;
	public TimelineEventTrigger endOpeningCutscene;
	public GameObjectEntity entity;
	public NPC npc;
	public Animator animator;

	void OnEnable () {
		for (int i=0;i<openingDialogueTrigger.Length;i++) {
			openingDialogueTrigger[i].OnNPCStartDialogue += OnNPCStartDialogue;
		}
		endOpeningCutscene.OnEndOpeningMKF += EndOpeningScene;
	}

	void OnDisable () {
		for (int i=0;i<openingDialogueTrigger.Length;i++) {
			openingDialogueTrigger[i].OnNPCStartDialogue -= OnNPCStartDialogue;
		}
		endOpeningCutscene.OnEndOpeningMKF -= EndOpeningScene;
	}

	void OnNPCStartDialogue (string[] dialogList,double startTime,double endTime) {
		entity.enabled = true;
		npc.dialog.openingDialogs = dialogList;
		npc.state = NPCState.INTERACT;
		npc.IsInteracting = true;

		// Debug.Log("Finish set opening");
	}

	public void EndOpeningDialogue () {
		npc.state = NPCState.IDLE;
		npc.IsInteracting = false;
		npc.dialog.dialogIndex = 0;
		npc.dialog.letterIndex = 0;
		npc.dialog.dialogDeltaTime = 0f;
		npc.dialog.dialogState = DialogState.IDLE;

		npc.InteractIndex = 0;

		if (OnNPCEndDialogue != null) {
			OnNPCEndDialogue ();
		}
	}

	public void EndOpeningScene() {
		npc.npcType = NPCType.SHOP;
		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_X, 1);
		animator.SetFloat(Constants.AnimatorParameter.Float.FACE_Y, 0);
	}
}
