using UnityEngine;

public class Portal : MonoBehaviour {
	[HeaderAttribute("Reference")]
	public UIFader uiFader;
	public string sceneDestination;
	public int startPosIndex;
	[HeaderAttribute("Current")]
	public bool triggered = false;
	
	
	[HeaderAttribute("1 Bottom, 2 Left, 3 Up, 4 Right")]
	public int dir;

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == Constants.Tag.PLAYER){
			 // Debug.Log("Player Hit Portal Destination: "+sceneDestination);
			triggered = true;
		}
	}
}
