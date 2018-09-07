using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour {
	public PlayableDirector playableDirector;
	public PlayableAsset[] playableAssets;
	public NPCOpening npcOpening;
	public TimelineEventTrigger openingDialogueTrigger;
	public TimelineEventTrigger endingTimelineTrigger;
	
	void Awake () {
		//TODO : Check If First Time

		SetPlayableAsset(0);
		playableDirector.extrapolationMode = DirectorWrapMode.Hold;
	}

	void OnEnable () {
		npcOpening.OnNPCEndDialogueTrigger += OnNPCEndDialogueTrigger;
		openingDialogueTrigger.OnNPCDialogueTrigger += OnNPCDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger += OnEndTimelineTrigger;
	}

	void OnDisable () {
		npcOpening.OnNPCEndDialogueTrigger -= OnNPCEndDialogueTrigger;
		openingDialogueTrigger.OnNPCDialogueTrigger -= OnNPCDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger -= OnEndTimelineTrigger;
	}

	void OnNPCDialogueTrigger () {
		SetPlayableAsset(1);
		playableDirector.extrapolationMode = DirectorWrapMode.Loop;
		Debug.Log("START DIALOGUE");
	}

	void OnNPCEndDialogueTrigger () {
		SetPlayableAsset(2);
		playableDirector.extrapolationMode = DirectorWrapMode.Hold;
		Debug.Log("END DIALOGUE");
	}

	void OnEndTimelineTrigger () {
		playableDirector.Stop();
		Debug.Log("STOP TIMELINE");
	}

	void SetPlayableAsset (int index) {
		playableDirector.playableAsset = playableAssets[index];
		playableDirector.Play();
	}
}
