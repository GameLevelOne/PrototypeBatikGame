using UnityEngine;
using Unity.Entities;

public class PlayerFXSystem : ComponentSystem {
	public struct PlayerFXData {
		public readonly int Length;
		public ComponentArray<PlayerFX> PlayerFX;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] PlayerFXData playerFXData;

	public PlayerFX playerFX;

	Transform playerFXTransform;

	protected override void OnUpdate () {
		if (playerFXData.Length == 0) return;

		for (int i=0; i<playerFXData.Length; i++) {
			playerFX = playerFXData.PlayerFX[i];
			playerFXTransform = playerFXData.Transform[i];

		}
	}

	public void SpawnObj (GameObject obj,  Vector3 spawnPos) {
        GameObject spawnedObj = GameObjectEntity.Instantiate(obj, spawnPos, SetFacing(spawnPos));
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

        spawnedObj.SetActive(true);
    }

	public void SpawnObj (GameObject obj) {
        GameObject spawnedObj = GameObjectEntity.Instantiate(obj, playerFXTransform.position, Quaternion.Euler(new Vector3 (40f, 0f, 0f)));
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

        spawnedObj.SetActive(true);
    }

    Quaternion SetFacing (Vector3 spawnPos) {
        Vector3 targetPos = spawnPos;
        Vector3 initPos = playerFXTransform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.z -= initPos.z;
        float angle = Mathf.Atan2 (targetPos.z, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle));

        return targetRot;
    }
}
