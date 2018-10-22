using UnityEngine;
using Unity.Entities;

public class CutsceneTriggerSystem : ComponentSystem {
	public struct CutsceneTriggerData {
		public readonly int Length;
		public ComponentArray<CutsceneTrigger> CutsceneTrigger;
	}
	[InjectAttribute] CutsceneTriggerData cutsceneTriggerData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	CutsceneTrigger cutsceneTrigger;

	protected override void OnUpdate () {
		for (int i=0; i<cutsceneTriggerData.Length; i++) {
			cutsceneTrigger = cutsceneTriggerData.CutsceneTrigger[i];

			if (!cutsceneTrigger.isInitCutscene) {
				InitCutScene();
			} else if (cutsceneTrigger.isTriggered) {
				//PLAY CUTSCENE
				cutsceneTrigger.timelineManager.playerEntity.GetComponent<Animation2D>().animator.Play(Constants.BlendTreeName.IDLE_STAND);
				playerInputSystem.SetDir(0f,0f); //0,1
				// playerInputSystem.ChangeDir(0f, 1f);
				// playerInputSystem.CheckLockDir(2, 1, 3);
				cutsceneTrigger.timelineManager.playerEntity.GetComponent<PlayerInput>().moveDir = Vector3.zero;
				cutsceneTrigger.timelineManager.playerEntity.GetComponent<Rigidbody>().velocity = Vector3.zero;
				cutsceneTrigger.timelineManager.playerEntity.GetComponent<Player>().SetPlayerIdle();
				cutsceneTrigger.timelineManager.playerEntity.enabled = false;
				for (int j=0;j<cutsceneTrigger.timelineManager.enemyEntity.Length;j++) {
					if (cutsceneTrigger.timelineManager.enemyEntity[j]!=null)
						cutsceneTrigger.timelineManager.enemyEntity[j].enabled = false;
				}
				

				//SET TRACK & PLAY
				cutsceneTrigger.timelineManager.playableDirector.playableAsset = cutsceneTrigger.playableCutscene;
				if(cutsceneTrigger.playableCutscene.name == "Level2-2") SoundManager.Instance.PlayBGM(BGM.CutScene22);
				cutsceneTrigger.timelineManager.playableDirector.Play();
				cutsceneTrigger.triggerCol.enabled = false;

				cutsceneTrigger.isTriggered = false;
			}
		}
	}

	void InitCutScene () {
		if (IsAlreadyPlayedTimeline()) {
			cutsceneTrigger.triggerCol.enabled = false;
			if (cutsceneTrigger.timelineManager.npcOpening!=null)
				cutsceneTrigger.timelineManager.npcOpening.npc.npcType = NPCType.SHOP;
		} else {
			cutsceneTrigger.triggerCol.enabled = true;
		
			if (cutsceneTrigger.timelineManager.npcOpening!=null) {
				cutsceneTrigger.timelineManager.npcOpening.npc.npcType = NPCType.OPENING;
				// GameStorage.Instance.PlayBGM(BGMType.CUTSCENE11);
				
			}
		}

		cutsceneTrigger.isInitCutscene = true;
	}

	bool IsAlreadyPlayedTimeline () {
		return PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+cutsceneTrigger.playableCutscene.name, 0) == 1 ? true : false;
	}
}
