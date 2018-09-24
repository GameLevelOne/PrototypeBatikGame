using UnityEngine;
using Unity.Entities;

public class QuestTriggerSystem : ComponentSystem {
	public struct QuestTriggerData {
		public readonly int Length;
		public ComponentArray<QuestTrigger> QuestTrigger;
	}
	[InjectAttribute] QuestTriggerData questTriggerData;

	public struct QuestData {
		public readonly int Length;
		public ComponentArray<Quest> Quest;
	}
	[InjectAttribute] QuestData questData;


	// [InjectAttribute] QuestSystem questSystem;

	QuestTrigger questTrigger;

	protected override void OnUpdate () {
		for (int i=0; i<questTriggerData.Length; i++) {
			questTrigger = questTriggerData.QuestTrigger[i];

			if (!questTrigger.isInitQuestTrigger) {
				InitQuestTrigger();
			} else {
				CheckQuestTrigger();
			}
		}
	}

	void InitQuestTrigger () {
		//

		questTrigger.isInitQuestTrigger = true;
	}

	void CheckQuestTrigger () {
		if (questTrigger.isDoQuest) {
			Debug.Log("QuestTriggerSystem Do Quest : "+questTrigger.questIndex);

			for (int i=0; i<questData.Length; i++) {
				Quest quest = questData.Quest[i];
				
				quest.OnQuestProcess(questTrigger.questIndex);
			}

			questTrigger.isDoQuest = false;
		}
	}
}
