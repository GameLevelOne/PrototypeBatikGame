using Unity.Entities;
using UnityEngine;
using System.Collections;

public class BombSystem : ComponentSystem {

	public struct BombComponent{
		public Bomb bomb;
	}

	protected override void OnUpdate()
	{
		foreach(var e in GetEntities<BombComponent>()){
			TickBomb(e);
			
			if(e.bomb.destroy){
				GameObject.Destroy(e.bomb.gameObject);
				return; //TEMP, Error without this
				
				// GameObjectEntity.Destroy(e.bomb.gameObject);
				// UpdateInjectedComponentGroups(); //TEMP, Error without this
			}
		}
	}

	void TickBomb(BombComponent e)
	{
		if(e.bomb.timer <= 0){
			if(!e.bomb.explode){
				e.bomb.explode = true;
				Explode(e);
			}
		}else{
			float deltaTime = Time.fixedDeltaTime;
			e.bomb.timer -= deltaTime;
		}
	}


	void Explode(BombComponent e)
	{
		e.bomb.bombAnimator.SetTrigger(Constants.AnimatorParameter.Trigger.EXPLODE);
		
	}
}