using UnityEngine;
// using UnityEngine.UI;

public class UIShop : MonoBehaviour {
	public delegate void ShopControl(LootableType type, int price);
	public event ShopControl OnBuyItem;	

	public UIItemShop[] itemShops;

	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelUIShop;
	// public CanvasGroup canvasShopGroup;
	// public float showMultiplier;
	// public float hideMultiplier;
	public bool isOpeningShop;
	public bool isPlayingAnimation;

	void OnEnable () {
		animationControl.OnExitAnimation += OnExitAnimation;
	}

	void OnDisable () {
		animationControl.OnExitAnimation -= OnExitAnimation;
	}

	void OnExitAnimation () {
		isPlayingAnimation = false;
	}

	public void BuyItem (LootableType type, int price) {
		if (OnBuyItem != null) {
			OnBuyItem(type, price);
		}
	}
}
