using UnityEngine;

public class Portal : MonoBehaviour {
	public string sceneDestination;
	public bool triggered = false;
	public UIFader uiFader;
	
	[HeaderAttribute("1 Bottom, 2 Left, 3 Up, 4 Right")]
	public int dir;

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == Constants.Tag.PLAYER){
			triggered = true;
		}
	}
}
