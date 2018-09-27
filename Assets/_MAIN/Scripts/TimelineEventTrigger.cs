// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum EventType {
	NPC_OPENING_DIALOGUE,
	END_OPENING_ENTRANCE,
	BEFORE_BOSS_FIGHT,
	AFTER_BOSS_FIGHT
}

public class TimelineEventTrigger : MonoBehaviour {
	public delegate void TimelineEvent();

	// Entrance Mada Kari Forest
	public event TimelineEvent OnNPCStartDialogue;
	public event TimelineEvent OnWaitingDialogue;
	public event TimelineEvent OnEndOpeningMKF;

	// Entrance Boss Area
	public event TimelineEvent OnEntranceBossArea;
	public event TimelineEvent OnBossFightFinish;
	public event TimelineEvent OnEndBossArea;

	public EventType type;	

	public bool isInitEnableTrigger = false;
	// public bool isInitDisableTrigger = false;
	public bool isInitEvent = false;

	void OnEnable () {
		if (!isInitEnableTrigger) {
			isInitEnableTrigger = true;
		} else {
			// Debug.Log("Call something "+type);
			if (type == EventType.NPC_OPENING_DIALOGUE) {
				if (!isInitEvent) {	
					if (OnNPCStartDialogue != null) {
						OnNPCStartDialogue();
					}

					isInitEvent = true;
				} else {
					if (OnWaitingDialogue != null) {
						OnWaitingDialogue();
					}
				}
			} else if (type == EventType.END_OPENING_ENTRANCE) {
				if (!isInitEvent) {
					if (OnEndOpeningMKF != null) {
						OnEndOpeningMKF();
					}

					isInitEvent = true;
				} else {
					if (OnWaitingDialogue != null) {
						OnWaitingDialogue();
					}
				}
			} else if (type == EventType.BEFORE_BOSS_FIGHT) {
				if (!isInitEvent) {
					if (OnEntranceBossArea != null) {
						OnEntranceBossArea();
					}

					isInitEvent = true;
				}
			} else if (type == EventType.AFTER_BOSS_FIGHT) {
				if (!isInitEvent) {
					if (OnEndBossArea != null) {
						OnEndBossArea();
					}

					Debug.Log("OnEndBossArea isInitEvent");
					isInitEvent = true;
				}
			}	
		}
	}

	void OnDisable () {
		// if (!isInitDisableTrigger) {
		// 	isInitDisableTrigger = true;
		// } else {
			// Debug.Log("End Call "+type);
		// }
	}

	public void JatayuDie()
	{
		if(OnBossFightFinish != null) OnBossFightFinish();
	}
}
