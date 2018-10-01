using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class GameStorageSystem : ComponentSystem {
	public struct GameStorageData {
		public readonly int Length;
		public ComponentArray<GameStorage> GameStorage;
	}
	[InjectAttribute] GameStorageData gameStorageData;

	public struct PlayerData {
		public readonly int Length;
		public ComponentArray<Player> Player;
	}
	[InjectAttribute] PlayerData playerData;

	GameStorage gameStorage;
	Player player;

	protected override void OnUpdate () {
		for (int i=0; i<gameStorageData.Length; i++) {
			gameStorage = gameStorageData.GameStorage[i];

			if (!gameStorage.isInitGameStorage) {
				InitGameStorage();
			}
		}
		
		for (int i=0; i<playerData.Length; i++) {
			player = playerData.Player[i];

			if (!player.isInitPlayer) {
				InitPlayer();
			}
		}
	}

	void InitGameStorage () {
		//

		gameStorage.isInitGameStorage = true;
	}

	void InitPlayer () {
		//

		player.isInitPlayer = true;
	}

	public void SaveState () {
		gameStorage.SavedHP = player.health.PlayerHP;
		gameStorage.SavedMP = player.mana.PlayerMP;

		// if (SceneManager.GetActiveScene().name=="SceneMenu") 
		// 	GameStorage.Instance.PlayBGM(BGMType.TITLE);
		// else if (SceneManager.GetActiveScene().name=="SceneLevel_Jatayu") 
		// 	GameStorage.Instance.PlayBGM(BGMType.BEFORE_JATAYU);
		// else 
		// 	GameStorage.Instance.PlayBGM(BGMType.MAIN);
		Debug.Log("Save State");
	}

    // Values : n
	// n<=0 LoadLastSaved
	// n>0 Load n% of Player Stats
	public void LoadState (float value) {
		if (value <= 0) {
			LoadPlayerStats(gameStorage.SavedHP, gameStorage.SavedMP);
			//
		} else {
			CalculatePlayerStats(value);
			//
		}
		Debug.Log("Load State");
	}

	void CalculatePlayerStats (float precentage) {
		float hp = player.MaxHP * precentage * 0.01f;
		float mp = player.MaxHP * precentage * 0.01f;
		int coin = Mathf.FloorToInt(gameStorage.PlayerCoin * precentage * 0.01f);
		
		LoadPlayerStats(hp, mp);
	}

	void LoadPlayerStats (float hpValue, float mpValue) {
		player.health.PlayerHP = hpValue;
		player.mana.PlayerMP = mpValue;
	}
}
