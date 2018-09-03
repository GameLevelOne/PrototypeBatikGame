﻿using UnityEngine;
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
				CheckShowingShop ();	
			}
		}
	}

	void InitShop () {
		isOpeningShop = false;
		// isInitShowShop = false;
		// timeSwitch = 1;
		// alphaValue = 0f;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		// uiShop.canvasShopGroup.alpha = 0f;
		animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		uiShop.panelUIShop.SetActive(false);
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
			isActivatingShop = true;
		} else {
			if (!isPlayingAnimation) {
				animator.Play(Constants.AnimationName.CANVAS_VISIBLE);
				uiShop.isPlayingAnimation = true;
				// isInitShowShop = true;
			} else {
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton11)) {
					uiShop.isOpeningShop = false;
				}
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
