using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class SecretDigSystem : ComponentSystem {

	public struct SecretDigComponent {
		public SecretDig secretDig;
	}

	protected override void OnUpdate () {
		foreach (var e in GetEntities<SecretDigComponent>()) {
			SecretDig secretDig = e.secretDig;
			
			if (secretDig.IsSecretDigHit && !secretDig.isAlreadyDigged) {
				//Spawn something
				GameObjectEntity.Instantiate(secretDig.secretRewardObj, secretDig.transform.position, Quaternion.identity);

				secretDig.IsSecretDigHit = false;
				secretDig.isAlreadyDigged = true;
			}
		}
	}
}
