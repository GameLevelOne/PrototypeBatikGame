using UnityEngine;
using Unity.Entities;

public class UVAnimationSystem : ComponentSystem {
	public struct UVAnimationData {
		public readonly int Length;
		public ComponentArray<UVAnimation> UVAnimation;
	}
	[InjectAttribute] UVAnimationData uvAnimationData;

	UVAnimation currentUVAnimation;

    int currentAnimMatIndex;
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
                currentAnimMatIndex = currentUVAnimation.animationMaterialIndex;
                currentUVIndex = currentUVAnimation.uvIndex;
                currentUVTileX = currentUVAnimation.uvTileX;
                currentUVTileY = currentUVAnimation.uvTileY; 
                currentUVSize = currentUVAnimation.uvSize;         
                CheckUVMaterial ();
            }
        }
    }

    void InitUV () {
        //Set size of each tile
        currentUVAnimation.uvSize = new Vector2(1.0f / currentUVAnimation.uvTileX, 1.0f / currentUVAnimation.uvTileY);

        //Set first rendered material (index 0, for DOWN)
        SetMaterialUV(0, 0);
        SetMaterialAttributes (0);
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

    void SetMaterialAttributes (int index) {
        currentUVAnimation.uvTileX = currentUVAnimation.animationMaterials[index].tileX;
        currentUVAnimation.uvTileY = currentUVAnimation.animationMaterials[index].tileY;
        currentUVAnimation.isLooping = currentUVAnimation.animationMaterials[index].isLooping;
        currentUVAnimation.isCanSpawningSomething = currentUVAnimation.animationMaterials[index].isCanSpawningSomething;
        currentUVAnimation.spawnIndex = currentUVAnimation.animationMaterials[index].spawnIndex;
    }

    public void SetMaterialUV (int animMatIndex, int matIndex) {
        currentUVAnimation.animationMaterialIndex = animMatIndex;
        currentUVAnimation.materialIndex = matIndex;
        // Debug.Log(animMatIndex+" , "+matIndex);
        currentUVAnimation.uvRenderer.material = currentUVAnimation.animationMaterials[animMatIndex].materials[matIndex];
        
        //Set texture scale by its tile size
        currentUVAnimation.uvRenderer.material.SetTextureScale ("_MainTex", currentUVAnimation.uvSize);  
    }
}