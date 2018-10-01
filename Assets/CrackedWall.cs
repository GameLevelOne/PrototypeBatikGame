// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour {
	public GameObject crackedWallObj;
	public Collider crackedWallMainCol;
	public ParticleSystem crackedWallExplodeFX;

	[HeaderAttribute("Saved ID")]
	public int crackedWallID;

	[HeaderAttribute("Current")]
	public bool isInitCrackedWall = false;
	public bool isCrackedWallDestroyed = false;
	public bool destroy = false;

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.EXPLOSION) {
			crackedWallExplodeFX.Play(true);
			destroy = true;
		}
	}
}
