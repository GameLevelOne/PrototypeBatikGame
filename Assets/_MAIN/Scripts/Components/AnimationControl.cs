// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {
	public delegate void ControllingAnimation();
	public event ControllingAnimation OnStartAnimation;
	public event ControllingAnimation OnSpawnSomethingOnAnimation;
	public event ControllingAnimation OnExitAnimation;
	public event ControllingAnimation OnEndAttackAnimation;
	
	public delegate void ControllingStandAnimation();
	public event ControllingStandAnimation OnStartStandAnimation;
	public event ControllingStandAnimation OnExitStandAnimation;

	Animator anim;
	bool isAnimating;

	void Awake () {
		anim = GetComponent<Animator>();
	}

	public void StartAnim () {
		if (isAnimating) return;

		isAnimating = true;
		if (OnStartAnimation != null) {
			OnStartAnimation();
		}
	}

	public void ExitAnim () {
		isAnimating = false;
		if (OnExitAnimation != null) {
			OnExitAnimation();
		}
	}

	public void SpawnSomethingAnim () {
		if (OnSpawnSomethingOnAnimation != null) {
			OnSpawnSomethingOnAnimation();
		}
	}

	public void EndAttackAnim () {
		if (OnEndAttackAnimation != null) {
			OnEndAttackAnimation();
		}
	}

	public void StartStandAnim () {
		if (isAnimating)

		isAnimating = true;
		if (OnStartStandAnimation != null) {
			OnStartStandAnimation();
		}
	}

	public void ExitStandAnim () {
		isAnimating = false;
		if (OnExitStandAnimation != null) {
			OnExitStandAnimation();
		}
	}

	public void StopAnim () {
		anim.enabled = false;
	}

	// void Update()
	// {
	// 	GetComponent<Animator>().Play(Constants.BlendTreeName.NORMAL_ATTACK_1,0,0f);
	// }
}
