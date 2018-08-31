using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class NPCDialogSystem : ComponentSystem {
	public struct DialogData {
		public readonly int Length;
		public ComponentArray<NPC> NPC;
		public ComponentArray<Dialog> Dialog;
	}
	[InjectAttribute] DialogData dialogData;

	NPC currentNPC;

	Dialog currentDialog;
	
	NPCState currentState;

	float showDialogTime;
	float showDialogDuration;
	float showDialogDelay;
	float deltaTime;
	float timer;

	int letterIndex;

	bool isInitShowingDialog;
	bool isShowingDialog;
	bool isFinishShowingDialog;
	
	// string testStr = "Hello World";
	// string blackColor = "000000ff";
	string transparentColor = "00000000";
	string[] tag = new string[]{"<color=#", ">", "</color>"};
	char[] tempChar;
	string openingTag;

	protected override void OnUpdate () {
		if (dialogData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<dialogData.Length; i++) {
			currentNPC = dialogData.NPC[i];
			currentDialog = dialogData.Dialog[i];

			currentState = currentNPC.state;
			showDialogDuration = currentDialog.showDialogDuration;
			showDialogDelay = currentDialog.showDialogDelay;
			isFinishShowingDialog = currentDialog.isFinishShowingDialog;
			isInitShowingDialog = currentDialog.isInitShowingDialog;
			isShowingDialog = currentDialog.isShowingDialog;
			letterIndex = currentDialog.letterIndex;
			// dialogIndex = currentDialog.dialogIndex;
			showDialogTime = currentDialog.showDialogTime;
			timer = currentDialog.dialogTime;

			if (!isInitShowingDialog) {
				InitDialog ();
			} else {
				CheckNPCState ();
			}
		}
	}

	void InitDialog () {
		currentDialog.panelDialog.SetActive(false);

		currentDialog.letterIndex = 0;
		currentDialog.isInitShowingDialog = true;
	}

	void CheckNPCState () {
		switch (currentState) {
			case NPCState.IDLE:
				if (timer < showDialogDelay) {
					currentDialog.dialogTime += deltaTime;
				} else {
					CheckNPCIdleDialog ();
				}

				break;
			case NPCState.INTERACT:
					CheckNPCInteractDialog ();
				break;
		}
	}

	void CheckNPCIdleDialog () {
		if (!isShowingDialog) {
			currentDialog.dialogIndex = Random.Range(0, currentDialog.idleDialogs.Length);
			SetList (GetDialogStringType(currentState, currentDialog.dialogIndex));			
			ShowDialog ();
		} else if (!isFinishShowingDialog) {
			PrintLetterOneByOne ();
		} else {
			if (showDialogTime < showDialogDuration) {
				currentDialog.showDialogTime += deltaTime;
			} else {
				HideDialog ();
			}
		}
	}

	void CheckNPCInteractDialog () {
		if (!isShowingDialog) {
			// Debug.Log(currentNPC.InteractIndex);
			currentDialog.dialogIndex = currentNPC.InteractIndex;
			SetList (GetDialogStringType(currentState, currentDialog.dialogIndex));			
			ShowDialog ();
		} else if (!isFinishShowingDialog) {
			PrintLetterOneByOne ();

			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
				currentDialog.textDialog.text = GetDialogStringType(currentState, currentDialog.dialogIndex) + openingTag + tag[2];
				currentDialog.isFinishShowingDialog = true;
			}
		} else {
			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
				HideDialog ();

				if (currentNPC.InteractIndex < currentDialog.interactDialogs.Length-1) {
					currentNPC.InteractIndex++;
				}
			}
			// if (showDialogTime < showDialogDuration) {
			// 	currentDialog.showDialogTime += deltaTime;
			// } else {
			// 	HideDialog ();
			// }
		}
	}

	// void GetInteractIndexType () {

	// }

	string GetDialogStringType (NPCState state, int dialogIdx) {
		switch (state) {
			case NPCState.IDLE:
				return currentDialog.idleDialogs[dialogIdx];
			case NPCState.INTERACT:
				return currentDialog.interactDialogs[dialogIdx];
			default:
				return "NOTHING";
		}
	}

	void SetList (string strDialog) {
		tempChar = strDialog.ToCharArray();

		openingTag = tag[0] + transparentColor + tag[1];
		currentDialog.textDialog.text = openingTag + strDialog + tag[2];

		//Set List
		currentDialog.letterList = new List<string>();
		currentDialog.letterList.Add(openingTag);
		for (int i=0; i<tempChar.Length; i++) {
			currentDialog.letterList.Add(tempChar[i].ToString());
		} 
		currentDialog.letterList.Add(tag[2]);
	}

	void ShowDialog () {
		Debug.Log(currentNPC.gameObject.name + "is showing currentDialog");
		currentDialog.panelDialog.SetActive (true);
		currentDialog.isFinishShowingDialog = false;

		currentDialog.isShowingDialog = true;
	}

	void HideDialog () {
		Debug.Log(currentNPC.gameObject.name + "is hiding currentDialog");
		currentDialog.panelDialog.SetActive (false);
		currentDialog.isShowingDialog = false;
		currentDialog.isFinishShowingDialog = false;
		currentDialog.letterIndex = 0;
		currentDialog.showDialogTime = 0f;
		currentDialog.dialogTime = 0f;
	}

	void PrintLetterOneByOne () {
		if (letterIndex == currentDialog.letterList.Count-2) {
			currentDialog.isFinishShowingDialog = true;
			return;
		}
		
		string temp = "";
		int tempIdx = letterIndex + 1;
		if (currentDialog.letterList[letterIndex] == openingTag) {
			SwapList (currentDialog.letterList, letterIndex, tempIdx);
			currentDialog.letterIndex++;
		}

		// Print List
		for (int i=0; i<currentDialog.letterList.Count; i++) {
			temp += currentDialog.letterList[i];
		}

		currentDialog.textDialog.text = temp;
	}

	public void SwapList<T> (List<T> list, int idxA, int idxB) {
		T tmp = list[idxA];
		list[idxA] = list[idxB];
		list[idxB] = tmp;
	}
}