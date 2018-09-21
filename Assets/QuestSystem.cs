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

	// [InjectAttribute] AreaDissolverSystem areaDissolverSystem;

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
		//

		quest.isInitQuest = true;
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
			for (int i=0; i<areaDissolverData.Length; i++) {
				AreaDissolver areaDissolver = areaDissolverData.AreaDissolver[i];

				if (areaDissolver.levelQuestIndex == questIdx && !areaDissolver.isAreaAlreadyDissolved) {
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
