using UnityEngine;
using UnityEngine.Playables;

public class EndGameTrigger : MonoBehaviour {
	public UIEndGame UIEndGame;
	public BoxCollider triggerCol;

	[HeaderAttribute("Current")]
	public bool isInitEndGame;
	public bool isTriggered;
	
	// [HeaderAttribute("Testing")]
	// public bool resetPlayerPref;

	void Awake () {
		// if (resetPlayerPref) {
		// 	PlayerPrefs.SetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+playableCutscene.name, 1);
		// }
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.PLAYER) {
			isTriggered = true;
		}
	}
}
