using UnityEngine;
using Unity.Entities;

public class UIShopSystem : ComponentSystem {
	public struct UIShopData {
		public readonly int Length;
		public ComponentArray<UIShop> UIShop;
	}
	[InjectAttribute] UIShopData uiShopData;
	
	UIShop uiShop;

	bool isInitShop = false;
	bool isShowingShop = false;
	bool isAfterPressPause = false;
	bool isActivatingShop = false;
	bool isInitShowShop = false;
	// int timeSwitch = 1;
	float showTime;
	float showMultiplier;
	float hideMultiplier;
	float alphaValue;
	float deltaTime;

	protected override void OnUpdate () {
		if (uiShopData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<uiShopData.Length; i++) {
			uiShop = uiShopData.UIShop[i];
			showMultiplier = uiShop.showMultiplier;
			hideMultiplier = uiShop.hideMultiplier;

			if (!isInitShop) {
				try {
					InitPlayerInfo ();
				} catch {
					return;
				}

				isInitShop = true;
			} else {
				isShowingShop = uiShop.isOpeningShop;
				CheckShowingShop ();
			}
		}
	}

	void InitPlayerInfo () {
		isShowingShop = false;
		isInitShowShop = false;
		// timeSwitch = 1;
		alphaValue = 0f;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		uiShop.canvasShopGroup.alpha = 0f;
		uiShop.panelUIShop.SetActive(false);
	}

	void CheckShowingShop () {
		if (isShowingShop) {
			if (!isActivatingShop) {
				uiShop.panelUIShop.SetActive(true);
				// isInitShowShop = false;
				isActivatingShop = true;
			} else {
				ShowShop ();
			}
		} else if (!isShowingShop && Time.timeScale == 0) {
			Time.timeScale = 1;
		} else {
			HideShop ();
		}
	}

	void ShowShop () {
		if (!isInitShowShop) {
			if (alphaValue < 1f) {
				alphaValue += deltaTime * showMultiplier;
				uiShop.canvasShopGroup.alpha = alphaValue;
			} else {
				Time.timeScale = 0;
				isInitShowShop = true;
			}
		}
	}

	void HideShop () {
		if (isInitShowShop) {
			if (alphaValue > 0f) {
				alphaValue -= deltaTime * hideMultiplier;
				uiShop.canvasShopGroup.alpha = alphaValue;
			} else {
				uiShop.panelUIShop.SetActive(false);
				isActivatingShop = false;
				isInitShowShop = false;
			}	
		}
	}
}
