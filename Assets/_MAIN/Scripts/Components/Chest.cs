using UnityEngine;

public enum ChestType {
	NORMAL,
	TREASURE,
}

public class Chest : MonoBehaviour {
	public ChestType chestType;
	public SpriteRenderer chestSpriteRen;
	
	public Sprite closedChestSprite;
	public Sprite openedChestSprite;
	public GameObject treasurePrize;
	public GameObject[] normalPrizes;
	public QuestTrigger questTrigger;

	[HeaderAttribute("Saved ID")]
	public int chestID;

	[HeaderAttribute("Current")]
	public bool isInitChest = false;
	public bool isOpened = false;
	public bool isSelected = false;
	
	// [HeaderAttribute("Testing")]
	// public bool resetPrefKey;

	void Awake () {
		// if (resetPrefKey) {
		// 	PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + chestID, 0);
		// }
	}
}
