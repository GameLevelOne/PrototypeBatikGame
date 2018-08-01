using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class SecretDigSystem : ComponentSystem {
	public struct SecretDigData {
		public readonly int Length;
		public ComponentArray<SecretDig> secretDig;
	}
	[InjectAttribute] SecretDigData secretDigData;

	SecretDig secretDig;

	protected override void OnUpdate () {
		if (secretDigData.Length == 0) return;

		for (int i=0; i<secretDigData.Length; i++) { 
			secretDig = secretDigData.secretDig[i];
			
			if (secretDig.IsSecretDigHit && !secretDig.isAlreadyDigged) {
				//Spawn something
				GameObjectEntity.Instantiate(secretDig.secretRewardObj, secretDig.transform.position, Quaternion.identity);

				secretDig.IsSecretDigHit = false;
				secretDig.isAlreadyDigged = true;
			}
		}

		// foreach (var e in GetEntities<SecretDigComponent>()) {
		// 	SecretDig secretDig = e.secretDig;
			
		// 	if (secretDig.IsSecretDigHit && !secretDig.isAlreadyDigged) {
		// 		//Spawn something
		// 		GameObjectEntity.Instantiate(secretDig.secretRewardObj, secretDig.transform.position, Quaternion.identity);

		// 		secretDig.IsSecretDigHit = false;
		// 		secretDig.isAlreadyDigged = true;
		// 	}
		// }
	}
}
