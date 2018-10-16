using UnityEngine;
using Unity.Entities;

public class BombSystem : ComponentSystem {
	public struct BombData{
		public readonly int Length;
		public ComponentArray<Bomb> Bomb;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] BombData bombData;
	[InjectAttribute] CameraShakerSystem cameraShakerSystem;

	Bomb bomb;

	Transform bombTransform;

	float deltaTime;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;
		// if (bombData.Length == 0) return;
		// deltaTime = Time.fixedDeltaTime;

		for (int i=0; i<bombData.Length; i++) { 
			bomb = bombData.Bomb[i];
			bombTransform= bombData.Transform[i];

			TickBomb();
			
			// if (bomb.destroy){				
			// 	GameObjectEntity.Destroy(bomb.gameObject);
			// 	UpdateInjectedComponentGroups(); //TEMP, Error without this
			// }
		}
	}

	void TickBomb()
	{
		if (bomb.timer <= 0){
			if(!bomb.explode){
				cameraShakerSystem.ShakeCamera(true);
				SpawnExplosion ();
				// Explode();
				DestroyBomb ();
				bomb.explode = true;
			}
		} else {
			bomb.timer -= deltaTime;
		}
	}

	void SpawnExplosion () {
		Quaternion rot = Quaternion.Euler(40f, 0f, 0f);

		GameObject spawnedObj = GameObjectEntity.Instantiate(bomb.bombExplodeAreaObj, bombTransform.position, rot);
	}

	void DestroyBomb () {
		GameObjectEntity.Destroy(bomb.gameObject);
		UpdateInjectedComponentGroups(); //TEMP, Error without this
	}

	// void Explode()
	// {
	// 	bomb.bombAnimator.SetTrigger(Constants.AnimatorParameter.Trigger.EXPLODE);
	// }
}