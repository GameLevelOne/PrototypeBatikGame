using UnityEngine;
using System.Collections.Generic;

public class Beehive : MonoBehaviour {
	[HeaderAttribute("Beehive Attribute")]
	public TriggerDetection playerTriggerDetection;
	public GameObject prefabBee;
	public int spawnAmount;
	public float spawnInterval;
	public bool flagSpawn = false;
	public bool flagFinishSpawn = false;
	public bool destroyed = false;
	float tSpawn;

	[HeaderAttribute("Current")]
	public GameObject playerObject;
	public List<Bee> currentBees;
	

	public float TSpawn {
		get { return tSpawn; }
		set { tSpawn = value; }
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnter += SetPlayer;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnter -= SetPlayer;
	}

	void SetPlayer(GameObject playerObj)
	{
		playerObject = playerObj;
	}

	void RemovePlayer(GameObject playerObj)
	{
		playerObject = null;
	}
}