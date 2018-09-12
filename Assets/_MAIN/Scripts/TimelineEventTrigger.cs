﻿// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum EventType {
	NPC_DIALOGUE,
	END_TIMELINE
}

public class TimelineEventTrigger : MonoBehaviour {
	public delegate void TimelineEvent();
	public event TimelineEvent OnNPCDialogueTrigger;
	public event TimelineEvent OnDialogingTrigger;
	public event TimelineEvent OnEndTimelineTrigger;

	public EventType type;	

	public bool isInitEnableTrigger = false;
	// public bool isInitDisableTrigger = false;
	public bool isInitStartEntranceDialogue = false;

	void OnEnable () {
		if (!isInitEnableTrigger) {
			isInitEnableTrigger = true;
		} else {
			// Debug.Log("Call something "+type);
			if (type == EventType.NPC_DIALOGUE) {
				if (!isInitStartEntranceDialogue) {	
					if (OnNPCDialogueTrigger != null) {
						OnNPCDialogueTrigger();
					}

					isInitStartEntranceDialogue = true;
				} else {
					if (OnDialogingTrigger != null) {
						OnDialogingTrigger();
					}
				}
			} else if (type == EventType.END_TIMELINE) {
				if (OnEndTimelineTrigger != null) {
					OnEndTimelineTrigger();
				}
			}		
		}
	}

	void OnDisable () {
		// if (!isInitDisableTrigger) {
		// 	isInitDisableTrigger = true;
		// } else {
			Debug.Log("End Call "+type);
		// }
	}
}
