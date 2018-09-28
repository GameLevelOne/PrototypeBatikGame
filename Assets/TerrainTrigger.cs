using UnityEngine;

public enum TerrainType {
	NONE,
	DIRT,
	GRASS,
	WATER
}

public class TerrainTrigger : MonoBehaviour {
	public TerrainType terrainType; 
}
