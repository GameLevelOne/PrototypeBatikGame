using UnityEngine;

public class Mana : MonoBehaviour {
	public delegate void ManaChange ();
	public event ManaChange OnManaChange;

	public float mana = 0;
	public float manaRegenPerSecond = 0;

	// public bool isCheckingMana = false;
	public bool isTestMana = false;

	void Awake () { //TEMP
		if (isTestMana) {
			PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, mana);
		} 
	}

	public float PlayerMP {
		get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, mana);}
		set{
			if (PlayerMP == value) return;

			PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MANA, value);
			
			if (OnManaChange != null) {
				OnManaChange();
			}
		}
	}
}
