using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class DialogSystem : ComponentSystem {
	public struct DialogData {
		public readonly int Length;
		public ComponentArray<Dialog> Dialog;
	}
	[InjectAttribute] DialogData dialogData;

	NPC currentNPC;

	Dialog dialog;
	
	NPCState currentState;

	float showDialogTime;
	float showDialogDuration;
	float showDialogDelay;
	float deltaTime;
	float timer;

	// int dialogIndex;
	int letterIndex;
	// int listEndIdx;
	// int listCount;

	bool isInitShowingDialog;
	bool isShowingDialog = false;

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
			dialog = dialogData.Dialog[i];

			currentNPC = dialog.npc;
			currentState = currentNPC.state;
			showDialogDuration = dialog.showDialogDuration;
			showDialogDelay = dialog.showDialogDelay;
			isFinishShowingDialog = dialog.isFinishShowingDialog;
			isInitShowingDialog = dialog.isInitShowingDialog;
			isShowingDialog = dialog.isShowingDialog;
			letterIndex = dialog.letterIndex;
			// dialogIndex = dialog.dialogIndex;
			showDialogTime = dialog.showDialogTime;
			timer = dialog.dialogTime;

			if (!isInitShowingDialog) {
				InitDialog ();
			} else {
				CheckNPCState ();
			}
		}
	}

	void InitDialog () {
		dialog.panelDialog.SetActive(false);

		dialog.letterIndex = 0;
		dialog.isInitShowingDialog = true;
	}

	void CheckNPCState () {
		switch (currentState) {
			case NPCState.IDLE:
			if (timer < showDialogDelay) {
					dialog.dialogTime += deltaTime;
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
			dialog.dialogIndex = GetRandomIndexType(currentState);
			SetList (GetDialogStringType(currentState, dialog.dialogIndex));			
			ShowDialog ();
		} else if (!isFinishShowingDialog) {
			PrintLetterOneByOne ();
		} else {
			if (showDialogTime < showDialogDuration) {
				dialog.showDialogTime += deltaTime;
			} else {
				HideDialog ();
			}
		}
	}

	void CheckNPCInteractDialog () {
		if (!isShowingDialog) {
			dialog.dialogIndex = GetRandomIndexType(currentState);
			SetList (GetDialogStringType(currentState, dialog.dialogIndex));			
			ShowDialog ();
		} else if (!isFinishShowingDialog) {
			PrintLetterOneByOne ();

			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
				dialog.textDialog.text = GetDialogStringType(currentState, dialog.dialogIndex) + openingTag + tag[2];
				dialog.isFinishShowingDialog = true;
			}
		} else {
			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Keypad0)) {
				HideDialog ();
			}
			// if (showDialogTime < showDialogDuration) {
			// 	dialog.showDialogTime += deltaTime;
			// } else {
			// 	HideDialog ();
			// }
		}
	}

	int GetRandomIndexType (NPCState state) {
		switch (state) {
			case NPCState.IDLE:
				return Random.Range(0, dialog.idleDialogs.Length);
			case NPCState.INTERACT:
				return Random.Range(0, dialog.interactDialogs.Length);
			default:
				return 0;
		}
	}


	string GetDialogStringType (NPCState state, int dialogIdx) {
		switch (state) {
			case NPCState.IDLE:
				return dialog.idleDialogs[dialogIdx];
			case NPCState.INTERACT:
				return dialog.interactDialogs[dialogIdx];
			default:
				return "NOTHING";
		}
	}

	void SetList (string strDialog) {
		tempChar = strDialog.ToCharArray();

		openingTag = tag[0] + transparentColor + tag[1];
		dialog.textDialog.text = openingTag + strDialog + tag[2];

		//Set List
		dialog.letterList = new List<string>();
		dialog.letterList.Add(openingTag);
		for (int i=0; i<tempChar.Length; i++) {
			dialog.letterList.Add(tempChar[i].ToString());
		} 
		dialog.letterList.Add(tag[2]);
	}

	void ShowDialog () {
		Debug.Log(currentNPC.gameObject.name + "is showing dialog");
		dialog.panelDialog.SetActive (true);
		dialog.isFinishShowingDialog = false;

		dialog.isShowingDialog = true;
	}

	void HideDialog () {
		Debug.Log(currentNPC.gameObject.name + "is hiding dialog");
		dialog.panelDialog.SetActive (false);
		dialog.isShowingDialog = false;
		dialog.isFinishShowingDialog = false;
		dialog.letterIndex = 0;
		dialog.showDialogTime = 0f;
		dialog.dialogTime = 0f;
	}

	void PrintLetterOneByOne () {
		if (letterIndex == dialog.letterList.Count-2) {
			dialog.isFinishShowingDialog = true;
			return;
		}
		
		string temp = "";
		int tempIdx = letterIndex + 1;
		if (dialog.letterList[letterIndex] == openingTag) {
			SwapList (dialog.letterList, letterIndex, tempIdx);
			dialog.letterIndex++;
		}

		// Print List
		for (int i=0; i<dialog.letterList.Count; i++) {
			temp += dialog.letterList[i];
		}

		dialog.textDialog.text = temp;
	}

	public void SwapList<T> (List<T> list, int idxA, int idxB) {
		T tmp = list[idxA];
		list[idxA] = list[idxB];
		list[idxB] = tmp;
	}
}