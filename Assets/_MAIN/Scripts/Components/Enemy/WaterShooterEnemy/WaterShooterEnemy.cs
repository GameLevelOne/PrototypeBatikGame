﻿using UnityEngine;

public class WaterShooterEnemy : MonoBehaviour {
	[HeaderAttribute("WaterShooterEnemy Attributes")]
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public TriggerDetection playerTriggerDetection;
	public GameObject bullet;

	[SpaceAttribute(10f)]
	public float shootInterval;

	[SerializeField] float tShootInterval;

	[HeaderAttribute("Current")]
	public  bool startAttack = false;

	public float TShootInterval{
		get{return tShootInterval;}
		set{tShootInterval = value;}
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += SetPlayerTransform;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayerTransform;
	}

	void SetPlayerTransform(GameObject player)
	{
		if(player == null) enemy.playerTransform = null;
		else enemy.playerTransform = player.transform;
	}

	#region animation event
	void EnableAttackHit()
	{
		enemy.attackHit = true;
	}
	void OnEndAttack()
	{
		enemy.initAttack = false;
	}

	void OnStartDamaged()
	{
		enemy.initDamaged = false;
	}

	void OnEndDamaged()
	{
		enemy.isFinishDamaged = true;
	}

	void OnEndAggro()
	{
		enemy.isFinishAggro = true;
	}
	#endregion
}
