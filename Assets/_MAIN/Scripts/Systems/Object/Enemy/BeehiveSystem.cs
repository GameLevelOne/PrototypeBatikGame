﻿using UnityEngine;
using Unity.Entities;

public class BeehiveSystem : ComponentSystem{

	public struct BeehiveComponent
	{
		public readonly int Length;
		public ComponentArray<Transform> beehiveTransform;
		public ComponentArray<Beehive> beehive;
		public ComponentArray<Health> health;
	}

	#region injected component
	[InjectAttribute] BeehiveComponent beehiveComponent;
	public Transform currBeehiveTransform;
	public Beehive currBeehive;
	public Health currHealth;
	#endregion

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i = 0;i<beehiveComponent.Length;i++){
			currBeehiveTransform = beehiveComponent.beehiveTransform[i];
			currBeehive = beehiveComponent.beehive[i];
			currHealth = beehiveComponent.health[i];

			if(!currBeehive.flagFinishSpawn) Spawnbee();
			
			CheckHealth();
		}
	}

	void Spawnbee()
	{
		deltaTime = Time.deltaTime;
		if(!currBeehive.flagSpawn){
			currBeehive.TSpawn -= deltaTime;
			if(currBeehive.TSpawn <= 0f) currBeehive.flagSpawn = true;
		}else{
			GameObject currentBee = GameObject.Instantiate(currBeehive.prefabBee,currBeehiveTransform.position,Quaternion.identity) as GameObject;
			currentBee.GetComponent<BeeMovement>().beeHiveTransform = currBeehiveTransform;
			currBeehive.currentBees.Add(currentBee.GetComponent<Bee>());
			
			currBeehive.flagSpawn = false;
			currBeehive.TSpawn = currBeehive.spawnInterval;
			
			if(currBeehive.currentBees.Count >= currBeehive.spawnAmount) currBeehive.flagFinishSpawn = true;
		}
	}

	void CheckHealth()
	{
		if(currHealth.HealthPower <= 0f || currBeehive.destroyed){
			DestroyBeehive();
		}
	}

	void DestroyBeehive()
	{
		//destroy the hive, then send startle bees

		foreach(Bee bee in currBeehive.currentBees){
			if(currBeehive.playerObject != null){
				Debug.Log("SET BEE TO CHASE");
				bee.playerTransform = currBeehive.playerObject.transform;
				bee.beeState = BeeState.Chase;
			}else{
				bee.isStartled = true;
				bee.beeState = BeeState.Startled;
			}
		}
		GameObject.Destroy(currBeehive.gameObject);
		UpdateInjectedComponentGroups();
		//Debug.Log("ASLDKLAKDLAKDSL");
	}
}
