using Unity.Entities;
using UnityEngine;

public class LevelDataSystem : ComponentSystem {

	[Inject] public CameraSystem cameraSystem;
	[Inject] public MapChunkSystem mapChunkSystem;
	public struct Component{
		public LevelData levelData;
	}

	// void OnLevelWasLoaded(int level)
	// {
	// 	this.Enabled = true;
	// }

	protected override void OnUpdate()
	{
		foreach(var e in GetEntities<Component>()){
			if(!e.levelData.isInitialied){
				InitializeLevelData(e);
			}
		}
		
	}

	void InitializeLevelData(Component e)
	{
		e.levelData.isInitialied = true;
		// e.levelData.currentPlayer = (GameObject) GameObject.Instantiate(e.levelData.playerObj,e.levelData.playerStartPos,Quaternion.identity);
		cameraSystem.Enabled = true;
		mapChunkSystem.Enabled = true;
		// this.Enabled = false;
	}
}
