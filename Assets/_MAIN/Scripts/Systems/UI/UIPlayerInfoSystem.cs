using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;
using System.Collections.Generic;

public class UIPlayerInfoSystem : ComponentSystem {
	public struct UIPlayerInfoData {
		public readonly int Length;
		public ComponentArray<UIInfo> UIInfo;
		public ComponentArray<UIPlayerStatus> UIPlayerStatus;
		public ComponentArray<UIToolsNSummons> UIToolsNSummons;
	}
	[InjectAttribute] UIPlayerInfoData uiPlayerInfoData;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	[InjectAttribute] ToolSystem toolSystem;

	UIInfo uiInfo;
	UIPlayerStatus uiPlayerStatus;
	UIToolsNSummons uiToolsNSummons;

	List<ButtonToolNSummon> listOfToolsNSummons;

	bool isInitPlayerInfo = false;
	bool isShowingInfo = false;
	bool isAfterPressPause = false;
	bool isActivatingPlayerInfo = false;
	// int timeSwitch = 1;
	int usedToolNSummonIdx;
	int selectedToolNSummonIdx;
	float showTime;
	float showMultiplier;
	float hideMultiplier;
	float alphaValue;
	float deltaTime;

	protected override void OnUpdate () {
		if (uiPlayerInfoData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<uiPlayerInfoData.Length; i++) {
			uiInfo = uiPlayerInfoData.UIInfo[i];
			uiPlayerStatus = uiPlayerInfoData.UIPlayerStatus[i];
			uiToolsNSummons = uiPlayerInfoData.UIToolsNSummons[i];
			showMultiplier = uiInfo.showMultiplier;
			hideMultiplier = uiInfo.hideMultiplier;

			if (!isInitPlayerInfo) {
				try {
					InitPlayerInfo ();
				} catch {
					return;
				}

				isInitPlayerInfo = true;
			} else {
				CheckInput ();
				CheckShowingTools ();
			}
		}
	}

	void InitPlayerInfo () {
		isShowingInfo = false;
		// timeSwitch = 1;
		alphaValue = 0f;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		uiInfo.panelUIInfo.SetActive(false);
		InitTool ();
	}

	void InitTool () {
		listOfToolsNSummons = new List<ButtonToolNSummon>();

		for (int i=0; i<uiToolsNSummons.listOfButtonToolsNSummons.Count; i++) {
			listOfToolsNSummons.Add(uiToolsNSummons.listOfButtonToolsNSummons[i].GetComponent<ButtonToolNSummon>());
		}

		CheckActiveTool ();
	}

	void CheckInput () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton11)) { //ESCAPE / START (Gamepad)
			isShowingInfo = !isShowingInfo;
			// timeSwitch = Mathf.Abs(timeSwitch-1);

			GameStatus.Bool.IsPauseGame = isShowingInfo;
			// uiInfo.panelUIInfo.SetActive(isShowingInfo);
			
			if (isShowingInfo) {
				CheckActiveTool ();
			} else {
				Time.timeScale = 1;
			}
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			PrevButtonTool ();
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			NextButtonTool ();
		} else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
			SetSelectedTool ();
		}
	}

	void CheckActiveTool () {
		for (int i=0; i<listOfToolsNSummons.Count; i++) {
			if (toolSystem.tool.currentTool == listOfToolsNSummons[i].buttonToolNSummonType) {
				usedToolNSummonIdx = i;
				selectedToolNSummonIdx = i;
				// EventSystem.current.SetSelectedGameObject(null);
				// uiToolsNSummons.listOfButtonToolsNSummons[i].Select();
				uiToolsNSummons.listOfButtonToolsNSummons[i].interactable = false;
			} else {
				uiToolsNSummons.listOfButtonToolsNSummons[i].interactable = true;
			}
		}
	}

	void SetSelectedTool () {
		for (int i=0; i<listOfToolsNSummons.Count; i++) {
			if (i == selectedToolNSummonIdx) {
				usedToolNSummonIdx = i;
				uiToolsNSummons.listOfButtonToolsNSummons[i].interactable = false;
				toolSystem.tool.currentTool = listOfToolsNSummons[i].buttonToolNSummonType;

				// PRINT TOOL ON TOOLSSELECTIONSYSTEM
				uiToolsSelectionSystem.InitImages(true);
			} else {
				uiToolsNSummons.listOfButtonToolsNSummons[i].interactable = true;
			}
		}
	}

	void NextButtonTool () {
		if (selectedToolNSummonIdx >= listOfToolsNSummons.Count-1){
			selectedToolNSummonIdx = 0;
		} else {
			selectedToolNSummonIdx++;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			NextButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	void PrevButtonTool () {
		if (selectedToolNSummonIdx <= 0){
			selectedToolNSummonIdx = listOfToolsNSummons.Count-1;
		} else {
			selectedToolNSummonIdx--;
		}

		if (selectedToolNSummonIdx == usedToolNSummonIdx || !CheckIfToolHasBeenUnlocked (selectedToolNSummonIdx)) {
			PrevButtonTool ();
		}

		SelectTool(selectedToolNSummonIdx);
	}

	bool CheckIfToolHasBeenUnlocked (int type) {
		if (toolSystem.tool.CheckIfToolHasBeenUnlocked((int) listOfToolsNSummons[type].buttonToolNSummonType) > 0) {
			// selectedToolNSummonIdx = type;
			return true;
		} else {
			return false;
		}
	}

	void SelectTool (int idx) {
		uiToolsNSummons.listOfButtonToolsNSummons[idx].Select();
	}

	public void PrintHP (string value) {
		uiPlayerStatus.textHP.text = value;
	}

	public void PrintMP (string value) {
		uiPlayerStatus.textMP.text = value;
	}

	public void PrintTool (string value) {
		uiPlayerStatus.textTool.text = value;
	}

	void CheckShowingTools () {
		if (isShowingInfo) {
			if (!isActivatingPlayerInfo) {
				uiInfo.panelUIInfo.SetActive(true);
				isActivatingPlayerInfo = true;
			} else {
				ShowTools ();
			}
		} else {
			HideTools ();
		}
	}

	void ShowTools () {
		if (alphaValue < 1f) {
			alphaValue += deltaTime * showMultiplier;
			uiInfo.canvasInfoGroup.alpha = alphaValue;
		} else {
			Time.timeScale = 0;
		}
	}

	void HideTools () {
		if (alphaValue > 0f) {
			alphaValue -= deltaTime * hideMultiplier;
			uiInfo.canvasInfoGroup.alpha = alphaValue;
		} else {
			uiInfo.panelUIInfo.SetActive(false);
			isActivatingPlayerInfo = false;
		}
	}
}
