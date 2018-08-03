using UnityEngine;

public enum AnimationState {
	START_SLASH,
	START_CHARGE,
	START_DODGE,
	AFTER_SLASH,
	AFTER_CHARGE,
	AFTER_DODGE,
	START_COUNTER,
	AFTER_COUNTER,
	START_RAPIDSLASH,
	AFTER_RAPIDSLASH,
	START_BLOCK,
	AFTER_BLOCK,
	START_DASH,
	AFTER_DASH,
	START_BRAKING,
	AFTER_BRAKING,
	START_HURT,
	AFTER_HURT,
	START_GRAB,
	AFTER_GRAB,
	START_UNGRAB,
	AFTER_UNGRAB,
	START_THROW,
	AFTER_THROW,
	START_LIFT,
	AFTER_LIFT
}

public enum StandAnimationState {
	START_USING_TOOL,
	AFTER_USING_TOOL
}

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public AnimationState animState;
	public StandAnimationState standAnimState;

	[SerializeField] bool isCheckAfterAnimation = false;
	[SerializeField] bool isCheckBeforeAnimation = false;
	[SerializeField] bool isCheckAfterStandAnimation = false;
	[SerializeField] bool isCheckBeforeStandAnimation = false;

	void OnEnable () {
		animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
		animationControl.OnStartAnimation += StartAnimation;
		animationControl.OnExitAnimation += ExitAnimation;
		animationControl.OnStartStandAnimation += StartStandAnimation;
		animationControl.OnExitStandAnimation += ExitStandAnimation;
	}

	void OnDisable () {
		animationControl.OnStartAnimation -= StartAnimation;
		animationControl.OnExitAnimation -= ExitAnimation;
		animationControl.OnStartStandAnimation -= StartStandAnimation;
		animationControl.OnExitStandAnimation -= ExitStandAnimation;
	}
	
	public bool IsCheckBeforeAnimation {
		get {return isCheckBeforeAnimation;}
		set {
			if (isCheckBeforeAnimation == value) return;

			isCheckBeforeAnimation = value;
		}
	}
	
	public bool IsCheckAfterAnimation {
		get {return isCheckAfterAnimation;}
		set {
			if (isCheckAfterAnimation == value) return;

			isCheckAfterAnimation = value;
		}
	}
	
	public bool IsCheckBeforeStandAnimation {
		get {return isCheckBeforeStandAnimation;}
		set {
			if (isCheckBeforeStandAnimation == value) return;

			isCheckBeforeStandAnimation = value;
		}
	}
	
	public bool IsCheckAfterStandAnimation {
		get {return isCheckAfterStandAnimation;}
		set {
			if (isCheckAfterStandAnimation == value) return;

			isCheckAfterStandAnimation = value;
		}
	}

	#region Player and Enemy Animation
	void StartAnimation (AnimationState state) {
		isCheckBeforeAnimation = false;
		animState = state;
	}

	void ExitAnimation (AnimationState state) {
		isCheckAfterAnimation = false;
		animState = state;
	}
	#endregion

	#region Stand Animation
	void StartStandAnimation (StandAnimationState state) {
		isCheckBeforeStandAnimation = false;
		standAnimState = state;
	}
	
	void ExitStandAnimation (StandAnimationState state) {
		isCheckAfterStandAnimation = false;
		standAnimState = state;
	}
	#endregion
}
