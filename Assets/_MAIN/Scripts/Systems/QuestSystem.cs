using UnityEngine;
using Unity.Entities;

public class QuestSystem : ComponentSystem {
	public struct QuestData {
		public readonly int Length;
		public ComponentArray<Quest> Quest;
	}
	[InjectAttribute] QuestData questData;

	public struct AreaDissolverData {
		public readonly int Length;
		public ComponentArray<AreaDissolver> AreaDissolver;
	}
	[InjectAttribute] AreaDissolverData areaDissolverData;

	[InjectAttribute] AreaDissolverSystem areaDissolverSystem;

	Quest quest;

	protected override void OnUpdate () {
		for (int i=0; i<questData.Length; i++) {
			quest = questData.Quest[i];

			if (!quest.isInitQuest) {
				InitQuest();
			} else {
				CheckQuest();
			}
		}
	}

	void InitQuest () {
		// if (quest.isTesting) {
		// 	SaveQuest(quest.currentQuestIndex, 0);
		// }

		LoadQuest();

		quest.isInitQuest = true;
	}

	void LoadQuest () {
		for (int i=0; i<quest.questCurrentPoint.Length; i++) {
			string questCurrentPointStr = Constants.QuestPrefKey.QUEST_INDEX + i;

			quest.questCurrentPoint[i] = PlayerPrefs.GetInt(questCurrentPointStr, 0);

			Debug.Log(questCurrentPointStr);
		}
	}

	void SaveQuest (int questIdx, int value) {
		string questCurrentPointStr = Constants.QuestPrefKey.QUEST_INDEX + questIdx;

		PlayerPrefs.SetInt(questCurrentPointStr, value);

		Debug.Log(questCurrentPointStr);
	}

	void CheckQuest () {
		if (quest.isQuestProcess) {
			ProcessQuest(quest.currentQuestIndex);

			quest.isQuestProcess = false;
		}
	}

	void ProcessQuest (int questIdx) {
		Debug.Log("Process Quest : "+questIdx);
		quest.questCurrentPoint[questIdx]++;

		if (CheckIfQuestIsComplete(questIdx)) {
			SaveQuest (questIdx, quest.questPointRequired[questIdx]);

			for (int i=0; i<areaDissolverData.Length; i++) {
				AreaDissolver areaDissolver = areaDissolverData.AreaDissolver[i];

				if (areaDissolverSystem.CheckCurrentLevelbyQuest(questIdx)) {
					areaDissolver.isDissolveArea = true;
					Debug.Log("Dissolving Area Quest "+questIdx);
				}
			}
			
			Debug.Log("Quest "+questIdx+" is Complete");
		}
	}

	public bool CheckIfQuestIsComplete (int questIdx) {
		if (quest.questCurrentPoint[questIdx] >= quest.questPointRequired[questIdx]) {
			return true;
		} else {
			return false;
		}
	}
}
