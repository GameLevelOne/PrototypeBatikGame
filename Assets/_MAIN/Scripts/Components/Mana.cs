using UnityEngine;

public class Mana : MonoBehaviour {
	public int mana = 0;
	public int manaRegenPerSecond = 0;

	// public bool isCheckingMana = false;

	public int PlayerMana {
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MANA, mana);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MANA, value);}
	}
}
