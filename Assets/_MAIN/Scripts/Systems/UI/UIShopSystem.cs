using UnityEngine;
using UnityEngine.EventSystems;
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
	bool curDownPressed;
	bool curUpPressed;

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
					Debug.Log("Error init UIShopSystem");
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
		curDownPressed = false;
		curUpPressed = false;

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
		if (isOpeningShop) {
			if (GameInput.IsDodgePressed) {
				uiShop.isOpeningShop = false;
			}
			if (!curUpPressed && GameInput.IsUpDirectionHeld) {
				curUpPressed = true;
				PrevButtonTool ();
			} else if (!GameInput.IsUpDirectionHeld) {
				curUpPressed = false;
			}

			if (!curDownPressed && GameInput.IsDownDirectionHeld) {
				curDownPressed = true;
				NextButtonTool ();
			} else if (!GameInput.IsDownDirectionHeld) {
				curDownPressed = false;
			} 
			if (GameInput.IsAttackPressed) {
				BuyItemShop ();
			}
		}
	}

	void NextButtonTool () {
		if (buttonIndex >= buttonItemLength-1){
			buttonIndex = 0;
		} else {
			buttonIndex++;
		}

		SelectItemShop();
	}

	void PrevButtonTool () {
		if (buttonIndex <= 0){
			buttonIndex = buttonItemLength-1;
		} else {
			buttonIndex--;
		}

		SelectItemShop();
	}

	void SelectItemShop () {
		uiShop.itemShops[buttonIndex].buttonItem.Select();
	}

	void BuyItemShop () {
		uiShop.BuyItem(uiShop.itemShops[buttonIndex].lootableType, uiShop.itemShops[buttonIndex].itemPrice);
	}

	void CheckShowingShop () {
		if (isOpeningShop) {
			ShowShop ();
		} else {
			HideShop ();
		}
	}

	void ShowShop () {
		if (!isActivatingShop) {
			Time.timeScale = 0;
			uiShop.panelUIShop.SetActive(true);
			uiShop.isPlayingAnimation = true;
			animator.Play(Constants.AnimationName.FADE_IN);
			// isInitShowShop = false;
			EventSystem.current.SetSelectedGameObject(null);
			uiShop.itemShops[buttonIndex].buttonItem.Select();
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
