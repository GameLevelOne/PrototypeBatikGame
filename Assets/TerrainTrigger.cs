using UnityEngine;

public enum TerrainType {
	NONE,
	DIRT,
	GRASS,
	WATERY
}

public class TerrainTrigger : MonoBehaviour {
	public TerrainType terrainType; 
}
