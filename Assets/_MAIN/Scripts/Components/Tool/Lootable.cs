using UnityEngine;

public enum TreasureType {
	NONE,
	FISH,
	POWERARROW,
	KEY,
	FISHINGROD
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

	[HeaderAttribute("Non Treasure Only")]
	public float destroyDuration = 0f;

	[HeaderAttribute("Treasure Only")]
	public TreasureType treasureType;
	public GameObject initSprite;
	
	[HeaderAttribute("Treasure KEY Only")]
	public int keyID;

	[HeaderAttribute("Current")]
	public bool isInitLootable = false;
	public bool isLooted = false;
	public bool isDestroyed = false;
	public float destroyTimer = 0f;

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag == Constants.Tag.GROUND){
	// 		mainCol.isTrigger = true;
	// 	}
	// }
}
