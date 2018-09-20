using UnityEngine;

public class Bush : MonoBehaviour {
	public GameObject bushCutFX;
	public float spawnItemProbability;
	
	[HeaderAttribute("Current")]
	public bool init = false;
	public bool animateEnd = false;
	public bool destroy = false;
	
	void OnBushAnimateEnd()
	{
		animateEnd = true;
	}
}
