using UnityEngine;

public class QuestTrigger : MonoBehaviour {
	// public delegate void QuestEvent(int questIndex);
	// public event QuestEvent OnQuestProcess;
	// public event QuestEvent OnQuestComplete;

	public int questIndex;
	public bool isInitQuestTrigger = false;

	// [HeaderAttribute("Current")]
	public bool isDoQuest = false;

	// public void QuestProcess () {
	// 	if (OnQuestProcess != null) {
	// 		OnQuestProcess(questIndex);
	// 	}
	// }

	// public void QuestComplete () {
	// 	if (OnQuestComplete != null) {
	// 		OnQuestComplete(questIndex);
	// 	}
	// }
}
