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
	public GameObject lootableFX;
	public float initPosY;
	// public Collider mainCol;

	[HeaderAttribute("NONE TreasureType Only")]
	public Animator parentAnimator;
	public int lootQuantity;
	public float startDestroyDuration = 0f;

	[HeaderAttribute("Treasure Only")]
	public TreasureType treasureType;

	[HeaderAttribute("FISH TreasureType Only")]
	public GameObject initSprite;
	public GameObject mainSprite;
	
	[HeaderAttribute("KEY TreasureType Only")]
	public int keyID;

	[HeaderAttribute("Current")]
	public bool isInitLootable = false;
	public bool isLooted = false;
	public bool isInitDestroy = false;
	public bool isDestroyed = false;
	public float destroyTimer = 0f;

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag == Constants.Tag.GROUND){
	// 		mainCol.isTrigger = true;
	// 	}
	// }

	void DestroyMe () {
		isDestroyed = true;
	}
}
