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
	[InjectAttribute] UIPlayerInfoSystem uiPlayerInfoSystem;

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
				Debug.Log("=====START CHECK SYSTEM MANAGER=====");
				Debug.Log("=====Map Index : "+systemManager.currentMapIdx+"=====");
				int currentMapIdx = systemManager.currentMapIdx;
				string currentScene = GameStorage.Instance.CurrentScene;
				Debug.Log(currentScene);
				if (currentScene != systemManager.menuSceneName) {
				//SAVE PLAYER STATS
					Debug.Log("Save State by gameStorageSystem");
					gameStorageSystem.SaveOrLoadState();

					//LOAD MAP STATS
					Debug.Log("CheckCurrentMap : Completed Quest & Dissolver");
					CheckCurrentMap(currentMapIdx);
				
					//SET MAP NAME 
					Debug.Log("Check Map name by uiPlayerInfoSystem");
					uiPlayerInfoSystem.SetMapName(currentMapIdx);

					//SET BGM
					if (currentScene=="SceneLevel_Jatayu") {
						// GameStorage.Instance.PlayBGM(BGMType.BEFORE_JATAYU);
						SoundManager.Instance.PlayBGM(BGM.LevelJatayu);
					} else {
						int cutScene22Complete = PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"Level2-2",0);
						SoundManager.Instance.PlayBGM(cutScene22Complete == 1 ? BGM.MainAfterCutScene22: BGM.MainAfterCutScene22);
					}
				} else {
					// GameStorage.Instance.PlayBGM(BGMType.TITLE);
					SoundManager.Instance.PlayBGM(BGM.Title);
				}
			} catch (System.Exception e) {
				Debug.Log("ERROR : "+e);
				return;
			}

			Debug.Log("=====FINISH CHECK SYSTEM MANAGER=====");
			SetSystems (true);
			systemManager.isChangeScene = false;
		}

		// systemManager.isDoneInitDisabledSystem = true;
	}

	public void SetSystems (bool value) {
		// Debug.Log("Set system to "+value);
		playerInputSystem.Enabled = value;
		damageSystem.Enabled = value;
	}

	void CheckCurrentMap (int mapIdx) {
		// switch (mapIdx) {
		// 	case 4:
				// Debug.Log("CheckIfQuestIsComplete "+mapIdx+" : "+questSystem.CheckIfQuestIsComplete(mapIdx));
				// Debug.Log("CheckCurrentLevelbyQuest "+mapIdx+" : "+areaDissolverSystem.CheckCurrentLevelbyQuest(mapIdx));

				if (questSystem.CheckIfQuestIsComplete(mapIdx)) {
					if (areaDissolverSystem.CheckCurrentLevelbyQuest(mapIdx)) {
						// Debug.Log("THIS MAP IS NOT DISSOLVED");
						areaDissolverSystem.DissolvedArea(mapIdx);
					} else {
						// Debug.Log("THIS MAP ALREADY DISSOLVED");
						areaDissolverSystem.DissableGreyDissolver(mapIdx);
					}
				} else {
					// Debug.Log("THIS MAP IS NOT COMPLETE");
				}
		// 		break;
		// }
	}
}
