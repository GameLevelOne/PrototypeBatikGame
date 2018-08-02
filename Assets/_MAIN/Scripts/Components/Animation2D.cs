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
	AFTER_THROW
}

public enum StandAnimationState {
	START_USING_TOOL,
	AFTER_USING_TOOL
}

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	// public bool isAnimating;
	public Animator animator;
	public AnimationState animState;
	public StandAnimationState standAnimState;
	// public Role role; //for Animation system

	// PlayerInput playerInput;
	// EnemyInput enemyInput;
	public Attack attack;
	public PlayerTool tool;

	// [SerializeField] bool isCurrentCheckBeforeAnimation = false;
	[SerializeField] bool isCheckAfterAnimation = false;

	void Awake () {
		animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
		// role = GetComponent<Role>();

		// if (role.gameRole == GameRole.Player) {
		// 	playerInput = GetComponent<PlayerInput>();
		// } else { //ENEMy
		// 	enemyInput = GetComponent<EnemyInput>();
		// }
	}

	void OnEnable () {
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

	// public bool isCheckBeforeAnimation {
	// 	get {return isCurrentCheckBeforeAnimation;}
	// 	set {
	// 		if (isCurrentCheckBeforeAnimation == value) return;

	// 		isCurrentCheckBeforeAnimation = value;
	// 	}
	// }
	
	public bool IsCheckAfterAnimation {
		get {return isCheckAfterAnimation;}
		set {
			if (isCheckAfterAnimation == value) return;

			isCheckAfterAnimation = value;
		}
	}

	#region Player and Enemy Animation
	void StartAnimation (AnimationState state) {
		switch (state) {
			case AnimationState.START_SLASH:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_CHARGE:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_DODGE:
				//
				break;
			case AnimationState.START_COUNTER:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_RAPIDSLASH:
				attack.isAttacking  = true;
				break;
			case AnimationState.START_BLOCK:
				//
				break;
			case AnimationState.START_HURT:
				//
				break;
			// case AnimationState.START_DASH:
			// 	//
			// 	break;
			// case AnimationState.START_BRAKING:
			// 	//
			// 	break;
			case AnimationState.START_GRAB:
				//
				break;
			case AnimationState.START_UNGRAB:
				//
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}

	void ExitAnimation (AnimationState state) {
		isCheckAfterAnimation = false;
		
		switch (state) {
			case AnimationState.AFTER_SLASH:
				//
				break;
			case AnimationState.AFTER_CHARGE:
				//
				break;
			case AnimationState.AFTER_DODGE:
				//
				break;
			case AnimationState.AFTER_COUNTER:
				//
				break;
			case AnimationState.AFTER_RAPIDSLASH:
				//
				break;
			case AnimationState.AFTER_BLOCK:
				//
				break;
			case AnimationState.AFTER_HURT:
				//
				break;
			// case AnimationState.AFTER_DASH:
			// 	//
			// 	break;
			// case AnimationState.AFTER_BRAKING:
			// 	//
			// 	break;
			case AnimationState.AFTER_GRAB:
				Debug.Log("After Grab");
				//
				break;
			case AnimationState.AFTER_UNGRAB:
				Debug.Log("After Ungrab");
				//
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}
	#endregion

	#region Stand Animation
	void StartStandAnimation (StandAnimationState state) {
		switch (state) {
			case StandAnimationState.START_USING_TOOL:
				tool.IsActToolReady = true;
				break;
			default:
				Debug.LogWarning ("Unknown Stand Animation played");
				break;
		}

		standAnimState = state;
	}
	
	void ExitStandAnimation (StandAnimationState state) {
		isCheckAfterAnimation = false;

		switch (state) {
			case StandAnimationState.AFTER_USING_TOOL:
				//
				break;
			default:
				Debug.LogWarning ("Unknown Stand Animation played");
				break;
		}

		standAnimState = state;
	}
	#endregion
}
