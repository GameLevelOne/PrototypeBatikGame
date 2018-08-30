using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using System.Collections.Generic;

public class UIToolsSelectionSystem : ComponentSystem {
	public struct UIToolsSelectionData {
		public readonly int Length;
		public ComponentArray<UIToolsSelection> UIToolsSelection;
		public ComponentArray<Animation2D> Animation;
	}
	[InjectAttribute] UIToolsSelectionData uiToolsSelectionData;
	
	[InjectAttribute] UIHPManaToolSystem uiHPManaToolSystem;

	public UIToolsSelection uiToolsSelection;
	
	PlayerTool tool;
	Animation2D anim;

	Animator animator;

	public bool isChangingTool = false;

	Image[] toolImages;
	Sprite[] toolSprites;

	int changeIndex = 0;
	bool isInitToolImage = false;
	bool isShowingTools = false;
	bool isActivatingPlayerInfo = false;
	bool isInitShowInfo = false;
	float deltaTime;
	float showTime;
	float showDuration;
	float showMultiplier;
	float hideMultiplier;
	float alphaValue;

	protected override void OnUpdate () {
		if (uiToolsSelectionData.Length == 0) return;
		
		// if (GameStatus.Bool.IsPauseGame) {
		// 	// isUpdateList = false;
		// 	return;
		// } 
		// else {			
		// 	// CheckToolIndexes ();
		// 	Debug.Log("Game Is Running");
		// }

		deltaTime = Time.deltaTime;

		for (int i=0; i<uiToolsSelectionData.Length; i++) {
			uiToolsSelection = uiToolsSelectionData.UIToolsSelection[i];
			tool = uiToolsSelection.tool;
			anim = uiToolsSelectionData.Animation[i];
			
			animator = anim.animator;
			toolSprites = uiToolsSelection.arrayOfToolSprites;
			toolImages = uiToolsSelection.arrayOfToolImages;
			showDuration = uiToolsSelection.showDuration;
			showMultiplier = uiToolsSelection.showMultiplier;
			hideMultiplier = uiToolsSelection.hideMultiplier;

			if (!isInitToolImage) {
				try {
					InitToolsSelection ();
				} catch {
					return;
				}
				
				isInitToolImage = true;
			} else {
				CheckAnimation ();
				CheckShowingTools ();
				
				if (uiToolsSelection.isToolChange && !isChangingTool) {
					changeIndex = uiToolsSelection.changeIndex;

					if (changeIndex == 0) {
						ResetChange();
					} else {
						isChangingTool = true;
						CheckToolsButton ();
					}				
				}
			}
		}
	}

	void InitToolsSelection () {
		InitImages (false);

		showTime = 0f;
		alphaValue = 0f;
		uiToolsSelection.canvasToolsGroup.alpha = 0f;
		uiToolsSelection.panelToolsSelection.SetActive(false);
		isShowingTools = true;
		isInitShowInfo = false;
	}

	public void InitImages (bool isUpdatedList) {
		uiToolsSelection.toolIndexes = new List<int>();

		if (!isUpdatedList) {
			for (int i=0; i<toolSprites.Length; i++) {
				if (tool.CheckIfToolHasBeenUnlocked(i) > 0) {
					uiToolsSelection.toolIndexes.Add(i);
				}
			}
		
			SetImages();
		} else {
			// int whileIdx = 0;
			int tempIdx = (int) tool.currentTool;
			uiToolsSelection.checker = new bool[toolSprites.Length];
			
			while (uiToolsSelection.checker[tempIdx] != true) {
				if (!uiToolsSelection.checker[tempIdx]) {
					if (tool.CheckIfToolHasBeenUnlocked(tempIdx) > 0) {
						uiToolsSelection.toolIndexes.Add(tempIdx);
						Debug.Log("Add");
					}
					
					uiToolsSelection.checker[tempIdx] = true;
					Debug.Log("Check : "+tempIdx+" "+uiToolsSelection.checker[tempIdx]);

					if (tempIdx >= (int) ToolType.Boots) {
						tempIdx = 0;
					} else {
						tempIdx++;
					}
				}
			}

			ResetChange();
		}
	}

	void SetImages () {
		for (int i=0; i<=toolImages.Length/2; i++) {
			toolImages[i].sprite = toolSprites[uiToolsSelection.toolIndexes[i]];
		}

		int listToolsCount = uiToolsSelection.toolIndexes.Count;

		for (int i=toolImages.Length-1; i>toolImages.Length/2; i--) {
			listToolsCount--;
			toolImages[i].sprite = toolSprites[uiToolsSelection.toolIndexes[listToolsCount]];
		}
	}

	void CheckToolIndexes () {
		// if (!isUpdateList) {
		// 	if (uiToolsSelection.toolIndexes[0] != (int) tool.currentTool) {
		// 		int tempIdx = (int) tool.currentTool;
		// 		bool[] checker = new bool[uiToolsSelection.toolIndexes.Count];
		// 		List<int> tempList = uiToolsSelection.toolIndexes;

		// 		uiToolsSelection.toolIndexes = new List<int>();

		// 		for (int i=0; i<checker.Length; i++) {
		// 			if (!checker[i]) {
		// 				uiToolsSelection.toolIndexes[i] = 1;
		// 				checker[i] = true;

		// 				if (tempIdx >= (int) ToolType.Boots) {
		// 					tempIdx = 1;
		// 				} else {
		// 					tempIdx++;
		// 				}
		// 			}
		// 		}
		// 	}
			
		// 	isUpdateList = true;
		// }
	}

	public void SetPrintedTool () {
		uiHPManaToolSystem.PrintTool(toolImages[0].sprite, tool.currentTool.ToString());
	}

	void CheckShowingTools () {
		if (isShowingTools) {
			if (!isActivatingPlayerInfo) {
				uiToolsSelection.panelToolsSelection.SetActive(true);
				isActivatingPlayerInfo = true;
			} else {
				ShowTools ();
			}
		} else {
			HideTools ();
		}
	}

	void ShowTools () {
		if (!isInitShowInfo) {
			if (alphaValue < 1f) {
				alphaValue += deltaTime * showMultiplier;
				uiToolsSelection.canvasToolsGroup.alpha = alphaValue;
			} else {
				if (showTime < showDuration) {
					showTime += deltaTime;
				} else {
					isShowingTools = false;
					isInitShowInfo = true;
				}
			}
		}
	}

	void HideTools () {
		if (alphaValue > 0f) {
			alphaValue -= deltaTime * hideMultiplier;
			uiToolsSelection.canvasToolsGroup.alpha = alphaValue;
		} else {
			// uiToolsSelection.panelToolsSelection.SetActive(false);
			isActivatingPlayerInfo = false;
		}
	}
	
	void CheckAnimation () {
		if (!anim.isCheckBeforeAnimation) {
			CheckStartAnimation ();
			anim.isCheckBeforeAnimation = true;
		} else if (!anim.isCheckAfterAnimation) {
			CheckEndAnimation ();
			anim.isCheckAfterAnimation = true;
		}
	}

	void CheckToolsButton () {
		if (changeIndex == 1) {
			NextTools();
		} else if (changeIndex == -1) {
			PrevTools();
		}

		Debug.Log("isShowingTools isInitShowInfo");
		isShowingTools = true;
		isInitShowInfo = false;
		showTime = 0f;
	}

	void NextTools () {
		animator.Play(Constants.AnimationName.SLIDE_LEFT);

		int tempToolIdx = uiToolsSelection.toolIndexes[0];
		uiToolsSelection.toolIndexes.RemoveAt(0);
		uiToolsSelection.toolIndexes.Add(tempToolIdx);
	}

	void PrevTools () {
		animator.Play(Constants.AnimationName.SLIDE_RIGHT);

		int lastToolIdx = uiToolsSelection.toolIndexes.Count-1;
		int tempToolIdx = uiToolsSelection.toolIndexes[lastToolIdx];
		uiToolsSelection.toolIndexes.RemoveAt(lastToolIdx);
		uiToolsSelection.toolIndexes.Insert(0,tempToolIdx);
	}

	void CheckStartAnimation () {
		// Debug.Log("CheckStartAnimation");
	}

	void CheckEndAnimation () {
		// Debug.Log("CheckEndAnimation");
		ResetChange ();
	}

	void ResetChange () {
		Debug.Log("ResetChange");
		animator.Play(Constants.AnimationName.SLIDE_IDLE);
		SetImages();
		SetPrintedTool ();
		uiToolsSelection.changeIndex = 0;
		uiToolsSelection.isToolChange = false;
		isChangingTool = false;
		
		if (!isActivatingPlayerInfo) {
			uiToolsSelection.panelToolsSelection.SetActive(false);
		}
	}
}
