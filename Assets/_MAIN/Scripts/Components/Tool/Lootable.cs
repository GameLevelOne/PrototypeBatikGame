using UnityEngine;

public enum LootableType {
	NONE,
	GOLD,
	HP_POTION,
	MANA_POTION
}

public class Lootable : MonoBehaviour {
	public LootableType lootableType;
}
