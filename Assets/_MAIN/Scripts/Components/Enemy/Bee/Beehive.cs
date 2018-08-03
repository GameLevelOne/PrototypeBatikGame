using UnityEngine;
using System.Collections.Generic;

public class Beehive : MonoBehaviour {
	[HeaderAttribute("Beehive Attribute")]
	public GameObject prefabBee;
	public int spawnAmount;
	public float spawnInterval;
	public bool flagSpawn = false;
	
	float tSpawn;

	[HeaderAttribute("Current")]
	public List<GameObject> currentBees;

	public float TSpawn {
		get { return tSpawn; }
		set { tSpawn = value; }
	}
}