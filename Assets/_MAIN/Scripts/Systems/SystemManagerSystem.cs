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
	[InjectAttribute] GameStorageSystem gameStorageSystem;
	[InjectAttribute] QuestSystem questSystem;
	[InjectAttribute] AreaDissolverSystem areaDissolverSystem;

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
			if(startPosIndex >= currLevelData.playerStartPos.Length) startPosIndex = 0;
			currLevelData.playerObj.transform.position = currLevelData.playerStartPos[startPosIndex];
		}
	}

	void CheckSystemManager() {
		if (systemManager.isChangeScene) {
			try {
				Debug.Log("Try CheckSystemManager \n isChangeScene :"+systemManager.isChangeScene);

				//SAVE PLAYER STATS
				gameStorageSystem.SaveState();

				//LOAD MAP STATS
				CheckCurrentMap(systemManager.currentMapIdx);
				Debug.Log("CheckCurrentMap : "+systemManager.currentMapIdx);
			} catch (System.Exception e) {
				Debug.Log("ERROR : "+e);
				return;
			}

			Debug.Log("Finish CheckSystemManager \n isChangeScene :"+systemManager.isChangeScene);
			SetSystems (true);
			systemManager.isChangeScene = false;
		}

		// systemManager.isDoneInitDisabledSystem = true;
	}

	public void SetSystems (bool value) {
		playerInputSystem.Enabled = value;
		damageSystem.Enabled = value;
	}

	void CheckCurrentMap (int mapIdx) {
		// switch (mapIdx) {
		// 	case 4:
				Debug.Log("CheckIfQuestIsComplete "+mapIdx+" : "+questSystem.CheckIfQuestIsComplete(mapIdx));
				Debug.Log("CheckCurrentLevelbyQuest "+mapIdx+" : "+areaDissolverSystem.CheckCurrentLevelbyQuest(mapIdx));

				if (questSystem.CheckIfQuestIsComplete(mapIdx)) {
					if (areaDissolverSystem.CheckCurrentLevelbyQuest(mapIdx)) {
						Debug.Log("THIS MAP IS NOT DISSOLVED");
						areaDissolverSystem.DissolvedArea(mapIdx);
					} else {
						Debug.Log("THIS MAP ALREADY DISSOLVED");
						areaDissolverSystem.DissableGreyDissolver(mapIdx);
					}
				} else {
					Debug.Log("THIS MAP IS NOT COMPLETE");
				}
		// 		break;
		// }
	}
}
