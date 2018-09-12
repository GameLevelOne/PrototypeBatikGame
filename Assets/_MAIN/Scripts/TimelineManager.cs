using UnityEngine;
using UnityEngine.Playables;
using Unity.Entities;
// using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour {
	public PlayableDirector playableDirector;
	public PlayableAsset[] playableAssets;
	public NPCOpening npcOpening;	
	public GameObjectEntity playerEntity;
	public GameObjectEntity npcEntity;
	public TimelineEventTrigger openingDialogueTrigger;
	public TimelineEventTrigger endingTimelineTrigger;

	public double startDialogueInitTime;
	public double endDialogueInitTime;

	public bool isTesting = true;
	public int initPlayableAssetIndex;
	
	void Awake () {
		//TODO : Check If First Time
		if (isTesting) {
			SetPlayableAsset(initPlayableAssetIndex);
		}
		// playableDirector.Play();
	}

	void OnEnable () {
		openingDialogueTrigger.OnNPCDialogueTrigger += OnNPCDialogueTrigger;
		openingDialogueTrigger.OnStopPlaybackTrigger += OnStopPlaybackTrigger;
		npcOpening.OnNPCEndDialogueTrigger += OnNPCEndDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger += OnEndTimelineTrigger;
	}

	void OnDisable () {
		openingDialogueTrigger.OnNPCDialogueTrigger -= OnNPCDialogueTrigger;
		openingDialogueTrigger.OnStopPlaybackTrigger -= OnStopPlaybackTrigger;
		npcOpening.OnNPCEndDialogueTrigger -= OnNPCEndDialogueTrigger;
		endingTimelineTrigger.OnEndTimelineTrigger -= OnEndTimelineTrigger;
	}

	void OnNPCDialogueTrigger () {
		playableDirector.initialTime = startDialogueInitTime;
		OnStopPlaybackTrigger();
		Debug.Log("START DIALOGUE SESSION");
	}

	void OnStopPlaybackTrigger () {
		playableDirector.Stop();
		playableDirector.DeferredEvaluate(); //IMPORTANT
		playableDirector.Play();
		// Debug.Log("REPEAT IDLE ANIMATION FOR DIALOGUE SESSION");
	}

	void OnNPCEndDialogueTrigger () {
		playableDirector.initialTime = endDialogueInitTime;
		OnStopPlaybackTrigger();
		// Debug.Log("END DIALOGUE SESSION");
	}

	void OnEndTimelineTrigger () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		npcEntity.enabled = true;
		// GameStorage.Instance.
		//
		// Debug.Log("STOP TIMELINE");
	}

	void SetPlayableAsset (int index) {
		playableDirector.playableAsset = playableAssets[index];
		playableDirector.Play();
	}
}
