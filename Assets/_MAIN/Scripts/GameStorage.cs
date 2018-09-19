using UnityEngine;

public class GameStorage : MonoBehaviour {
	public GlobalSettingsSO settings;
	public int initCoin;

	public bool isTesting;

	public int PlayerCoin {
		get {return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_COIN, initCoin);}
		set {PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_COIN, value);
		}
	}

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
	}
#endregion
}
