using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class BombSystem : ComponentSystem {
	public struct BombData{
		public readonly int Length;
		public ComponentArray<Bomb> bomb;
	}
	[InjectAttribute] BombData bombData;

	Bomb bomb;

	protected override void OnUpdate()
	{
		if (bombData.Length == 0) return;

		for (int i=0; i<bombData.Length; i++) { 
			bomb = bombData.bomb[i];

			TickBomb();
			
			if (bomb.destroy){				
				GameObjectEntity.Destroy(bomb.gameObject);
				UpdateInjectedComponentGroups(); //TEMP, Error without this
			}
		}

		// foreach(var e in GetEntities<BombComponent>()){
		// 	TickBomb(e);
			
		// 	if(e.bomb.destroy){
		// 		GameObject.Destroy(e.bomb.gameObject);
		// 		return; //TEMP, Error without this
				
		// 		// GameObjectEntity.Destroy(e.bomb.gameObject);
		// 		// UpdateInjectedComponentGroups(); //TEMP, Error without this
		// 	}
		// }
	}

	void TickBomb()
	{
		if (bomb.timer <= 0){
			if(!bomb.explode){
				bomb.explode = true;
				Explode();
			}
		} else {
			float deltaTime = Time.fixedDeltaTime;
			bomb.timer -= deltaTime;
		}
	}


	void Explode()
	{
		bomb.bombAnimator.SetTrigger(Constants.AnimatorParameter.Trigger.EXPLODE);
	}
}