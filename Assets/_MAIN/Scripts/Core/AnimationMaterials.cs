using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New AnimationMaterials")]
public class AnimationMaterials : ScriptableObject {
	public new string animationName;
	public int tileX;
	public int tileY;
	public bool isLooping;
	
	[HeaderAttribute("If can spawn only")]
	public bool isCanSpawningSomething = false;
	public int spawnIndex;

	[HeaderAttribute("Down, Left, Up, Right")]
	public Material[] materials = new Material[4]; //4 Directional
}
