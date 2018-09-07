// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum EventType {
	NPC_DIALOGUE,
	COMINGSOON
}

public class TimelineEventTrigger : MonoBehaviour {
	public delegate void TimelineEvent();
	public event TimelineEvent OnNPCDialogueTrigger;


	public EventType type;	

	public bool isInitEnableTrigger = false;
	public bool isInitDisableTrigger = false;

	void OnEnable () {
		if (!isInitEnableTrigger) {
			isInitEnableTrigger = true;
		} else {
			Debug.Log("Call something "+type);
			if (type == EventType.NPC_DIALOGUE) {
				if (OnNPCDialogueTrigger != null) {
					OnNPCDialogueTrigger();
				}
			}			
		}
	}

	void OnDisable () {
		if (!isInitDisableTrigger) {
			isInitDisableTrigger = true;
		} else {
			Debug.Log("End Call "+type);
		}
	}
}
