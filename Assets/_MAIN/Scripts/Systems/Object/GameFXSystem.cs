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
                // PlayFXAudio(particleFX);
            } else {
                Debug.Log("B Playing Particle "+particleFX);
                particleFX.Stop(true);
                particleFX.Play(true);
                // PlayFXAudio(particleFX);
            }
        } else {
            particleFX.Stop(true);
        }
    }

    void PlayFXAudio(ParticleSystem particleFX) {
        if (!gameFX.audioSource.isPlaying) {
            if (particleFX==gameFX.runOnGrassEffect) {
                gameFX.audioSource.clip = gameFX.grassAudio[gameFX.runVariantIdx];
                gameFX.audioSource.Play();
            } else if (particleFX==gameFX.runOnDirtEffect) {
                gameFX.audioSource.clip = gameFX.dirtAudio[gameFX.runVariantIdx];
                gameFX.audioSource.Play();
            } else if (particleFX==gameFX.runOnWaterEffect) {
                gameFX.audioSource.clip = gameFX.waterAudio[gameFX.runVariantIdx];
                gameFX.audioSource.Play();
            } else if (particleFX==gameFX.runOnStoneSwampEffect) {
                gameFX.audioSource.clip = gameFX.stoneSwampAudio[gameFX.runVariantIdx];
                gameFX.audioSource.Play();
            }

            gameFX.runVariantIdx = gameFX.runVariantIdx==0 ? 1 : 0;
        }
    }

	public void ToggleRunFX (bool isON) {
        if (isON) {
            TerrainType currTerrainType = player.terrainType;
            Debug.Log("Play : "+player.terrainType);
            if (currTerrainType == TerrainType.GRASS || currTerrainType == TerrainType.NONE) {
                SetWalkParticle(true, false, false, false);
                PlayFXAudio(gameFX.runOnGrassEffect);
            } else if (currTerrainType == TerrainType.DIRT) {
                SetWalkParticle(false, true, false, false);
                PlayFXAudio(gameFX.runOnDirtEffect);
            } else if (currTerrainType == TerrainType.WATER) {
                SetWalkParticle(false, false, true, false);
                PlayFXAudio(gameFX.runOnWaterEffect);
            } else if (currTerrainType == TerrainType.STONESWAMP) {
                SetWalkParticle(false, false, false, true);
                PlayFXAudio(gameFX.runOnStoneSwampEffect);
            }
        } else SetWalkParticle(false, false, false, false);
	}

    void SetWalkParticle (bool isGrass, bool isDirt, bool isWater, bool isStoneSwamp) {
        if (isGrass) {
            if (!gameFX.runOnGrassEffect.isPlaying) gameFX.runOnGrassEffect.Play(true);
        } 
        else gameFX.runOnGrassEffect.Stop(true);

        if (isDirt) {
            if (!gameFX.runOnDirtEffect.isPlaying) gameFX.runOnDirtEffect.Play(true);
        }  
        else gameFX.runOnDirtEffect.Stop(true);

        if (isWater) {
            if (!gameFX.runOnWaterEffect.isPlaying) gameFX.runOnWaterEffect.Play(true);
        }  
        else gameFX.runOnWaterEffect.Stop(true);

        if (isStoneSwamp) {
            if (!gameFX.runOnStoneSwampEffect.isPlaying) gameFX.runOnStoneSwampEffect.Play(true);
        }  
        else gameFX.runOnStoneSwampEffect.Stop(true);
        
        // ToggleParticleEffect(gameFX.runOnGrassEffect, isGrass);
        // ToggleParticleEffect(gameFX.runOnDirtEffect, isDirt);
        // ToggleParticleEffect(gameFX.runOnWaterEffect, isWater);
        // ToggleParticleEffect(gameFX.runOnStoneSwampEffect, isStoneSwamp);
    }

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

	public void PlaySFXOneShot(AudioClip clip)	{
		gameFX.audioSource.PlayOneShot(clip);
	}
}
