using UnityEngine;

public enum TerrainType {
	NONE,
	DIRT,
	GRASS,
	WATER,
	STONESWAMP
}

public class TerrainTrigger : MonoBehaviour {
	public TerrainType terrainType; 
}
