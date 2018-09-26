using UnityEngine;
using Unity.Entities;

public class LootableSystem : ComponentSystem {
	public struct LootableData {
		public readonly int Length;
		public ComponentArray<Lootable> Lootable;
	}
	[InjectAttribute] LootableData lootableData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	Lootable lootable;


	float deltaTime;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;

		for (int i=0; i<lootableData.Length; i++) {
			lootable = lootableData.Lootable[i];

			if (!lootable.isInitLootable) {
				InitLootable();
			} else {
				TreasureType treasureType = lootable.treasureType;
				
				if (!lootable.isDestroyed) {
					if (lootable.isLooted) {
						CheckLootable();
					} else if (treasureType == TreasureType.NONE) {
						if (lootable.destroyTimer < lootable.destroyDuration) {
							lootable.destroyTimer += deltaTime;
						} else {
							lootable.isDestroyed = true;
						}
					}
				} else {
					DestroyLootable();
				}
			}
		}
	}

	void InitLootable () {
		//
		lootable.isInitLootable = true;
	}

	void CheckLootable () {
		//PROCESS LOOTABLE ITEM
		int lootQTY = 0;
		LootableType lootableType = lootable.lootableType;

		lootQTY = lootable.lootQuantity;
		Debug.Log("You got "+lootQTY+" "+lootableType);

		switch (lootableType) { //TEMP
			case LootableType.GOLD: 
				GameStorage.Instance.PlayerCoin += lootQTY;
				break;
			case LootableType.HP_POTION: 
				playerInputSystem.player.health.PlayerHP += lootQTY;
				break;
			case LootableType.MANA_POTION: 
				playerInputSystem.player.mana.PlayerMP += lootQTY;
				break;
			default:
				Debug.Log("Unknown LootableType : "+lootableType);
				break;
		}

		lootable.isDestroyed = true;
	}

	void DestroyLootable () {
		GameObject.Destroy(lootable.gameObject);
		UpdateInjectedComponentGroups();
	}
}
