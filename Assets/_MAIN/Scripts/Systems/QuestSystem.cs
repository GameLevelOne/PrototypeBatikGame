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

	public struct UIQuestData {
		public readonly int Length;
		public ComponentArray<UIQuest> uiQuest;

	}
	public struct UINotifData {
		public readonly int Length;
		public ComponentArray<UINotif> uiNotif;

	}
	[InjectAttribute] AreaDissolverData areaDissolverData;

	[InjectAttribute] AreaDissolverSystem areaDissolverSystem;
	[InjectAttribute] UIQuestData uiQuestData;
	[InjectAttribute] UIQuestSystem uiQuestSystem;
	[InjectAttribute] UINotifData uiNotifData;

	public Quest quest;

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

		Debug.Log("Init Quest");
		LoadQuest();

		quest.isInitQuest = true;
	}

	void LoadQuest () {
		for (int i=0; i<quest.questCurrentPoint.Length; i++) {
			string questCurrentPointStr = Constants.QuestPrefKey.QUEST_INDEX + i;

			quest.questCurrentPoint[i] = PlayerPrefs.GetInt(questCurrentPointStr, 0);

			Debug.Log(questCurrentPointStr);
			// if (CheckIfQuestIsComplete(i)) {
			// 	quest.isQuestDoneForUI = true;
			// 	Debug.Log("Quest "+i+" is Complete");
			// }
		}

		quest.isQuestDoneForUI = true;
	}

	void SaveQuest (int questIdx, int value) {
		string questCurrentPointStr = Constants.QuestPrefKey.QUEST_INDEX + questIdx;

		PlayerPrefs.SetInt(questCurrentPointStr, value);

		// Debug.Log(questCurrentPointStr);
	}

	void CheckQuest () {
		if (quest.isQuestProcess) {
			ProcessQuest(quest.currentQuestIndex);

			quest.isQuestProcess = false;
		}
	}

	void ProcessQuest (int questIdx) {
		// Debug.Log("QuestSystem ProcessQuest : "+questIdx);
		quest.questCurrentPoint[questIdx]++;

		if (CheckIfQuestIsComplete(questIdx)) {
			SaveQuest (questIdx, quest.questPointRequired[questIdx]);

			for (int i=0; i<areaDissolverData.Length; i++) {
				AreaDissolver areaDissolver = areaDissolverData.AreaDissolver[i];

				if (areaDissolverSystem.CheckCurrentLevelbyQuest(questIdx)) {
					areaDissolver.isDissolveArea = true;
					Debug.Log("QuestSystem Dissolving Area Quest "+questIdx);
				}
			}

			string textToShow="";
			for (int i=0;i<uiQuestData.Length; i++) {
				UIQuest uiQuest = uiQuestData.uiQuest[i];

				uiQuestSystem.CheckIfUIQuestIsComplete();

				textToShow = uiQuest.questTexts[questIdx].text;
				textToShow += " <color=#00ff00ff>(COMPLETED!)</color>";				
			}
			Debug.Log("QuestSystem text to show: "+textToShow);
			for (int i=0;i<uiNotifData.Length;i++)
			{
				UINotif uiNotif = uiNotifData.uiNotif[i];
				uiNotif.TextToShow = textToShow;
				uiNotif.call = true;
			}
			
			Debug.Log("QuestSystem Quest "+questIdx+" is Complete");
			quest.isQuestDoneForUI = true;
		}
	}

	public bool CheckIfQuestIsComplete (int questIdx) {
		if (quest.questCurrentPoint[questIdx] >= quest.questPointRequired[questIdx]) {
			// Debug.Log("IsQuest "+questIdx+" Complete "+true);
			return true;
		} else {
			// Debug.Log("IsQuest "+questIdx+" Complete "+false);
			return false;
		}
	}
}
