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
	AFTER_RAPIDSLASH
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
	public bool isCanAttack = false;

	// PlayerInput playerInput;
	// EnemyInput enemyInput;
	Attack attack;

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

		if (isCanAttack) {
			attack = GetComponent<Attack>();
		}
	}

	void OnEnable () {
		animationControl.OnStartAnimation += StartAnimation;
		animationControl.OnExitAnimation += ExitAnimation;
		animationControl.OnStartStandAnimation += SetStandStateAnimation;
		animationControl.OnExitStandAnimation += SetStandStateAnimation;
	}

	void OnDisable () {
		animationControl.OnStartAnimation -= StartAnimation;
		animationControl.OnExitAnimation -= ExitAnimation;
		animationControl.OnStartStandAnimation -= SetStandStateAnimation;
		animationControl.OnExitStandAnimation -= SetStandStateAnimation;
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
		//enable attack effect
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
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}

	void ExitAnimation (AnimationState state) {
		isCheckAfterAnimation = false;
		//disable attack effect
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
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}
	#endregion

	#region Stand Animation
	void SetStandStateAnimation (StandAnimationState state) {
		isCheckAfterAnimation = false;
		standAnimState = state;
	}
	#endregion
}
