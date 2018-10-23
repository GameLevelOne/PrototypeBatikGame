using UnityEngine;

public class WaterRippleFX : MonoBehaviour {	
	[HeaderAttribute("Current")]
	public bool animDone = false;
	public bool initDelay = false;
	public float tDelay = 0f;
	
	void OnEndAnim()
	{
		animDone = true;
	}
}
