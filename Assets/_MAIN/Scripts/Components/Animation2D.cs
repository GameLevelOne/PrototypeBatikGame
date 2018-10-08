﻿using UnityEngine;

// public enum AnimationState {
// 	START_SLASH,
// 	START_CHARGE,
// 	START_DODGE,
// 	AFTER_SLASH,
// 	AFTER_CHARGE,
// 	AFTER_DODGE,
// 	START_COUNTER,
// 	AFTER_COUNTER,
// 	START_RAPIDSLASH,
// 	AFTER_RAPIDSLASH,
// 	START_BLOCK,
// 	AFTER_BLOCK,
// 	START_DASH,
// 	AFTER_DASH,
// 	START_BRAKING,
// 	AFTER_BRAKING,
// 	START_HURT,
// 	AFTER_HURT,
// 	START_GRAB,
// 	AFTER_GRAB,
// 	START_UNGRAB,
// 	AFTER_UNGRAB,
// 	START_THROW,
// 	AFTER_THROW,
// 	START_LIFT,
// 	AFTER_LIFT,
// 	START_FISHING,
// 	AFTER_FISHING
// }

// public enum StandAnimationState {
// 	START_USING_TOOL,
// 	AFTER_USING_TOOL
// }

public enum AnimationAudio {
	HAMMER,
	SHOVEL
}

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public AudioSource audioSource;
	public AudioClip[] audioClip;

	public bool isInitAnimation = false;

	[HeaderAttribute("Current")]
	public string currentAnimName;
	public bool isFinishAttackAnimation = true;
	[SerializeField] bool isFinishAnyAnim = true;
	public bool isFinishAnyAnimation {
		get {return isFinishAnyAnim;}
		set {
			isFinishAnyAnim = value;

			// if (value) {
			// 	Debug.Log("Set isFinishAnyAnim TRUE");
			// }
		}
	}


	[HeaderAttribute("For Player & Enemy")]
	public bool isCheckAfterAnimation = false;
	public bool isCheckBeforeAnimation = false;


	// [SpaceAttribute(10f)]
	[HeaderAttribute("For Attack / Stand / Tool")]
	public bool isSpawnSomethingOnAnimation = false;
	public bool isCheckAfterStandAnimation = false;
	public bool isCheckBeforeStandAnimation = false;

	void OnEnable () {
		// animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
		animationControl.OnStartAnimation += StartAnimation;
		animationControl.OnExitAnimation += ExitAnimation;
		animationControl.OnSpawnSomethingOnAnimation += SpawnSomethingOnAnimation;
		animationControl.OnStartStandAnimation += StartStandAnimation;
		animationControl.OnExitStandAnimation += ExitStandAnimation;
	}

	void OnDisable () {
		animationControl.OnStartAnimation -= StartAnimation;
		animationControl.OnExitAnimation -= ExitAnimation;
		animationControl.OnSpawnSomethingOnAnimation -= SpawnSomethingOnAnimation;
		animationControl.OnStartStandAnimation -= StartStandAnimation;
		animationControl.OnExitStandAnimation -= ExitStandAnimation;
	}

	#region Player and Enemy Animation
	void StartAnimation () {
		// isCheckBeforeAnimation = false;
		// Debug.Log("isCheckBeforeAnimation = false");
	}

	void ExitAnimation () {
		isCheckAfterAnimation = false;
		// Debug.Log("Check after animation FALSE");
	}

	void SpawnSomethingOnAnimation () {
		isSpawnSomethingOnAnimation = true;
	}
	#endregion

	#region Stand Animation
	void StartStandAnimation () {
		isCheckBeforeStandAnimation = false;
		// Debug.Log("Set isCheckBeforeStandAnimation FALSE StartStan");
	}
	
	void ExitStandAnimation () {
		isCheckAfterStandAnimation = false;
		// Debug.Log("Set isCheckAfterStandAnimation FALSE ExitStand");
	}
	#endregion
}
