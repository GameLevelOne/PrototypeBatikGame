using UnityEngine;
using UnityEngine.EventSystems; //TEMP
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

	protected override void OnUpdate () {
		if (uiPlayerInfoData.Length == 0) return;

		// deltaTime = Time.deltaTime;

		for (int i=0; i<uiPlayerInfoData.Length; i++) {
			uiInfo = uiPlayerInfoData.UIInfo[i];
			uiPlayerStatus = uiPlayerInfoData.UIPlayerStatus[i];
			uiToolsNSummons = uiPlayerInfoData.UIToolsNSummons[i];
			
			animator = uiInfo.animator;
			// showMultiplier = uiInfo.showMultiplier;
			// hideMultiplier = uiInfo.hideMultiplier;

			if (!uiInfo.isInitUIInfo) {
				try {
					InitPlayerInfo ();
				} catch {
					Debug.Log("Error init UIPlayerInfoSystem");
					return;
				}

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

		for (int i=0; i<uiToolsNSummons.listOfButtonToolsNSummons.Count; i++) {
			listOfToolsNSummons.Add(uiToolsNSummons.listOfButtonToolsNSummons[i].GetComponent<ButtonToolNSummon>());
		}

		CheckActiveTool ();
	}

	void CheckInput () {
		if (!isShowingInfo) {
			if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.JoystickButton8)) { //ESCAPE / START (Gamepad)
				// isInitShowInfo = false;
				CheckActiveTool ();
				isShowingInfo = true;
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton11)) { //ESCAPE / START (Gamepad)
				isShowingInfo = false;
			} else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
				PrevButtonTool ();
			} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
				NextButtonTool ();
			} else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
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
