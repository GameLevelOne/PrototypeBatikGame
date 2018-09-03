using UnityEngine;
using Unity.Entities;

public class UIShopSystem : ComponentSystem {
	public struct UIShopData {
		public readonly int Length;
		public ComponentArray<UIShop> UIShop;
	}
	[InjectAttribute] UIShopData uiShopData;
	
	UIShop uiShop;

	Animator animator;

	bool isInitShop = false;
	bool isOpeningShop = false;
	bool isPlayingAnimation = false;
	// bool isAfterPressPause = false;
	bool isActivatingShop = false;
	// bool isInitShowShop = false;
	// int timeSwitch = 1;
	// float showTime;
	// float showMultiplier;
	// float hideMultiplier;
	// float alphaValue;
	// float deltaTime;
	int buttonIndex;
	int buttonItemLength;

	protected override void OnUpdate () {
		if (uiShopData.Length == 0) return;

		// deltaTime = Time.deltaTime;

		for (int i=0; i<uiShopData.Length; i++) {
			uiShop = uiShopData.UIShop[i];
			
			animator = uiShop.animator;
			// showMultiplier = uiShop.showMultiplier;
			// hideMultiplier = uiShop.hideMultiplier;

			if (!isInitShop) {
				try {
					InitShop ();
				} catch {
					return;
				}

				isInitShop = true;
			} else {
				isOpeningShop = uiShop.isOpeningShop;
				isPlayingAnimation = uiShop.isPlayingAnimation;
				CheckInput ();
				CheckShowingShop ();	
			}
		}
	}

	void InitShop () {
		isOpeningShop = false;
		buttonIndex = 0;
		buttonItemLength = uiShop.itemShops.Length;
		// isInitShowShop = false;
		// timeSwitch = 1;
		// alphaValue = 0f;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		// uiShop.canvasShopGroup.alpha = 0f;
		animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		uiShop.itemShops[buttonIndex].buttonItem.Select();
		uiShop.panelUIShop.SetActive(false);
	}

	void CheckInput () {
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			PrevButtonTool ();
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			NextButtonTool ();
		} else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton11)) {
			if (isActivatingShop) {
				uiShop.isOpeningShop = false;
			}
		}
	}

	void NextButtonTool () {
		if (buttonIndex >= buttonItemLength-1){
			buttonIndex = 0;
		} else {
			buttonIndex++;
		}

		SelectTool(buttonIndex);
	}

	void PrevButtonTool () {
		if (buttonIndex <= 0){
			buttonIndex = buttonItemLength-1;
		} else {
			buttonIndex--;
		}

		SelectTool(buttonIndex);
	}

	void SelectTool (int idx) {
		uiShop.itemShops[buttonIndex].buttonItem.Select();
		uiShop.BuyItem(uiShop.itemShops[idx].lootableType, uiShop.itemShops[idx].itemPrice);
	}

	void CheckShowingShop () {
		if (isOpeningShop) {
			ShowShop ();
		} else {
			HideShop ();
		}
		Debug.Log("Set timescale CheckShowingShop : "+Time.timeScale);
	}

	void ShowShop () {
		if (!isActivatingShop) {
			Time.timeScale = 0;
			uiShop.panelUIShop.SetActive(true);
			uiShop.isPlayingAnimation = true;
			animator.Play(Constants.AnimationName.FADE_IN);
			// isInitShowShop = false;
			isActivatingShop = true;
		} else {
			if (!isPlayingAnimation) {
				animator.Play(Constants.AnimationName.CANVAS_VISIBLE);
				uiShop.isPlayingAnimation = true;
				// isInitShowShop = true;
			} else {
				//
			}
		}
	}

	void HideShop () {
		if (isActivatingShop) {
			// isInitShowShop = false;
			uiShop.isPlayingAnimation = true;
			animator.Play(Constants.AnimationName.FADE_OUT);
			isActivatingShop = false;
		} else {
			if (!isPlayingAnimation) {
				// isInitShowShop = false;
				animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
				uiShop.isPlayingAnimation = true;
				uiShop.panelUIShop.SetActive(false);
				Time.timeScale = 1;
			}
		}
	}
}
