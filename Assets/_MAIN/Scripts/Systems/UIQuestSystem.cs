// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class UIQuestSystem : ComponentSystem {
	public struct UIQuestData {
		public readonly int Length;
		public ComponentArray<UIQuest> UIQuest;
	}
	[InjectAttribute] UIQuestData uiQuestData;
	[InjectAttribute] QuestSystem questSystem;

	UIQuest uiQuest;

	protected override void OnUpdate () {
		for (int i=0; i<uiQuestData.Length; i++) {
			uiQuest = uiQuestData.UIQuest[i];

			if (!uiQuest.isInitUIQuest) {
				try {
					InitUIQuest();
				} catch {
					Debug.Log("Error on init quest");
					return;
				}
			} else {
				// CheckIfAnyQuestIsDone();
			}
		}
	}

	void InitUIQuest () {
		CheckIfUIQuestIsComplete ();

		uiQuest.isInitUIQuest = true;
	}

	public void CheckIfAnyQuestIsDone () {
		// for (int i=0; i<uiQuest.questTexts.Length; i++) {
		// 	Debug.Log("Quest index "+i+", "+questSystem.quest.questPointRequired[i]);
		// }
		Quest quest = questSystem.quest;
		int uiQuestIndex = quest.currentQuestIndex-1;

		if (quest.isQuestProcessForUI) {
			Debug.Log("UIQuestSystem Check UI Quest : "+uiQuestIndex);
			Debug.Log("UIQuestSystem Do quest for : "+uiQuest.questTexts[uiQuestIndex].text);

			quest.isQuestProcessForUI = false;
		} else if (quest.isQuestDoneForUI) {
			CheckIfUIQuestIsComplete ();
			
			quest.isQuestDoneForUI = false;
		}
	}

	public void CheckIfUIQuestIsComplete () {
		for (int i=0; i<uiQuest.questTexts.Length; i++) {
			// int questIndex = i + 1;
			CheckQuestIndex(i);

			if (questSystem.CheckIfQuestIsComplete(i)) {
				uiQuest.questTexts[i].color = uiQuest.completedTextColor;
				uiQuest.questTexts[i].GetComponent<Outline>().enabled = true;
				uiQuest.questImages[i].sprite = uiQuest.completeSprite;
				// uiQuest.completeTexts[i].enabled = true;
				// Debug.LogWarning("UIQuestSystem Quest "+i+" is Complete");
			} else {
				uiQuest.questTexts[i].color = uiQuest.initTextColor;
				uiQuest.questTexts[i].GetComponent<Outline>().enabled = false;
				uiQuest.questImages[i].sprite = uiQuest.unCompleteSprite;
				// uiQuest.completeTexts[i].enabled = false;
			}
		}
	}

	void CheckQuestIndex (int idx) {
		switch (idx) {
			case 0: 
				uiQuest.questTexts[0].text = "KILL ("+questSystem.quest.questCurrentPoint[0]+"/"+questSystem.quest.questPointRequired[0]+") BEE ON FOREST ENTRANCE";
				break;
			case 1:
				//
				break;
			case 2: 
				uiQuest.questTexts[2].text = "KILL ("+questSystem.quest.questCurrentPoint[2]+"/"+questSystem.quest.questPointRequired[2]+") WANDERING SPRIRIT ON ABANDONED CLEARING";
				break;
			case 3: 
				uiQuest.questTexts[3].text = "KILL ("+questSystem.quest.questCurrentPoint[3]+"/"+questSystem.quest.questPointRequired[3]+") ENEMY IN HIDDEN CLEARING";
				break;
			case 4:
				//
				break;
			case 5: 
				uiQuest.questTexts[5].text = "KILL ("+questSystem.quest.questCurrentPoint[5]+"/"+questSystem.quest.questPointRequired[5]+") ENEMY IN HIDDEN CAVE";
				break;
			case 6: 
				//
				break;
		}
	}
}
