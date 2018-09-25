using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {
	public TimelineManager timelineManager;
	public PlayableAsset playableCutscene;
	public Collider triggerCol;

	[HeaderAttribute("Current")]
	public bool isInitCutscene;
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
