// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum EventType {
	NPC_DIALOGUE,
	END_TIMELINE
}

public class TimelineEventTrigger : MonoBehaviour {
	public delegate void TimelineEvent();
	public event TimelineEvent OnNPCDialogueTrigger;
	public event TimelineEvent OnStopPlaybackTrigger;
	public event TimelineEvent OnEndTimelineTrigger;

	public EventType type;	

	public bool isInitEnableTrigger = false;
	// public bool isInitDisableTrigger = false;
	public bool isInitEvent = false;

	void OnEnable () {
		if (!isInitEnableTrigger) {
			isInitEnableTrigger = true;
		} else {
			// Debug.Log("Call something "+type);
			if (type == EventType.NPC_DIALOGUE) {
				if (!isInitEvent) {	
					if (OnNPCDialogueTrigger != null) {
						OnNPCDialogueTrigger();
					}

					isInitEvent = true;
				} else {
					if (OnStopPlaybackTrigger != null) {
						OnStopPlaybackTrigger();
					}
				}
			} else if (type == EventType.END_TIMELINE) {
				if (!isInitEvent) {
					if (OnEndTimelineTrigger != null) {
						OnEndTimelineTrigger();
					}

					isInitEvent = true;
				} else {
					if (OnStopPlaybackTrigger != null) {
						OnStopPlaybackTrigger();
					}
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
