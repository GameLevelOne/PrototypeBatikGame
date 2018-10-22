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
	[InjectAttribute] ContainerSystem containerSystem;

	public UIToolsSelection uiToolsSelection;
	
	PlayerTool tool;
	Animation2D anim;

	Animator sliderAnimator;
	Animator panelAnimator;

	public bool isChangingTool = false;

	Image[] toolImages;
	Sprite[] toolSprites;

	int changeIndex;
	bool isShowingTools;
	bool isPlayingAnimation;
	bool isActivatingTools;
	// bool isInitShowInfo = false;
	float deltaTime;
	float showTime;
	float showDuration;
	// float showMultiplier;
	// float hideMultiplier;
	// float alphaValue;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		// if (uiToolsSelectionData.Length == 0) return;

		for (int i=0; i<uiToolsSelectionData.Length; i++) {
			uiToolsSelection = uiToolsSelectionData.UIToolsSelection[i];
			tool = uiToolsSelection.tool;
			anim = uiToolsSelectionData.Animation[i];
			
			sliderAnimator = anim.animator;
			panelAnimator = uiToolsSelection.animator;
			toolSprites = uiToolsSelection.arrayOfToolSprites;
			toolImages = uiToolsSelection.arrayOfToolImages;
			showDuration = uiToolsSelection.showDuration;
			// showMultiplier = uiToolsSelection.showMultiplier;
			// hideMultiplier = uiToolsSelection.hideMultiplier;

			if (!uiToolsSelection.isInitToolImage) {
				try {
					InitToolsSelection ();
				} catch (System.Exception e) {
					Debug.Log("ERROR : "+e);
					return;
				}
				
				uiToolsSelection.isInitToolImage = true;
			} else {
				isPlayingAnimation = uiToolsSelection.isPlayingAnimation;
				CheckAnimation ();
				CheckShowingTools ();
				
				if (uiToolsSelection.isToolChange && !isChangingTool ) {
					changeIndex = uiToolsSelection.changeIndex;
					// Debug.Log("ChangingTools");

					if (changeIndex == 0) {
						ResetChange();
					} else {
						if (!uiToolsSelection.panelToolsSelection.activeSelf) {
							uiToolsSelection.panelToolsSelection.SetActive(true);
							panelAnimator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
						}

						CheckToolsButton ();
						isChangingTool = true;
					}				
				}
			}
		}
	}

	void InitToolsSelection () {
		// Debug.Log("UIToolSelectionSystem "+tool.currentTool);
		// InitImages (false); //OLD
		showTime = 0f;
		isShowingTools = false;
		isPlayingAnimation = false;
		isActivatingTools = false;
		// alphaValue = 0f;
		// uiToolsSelection.canvasToolsGroup.alpha = 0f;
		panelAnimator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		uiToolsSelection.panelToolsSelection.SetActive(false);
		// isShowingTools = true;
		// isInitShowInfo = false;
		InitImages (true);
	}

	public void InitImages (bool isUpdatedList) {
		uiToolsSelection.toolIndexes = new List<int>();

		if (!isUpdatedList) {
			for (int i=0; i<toolSprites.Length; i++) {
				if (tool.CheckIfToolHasBeenUnlocked(i) > 0) {
					uiToolsSelection.toolIndexes.Add(i);

					CheckContainerImage (i);
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
						// Debug.Log("Add");
						CheckContainerImage (tempIdx);
					}
					
					uiToolsSelection.checker[tempIdx] = true;
					// Debug.Log("Check : "+tempIdx+" "+uiToolsSelection.checker[tempIdx]);

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

	void CheckContainerImage(int idx) {
		if (idx == (int) ToolType.Container1) {
			Debug.Log(idx+" "+containerSystem.container.lootableTypes[0].ToString());
			SetContainerImage(containerSystem.container.lootableTypes[0], idx);
		} else if (idx == (int) ToolType.Container2) {
			Debug.Log(idx+" "+containerSystem.container.lootableTypes[1].ToString());
			SetContainerImage(containerSystem.container.lootableTypes[1], idx);
		} else if (idx == (int) ToolType.Container3) {
			Debug.Log(idx+" "+containerSystem.container.lootableTypes[2].ToString());
			SetContainerImage(containerSystem.container.lootableTypes[2], idx);
		} else if (idx == (int) ToolType.Container4) {
			Debug.Log(idx+" "+containerSystem.container.lootableTypes[3].ToString());
			SetContainerImage(containerSystem.container.lootableTypes[3], idx);
		}
	}

	void SetContainerImage (LootableType type, int toolSpriteIdx) {
		switch (type) {
			case LootableType.HP_POTION:
				toolSprites[toolSpriteIdx] = uiToolsSelection.hpPotSprite;
				break;
			case LootableType.MANA_POTION:
				toolSprites[toolSpriteIdx] = uiToolsSelection.mpPotSprite;
				break;
			case LootableType.NONE:
				toolSprites[toolSpriteIdx] = uiToolsSelection.initContainerSprite;
				break;
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

	public void SetPrintedTool () {
		// Debug.Log("SetPrintedTool "+(int) tool.currentTool);
		// uiHPManaToolSystem.PrintTool(toolImages[(int) tool.currentTool].sprite, tool.currentTool.ToString());
		uiHPManaToolSystem.PrintTool(toolSprites[(int) tool.currentTool], tool.currentTool.ToString());
	}

	void CheckShowingTools () {
		if (isShowingTools) {
			ShowTools ();
		} else {
			HideTools ();
		}
	}

	void ShowTools () {
		if (!isActivatingTools) {
			uiToolsSelection.panelToolsSelection.SetActive(true);
			uiToolsSelection.isPlayingAnimation = true;
			panelAnimator.Play(Constants.AnimationName.FADE_IN);
			// isInitShowShop = false;
			isActivatingTools = true;
		} else {
			if (!isPlayingAnimation) {
				panelAnimator.Play(Constants.AnimationName.CANVAS_VISIBLE);
				uiToolsSelection.isPlayingAnimation = true;
				// isInitShowShop = true;
			} else {
				if (showTime < showDuration) {
					showTime += deltaTime;
				} else {
					isShowingTools = false;
					// isInitShowInfo = true;
				}
			}
		}
	}

	void HideTools () {
		if (isActivatingTools) {
			// isInitShowShop = false;
			uiToolsSelection.isPlayingAnimation = true;
			panelAnimator.Play(Constants.AnimationName.FADE_OUT);
			isActivatingTools = false;
		} else {
			if (!isPlayingAnimation) {
				// isInitShowShop = false;
				panelAnimator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
				uiToolsSelection.isPlayingAnimation = true;
				uiToolsSelection.panelToolsSelection.SetActive(false);
			} else {
				// if (!isChangingTool) {
				// 	panelAnimator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
				// 	uiToolsSelection.isPlayingAnimation = true;
				// 	uiToolsSelection.panelToolsSelection.SetActive(false);
				// }
			}
		}
	}

#region OLD Show & Hide
	// void CheckShowingTools () {
	// 	if (isShowingTools) {
	// 		if (!isActivatingPlayerInfo) {
	// 			uiToolsSelection.panelToolsSelection.SetActive(true);
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
	// 			uiToolsSelection.canvasToolsGroup.alpha = alphaValue;
	// 		} else {
	// 			if (showTime < showDuration) {
	// 				showTime += deltaTime;
	// 			} else {
	// 				isShowingTools = false;
	// 				isInitShowInfo = true;
	// 			}
	// 		}
	// 	}
	// }

	// void HideTools () {
	// 	if (alphaValue > 0f) {
	// 		alphaValue -= deltaTime * hideMultiplier;
	// 		uiToolsSelection.canvasToolsGroup.alpha = alphaValue;
	// 	} else {
	// 		// uiToolsSelection.panelToolsSelection.SetActive(false);
	// 		isActivatingPlayerInfo = false;
	// 	}
	// }
#endregion
	
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
		isShowingTools = true;
		
		if (changeIndex == 1) {
			NextTools();
		} else if (changeIndex == -1) {
			PrevTools();
		}

		// isInitShowInfo = false;
		showTime = 0f;
	}

	void NextTools () {
		// Debug.Log("NextTools "+sliderAnimator.name);
		sliderAnimator.Play(Constants.AnimationName.SLIDE_LEFT);
		int tempToolIdx = uiToolsSelection.toolIndexes[0];
		uiToolsSelection.toolIndexes.RemoveAt(0);
		uiToolsSelection.toolIndexes.Add(tempToolIdx);
	}

	void PrevTools () {
		// Debug.Log("PrevTools "+sliderAnimator.name);
		sliderAnimator.Play(Constants.AnimationName.SLIDE_RIGHT);
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
		// Debug.Log("ResetChange");
		sliderAnimator.Play(Constants.AnimationName.SLIDE_IDLE);
		SetImages();
		SetPrintedTool ();
		uiToolsSelection.changeIndex = 0;
		uiToolsSelection.isToolChange = false;
		isChangingTool = false;
		
		// if (!isActivatingTools) {
		// 	panelAnimator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		// 	uiToolsSelection.isPlayingAnimation = true;
		// 	uiToolsSelection.panelToolsSelection.SetActive(false);
		// }
	}
}
