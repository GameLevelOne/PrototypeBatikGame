using UnityEngine;

public class TerrainCheker : MonoBehaviour {
	public Player player;

	void OnTriggerStay (Collider other) {
		if (other.GetComponent<TerrainTrigger>() != null) {
			TerrainTrigger terrainTrigger = other.GetComponent<TerrainTrigger>();
			
			if (terrainTrigger.terrainType == TerrainType.DIRT) {
				player.terrainType = TerrainType.DIRT;
			} else if (terrainTrigger.terrainType == TerrainType.GRASS) {
				player.terrainType = TerrainType.GRASS;
			} else if (terrainTrigger.terrainType == TerrainType.WATER) {
				player.terrainType = TerrainType.WATER;
			}
		}
	}
}
