﻿using UnityEngine;

public class TriggerDetection : MonoBehaviour {
	public delegate void TriggerEnter(GameObject other);
	public event TriggerEnter OnTriggerEnterObj;

	public string tagName;

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == tagName){
			if(OnTriggerEnterObj != null) OnTriggerEnterObj(other.gameObject);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.tag == tagName){
			if(OnTriggerEnterObj != null) OnTriggerEnterObj(null);
		}
	}
}
