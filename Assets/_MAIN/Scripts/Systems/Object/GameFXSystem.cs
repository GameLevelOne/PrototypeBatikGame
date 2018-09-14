using UnityEngine;
using Unity.Entities;

public class GameFXSystem : ComponentSystem {
	public struct GameFXData {
		public readonly int Length;
		public ComponentArray<GameFX> GameFX;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] GameFXData gameFXData;

	public GameFX gameFX;

	// Transform gameFXTransform;

	protected override void OnUpdate () {
		if (gameFXData.Length == 0) return;

		for (int i=0; i<gameFXData.Length; i++) {
			gameFX = gameFXData.GameFX[i];
			// playerFXTransform = gameFXData.Transform[i];

            if (gameFX.isEnableDodgeEffect) {
                PlayDodgeEffect ();
            }
		}
	}

	public void SpawnObj (GameObject obj,  Vector3 spawnPos, Vector3 spawnInitPos) {
        GameObject spawnedObj = GameObjectEntity.Instantiate(obj, spawnPos, SetFacing(spawnPos, spawnInitPos));
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

        spawnedObj.SetActive(true);
    }

	public void SpawnObj (GameObject obj,  Vector3 spawnPos) {
        GameObject spawnedObj = GameObjectEntity.Instantiate(obj, spawnPos, Quaternion.Euler(new Vector3 (40f, 0f, 0f)));
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

        spawnedObj.SetActive(true);
    }

    public void ToggleEffect (GameObject effectObj, bool value) {
        if (effectObj.activeSelf != value) {
            effectObj.SetActive(value);
            
            // Debug.Log("Toogle effect "+effectObj+" to "+value);
        }
    }

    public void ToggleDodgeFlag (bool value) {
        gameFX.isEnableDodgeEffect = value;

        // if (value && !gameFX.dodgeEffect.isPlaying) {
        //     gameFX.dodgeEffect.Play();
        // } else {
        //     gameFX.dodgeEffect.Stop();
        //     Debug.Log("StopDodgeEffect");
        // }
        // Debug.Log(value);
    }

    void PlayDodgeEffect () {
        if (!gameFX.dodgeEffect.isPlaying) {
            gameFX.dodgeEffect.Play();
            // Debug.Log("PlayDodgeEffect");
        }
        
        gameFX.dodgeEffect.GetComponent<Renderer>().material.mainTexture = gameFX.playerSprite.sprite.texture;
    }

    Quaternion SetFacing (Vector3 spawnPos, Vector3 spawnInitPos) {
        Vector3 targetPos = spawnPos;
        Vector3 initPos = spawnInitPos; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.z -= initPos.z;
        float angle = Mathf.Atan2 (targetPos.z, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle));

        return targetRot;
    }
}
