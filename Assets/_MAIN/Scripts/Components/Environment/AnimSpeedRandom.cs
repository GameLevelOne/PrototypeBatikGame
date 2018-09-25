using UnityEngine;

public class AnimSpeedRandom : MonoBehaviour {
	public float minSpeed = 1f;
	public float maxSpeed = 2f;
	[HeaderAttribute("Current")]
	public float animSpeed;
	public bool initRandomSpeed = false;
	
	void OnIdleEnd()
	{
		initRandomSpeed = false;
	}
}
