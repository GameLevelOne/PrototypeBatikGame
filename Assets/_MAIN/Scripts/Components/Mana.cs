using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {
	public Text textMana;
	public float mana = 0;
	public float manaRegenPerSecond = 0;

	// public bool isCheckingMana = false;
	public bool isTestMana = false;

	void Awake () { //TEMP
		if (isTestMana) {
			PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, mana);
		} 
	}

	public float PlayerMana {
		get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, mana);}
		set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, value);}
	}
}
