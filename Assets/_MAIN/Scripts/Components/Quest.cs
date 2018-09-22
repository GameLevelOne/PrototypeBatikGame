using UnityEngine;

// public enum QuestType {
// 	DESTROY, 
// 	COLLECT,
// 	OPEN
// }

public class Quest : MonoBehaviour {
	// public QuestTrigger questTrigger;
	
	public int[] questPointRequired; 
	public int[] questCurrentPoint;
	// public int[] questComplete;

	public bool isInitQuest = false;

	[HeaderAttribute("Current")]
	public int currentQuestIndex;
	public bool isQuestProcess = false;

	[HeaderAttribute("TESTING")]
	public bool isTesting = false;

	// void OnEnable () {
	// 	questTrigger.OnQuestProcess += OnQuestProcess;
	// 	questTrigger.OnQuestComplete += OnQuestComplete;
	// }

	// void OnDisable () {
	// 	questTrigger.OnQuestProcess -= OnQuestProcess;
	// 	questTrigger.OnQuestComplete -= OnQuestComplete;
	// }

	public void OnQuestProcess (int questIdx) {
		if (questCurrentPoint[questIdx] < questPointRequired[questIdx]) {
			currentQuestIndex = questIdx;
			isQuestProcess = true;

			Debug.Log("Quest Process : "+questIdx);
		} else {
			Debug.Log("Quest "+questIdx+" already completed");
		}
	}

	// void OnQuestComplete (int questIdx) {
	// 	//
	// }
}
