// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum TimelineEventType {
	NPC_OPENING_DIALOGUE,
	END_OPENING_ENTRANCE,
	AFTER_SHOW_GHOSTS,
	BEFORE_BOSS_FIGHT,
	AFTER_BOSS_FIGHT
}

public class TimelineEventTrigger : MonoBehaviour {
	public delegate void TimelineEvent();
	public delegate void TimelineDialogEvent(string[] dialogList,double startTime,double endTime);

	// Entrance Mada Kari Forest
	public event TimelineDialogEvent OnNPCStartDialogue;
	public event TimelineDialogEvent OnWaitingDialogue;
	public event TimelineEvent OnEndOpeningMKF;

	// Show Ghosts
	// public event TimelineEvent OnStartShowGhosts;
	public event TimelineEvent OnEndShowGhosts;

	// Entrance Boss Area
	public event TimelineEvent OnEntranceBossArea;
	public event TimelineEvent OnBossFightFinish;
	public event TimelineEvent OnEndBossArea;

	public TimelineEventType type;	

	public bool isInitEnableTrigger = false;
	// public bool isInitDisableTrigger = false;
	public bool isInitEvent = false;

	[HeaderAttribute("Data")]
	public string[] dialogs;
	public double dialogStartTime;
	public double dialogEndTime;

	void OnEnable () {
		if (!isInitEnableTrigger) {
			isInitEnableTrigger = true;
		} else {
			//  // Debug.Log("Call something "+type);
			if (type == TimelineEventType.NPC_OPENING_DIALOGUE) {
				if (!isInitEvent) {	
					if (OnNPCStartDialogue != null) {
						OnNPCStartDialogue(dialogs, dialogStartTime, dialogEndTime);
					}

					isInitEvent = true;
				} else {
					if (OnWaitingDialogue != null) {
						OnWaitingDialogue(dialogs, dialogStartTime, dialogEndTime);
					}
				}
			} else if (type == TimelineEventType.END_OPENING_ENTRANCE) {
				if (!isInitEvent) {
					if (OnEndOpeningMKF != null) {
						OnEndOpeningMKF();
					}

					isInitEvent = true;
				} else {
					if (OnWaitingDialogue != null) {
						OnWaitingDialogue(dialogs, dialogStartTime, dialogEndTime);
					}
				}
			}else if (type == TimelineEventType.AFTER_SHOW_GHOSTS) {
				if (!isInitEvent) {
					if (OnEndShowGhosts != null) {
						OnEndShowGhosts();
					}

					isInitEvent = true;
				}
			} else if (type == TimelineEventType.BEFORE_BOSS_FIGHT) {
				if (!isInitEvent) {
					if (OnEntranceBossArea != null) {
						OnEntranceBossArea();
					}

					isInitEvent = true;
				}
			} else if (type == TimelineEventType.AFTER_BOSS_FIGHT) {
				if (!isInitEvent) {
					if (OnEndBossArea != null) {
						OnEndBossArea();
					}

					 // Debug.Log("OnEndBossArea isInitEvent");
					isInitEvent = true;
				}
			}	
		}
	}

	void OnDisable () {
		// if (!isInitDisableTrigger) {
		// 	isInitDisableTrigger = true;
		// } else {
			//  // Debug.Log("End Call "+type);
		// }
	}

	public void JatayuDie()
	{
		if(OnBossFightFinish != null) OnBossFightFinish();
	}
}
