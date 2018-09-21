using UnityEngine;
using Unity.Entities;

public class QuestSystem : ComponentSystem {
	public struct QuestData {
		public readonly int Length;
		public ComponentArray<Quest> Quest;
	}
	[InjectAttribute] QuestData questData;

	Quest quest;

	protected override void OnUpdate () {
		for (int i=0; i<questData.Length; i++) {
			quest = questData.Quest[i];

			// if (!quest.isInitQuest) {
			// 	InitQuest();
			// } else {
				CheckQuest();
			// }
		}
	}

	// void InitQuest () {
	// 	//

	// 	quest.isInitQuest = true;
	// }

	void CheckQuest () {
		if (quest.isQuestProcess) {
			ProcessQuest(quest.currentQuestIndex);

			quest.isQuestProcess = false;
		}
	}

	void ProcessQuest (int questIdx) {
		Debug.Log("Process Quest : "+questIdx);
		quest.questCurrentPoint[questIdx]++;

		if (quest.questCurrentPoint[questIdx] >= quest.questPointRequired[questIdx]) {
			Debug.Log("Quest "+questIdx+" is Complete");
		}
	}
}
