using UnityEngine;
// using System.Collections;

public class UVAnimation : MonoBehaviour {
	public AnimationMaterials[] animationMaterials;
	
	[HeaderAttribute("UV Animation Attributes")]
	public Renderer uvRenderer;
	public UVAnimationControl uvAnimControl;
	public int uvTileX;
	public int uvTileY;
	public int fps = 30;
	public bool isLooping = false;
	
	[HeaderAttribute("If can spawn only")]
	public bool isCanSpawningSomething = false;
	public int spawnIndex;

	[HeaderAttribute("Current")]
	public int animationMaterialIndex;
	public int materialIndex;
	public int uvIndex;	
	public Vector2 uvSize;
	public bool isInitUV = false;
	public bool isCheckBeforeAnimation = false;
	public bool isCheckAfterAnimation = false;
	public bool isSpawnSomethingOnAnimation = false;
	public bool isChangeDirection = false;
	
	void OnEnable () {
		uvAnimControl.OnStartAnimation += StartAnimation;
		uvAnimControl.OnEndAnimation += ExitAnimation;
		uvAnimControl.OnSpawnSomethingOnAnimation += SpawnSomethingOnAnimation;
	}

	void OnDisable () {
		uvAnimControl.OnStartAnimation -= StartAnimation;
		uvAnimControl.OnEndAnimation -= ExitAnimation;
		uvAnimControl.OnSpawnSomethingOnAnimation -= SpawnSomethingOnAnimation;
	}

	void StartAnimation () {
		isCheckBeforeAnimation = false;
	}

	void ExitAnimation () {
		isCheckAfterAnimation = false;
	}

	void SpawnSomethingOnAnimation () {
		isSpawnSomethingOnAnimation = false;
	}

	// void Update () {
	// 	//Calculate the index
	// 	uvIndex = (int)(Time.time * fps);
		
	// 	//Repeat when exhausting all frames
	// 	uvIndex %= (uvTileX * uvTileY);
		
	// 	//size of each tile
	// 	Vector2 size = new Vector2(1.0f / uvTileX, 1.0f / uvTileY);

	// 	//Split into horizontal and vertical indexes
	// 	int horIndex = uvIndex % uvTileX;
	// 	int verIndex = uvIndex / uvTileX;

	// 	//Build offset
	// 	Vector2 offset = new Vector2 (horIndex * size.x, 1.0f - size.y - verIndex * size.y);

	// 	renderer.material.SetTextureOffset ("_MainTex", offset);
	// 	renderer.material.SetTextureScale ("_MainTex", size);
	// }
}
