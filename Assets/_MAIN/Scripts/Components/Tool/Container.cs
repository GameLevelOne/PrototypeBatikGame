using UnityEngine;

public class Container : MonoBehaviour {
	[HeaderAttribute("4 Container types")]
	public LootableType[] lootableTypes = new LootableType[4];

	[SpaceAttribute(10f)]
	public Player player;
	public UIShop uiShop;
	public LootableType boughtItemLootabelType;
	
	// [HeaderAttribute("Quantity of Collectible types")]
	// public GameObject[] collectibleObj;

	public bool isProcessingBuyItem;

	public int boughtItemPrice;

	void OnEnable () {
		uiShop.OnBuyItem += OnBuyItem;
	}

	void OnDisable () {
		if (uiShop!=null)
			uiShop.OnBuyItem -= OnBuyItem;
	}

	void OnBuyItem (LootableType type, int price) {
		boughtItemLootabelType = type;
		boughtItemPrice = price;
		isProcessingBuyItem = true;
	}

	public bool CheckIfContainerIsEmpty (int collectibleTypeIdx) {
		if (lootableTypes[collectibleTypeIdx] == LootableType.NONE) {
			return true;
		} else {
			return false;
		}
	}
}
