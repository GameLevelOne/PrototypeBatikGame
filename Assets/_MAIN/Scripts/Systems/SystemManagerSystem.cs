using UnityEngine;
using Unity.Entities;

public class SystemManagerSystem : ComponentSystem {
	public struct SystemManagerComponent{
		public readonly int Length;
		public ComponentArray<SystemManager> SystemManager;
	}

	public struct LevelDataComponent{
		public readonly int Length;
		public ComponentArray<LevelData> levelData;
	}

	[InjectAttribute] SystemManagerComponent systemManagerComponent;[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] DamageSystem damageSystem;
	[InjectAttribute] LevelDataComponent levelDataComponent;
	LevelData currLevelData;

	// [InjectAttribute] ToolSystem toolSystem;
	// [InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	SystemManager systemManager;

	protected override void OnUpdate()
	{
		for(int i = 0;i<systemManagerComponent.Length;i++){
			systemManager = systemManagerComponent.SystemManager[i];
			
			// if (!systemManager.isDoneInitDisabledSystem) {
				CheckSystemManager();
			// }
		}

		for(int i = 0;i<levelDataComponent.Length;i++){
			currLevelData = levelDataComponent.levelData[i];

			SetPlayerStartPos();
		}
	}

	void SetPlayerStartPos()
	{
		if(!currLevelData.hasSetPlayerPos){
			currLevelData.hasSetPlayerPos = true;

			int startPosIndex = PlayerPrefs.GetInt(Constants.PlayerPrefKey.LEVEL_PLAYER_START_POS,0);
			if(startPosIndex > currLevelData.playerStartPos.Length) startPosIndex = 0;
			currLevelData.playerObj.transform.position = currLevelData.playerStartPos[startPosIndex];
		}
	}

	void CheckSystemManager() {
		if (systemManager.isChangeScene) {
			SetSystems (true);
			systemManager.isChangeScene = false;
		}

		// systemManager.isDoneInitDisabledSystem = true;
	}

	public void SetSystems (bool value) {
		playerInputSystem.Enabled = value;
		damageSystem.Enabled = value;
	}
}
