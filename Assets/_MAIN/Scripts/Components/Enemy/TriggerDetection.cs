using UnityEngine;

public class TriggerDetection : MonoBehaviour {
	public delegate void TriggerEnter(GameObject other);
	public event TriggerEnter OnTriggerEnter;

	public string tagName;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == tagName){
			if(OnTriggerEnter != null) OnTriggerEnter(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == tagName){
			if(OnTriggerEnter != null) OnTriggerEnter(null);
		}
	}
}
