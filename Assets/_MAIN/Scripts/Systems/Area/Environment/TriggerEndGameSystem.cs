using Unity.Entities;
using UnityEngine;

public class TriggerEndGameSystem : ComponentSystem {

	public struct TriggerEndGameComponent{
		public readonly int Length;
		public ComponentArray<TriggerEndGame> triggerEndGame;
	}

	[InjectAttribute] TriggerEndGameComponent triggerEndGameComponent;
	TriggerEndGame triggerEndGame;

	[InjectAttribute] PlayerInputSystem playerInputSystem;

	protected override void OnUpdate()
	{
		for(int i = 0;i<triggerEndGameComponent.Length;i++){
			triggerEndGame = triggerEndGameComponent.triggerEndGame[i];

			CheckTrigger();
		}
	}

	void CheckTrigger()
	{
		if(triggerEndGame.triggered){
			triggerEndGame.triggered = false;

			triggerEndGame.player.GetComponent<PlayerInput>().moveDir = Vector3.zero;
			triggerEndGame.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			triggerEndGame.player.GetComponent<Player>().SetPlayerIdle();
			triggerEndGame.player.enabled = true;

			playerInputSystem.Enabled = false;

			triggerEndGame.uiEndGame.call = true;
		}
	}
}
