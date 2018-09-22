using UnityEngine;
using UnityEngine.Playables;
using Unity.Entities;
// using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour {
	public PlayableDirector playableDirector;
	public PlayableAsset[] playableAssets;
	public GameObjectEntity playerEntity;

	[HeaderAttribute("Opening Mada Kari Forest")]
	public NPCOpening npcOpening;	
	public GameObjectEntity npcEntity;
	public TimelineEventTrigger npcOpeningDialogueTrigger;
	public TimelineEventTrigger endOpeningEntranceTrigger;
	public double startDialogueInitTime;
	public double endDialogueInitTime;

	[HeaderAttribute("Boss Area")]
	public TimelineEventTrigger beforeBossFightTrigger;
	public TimelineEventTrigger afterBossFightTrigger;
	public double AfterBossFightInitTime;

	[SpaceAttribute(10f)]
	public bool isTesting = true;
	public int initPlayableAssetIndex;
	
	void Awake () {
		//TODO : Check If First Time
		if (isTesting) {
			SetPlayableAsset(initPlayableAssetIndex);
		}

		playerEntity.enabled = false;
		// playableDirector.Play();
	}

	void OnEnable () {
		npcOpeningDialogueTrigger.OnNPCStartDialogue += OnNPCStartDialogue;
		npcOpeningDialogueTrigger.OnWaitingDialogue += OnWaitingDialogue;
		endOpeningEntranceTrigger.OnEndOpeningMKF += OnEndOpeningMKF;

		npcOpening.OnNPCEndDialogue += OnNPCEndDialogue;
		
		beforeBossFightTrigger.OnEntranceBossArea += OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea += OnEndBossArea;
	}

	void OnDisable () {
		npcOpeningDialogueTrigger.OnNPCStartDialogue -= OnNPCStartDialogue;
		npcOpeningDialogueTrigger.OnWaitingDialogue -= OnWaitingDialogue;
		endOpeningEntranceTrigger.OnEndOpeningMKF -= OnEndOpeningMKF;

		npcOpening.OnNPCEndDialogue -= OnNPCEndDialogue;
		
		beforeBossFightTrigger.OnEntranceBossArea -= OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea -= OnEndBossArea;
	}

#region Opening Mada Kari Forest
	void OnNPCStartDialogue () {
		// playableDirector.initialTime = startDialogueInitTime;
		SetPlayableInitTime(startDialogueInitTime);
		OnWaitingDialogue();
		// Debug.Log("START DIALOGUE SESSION");
	}

	void OnWaitingDialogue () {
		playableDirector.Stop();
		playableDirector.DeferredEvaluate(); //IMPORTANT
		playableDirector.Play();
		// Debug.Log("REPEAT IDLE ANIMATION FOR DIALOGUE SESSION");
	}

	void OnNPCEndDialogue () {
		// playableDirector.initialTime = endDialogueInitTime;
		SetPlayableInitTime(endDialogueInitTime);
		OnWaitingDialogue();
		// Debug.Log("END DIALOGUE SESSION");
	}

	void OnEndOpeningMKF () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		npcEntity.enabled = true;
		// GameStorage.Instance.
		//
		// Debug.Log("STOP TIMELINE");
	}
#endregion

#region Entrance Boss Area
	void OnEntranceBossArea () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
	}

	void OnEndBossArea () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		Debug.Log("OnEndBossArea TIMELINE");
	}
#endregion

	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 Opening Mada Kari Forest<br /></para>
	/// <para>1 Boss Fight<br /></para>
    /// </summary>
	public void SetPlayableAsset (int index) {
		playableDirector.playableAsset = playableAssets[index];
		playableDirector.enabled = true;
		playableDirector.Play();
	}

	public void SetPlayableInitTime (double time) {
		playableDirector.initialTime = time;
	}
}
