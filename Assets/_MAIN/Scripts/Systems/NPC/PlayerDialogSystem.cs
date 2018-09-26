using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class PlayerDialogSystem : ComponentSystem {
	public struct DialogData {
		public readonly int Length;
		public ComponentArray<Player> Player;
		public ComponentArray<Dialog> Dialog;
	}
	[InjectAttribute] DialogData dialogData;

	Player currentPlayer;
	PlayerInput input;

	Dialog currentDialog;
	
	PlayerState currentState;

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
		deltaTime = Time.deltaTime;
		// if (dialogData.Length == 0) return;

		for (int i=0; i<dialogData.Length; i++) {
			currentPlayer = dialogData.Player[i];
			currentDialog = dialogData.Dialog[i];

			// currentState = currentPlayer.state;
			// showDialogDuration = currentDialog.showDialogDuration;
			// showDialogDelay = currentDialog.showDialogDelay;
			// isFinishShowingDialog = currentDialog.isFinishShowingDialog;
			// isInitShowingDialog = currentDialog.isInitShowingDialog;
			// isShowingDialog = currentDialog.isShowingDialog;
			// letterIndex = currentDialog.letterIndex;
			// // dialogIndex = currentDialog.dialogIndex;
			// showDialogTime = currentDialog.showDialogTime;
			// timer = currentDialog.dialogTime;

			if (!isInitShowingDialog) {
				InitDialog ();
			} else {
				
			}
		}
	}

	void InitDialog () {
		currentDialog.panelDialog.SetActive(false);

		currentDialog.letterIndex = 0;
		// currentDialog.isInitShowingDialog = true;
	}

	//

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

	// void ShowDialog () {
	// 	currentDialog.panelDialog.SetActive (true);
	// 	currentDialog.isFinishShowingDialog = false;

	// 	currentDialog.isShowingDialog = true;
	// }

	// void HideDialog () {
	// 	currentDialog.panelDialog.SetActive (false);
	// 	currentDialog.isShowingDialog = false;
	// 	currentDialog.isFinishShowingDialog = false;
	// 	currentDialog.letterIndex = 0;
	// 	currentDialog.showDialogTime = 0f;
	// 	currentDialog.dialogTime = 0f;
	// }

	void PrintLetterOneByOne () {
		if (letterIndex == currentDialog.letterList.Count-2) {
			// currentDialog.isFinishShowingDialog = true;
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