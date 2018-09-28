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

	[HeaderAttribute("Show Ghosts")]
	public TimelineEventTrigger endShowGhosts;

	[HeaderAttribute("Boss Area")]
	public TimelineEventTrigger beforeBossFightTrigger;
	public TimelineEventTrigger afterBossFightTrigger;
	public double AfterBossFightInitTime;
	public Transform playerSpriteTransform;
	public Transform playerTransform;
	public GameObject jatayuObj;


	[SpaceAttribute(10f)]
	public bool isTesting = true;
	public int initPlayableAssetIndex;
	
	void Awake () {
		//TODO : Check If First Time
		// if (isTesting) {
		// 	SetPlayableAsset(initPlayableAssetIndex);
		// }
		// PlayerPrefs.DeleteAll();
		// if (!IsAlreadyPlayedTimeline()) {
		// 	playerEntity.enabled = false;
		// 	playableDirector.Play();
		// }
	}

	void OnEnable () {
		npcOpeningDialogueTrigger.OnNPCStartDialogue += OnNPCStartDialogue;
		npcOpeningDialogueTrigger.OnWaitingDialogue += OnWaitingDialogue;
		endOpeningEntranceTrigger.OnEndOpeningMKF += OnEndOpeningMKF;

		if(npcOpening != null) npcOpening.OnNPCEndDialogue += OnNPCEndDialogue;
		
		endShowGhosts.OnEndShowGhosts += OnEndShowGhosts;
		
		beforeBossFightTrigger.OnEntranceBossArea += OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea += OnEndBossArea;

		afterBossFightTrigger.OnBossFightFinish += ResumeBossFightTimeline;
	}

	void OnDisable () {
		npcOpeningDialogueTrigger.OnNPCStartDialogue -= OnNPCStartDialogue;
		npcOpeningDialogueTrigger.OnWaitingDialogue -= OnWaitingDialogue;
		endOpeningEntranceTrigger.OnEndOpeningMKF -= OnEndOpeningMKF;

		if(npcOpening != null) npcOpening.OnNPCEndDialogue -= OnNPCEndDialogue;
		
		endShowGhosts.OnEndShowGhosts -= OnEndShowGhosts;
		
		beforeBossFightTrigger.OnEntranceBossArea -= OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea -= OnEndBossArea;

		afterBossFightTrigger.OnBossFightFinish -= ResumeBossFightTimeline;
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
		// GameStorage.Instance.IsPlayerAlreadyEnterForest = true;
		
		SavePlayedTimeline ();
		//
		// Debug.Log("STOP TIMELINE");
	}
#endregion

#region Show Ghosts
	void OnEndShowGhosts () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;

		playerTransform.position = new Vector3(21f,0.9f,25f);
		Debug.Log(playerTransform.position);
		Debug.Log(playerTransform.localPosition);
		playerSpriteTransform.localPosition = new Vector3(0f,-0.9f,0f);

		SavePlayedTimeline ();
	}
#endregion

#region Entrance Boss Area
	void OnEntranceBossArea () {
		playableDirector.enabled = false;
		jatayuObj.SetActive(true);
		playerEntity.GetComponent<PlayerInput>().moveDir = Vector3.zero;
		playerEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
		playerEntity.GetComponent<Player>().SetPlayerIdle();
		playerEntity.enabled = true;
	}

	void OnEndBossArea () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		Debug.Log("OnEndBossArea TIMELINE");
		//SavePlayedTimeline ();
	}

	void ResumeBossFightTimeline()
	{
		playableDirector.enabled = true;
		playerEntity.enabled = true;
	}
#endregion

	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 Opening Mada Kari Forest<br /></para>
	/// <para>1 Boss Fight<br /></para>
    /// </summary>
	// public void SetPlayableAsset (int index) {
	// 	playableDirector.playableAsset = playableAssets[index];
	// 	playableDirector.enabled = true;
	// 	playableDirector.Play();
	// }

	public void SetPlayableInitTime (double time) {
		playableDirector.initialTime = time;
	}

	void SavePlayedTimeline () {
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+playableDirector.playableAsset.name, 1);
	}

	// bool IsAlreadyPlayedTimeline () {
	// 	return PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+playableDirector.playableAsset.name, 0) == 1 ? true : false;
	// }
}
