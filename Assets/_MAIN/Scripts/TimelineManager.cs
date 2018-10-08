using UnityEngine;
using UnityEngine.Playables;
using Unity.Entities;
// using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour {
	public PlayableDirector playableDirector;
	public PlayableAsset[] playableAssets;
	public GameObjectEntity playerEntity;
	public GameObjectEntity[] enemyEntity;

	[HeaderAttribute("Opening Mada Kari Forest")]
	public NPCOpening npcOpening;	
	public GameObjectEntity npcEntity;
	public TimelineEventTrigger[] npcOpeningDialogueTrigger;
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
	public Transform camera22Transform;
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
		for (int i=0;i<npcOpeningDialogueTrigger.Length;i++) {
			npcOpeningDialogueTrigger[i].OnNPCStartDialogue += OnNPCStartDialogue;
			npcOpeningDialogueTrigger[i].OnWaitingDialogue += OnWaitingDialogue;
		}
		endOpeningEntranceTrigger.OnEndOpeningMKF += OnEndOpeningMKF;

		if(npcOpening != null) npcOpening.OnNPCEndDialogue += OnNPCEndDialogue;
		
		endShowGhosts.OnEndShowGhosts += OnEndShowGhosts;
		
		beforeBossFightTrigger.OnEntranceBossArea += OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea += OnEndBossArea;

		afterBossFightTrigger.OnBossFightFinish += ResumeBossFightTimeline;
	}

	void OnDisable () {
		for (int i=0;i<npcOpeningDialogueTrigger.Length;i++) {
			npcOpeningDialogueTrigger[i].OnNPCStartDialogue -= OnNPCStartDialogue;
			npcOpeningDialogueTrigger[i].OnWaitingDialogue -= OnWaitingDialogue;
		}
		endOpeningEntranceTrigger.OnEndOpeningMKF -= OnEndOpeningMKF;

		if(npcOpening != null) npcOpening.OnNPCEndDialogue -= OnNPCEndDialogue;
		
		endShowGhosts.OnEndShowGhosts -= OnEndShowGhosts;
		
		beforeBossFightTrigger.OnEntranceBossArea -= OnEntranceBossArea;
		afterBossFightTrigger.OnEndBossArea -= OnEndBossArea;

		afterBossFightTrigger.OnBossFightFinish -= ResumeBossFightTimeline;
	}

#region Opening Mada Kari Forest
	void OnNPCStartDialogue (string[] dialogList,double startTime,double endTime) {
		// playableDirector.initialTime = startDialogueInitTime;

		startDialogueInitTime = startTime;
		endDialogueInitTime = endTime;

		SetPlayableInitTime(startDialogueInitTime);
		OnWaitingDialogue(dialogList,startTime,endTime);
		// Debug.Log("START DIALOGUE SESSION");
	}

	void OnWaitingDialogue (string[] dialogList,double startTime,double endTime) {
		// Debug.Log("REPEAT IDLE ANIMATION FOR DIALOGUE SESSION");
		playableDirector.Stop();
		playableDirector.DeferredEvaluate(); //IMPORTANT
		playableDirector.Play();
	}

	void OnNPCEndDialogue () {
		// playableDirector.initialTime = endDialogueInitTime;
		SetPlayableInitTime(endDialogueInitTime);
		OnWaitingDialogue(null,startDialogueInitTime,endDialogueInitTime);
		// Debug.Log("END DIALOGUE SESSION");
	}

	void OnEndOpeningMKF () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		for (int i=0;i<enemyEntity.Length;i++) {
			if (enemyEntity[i]!=null)
				enemyEntity[i].enabled = true;
		}
		npcEntity.enabled = true;
		// GameStorage.Instance.IsPlayerAlreadyEnterForest = true;
		// GameStorage.Instance.PlayBGM(BGMType.MAIN);
		int cutScene22Complete = PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"Level2-2",0);
		SoundManager.Instance.PlayBGM(cutScene22Complete == 1 ? BGM.MainAfterCutScene22 : BGM.MainBeforeCutScene22);
		SavePlayedTimeline ();
		//
		// Debug.Log("STOP TIMELINE");
	}
#endregion

#region Show Ghosts
	void OnEndShowGhosts () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;

		for (int i=0;i<enemyEntity.Length;i++) {
			if (enemyEntity[i]!=null)
				enemyEntity[i].enabled = true;
		}

		playerTransform.position = new Vector3(21f,0.9f,25f);
		Debug.Log(playerTransform.position);
		Debug.Log(playerTransform.localPosition);
		playerSpriteTransform.localPosition = new Vector3(0f,-0.9f,0f);
		camera22Transform.position = new Vector3(17f,10f,13f);
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
		// GameStorage.Instance.PlayBGM(BGMType.FIGHT_JATAYU);
		SoundManager.Instance.PlayBGM(BGM.JatayuFight);
	}

	void OnEndBossArea () {
		playableDirector.enabled = false;
		playerEntity.enabled = true;
		Debug.Log("OnEndBossArea TIMELINE");
		//SavePlayedTimeline ();
	}

	void ResumeBossFightTimeline()
	{
		// GameStorage.Instance.PlayBGM(BGMType.BEFORE_JATAYU);
		SoundManager.Instance.PlayBGM(BGM.LevelJatayu);
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
