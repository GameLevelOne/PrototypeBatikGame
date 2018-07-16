using UnityEngine;

public class Portal : MonoBehaviour {
	public string sceneDestination;
	public bool triggered = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player"){
			triggered = true;
		}
	}
}
