// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class UIQuestSystem : ComponentSystem {
	public struct UIQuestData {
		public readonly int Length;
		public ComponentArray<UIQuest> UIQuest;
	}
	[InjectAttribute] UIQuestData uiQuestData;

	UIQuest uiQuest;

	protected override void OnUpdate () {
		for (int i=0; i<uiQuestData.Length; i++) {
			uiQuest = uiQuestData.UIQuest[i];

			if (!uiQuest.isInitUIQuest) {
				InitUIQuest();
			}
		}
	}

	void InitUIQuest () {
		//

		uiQuest.isInitUIQuest = true;
	}

	public void CheckIfAnyQuestIsDone () {
		
	}
}
