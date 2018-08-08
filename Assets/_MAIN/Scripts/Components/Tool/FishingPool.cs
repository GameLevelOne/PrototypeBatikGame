using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPool : MonoBehaviour {
	public Collider2D fishingPoolCol;
	public GameObject fishCollectibleObj;
	public int maxSpawn;
	public int spawnInterval;
	public bool isSpawning;
	public bool isFinishSpawning;
	public List<Fish> fishList;

	[SerializeField] float timeSpawn;

	public float TimeSpawn {
		get {return timeSpawn;}
		set {timeSpawn = value;}
	}
}
