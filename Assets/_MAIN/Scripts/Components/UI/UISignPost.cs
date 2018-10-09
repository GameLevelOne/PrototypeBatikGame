using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISignPost : MonoBehaviour {

	public GameObject joyStickPanel;
	public GameObject keyboardPanel;
	public Animator anim;

	public bool call;
	public bool isInitSignPost;
	public bool isCloseSignPost;
	public bool curLeftPressed = false;
	public bool curRightPressed = false;

	public TimelineEventTrigger endOpeningEvent;

	void OnEnable () {
		if (endOpeningEvent!=null)
			endOpeningEvent.OnEndOpeningMKF += OnEndOpeningEvent;
	}

	void OnDisable () {
		if (endOpeningEvent!=null)
			endOpeningEvent.OnEndOpeningMKF -= OnEndOpeningEvent;
	}

	void OnEndOpeningEvent() {
		call = true;
	}
}
