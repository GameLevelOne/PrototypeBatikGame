using UnityEngine;

public class MonkeyTriggerDetection : MonoBehaviour {
	
	public delegate void DetectMonkey(Monkey monkey);
	public event DetectMonkey OnAddMonkey;
	public event DetectMonkey OnRemoveMonkey;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<Monkey>() != null){
			if(OnAddMonkey != null) OnAddMonkey(other.GetComponent<Monkey>());
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.GetComponent<Monkey>() != null){
			if(OnRemoveMonkey != null) OnRemoveMonkey(other.GetComponent<Monkey>());
		}
	}
}
