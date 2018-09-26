using UnityEngine;

public class TriggerDetection : MonoBehaviour {
	public delegate void TriggerEnter(GameObject other);
	public event TriggerEnter OnTriggerEnterObj;

	public string tagName;

	public string[] tagNames;

	void OnTriggerEnter (Collider other)
	{
		if (tagNames.Length == 0) {
			if(other.tag == tagName && OnTriggerEnterObj != null) OnTriggerEnterObj(other.gameObject);
			
		} else {
			GameObject hitObj = null;
			for(int i = 0;i<tagNames.Length;i++){
				if(other.tag == tagNames[i]){
					hitObj = other.gameObject;
					break;
				}
			}

			if(hitObj != null && OnTriggerEnterObj != null) OnTriggerEnterObj(other.gameObject);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(tagNames.Length == 0){
			if(other.tag == tagName && OnTriggerEnterObj != null) OnTriggerEnterObj(null);
			
		}else{
			GameObject hitObj = null;
			for(int i = 0;i<tagNames.Length;i++){
				if(other.tag == tagNames[i]){
					hitObj = other.gameObject;
					break;
				}
			}

			if(hitObj != null && OnTriggerEnterObj != null) OnTriggerEnterObj(null);
		}
	}
}
