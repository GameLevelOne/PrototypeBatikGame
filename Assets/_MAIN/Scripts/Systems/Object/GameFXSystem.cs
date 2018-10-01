using UnityEngine;
using Unity.Entities;

public class GameFXSystem : ComponentSystem {
	public struct GameFXData {
		public readonly int Length;
		public ComponentArray<GameFX> GameFX;
		public ComponentArray<Player> Player;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] GameFXData gameFXData;

	public GameFX gameFX;
    Player player;

	// Transform gameFXTransform;

	protected override void OnUpdate () {
		if (gameFXData.Length == 0) return;

		for (int i=0; i<gameFXData.Length; i++) {
			gameFX = gameFXData.GameFX[i];
            player = gameFXData.Player[i];
			// playerFXTransform = gameFXData.Transform[i];

            // if (gameFX.isEnableDodgeEffect) {
            //     PlayDodgeEffect ();
            // }
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

    public void ToggleObjectEffect (GameObject objFX, bool value) {
        if (objFX.activeSelf != value) {
            objFX.SetActive(value);
            
            // Debug.Log("Toogle effect "+effectObj+" to "+value);
        }
    }

    public void ToggleParticleEffect (ParticleSystem particleFX, bool value) {
        if (value) {
            if (!particleFX.isPlaying) {
                Debug.Log("A Playing Particle "+particleFX);
                particleFX.Play(true);
                PlayFXAudio(particleFX);
            } else {
                Debug.Log("B Playing Particle "+particleFX);
                particleFX.Stop(true);
                particleFX.Play(true);
                PlayFXAudio(particleFX);
            }
        } else {
            particleFX.Stop(true);
        }
    }

    void PlayFXAudio(ParticleSystem particleFX) {
        // if (!gameFX.audioSource.isPlaying) {
        //     if (particleFX==gameFX.runOnDirtEffect) {
        //         gameFX.audioSource.clip = gameFX.dirtAudio[gameFX.runVariantIdx];
        //         gameFX.audioSource.Play();
        //     } else if (particleFX==gameFX.runOnGrassEffect) {
        //         gameFX.audioSource.clip = gameFX.grassAudio[gameFX.runVariantIdx];
        //         gameFX.audioSource.Play();
        //     } else if (particleFX==gameFX.runOnWaterEffect) {
        //         gameFX.audioSource.clip = gameFX.waterAudio[gameFX.runVariantIdx];
        //         gameFX.audioSource.Play();
        //     }
        //     gameFX.runVariantIdx = gameFX.runVariantIdx==0 ? 1 : 0;
        // }
    }

	public void ToggleRunFX (bool isON) {
        if (isON) {
            // Debug.Log("Play : "+player.terrainType);
            if (player.terrainType == TerrainType.DIRT) {
                ToggleParticleEffect(gameFX.runOnDirtEffect, true);
            } else if (player.terrainType == TerrainType.GRASS || (player.terrainType == TerrainType.NONE)) {
                ToggleParticleEffect(gameFX.runOnGrassEffect, true);
            } else if (player.terrainType == TerrainType.WATER) {
                ToggleParticleEffect(gameFX.runOnWaterEffect, true);
            }
        } else {
            // Debug.Log("Stop : "+player.terrainType);
            // if (player.terrainType == TerrainType.DIRT) {
                ToggleParticleEffect(gameFX.runOnDirtEffect, false);
            // } else if (player.terrainType == TerrainType.GRASS || (player.terrainType == TerrainType.NONE)) {
                ToggleParticleEffect(gameFX.runOnGrassEffect, false);
            // } else if (player.terrainType == TerrainType.WATER) {
                ToggleParticleEffect(gameFX.runOnWaterEffect, false);
            // }
        }
            Debug.Log("DIRT is Playing : "+gameFX.runOnDirtEffect.isPlaying);
	}

    // public void ToggleDodgeFlag (bool value) {
    //     gameFX.isEnableDodgeEffect = value;

        // if (value && !gameFX.dodgeEffect.isPlaying) {
        //     gameFX.dodgeEffect.Play();
        // } else {
        //     gameFX.dodgeEffect.Stop();
        //     Debug.Log("StopDodgeEffect");
        // }
        // Debug.Log(value);
    // }

    // public void PlayDodgeEffect () {
    //     if (!gameFX.dodgeEffect.isPlaying) {
    //         gameFX.dodgeEffect.Play();
    //     } else {
    //         gameFX.dodgeEffect.Stop();
    //         gameFX.dodgeEffect.Play();
    //     }
    // }

    public void PlayCounterChargeEffect () {
        if (!gameFX.counterChargeEffect.isPlaying) {
            gameFX.counterChargeEffect.Play();
        }
        
        gameFX.counterChargeEffect.GetComponent<Renderer>().material.mainTexture = gameFX.playerSprite.sprite.texture;
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
