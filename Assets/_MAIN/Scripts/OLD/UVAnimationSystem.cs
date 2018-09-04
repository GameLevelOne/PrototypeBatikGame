using UnityEngine;
using Unity.Entities;

public class UVAnimationSystem : ComponentSystem {
	public struct UVAnimationData {
		public readonly int Length;
		public ComponentArray<UVAnimation> UVAnimation;
	}
	[InjectAttribute] UVAnimationData uvAnimationData;

	UVAnimation currentUVAnimation;

    // int currentAnimMatIndex;
    int currentUVIndex;
    int currentUVTileX;
    int currentUVTileY;
    Vector2 currentUVSize;
    float justTime;

	protected override void OnUpdate () {
		if (uvAnimationData.Length == 0) return;

		justTime = Time.time;

		for (int i=0; i<uvAnimationData.Length; i++) {
			currentUVAnimation = uvAnimationData.UVAnimation[i];

            if (!currentUVAnimation.isInitUV) {
                try {
                    InitUV ();
                } catch {
                    Debug.Log("Something is wrong when initiating UVAnimation");
                    continue;
                }

                currentUVAnimation.isInitUV = true;
            } else {
                // currentAnimMatIndex = currentUVAnimation.animationMaterialIndex;
                currentUVIndex = currentUVAnimation.uvIndex;
                currentUVTileX = currentUVAnimation.uvTileX;
                currentUVTileY = currentUVAnimation.uvTileY; 
                currentUVSize = currentUVAnimation.uvSize;         
                CheckUVMaterial ();
            }
        }
    }

    void InitUV () {
        currentUVAnimation.animationMaterialIndex = 0;
        currentUVAnimation.materialIndex = 0;

        //Set first rendered material (index 0, for DOWN)
        SetAnimationMaterials (0);
    }

    void CheckUVMaterial () {
        currentUVIndex = (int)(justTime * currentUVAnimation.fps);

        currentUVIndex %= (currentUVTileX * currentUVTileY);

        if (!currentUVAnimation.isLooping) {
            CheckUVIndex ((currentUVTileX * currentUVTileY)-1);
        }

        int horIndex = currentUVIndex % currentUVTileX;
        int verIndex = currentUVIndex / currentUVTileX;

        Vector2 offset = new Vector2 (horIndex * currentUVSize.x, 1.0f - currentUVSize.y - verIndex * currentUVSize.y);

        currentUVAnimation.uvRenderer.material.SetTextureOffset ("_MainTex", offset);

        currentUVAnimation.uvIndex = currentUVIndex;
        currentUVAnimation.uvTileX = currentUVTileX;
        currentUVAnimation.uvTileY = currentUVTileY;
    }

    void CheckUVIndex (int endIndex) {
        if (currentUVIndex == 0) {
            currentUVAnimation.uvAnimControl.StartAnim();
        } else if (currentUVIndex == endIndex) {
            currentUVAnimation.uvAnimControl.EndAnim();
        } else {
            if (currentUVAnimation.isCanSpawningSomething) {
                if (currentUVIndex == currentUVAnimation.spawnIndex) {
                    currentUVAnimation.uvAnimControl.SpawnSomethingAnim();
                }
            }
        }
    }

    public void SetAnimationMaterials (int animMatIndex) {
        currentUVAnimation.animationMaterialIndex = animMatIndex;

        currentUVAnimation.uvTileX = currentUVAnimation.animationMaterials[animMatIndex].tileX;
        currentUVAnimation.uvTileY = currentUVAnimation.animationMaterials[animMatIndex].tileY;
        currentUVAnimation.isLooping = currentUVAnimation.animationMaterials[animMatIndex].isLooping;
        currentUVAnimation.isCanSpawningSomething = currentUVAnimation.animationMaterials[animMatIndex].isCanSpawningSomething;
        currentUVAnimation.spawnIndex = currentUVAnimation.animationMaterials[animMatIndex].spawnIndex;

        SetMaterial(currentUVAnimation.materialIndex);
    }

    public void SetMaterial (int matIndex) {
        currentUVAnimation.materialIndex = matIndex;
        
        currentUVAnimation.uvRenderer.material = currentUVAnimation.animationMaterials[currentUVAnimation.animationMaterialIndex].materials[matIndex];
        
        //Set size of each tile
        currentUVAnimation.uvSize = new Vector2(1.0f / currentUVAnimation.uvTileX, 1.0f / currentUVAnimation.uvTileY);

        //Set texture scale by its tile size
        currentUVAnimation.uvRenderer.material.SetTextureScale ("_MainTex", currentUVAnimation.uvSize);  
    }
}