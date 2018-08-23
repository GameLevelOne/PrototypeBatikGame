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

	protected override void OnUpdate () {
		if (uiToolsSelectionData.Length == 0) return;

		for (int i=0; i<uiToolsSelectionData.Length; i++) {
			uiToolsSelection = uiToolsSelectionData.UIToolsSelection[i];
			tool = uiToolsSelection.tool;
			anim = uiToolsSelectionData.Animation[i];
			
			animator = anim.animator;
			toolSprites = uiToolsSelection.arrayOfToolSprites;
			toolImages = uiToolsSelection.arrayOfToolImages;


			if (!isInitToolImage) {
				InitImages ();
			}

			CheckAnimation ();
			
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

	void InitImages () {
		uiToolsSelection.toolIndexes = new List<int>();

		for (int i=0; i<toolSprites.Length; i++) {
			if (tool.CheckIfToolHasBeenUnlocked(i) > 0) {
				// toolImages[i].sprite = toolSprites[i];
				uiToolsSelection.toolIndexes.Add(i);
			}
		}
		
		SetImages();
		isInitToolImage = true;
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
		uiHPManaToolSystem.PrintTool(toolImages[0].sprite);
		// Debug.Log(toolImages[0].sprite.name);
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
		Debug.Log("CheckStartAnimation");
	}

	void CheckEndAnimation () {
		Debug.Log("CheckEndAnimation");
		ResetChange ();
	}

	void ResetChange () {
		animator.Play(Constants.AnimationName.SLIDE_IDLE);
		SetImages();
		SetPrintedTool ();
		uiToolsSelection.changeIndex = 0;
		uiToolsSelection.isToolChange = false;
		isChangingTool = false;
	}
}
