using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPool : MonoBehaviour {
	public Collider2D fishingPoolCol;
	public GameObject fishCollectibleObj;
	public int maxSpawn;
	public int spawnInterval;
	public List<FishCollectible> fishCollectibleList;

	float tSpawn;
}
