using Unity.Entities;
using UnityEngine;

public class UIGameOverSystem : ComponentSystem {

	public struct PlayerComponent{
		public readonly int Length;
		public ComponentArray<Player> player;
	}

	public struct UIGameOverComponent{
		public readonly int Length;
		public ComponentArray<UIGameOver> uiGameOver;
	}

	[InjectAttribute] PlayerComponent playerComponent;
	[InjectAttribute] UIGameOverComponent uiGameOverComponent;
	Player currPlayer;
	UIGameOver currUIGameOVer;

	protected override void OnUpdate()
	{
		for(int i = 0;i<playerComponent.Length;i++){
			currPlayer = playerComponent.player[i];
		}

		for(int i = 0;i < uiGameOverComponent.Length;i++){
			currUIGameOVer = uiGameOverComponent.uiGameOver[i];
		}

		CheckPlayer();
	}

	void CheckPlayer()
	{
		if(currPlayer.state == PlayerState.DIE){
			Debug.Log("A. Current Player State =" +currPlayer.state);
			if(!currUIGameOVer.gameOverObj.activeSelf) currUIGameOVer.gameOverObj.SetActive(true);
		}else{
			Debug.Log("B. Current Player State =" +currPlayer.state);
			if(currUIGameOVer.gameObject.activeSelf) currUIGameOVer.gameOverObj.SetActive(false);
		}
	}
}
