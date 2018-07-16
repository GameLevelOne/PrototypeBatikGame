// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {
	public delegate void ControllingAnimation(string strAnim);
	public event ControllingAnimation OnStartAnimation;
	public event ControllingAnimation OnExitAnimation;

	Animator anim;
	bool isAnimating;

	void Awake () {
		anim = GetComponent<Animator>();
	}

	public void StartAnim (string animName) {
		if (isAnimating) {
			return;
		}

		isAnimating = true;
		if (OnStartAnimation != null) {
			OnStartAnimation(animName);
		}
	}

	public void ExitAnim (string animName) {
		isAnimating = false;
		if (OnExitAnimation != null) {
			OnExitAnimation(animName);
		}
	}

	// public void StopAnim () {
	// 	anim.enabled = false;
	// }
}
