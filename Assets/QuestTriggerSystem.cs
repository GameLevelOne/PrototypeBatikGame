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
	Quest quest;

	protected override void OnUpdate () {
		for (int i=0; i<questTriggerData.Length; i++) {
			questTrigger = questTriggerData.QuestTrigger[i];

			if (!questTrigger.isInitQuestTrigger) {
				InitQuestTrigger();
			} else {
				CheckQuestTrigger();
			}
		}

		for (int i=0; i<questData.Length; i++) {
			quest = questData.Quest[i];

			if (!quest.isInitQuest) {
				InitQuest();
			}
		}
	}

	void InitQuestTrigger () {
		//

		questTrigger.isInitQuestTrigger = true;
	}
	
	void InitQuest () {
		//

		quest.isInitQuest = true;
	}

	void CheckQuestTrigger () {
		if (questTrigger.isDoQuest) {
			Debug.Log("Do Quest : "+questTrigger.questIndex);
			quest.OnQuestProcess(questTrigger.questIndex);

			questTrigger.isDoQuest = false;
		}
	}
}
