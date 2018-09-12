using UnityEngine;
using UnityEngine.Playables;
// using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour {
	public PlayableDirector playableDirector;
	public PlayableAsset[] playableAssets;
	public NPCOpening npcOpening;
	public TimelineEventTrigger openingDialogueTrigger;
	public TimelineEventTrigger endingTimelineTrigger;

	public double startDialogueInitTime;
	public double endDialogueInitTime;
	
	void Awake () {
		//TODO : Check If First Time

		SetPlayableAsset(0);
		// playableDirector.extrapolationMode = DirectorWrapMode.Hold;
	}

	void OnEnable () {
		openingDialogueTrigger.OnNPCDialogueTrigger += OnNPCDialogueTrigger;
		openingDialogueTrigger.OnDialogingTrigger += OnDialogingTrigger;
		npcOpening.OnNPCEndDialogueTrigger += OnNPCEndDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger += OnEndTimelineTrigger;
	}

	void OnDisable () {
		openingDialogueTrigger.OnNPCDialogueTrigger -= OnNPCDialogueTrigger;
		openingDialogueTrigger.OnDialogingTrigger -= OnDialogingTrigger;
		npcOpening.OnNPCEndDialogueTrigger -= OnNPCEndDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger -= OnEndTimelineTrigger;
	}

	void OnNPCDialogueTrigger () {
		// SetPlayableAsset(1);
		playableDirector.initialTime = startDialogueInitTime;
		// SetPlayableAsset(0);
		playableDirector.Play(playableAssets[0]);
		// playableDirector.extrapolationMode = DirectorWrapMode.Loop;
		Debug.Log("START DIALOGUE SESSION");
	}

	void OnDialogingTrigger () {
		// SetPlayableAsset(0);
		playableDirector.Play(playableAssets[0]);
		Debug.Log("REPEAT IDLE ANIMATION FOR DIALOGUE SESSION");
	}

	void OnNPCEndDialogueTrigger () {
		SetPlayableAsset(2);
		// playableDirector.extrapolationMode = DirectorWrapMode.Hold;
		Debug.Log("END DIALOGUE SESSION");
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
