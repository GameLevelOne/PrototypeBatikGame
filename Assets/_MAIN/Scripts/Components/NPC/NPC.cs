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
	public string npcName;
	public NPCCharacteristic npcChar;
	public NPCState state;
	public Dialog dialog;

	[HeaderAttribute("Current")]
	public Player player;
	public bool isDoneInitNPC = false;
	[SerializeField] bool isInteracting; //TEMP Public
	
	[HeaderAttribute("Testing")]
	public bool isTestNPC = false;

	string savedStr;


	void Awake () { //TEMP
		if (isTestNPC) {
			InteractIndex = 0;
		} 
	}
	public int InteractIndex {
		get {
			savedStr = Constants.NPCPrefKey.NPC_INTERACT_STATE + npcName;

			return PlayerPrefs.GetInt(Constants.NPCPrefKey.NPC_INTERACT_STATE, 0);}
		set {
			savedStr = Constants.NPCPrefKey.NPC_INTERACT_STATE + npcName;

			PlayerPrefs.SetInt(Constants.NPCPrefKey.NPC_INTERACT_STATE, value);
		}
	}

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
				// Debug.Log("OK");
			}
		}
	}
}
