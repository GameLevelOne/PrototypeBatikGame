using UnityEngine;
using Unity.Entities;

public class SecretDigSystem : ComponentSystem {
	public struct SecretDigData {
		public readonly int Length;
		public ComponentArray<SecretDig> secretDig;
	}
	[InjectAttribute] SecretDigData secretDigData;

	[InjectAttribute] LootableSpawnerSystem lootableSpawnerSystem;

	SecretDig secretDig;

	protected override void OnUpdate () {
		if (secretDigData.Length == 0) return;

		for (int i=0; i<secretDigData.Length; i++) { 
			secretDig = secretDigData.secretDig[i];
			
			if (secretDig.isSecretDigHit && !secretDig.isAlreadyDigged) {
				// //Spawn something
				// GameObjectEntity.Instantiate(secretDig.secretRewardObj, secretDig.digResultPos, Quaternion.identity);

				//SPAWN ITEM
				lootableSpawnerSystem.CheckPlayerLuck(secretDig.spawnItemProbability, secretDig.digResultPos);

				secretDig.isAlreadyDigged = true;
			}
		}
	}
}
