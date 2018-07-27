using UnityEngine;

public enum CollectibleType {
	NONE,
	GEM_STONE,
	SACRED_STONE,
	MYSTICAL_STONE,
	HP_POTION,
	MANA_POTION
}

public class Collectible : MonoBehaviour {
	public CollectibleType collectibleType;
}
