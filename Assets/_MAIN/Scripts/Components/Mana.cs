using UnityEngine;

public class Mana : MonoBehaviour {
	public delegate void ManaChange ();
	public event ManaChange OnManaChange;

	public float mana = 0;
	public float manaRegenPerSecond = 0;
	public float manaRegenTimer = 0;

	// public bool isCheckingMana = false;
	// public bool isTestMana = false;

	public bool isManaFull;
	public bool isCheckingMana;
	public bool isInitMana = false;

	void Awake () { //TEMP
		// if (isTestMana) {
		// 	PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP, mana);
		// } 
	}

	public float PlayerMP {
		get{
			//  // Debug.Log("Player Remaining MP :"+PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP));
			return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP, mana);}
		set{
			if (PlayerMP == value) return;

			mana = value;
			float maxMP = PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMP, 100f);

			if (mana > maxMP) mana = maxMP;

			PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP, mana);
			
			if (OnManaChange != null) {
				OnManaChange();
			}
		}
	}
}
