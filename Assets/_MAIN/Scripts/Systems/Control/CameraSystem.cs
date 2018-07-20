using Unity.Entities;
using UnityEngine;

public class CameraSystem : ComponentSystem {

	public struct CameraComponent{
		public GameCamera gameCamera;
	}

	public struct PlayerComponent{
		public LevelData levelData;
	}

	public Transform cameraTransform;
	public Transform playerTransform;

	void Start()
	{
		this.Enabled = false;
	}

	protected override void OnUpdate()
	{
		foreach(var e in GetEntities<CameraComponent>()){
			if(cameraTransform == null || cameraTransform != e.gameCamera.transform){
				cameraTransform = e.gameCamera.transform;
			}
		}

		foreach(var e in GetEntities<PlayerComponent>()){
			if(playerTransform == null || playerTransform != e.levelData.currentPlayer){
				playerTransform = e.levelData.currentPlayer.transform;
			}
		}

		if(cameraTransform != null && playerTransform != null)
		{
			cameraTransform.position = new Vector3(playerTransform.position.x,playerTransform.position.y,-10f);
		}
	}
}