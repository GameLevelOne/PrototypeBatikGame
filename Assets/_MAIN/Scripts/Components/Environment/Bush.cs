using UnityEngine;

public class Bush : MonoBehaviour {
	public GameObject bushCutFX;
	public float spawnItemProbability;
	public GameObject rootObj;
	
	[HeaderAttribute("Current")]
	public bool isInitBush = false;
	public bool animateEnd = false;
	public bool isRootSpawned = false;
	public bool isLifted = false;
	public bool destroy = false;
	public Vector3 initBushPos;
	
	void OnBushAnimateEnd()
	{
		animateEnd = true;
	}
}
