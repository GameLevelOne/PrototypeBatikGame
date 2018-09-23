using UnityEngine;
using Unity.Entities;

public class GameStorage : MonoBehaviour {
	public GlobalSettingsSO settings;
	public int initCoin;
	public int initSavedHP;
	public int initSavedMP;

	public bool isTesting;

	public bool isInitGameStorage = false;

	// public bool IsPlayerAlreadyEnterForest {
	// 	get {return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_ALREADY_ENTER_FOREST, 0) == 1 ? true : false;}
	// 	set {
	// 		if (value) {
	// 			PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_ALREADY_ENTER_FOREST, 1);
	// 		} else {
	// 			PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_ALREADY_ENTER_FOREST, 0);
	// 		}			
	// 	}
	// }

	public int PlayerCoin {
		get {return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_COIN, initCoin);}
		set {PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_COIN, value);
		}
	}

	public float SavedHP {
		get {return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_HP, 0f);}
		set {PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_HP, value);
		}
	}

	public float SavedMP {
		get {return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_MP, 0f);}
		set {PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_MP, value);
		}
	}

#region  QUEST KEY / MAP
	// public int IsCompletedLevel_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_1, value);
	// 	}
	// }

	// public int IsCompletedLevel_2_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_2_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_2_1, value);
	// 	}
	// }

	// public int IsCompletedLevel_2_2 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_2_2, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_2_2, value);
	// 	}
	// }

	// public int IsCompletedLevel_2_3 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_2_3, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_2_3, value);
	// 	}
	// }

	// public int IsCompletedLevel_3_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_3_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_3_1, value);
	// 	}
	// }

	// public int IsCompletedLevel_3_2 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_3_2, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_3_2, value);
	// 	}
	// }

	// public int IsCompletedLevel_3_3 {
	// 	get {return PlayerPrefs.GetInt(Constants.LevelPrefKey.LEVEL_3_3, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.LevelPrefKey.LEVEL_3_3, value);
	// 	}
	// }

	// public int KillCountQuestLevel_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_1, value);
	// 	}
	// }

	// public int KillCountQuestLevel_2_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_1, value);
	// 	}
	// }

	// public int KillCountQuestLevel_2_2 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_2, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_2, value);
	// 	}
	// }

	// public int KillCountQuestLevel_2_3 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_3, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_2_3, value);
	// 	}
	// }

	// public int KillCountQuestLevel_3_1 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_1, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_1, value);
	// 	}
	// }

	// public int KillCountQuestLevel_3_2 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_2, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_2, value);
	// 	}
	// }

	// public int KillCountQuestLevel_3_3 {
	// 	get {return PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_3, 0);}
	// 	set {PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_LEVEL_3_3, value);
	// 	}
	// }
#endregion

	void Start () {
		if (isTesting) {
			PlayerCoin = initCoin;
		}
	}

#region SINGLETON
	private static GameStorage _instance;
	public static GameStorage Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameStorage>();
			} 

			return _instance;
		}
	}

	void Awake () {
		DontDestroyOnLoad(gameObject);
		if (_instance != null && _instance != this) {
			GameObject.Destroy(gameObject);			
			// gameObject.GetComponent<GameObjectEntity>().enabled = false;
			// gameObject.SetActive(false);
		}
	}
#endregion
}
