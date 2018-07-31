using Unity.Entities;
using UnityEngine;

public class LevelDataSystem : ComponentSystem {

	public struct LevelDataComponent{
		public readonly int Length;
		public ComponentArray<LevelData> levelData;
	}

	#region injected component
	[InjectAttribute] LevelDataComponent levelDataComponent;
	LevelData currLevelData;
	#endregion

	#region injected system
	[Inject] public CameraSystem cameraSystem;
	[Inject] public MapChunkSystem mapChunkSystem;
	#endregion

	protected override void OnUpdate()
	{
		// for(int i = 0;i<levelDataComponent.Length;i++){
		// 	currLevelData = levelDataComponent.levelData[i];
		// 	InitializeLevelData();
		// }		
	}

	void InitializeLevelData()
	{
		currLevelData.isInitialied = true;
		// e.levelData.currentPlayer = (GameObject) GameObject.Instantiate(e.levelData.playerObj,e.levelData.playerStartPos,Quaternion.identity);
		cameraSystem.Enabled = true;
		mapChunkSystem.Enabled = true;
		// this.Enabled = false;
	}
}
