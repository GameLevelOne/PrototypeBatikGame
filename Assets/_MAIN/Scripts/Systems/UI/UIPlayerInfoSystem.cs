﻿using UnityEngine;
using UnityEngine.EventSystems; //TEMP
using Unity.Entities;
using System.Collections.Generic;

public class UIPlayerInfoSystem : ComponentSystem {
	public struct UIPlayerInfoData {
		public readonly int Length;
		public ComponentArray<UIInfo> UIInfo;
		// public ComponentArray<UIPlayerStatus> UIPlayerStatus;
		// public ComponentArray<UIToolsNSummons> UIToolsNSummons;
	}
	[InjectAttribute] UIPlayerInfoData uiPlayerInfoData;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] UIQuestSystem uiQuestSystem;

	UIInfo uiInfo;
	// UIPlayerStatus uiPlayerStatus;
	// UIToolsNSummons uiToolsNSummons;

	Animator animator;

	List<ButtonToolNSummon> listOfToolsNSummons;

	bool isShowingInfo = false;
	bool isPlayingAnimation = false;
	// bool isAfterPressPause = false;
	bool isActivatingInfo = false;
	// bool isInitShowInfo = false;
	// int timeSwitch = 1;
	int usedToolNSummonIdx;
	int selectedToolNSummonIdx;
	// float showTime;
	// float showMultiplier;
	// float hideMultiplier;
	// float alphaValue;
	// float deltaTime;
	bool curDownPressed;
	bool curLeftPressed;
	bool curUpPressed;
	bool curRightPressed;

	protected override void OnUpdate () {
		if (uiPlayerInfoData.Length == 0) return;

		// deltaTime = Time.deltaTime;

		for (int i=0; i<uiPlayerInfoData.Length; i++) {
			uiInfo = uiPlayerInfoData.UIInfo[i];
			// uiPlayerStatus = uiPlayerInfoData.UIPlayerStatus[i];
			// uiToolsNSummons = uiPlayerInfoData.UIToolsNSummons[i];
			
			animator = uiInfo.animator;
			// showMultiplier = uiInfo.showMultiplier;
			// hideMultiplier = uiInfo.hideMultiplier;

			if (!uiInfo.isInitUIInfo) {
				try {
					InitPlayerInfo ();
				} catch {
					return;
				}

				Debug.Log("Success init UIPlayerInfoSystem");
				uiInfo.isInitUIInfo = true;
			} else {
				isPlayingAnimation = uiInfo.isPlayingAnimation;
				CheckInput ();
				CheckShowingInfo ();
			}
		}
	}

	void InitPlayerInfo () {
		isShowingInfo = false;
		// isInitShowInfo = false;
		// timeSwitch = 1;
		// alphaValue = 0f;

		curDownPressed = false;
		curLeftPressed = false;
		curUpPressed = false;
		curRightPressed = false;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		// uiInfo.canvasInfoGroup.alpha = 0f;
		animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		uiInfo.panelUIInfo.SetActive(false);
		InitTool ();
	}

	void InitTool () {
		listOfToolsNSummons = new List<ButtonToolNSummon>();

		for (int i=0; i<uiInfo.listOfButtonToolsNSummons.Count; i++) {
			listOfToolsNSummons.Add(uiInfo.listOfButtonToolsNSummons[i].GetComponent<ButtonToolNSummon>());
		}

		// CheckUnlockedButton ();
		CheckActiveTool ();
	}

	void CheckInput () {
		if (!isShowingInfo) {
			if (GameInput.IsInventoryPressed) { //ESCAPE / START (Gamepad)
				// isInitShowInfo = false;
				CheckActiveTool ();
				isShowingInfo = true;
			}
		} else {
			if (GameInput.IsInventoryPressed || GameInput.IsDodgePressed) { //ESCAPE / START (Gamepad)
				isShowingInfo = false;
			}
			
			if (!curUpPressed && GameInput.IsUpDirectionHeld) { //UP
				curUpPressed = true;
				UpButtonTool();
			} else if (!GameInput.IsUpDirectionHeld) {
				curUpPressed = false;
			}
			
			if (!curDownPressed && GameInput.IsDownDirectionHeld) { //DOWN
				curDownPressed = true;
				DownButtonTool();
			} else if (!GameInput.IsDownDirectionHeld) {
				curDownPressed = false;
			}
			
			if (!curRightPressed && GameInput.IsRightDirectionHeld) { //RIGHT
				curRightPressed = true;
				RightButtonTool();
			} else if (!GameInput.IsRightDirectionHeld) {
				curRightPressed = false;
			}
			
			if (!curLeftPressed && GameInput.IsLeftDirectionHeld) { //LEFT
				curLeftPressed = true;
				LeftButtonTool();
			} else if (!GameInput.IsLeftDirectionHeld) {
				curLeftPressed = false;
			}
			
			if (GameInput.IsAttackPressed) {
				SetSelectedTool ();
			}
		}
	}

	void CheckActiveTool () {
		for (int i=0; i<listOfToolsNSummons.Count; i++) {
			if (toolSystem.tool.currentTool == listOfToolsNSummons[i].buttonToolNSummonType) {
				usedToolNSummonIdx = i;
				selectedToolNSummonIdx = i;
				// EventSystem.current.SetSelectedGameObject(null);
				// uiToolsNSummons.listOfButtonToolsNSummons[i].Select();
				uiInfo.listOfButtonToolsNSummons[i].interactable = false;
				ChangeSelectedButtonSprite(i);
				
			} else {
				if (CheckIfToolHasBeenUnlocked(i)) {
					uiInfo.listOfButtonToolsNSummons[i].interactable = true;
					ChangeUnSelectedButtonSprite(i);
					// Debug.Log(uiToolsNSummons.listOfButtonToolsNSummons[i].name+" is unlocked");
				} else {
					uiInfo.listOfButtonToolsNSummons[i].interactable = false;
					ChangeUnSelectedButtonSprite(i);
				}
			}
		}
	}

	void ChangeSelectedButtonSprite (int idx) {
		ToolType buttonType = uiInfo.listOfButtonToolsNSummons[idx].GetComponent<ButtonToolNSummon>().buttonToolNSummonType;
		if (buttonType != ToolType.Container1 && buttonType != ToolType.Container2 && buttonType != ToolType.Container3 && buttonType != ToolType.Container4) {
			uiInfo.listOfButtonToolsNSummons[idx].image.sprite = uiInfo.selectedToolSprite;
		}
	}

	void ChangeUnSelectedButtonSprite (int idx) {
		ToolType buttonType = uiInfo.listOfButtonToolsNSummons[idx].GetComponent<ButtonToolNSummon>().buttonToolNSummonType;
		if (buttonType == ToolType.Container1 || buttonType == ToolType.Container2 || buttonType == ToolType.Container3 || buttonType == ToolType.Container4) {
			uiInfo.listOfButtonToolsNSummons[idx].image.sprite = uiInfo.initContainerSprite;
		} else {
			uiInfo.listOfButtonToolsNSummons[idx].image.sprite = uiInfo.initToolSprite;
		}
	}

	void SetSelectedTool () {
		for (int i=0; i<listOfToolsNSummons.Count; i++) {
			if (i == selectedToolNSummonIdx) {
				usedToolNSummonIdx = i;
				uiInfo.listOfButtonToolsNSummons[i].interactable = false;

				ChangeSelectedButtonSprite(i);

				toolSystem.tool.currentTool = listOfToolsNSummons[i].buttonToolNSummonType;

				// PRINT TOOL ON TOOLSSELECTIONSYSTEM
				uiToolsSelectionSystem.InitImages(true);
			} else {
				uiInfo.listOfButtonToolsNSummons[i].interactable = true;

				ChangeUnSelectedButtonSprite(i);
			}
		}
	}

	void RightButtonTool () {
		if (selectedToolNSummonIdx == 2 || selectedToolNSummonIdx == 5) {
			selectedToolNSummonIdx = 9;
		} else if (selectedToolNSummonIdx == 12){
			selectedToolNSummonIdx = 6;
		} else {
			selectedToolNSummonIdx++;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			RightButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	void LeftButtonTool () {
		if (selectedToolNSummonIdx == 0 || selectedToolNSummonIdx == 3 || selectedToolNSummonIdx == 6) {
			selectedToolNSummonIdx = 12;
		} else {
			selectedToolNSummonIdx--;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			LeftButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	void UpButtonTool () {
		if (selectedToolNSummonIdx == 0) {
			selectedToolNSummonIdx = 6;
		} else if (selectedToolNSummonIdx == 1) {
			selectedToolNSummonIdx = 7;
		} else if (selectedToolNSummonIdx == 2) {
			selectedToolNSummonIdx = 8;
		} else if (selectedToolNSummonIdx >= 9 && selectedToolNSummonIdx <= 12) {
			selectedToolNSummonIdx = 5;
			// return;
		} else {
			selectedToolNSummonIdx-=3;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			UpButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	void DownButtonTool () {
		if (selectedToolNSummonIdx == 6) {
			selectedToolNSummonIdx = 0;
		} else if (selectedToolNSummonIdx == 7) {
			selectedToolNSummonIdx = 1;
		} else if (selectedToolNSummonIdx == 8) {
			selectedToolNSummonIdx = 2;
		} else if (selectedToolNSummonIdx >= 9 && selectedToolNSummonIdx <= 12) {
			selectedToolNSummonIdx = 2;
			// return;
		} else {
			selectedToolNSummonIdx+=3;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			DownButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	// void NextButtonTool () {
	// 	if (selectedToolNSummonIdx >= listOfToolsNSummons.Count-1){
	// 		selectedToolNSummonIdx = 0;
	// 	} else {
	// 		selectedToolNSummonIdx++;
	// 	}

	// 	if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
	// 		NextButtonTool ();
	// 	}

	// 	SelectTool(selectedToolNSummonIdx);
	// }

	// void PrevButtonTool () {
	// 	if (selectedToolNSummonIdx <= 0){
	// 		selectedToolNSummonIdx = listOfToolsNSummons.Count-1;
	// 	} else {
	// 		selectedToolNSummonIdx--;
	// 	}

	// 	if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
	// 		PrevButtonTool ();
	// 	}

	// 	SelectTool(selectedToolNSummonIdx);
	// }

	bool CheckIfToolHasBeenUnlocked (int type) {
		if (toolSystem.tool.CheckIfToolHasBeenUnlocked((int) listOfToolsNSummons[type].buttonToolNSummonType) > 0) {
			// selectedToolNSummonIdx = type;
			return true;
		} else {
			return false;
		}
	}

	void SelectTool (int idx) {
		uiInfo.listOfButtonToolsNSummons[idx].Select();
	}

	// public void PrintHP (string value) {
	// 	uiPlayerStatus.textHP.text = value;
	// }

	// public void PrintMP (string value) {
	// 	uiPlayerStatus.textMP.text = value;
	// }

	// public void PrintTool (string value) {
	// 	uiPlayerStatus.textTool.text = value;
	// }

	void CheckShowingInfo () {
		if (isShowingInfo) {
			ShowInfo ();
		} else {
			HideInfo ();
		}
	}

	void ShowInfo () {
		if (!isActivatingInfo) {
			Time.timeScale = 0;
			uiInfo.panelUIInfo.SetActive(true);
			uiInfo.isPlayingAnimation = true;
			animator.Play(Constants.AnimationName.FADE_IN);
			// isInitShowShop = false;

			//CHECK QUEST
			uiQuestSystem.CheckIfUIQuestIsComplete();

			isActivatingInfo = true;
		} else {
			if (!isPlayingAnimation) {
				animator.Play(Constants.AnimationName.CANVAS_VISIBLE);
				uiInfo.isPlayingAnimation = true;
				// isInitShowShop = true;
			} else {
				//
			}
		}
	}

	void HideInfo () {
		if (isActivatingInfo) {
			// isInitShowShop = false;
			uiInfo.isPlayingAnimation = true;
			animator.Play(Constants.AnimationName.FADE_OUT);
			isActivatingInfo = false;
		} else {
			if (!isPlayingAnimation) {
				// isInitShowShop = false;
				animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
				uiInfo.isPlayingAnimation = true;
				uiInfo.panelUIInfo.SetActive(false);
				Time.timeScale = 1;
			}
		}
	}

	void CheckContainerImage(int idx) {
		if (idx == (int) ToolType.Container1) {
			// Debug.Log(uiToolsSelection.playerContainer.lootableTypes[0].ToString());
			SetContainerImage(uiToolsSelectionSystem.uiToolsSelection.playerContainer.lootableTypes[0], idx);
		} else if (idx == (int) ToolType.Container2) {
			SetContainerImage(uiToolsSelectionSystem.uiToolsSelection.playerContainer.lootableTypes[1], idx);
		} else if (idx == (int) ToolType.Container3) {
			SetContainerImage(uiToolsSelectionSystem.uiToolsSelection.playerContainer.lootableTypes[2], idx);
		} else if (idx == (int) ToolType.Container4) {
			SetContainerImage(uiToolsSelectionSystem.uiToolsSelection.playerContainer.lootableTypes[3], idx);
		}
	}

	void SetContainerImage (LootableType type, int toolSpriteIdx) {
		switch (type) {
			case LootableType.HP_POTION:
				uiInfo.listOfButtonToolsNSummons[toolSpriteIdx].image.sprite = uiInfo.hpPotSprite;
				break;
			case LootableType.MANA_POTION:
				uiInfo.listOfButtonToolsNSummons[toolSpriteIdx].image.sprite = uiInfo.mpPotSprite;
				break;
			case LootableType.NONE:
				uiInfo.listOfButtonToolsNSummons[toolSpriteIdx].image.sprite = uiInfo.initContainerSprite;
				break;
		}
	}

	//CHECK PLAYER LOCATION
	public void SetMapName (int mapIdx) {
#region DEVELOP
		if (GameStorage.Instance.CurrentScene == "ECS Movement 3D") {
			mapIdx = 0;
		}
#endregion

		uiInfo.areaName.text = uiInfo.areaNames[mapIdx];
		uiInfo.playerIndicator.localPosition = uiInfo.areaCoordinates[mapIdx].localPosition;
	}

#region OLD Show & Hide
	// void CheckShowingTools () {
	// 	if (isShowingInfo) {
	// 		if (!isActivatingPlayerInfo) {
	// 			uiInfo.panelUIInfo.SetActive(true);
	// 			isActivatingPlayerInfo = true;
	// 		} else {
	// 			ShowTools ();
	// 		}
	// 	} else {
	// 		HideTools ();
	// 	}
	// }

	// void ShowTools () {
	// 	if (!isInitShowInfo) {
	// 		if (alphaValue < 1f) {
	// 			alphaValue += deltaTime * showMultiplier;
	// 			uiInfo.canvasInfoGroup.alpha = alphaValue;
	// 		} else {
	// 			Time.timeScale = 0;
	// 			isInitShowInfo = true;
	// 		}
	// 	}
	// }

	// void HideTools () {
	// 	if (alphaValue > 0f) {
	// 		alphaValue -= deltaTime * hideMultiplier;
	// 		uiInfo.canvasInfoGroup.alpha = alphaValue;
	// 	} else {
	// 		uiInfo.panelUIInfo.SetActive(false);
	// 		isActivatingPlayerInfo = false;
	// 	}
	// }
#endregion
}
