using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour {
	public delegate void ShopControl(LootableType type, int price);
	public event ShopControl OnBuyItem;	

	public AnimationControl animationControl;
	public Animator animator;

	public Text playerMoneyLabel;
	public Text healthPriceLabel;
	public Text manaPriceLabel;

	public Animator handL;
	public Animator handR;
	public Animator[] healthContainer; 
	public Animator[] manaContainer; 


	public GameObject panelUIShop;
	[HeaderAttribute("Data")]
	public int healthPrice;
	public int manaPrice;
	
	[HeaderAttribute("Current")]
	public bool isInitShop;
	public bool isOpeningShop;
	public bool isShopFade;
	public bool isPlayingAnimation;
	public float buttonDelay;
	public LootableType curType;
	public bool leftPressed;
	public bool rightPressed;

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
