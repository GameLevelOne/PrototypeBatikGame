// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {
	public delegate void ControllingAnimation(AnimationState state);
	public event ControllingAnimation OnStartAnimation;
	public event ControllingAnimation OnExitAnimation;
	
	public delegate void ControllingStandAnimation(StandAnimationState state);
	public event ControllingStandAnimation OnStartStandAnimation;
	public event ControllingStandAnimation OnExitStandAnimation;

	Animator anim;
	bool isAnimating;

	void Awake () {
		anim = GetComponent<Animator>();
	}

	public void StartAnim (AnimationState startState) {
		if (isAnimating) {
			return;
		}

		isAnimating = true;
		if (OnStartAnimation != null) {
			OnStartAnimation(startState);
		}
	}

	public void ExitAnim (AnimationState endState) {
		isAnimating = false;
		if (OnExitAnimation != null) {
			OnExitAnimation(endState);
		}
	}

	public void StartStandAnim (StandAnimationState startState) {
		if (isAnimating) return;

		isAnimating = true;
		if (OnStartStandAnimation != null) {
			OnStartStandAnimation(startState);
		}
	}

	public void ExitStandAnim (StandAnimationState endState) {
		isAnimating = false;
		if (OnExitStandAnimation != null) {
			OnExitStandAnimation(endState);
		}
	}

	public void StopAnim () {
		anim.enabled = false;
	}
}
