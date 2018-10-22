using UnityEngine;

public class Bomb : MonoBehaviour {
	// public GameObject spriteObj;
	public GameObject bombExplodeAreaObj;
	// public Animator bombAnimator;
	public float explodeTimer = 3.5f;
	
	[Header("Current")]
	public bool isInitBomb = false;
	public float timer;
	public bool explode = false;
	// public bool destroy = false;

	// void OnExplode()
	// {
	// 	destroy = true;
	// }
}
