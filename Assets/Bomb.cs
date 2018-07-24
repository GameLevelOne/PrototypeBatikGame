using UnityEngine;

public class Bomb : MonoBehaviour {
	[HeaderAttribute("Reference")]
	public GameObject spriteObj;
	public GameObject bombExplodeAreaObj;
	public Animator bombAnimator;
	[Header("Variables")]
	public float timer = 5f;
	public bool explode = false;
	public bool destroy = false;

	void OnExplode()
	{
		destroy = true;
	}
}
