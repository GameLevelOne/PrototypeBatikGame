using UnityEngine;

public enum NPCCharacteristic {
	UNTALKABLE,
	TALKABLE
}

public enum NPCState {
	IDLE,
	INTERACT
}

public class NPC : MonoBehaviour {
	public NPCCharacteristic npcChar;
	public NPCState state;

	public Dialog dialog;

	[SerializeField] bool isInteracting; //TEMP Public
	
	public bool IsInteracting {
		get {return isInteracting;}
		set {
			if (isInteracting == value) return;
			else isInteracting = value;

			if (isInteracting) {
				dialog.isInitShowingDialog = false;
				dialog.isShowingDialog = false;
				dialog.dialogTime = dialog.showDialogDelay;
				dialog.showDialogTime = 0f;
				Debug.Log("OK");
			}
		}
	}
}
