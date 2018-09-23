using UnityEngine;
using Unity.Entities;

public class ChestSpawner : MonoBehaviour {
	public GameObject chestObj;
	// public GameObjectEntity chestEntity;
	// public Collider chestCol;
	// public Renderer chestSpriteRen;

	[HeaderAttribute("Saved ID")]
	public int chestSpawnerID;
	public int requiredSpawnTrigger;

	[HeaderAttribute("Current")]
	public bool isInitChestSpawner;
	public bool isSpawned;
	public int currentTotalSpawnTrigger;
}
