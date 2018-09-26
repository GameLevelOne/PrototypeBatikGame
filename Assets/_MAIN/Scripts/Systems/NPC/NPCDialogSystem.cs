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
	
	string curDialogText;
	float deltaTime;
	
	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;

		for (int i=0; i<dialogData.Length; i++) {
			currentNPC = dialogData.NPC[i];
			currentDialog = dialogData.Dialog[i];
			curDialogText = currentNPC.npcType == NPCType.OPENING ? currentDialog.openingDialogs[currentDialog.dialogIndex] : currentDialog.interactDialogs[currentDialog.dialogIndex];

			if (currentDialog.dialogState == DialogState.INIT) {
				InitDialog(true);
			} else if (currentDialog.dialogState == DialogState.SHOW) {
				ShowDialogWithIndex();
			} else if (currentDialog.dialogState == DialogState.WAITINPUT) {
				CheckInputForNextDialog();
			} else if (currentDialog.dialogState == DialogState.WAITDELAY) {
				WaitToEndDialog();
			// } else {
			// 	if (currentNPC.state == NPCState.INTERACT)
			// 		currentDialog.dialogState = DialogState.SHOW;
			}
		}
	}

	void InitDialog (bool firstInit) {
		currentDialog.panelDialog.SetActive(false);
		currentDialog.dialogButton.SetActive(false);
		currentDialog.textDialog.text = "";

		if (firstInit)
			currentDialog.dialogIndex = 0;
		currentDialog.letterIndex = 0;
		currentDialog.dialogDeltaTime = 0f;

		currentDialog.dialogState = DialogState.IDLE;
	}

	void ShowDialogWithIndex() {
		currentDialog.panelDialog.SetActive(true);//SHOW DIALOG BOX

		currentDialog.dialogDeltaTime += deltaTime;
		Debug.Log("Dialog Delta Time: "+currentDialog.dialogDeltaTime);

		if (currentDialog.dialogDeltaTime>=currentDialog.showTextDuration) {//SHOW NEXT LETTER AFTER DELAY
			currentDialog.dialogDeltaTime = 0f;
			string strNewLine = curDialogText.Replace('*','\n');//REPLACE ALL * WITH NEW LINE
			string strToShow = strNewLine.Substring(0,currentDialog.letterIndex+1); //LETTER BEING SHOWN
			strToShow += "<color=#00000000>";
			strToShow += strNewLine.Substring(currentDialog.letterIndex+1);//LETTER NOT SHOWN
			strToShow += "</color>";
			
			currentDialog.textDialog.text = strToShow;//SHOWN TO DIALOG BOX

			currentDialog.letterIndex ++;//NEXT LETTER
			if (currentDialog.letterIndex<curDialogText.Length-2) {
				if (currentDialog.letterIndex>1 && GameInput.IsAttackPressed)
					ShowDialogLastLetter(currentNPC.state == NPCState.INTERACT);
			} else {
				ShowDialogLastLetter(currentNPC.state == NPCState.INTERACT);
			}
		}
	}

	void ShowDialogLastLetter(bool waitForInput) {
		string strNewLine = curDialogText.Replace('*','\n');//REPLACE ALL * WITH NEW LINE
		currentDialog.textDialog.text = strNewLine;//SHOWN ALL TO DIALOG BOX
		currentDialog.dialogDeltaTime = 0f;

		if (waitForInput) {
			currentDialog.dialogState = DialogState.WAITINPUT;
			currentDialog.dialogButton.SetActive(true);
		} else {
			currentDialog.dialogState = DialogState.WAITDELAY;
		}
	}

	void CheckInputForNextDialog() {
		if (GameInput.IsAttackPressed) {
			currentDialog.dialogState = DialogState.SHOW;
			int indexToCheck = currentNPC.npcType == NPCType.OPENING ? currentDialog.openingDialogs.Length-1 : currentDialog.interactDialogs.Length-1;
			if (currentDialog.dialogIndex < indexToCheck) {
				currentDialog.dialogButton.SetActive(false);
				currentDialog.dialogIndex++;
				currentDialog.letterIndex = 0;
				currentDialog.dialogDeltaTime = 0f;
			} else {

				InitDialog(false);
				if (currentNPC.npcType == NPCType.SHOP) {
					if (!currentNPC.uiShop.isOpeningShop) {
						Debug.Log("Open Shop!");
						currentNPC.uiShop.isOpeningShop = true;
					}
				} else if (currentNPC.npcType == NPCType.OPENING) {
					Debug.Log("Send Event to timeline !!!");
					currentNPC.GetComponent<NPCOpening>().EndOpeningDialogue();
				} 
			}
		} else {
			if (currentNPC.state==NPCState.IDLE) {
				currentDialog.dialogState = DialogState.WAITDELAY;
			}
		}
	}

	void WaitToEndDialog() {
		currentDialog.dialogButton.SetActive(false);
		currentDialog.dialogDeltaTime += deltaTime;
		if (currentDialog.dialogDeltaTime>=currentDialog.delayDialogDuration) {//HIDE DIALOG AFTER SOME DELAY
			InitDialog(false);
		}
	}
}

