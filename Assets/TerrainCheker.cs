using UnityEngine;

public class TerrainCheker : MonoBehaviour {
	public Player player;

	[HeaderAttribute("Current")]
	[SerializeField] TerrainType currTerrainType;

	void OnTriggerStay (Collider other) {
		if (other.GetComponent<TerrainTrigger>() != null) {
			TerrainTrigger terrainTrigger = other.GetComponent<TerrainTrigger>();

			currTerrainType = player.terrainType;
			
			if (currTerrainType != TerrainType.DIRT && terrainTrigger.terrainType == TerrainType.DIRT) {
				player.terrainType = TerrainType.DIRT;
			} else if (currTerrainType != TerrainType.STONESWAMP && terrainTrigger.terrainType == TerrainType.STONESWAMP) {
				player.terrainType = TerrainType.STONESWAMP;
			} else if ((currTerrainType != TerrainType.STONESWAMP && currTerrainType != TerrainType.WATER) && terrainTrigger.terrainType == TerrainType.WATER) {
				player.terrainType = TerrainType.WATER;
			} 
			// else if (terrainTrigger.terrainType == TerrainType.GRASS) {
			// 	player.terrainType = TerrainType.GRASS;
			// } 
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<TerrainTrigger>() != null) {
			player.terrainType = TerrainType.NONE;
		}
	}
}
