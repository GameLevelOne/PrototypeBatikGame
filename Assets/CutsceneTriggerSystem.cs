﻿using UnityEngine;
using Unity.Entities;

public class CutsceneTriggerSystem : ComponentSystem {
	public struct CutsceneTriggerData {
		public readonly int Length;
		public ComponentArray<CutsceneTrigger> CutsceneTrigger;
	}
	[InjectAttribute] CutsceneTriggerData cutsceneTriggerData;

	CutsceneTrigger cutsceneTrigger;

	protected override void OnUpdate () {
		for (int i=0; i<cutsceneTriggerData.Length; i++) {
			cutsceneTrigger = cutsceneTriggerData.CutsceneTrigger[i];

			if (!cutsceneTrigger.isInitCutscene) {
				InitCutScene();
			} else if (cutsceneTrigger.isTriggered) {
				//PLAY CUTSCENE
				cutsceneTrigger.timelineManager.playerEntity.enabled = false;

				//SET TRACK & PLAY
				cutsceneTrigger.timelineManager.playableDirector.playableAsset = cutsceneTrigger.playableCutscene;
				cutsceneTrigger.timelineManager.playableDirector.Play();

				cutsceneTrigger.isTriggered = false;
			}
		}
	}

	void InitCutScene () {
		if (IsAlreadyPlayedTimeline()) {
			cutsceneTrigger.triggerCol.enabled = false;
		} else {
			cutsceneTrigger.triggerCol.enabled = true;
		}

		cutsceneTrigger.isInitCutscene = true;
	}

	bool IsAlreadyPlayedTimeline () {
		return PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+cutsceneTrigger.playableCutscene.name, 0) == 1 ? true : false;
	}
}
