using UnityEngine;

public enum TreasureType {
	NONE,
	FISH,
	POWERARROW //TEMP
}

public enum LootableType {
	NONE,
	GOLD,
	HP_POTION,
	MANA_POTION
}

public class Lootable : MonoBehaviour {
	public TreasureType treasureType;
	public LootableType lootableType;

	public GameObject initSprite;
	public GameObject mainSprite;

	public bool isLooted = false;
}
