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

	float showDialogDuration;
	float showDialogDelay;
	float deltaTime;
	float timer;

	bool isInitShowingDialog;

	// bool isFinishShowingDialog;
	string testStr = "Hello World";
	string blackColor = "000000ff";
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
			// isFinishShowingDialog = dialog.isFinishShowingDialog;
			isInitShowingDialog = dialog.isInitShowingDialog;

			if (!isInitShowingDialog) {
				InitDialog ();
			} else {
				if (timer < showDialogDelay) {
					timer += deltaTime;
				} else {
					CheckNPCDialog ();
					Debug.Log("Check Dialog");
				}
			}
		}
	}

	void InitDialog () {
		dialog.panelDialog.SetActive(false);

		dialog.isInitShowingDialog = true;
	}

	void CheckNPCDialog () {
		if (currentState == NPCState.IDLE) {
			if (!currentNPC.isInteracting) {
				Debug.Log(currentNPC.gameObject.name + "is showing dialog");
				dialog.panelDialog.SetActive (true);
				currentNPC.isInteracting = true;
				dialog.isFinishShowingDialog = false;
				
				tempChar = testStr.ToCharArray();
				openingTag = tag[0] + transparentColor + tag[1];
				dialog.textDialog.text = openingTag + testStr + tag[2];
		
				//Set List
				dialog.letterList = new List<string>();
				dialog.letterList.Add(openingTag);
				for (int i=0; i<tempChar.Length; i++) {
					dialog.letterList.Add(tempChar[i].ToString());
				} 
				dialog.letterList.Add(tag[2]);
			} else if (!dialog.isFinishShowingDialog) {
				PrintLetterOneByOne ();
			} else {
				if (dialog.showDialogTime < showDialogDuration) {
					dialog.showDialogTime += deltaTime;
				} else {
					Debug.Log(currentNPC.gameObject.name + "is hiding dialog");
					dialog.panelDialog.SetActive (false);
					currentNPC.isInteracting = false;
					dialog.showDialogTime = 0f;
					timer = 0f;
				}
			}
		}
	}

	void PrintLetterOneByOne () {
		string temp = "";

		for (int i=0; i<dialog.letterList.Count-2; i++) {
			int tempIdx = i+1;

			if (dialog.letterList[i] == openingTag) {
				SwapList (dialog.letterList, i, tempIdx);
				break;
			}
		}



		// Print List
		for (int i=0; i<dialog.letterList.Count; i++) {
			temp += dialog.letterList[i];
		}

		dialog.textDialog.text = temp;
		// Debug.Log(tempList.Count);
		Debug.Log(temp);
		
		if (dialog.letterList[dialog.letterList.Count-2] == openingTag) {
			dialog.isFinishShowingDialog = true;
		}
	}

	public void SwapList<T> (List<T> list, int idxA, int idxB) {
		T tmp = list[idxA];
		list[idxA] = list[idxB];
		list[idxB] = tmp;
	}
}
