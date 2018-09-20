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
	MANA_POTION,
	MANA_REGEN
}

public class Lootable : MonoBehaviour {
	public LootableType lootableType;
	public GameObject mainSprite;
	public int lootQuantity;
	public float initPosY;
	// public Collider mainCol;


	[HeaderAttribute("Treasure Only")]
	public TreasureType treasureType;
	public GameObject initSprite;

	[HeaderAttribute("Current")]
	public bool isLooted = false;

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag == Constants.Tag.GROUND){
	// 		mainCol.isTrigger = true;
	// 	}
	// }
}
