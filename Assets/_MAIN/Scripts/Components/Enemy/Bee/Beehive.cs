	using UnityEngine;
using System.Collections.Generic;

public class Beehive : MonoBehaviour {
	[HeaderAttribute("Beehive Attribute")]
	public TriggerDetection playerTriggerDetection;
	public TriggerDetection playerHitTrigger;
	public AnimationControl animationControl;
	public GameObject prefabBee;
	public Animator Animator;
	public int spawnAmount;
	public float spawnInterval;
	public bool flagSpawn = false;
	public bool destroyed = false;
	public bool isFinishDestroy = false;
	public bool isBeingHit = false;
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
		playerHitTrigger.OnTriggerEnterObj += OnPlayerHit;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayer;
		if (playerHitTrigger!=null)
			playerHitTrigger.OnTriggerEnterObj -= OnPlayerHit;
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

	void OnPlayerHit(GameObject playerObj)
	{
		Debug.Log("Player Hit");
		if ((playerObj!=null) && !destroyed) {
			Animator.Play(Constants.AnimationName.HIT);
		}
	}

	void SetDestroy () {
		isFinishDestroy = true;
	}
}