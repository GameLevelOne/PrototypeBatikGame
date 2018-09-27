using UnityEngine;

public class Container : MonoBehaviour {
	[HeaderAttribute("4 Container types")]
	public LootableType[] lootableTypes = new LootableType[4];

	[SpaceAttribute(10f)]
	public Player player;
	public LootableType boughtItemLootabelType;
	
	// [HeaderAttribute("Quantity of Collectible types")]
	// public GameObject[] collectibleObj;

	public bool CheckIfContainerIsEmpty (int collectibleTypeIdx) {
		if (lootableTypes[collectibleTypeIdx] == LootableType.NONE) {
			return true;
		} else {
			return false;
		}
	}
}
