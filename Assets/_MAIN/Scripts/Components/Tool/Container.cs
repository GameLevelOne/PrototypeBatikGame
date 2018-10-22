using UnityEngine;

public class Container : MonoBehaviour {
	[HeaderAttribute("4 Container types")]
	public LootableType[] lootableTypes = new LootableType[4];

	public bool isInitContainer = false;

	[SpaceAttribute(10f)]
	public Player player;
	public LootableType boughtItemLootabelType;
	
	// [HeaderAttribute("Quantity of Collectible types")]
	// public GameObject[] collectibleObj;

	[HeaderAttribute("DEBUGGING ONLY")]
	public bool isTestSaveContainer = false;

	void Start () {
		//TESTING
		if (isTestSaveContainer) SaveContainerDummy();
	}

	public bool CheckIfContainerIsEmpty (int collectibleTypeIdx) {
		if (lootableTypes[collectibleTypeIdx] == LootableType.NONE) {
			return true;
		} else {
			return false;
		}
	}

	void SaveContainerDummy () {
		for (int i=0; i<lootableTypes.Length; i++) {
			switch (lootableTypes[i]) {
				case LootableType.HP_POTION:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 1);
					break;
				case LootableType.MANA_POTION:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 2);
					break;
				default:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 0);
					break;
			}
		}
	}
}
