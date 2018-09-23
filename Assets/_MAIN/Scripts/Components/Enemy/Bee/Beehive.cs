using UnityEngine;
using System.Collections.Generic;

public class Beehive : MonoBehaviour {
	[HeaderAttribute("Beehive Attribute")]
	public TriggerDetection playerTriggerDetection;
	public AnimationControl animationControl;
	public GameObject prefabBee;
	public Animator Animator;
	public int spawnAmount;
	public float spawnInterval;
	public bool flagSpawn = false;
	public bool flagFinishSpawn = false;
	public bool destroyed = false;
	public bool isFinishDestroy = false;
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
		playerTriggerDetection.OnTriggerEnterObj += SetPlayer;
		animationControl.OnExitAnimation += SetDestroy;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayer;
		animationControl.OnExitAnimation -= SetDestroy;
	}

	void SetPlayer(GameObject playerObj)
	{
		playerObject = playerObj;
	}

	void RemovePlayer(GameObject playerObj)
	{
		playerObject = null;
	}

	void SetDestroy () {
		isFinishDestroy = true;
	}
}