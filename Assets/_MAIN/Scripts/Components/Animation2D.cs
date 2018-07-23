using UnityEngine;

public enum AnimationState {
	START_SLASH,
	START_CHARGE,
	START_DODGE,
	AFTER_SLASH,
	AFTER_CHARGE,
	AFTER_DODGE
}

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	// public bool isAnimating;
	public Animator animator;
	public AnimationState animState;
	public Role role;
	public bool isCanAttack = false;

	PlayerInput playerInput;
	Attack attack;

	// [SerializeField] bool isCurrentCheckBeforeAnimation = false;
	[SerializeField] bool isCurrentCheckAfterAnimation = false;

	void Awake () {
		animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
		role = GetComponent<Role>();
		// attack = GetComponent<Attack>();

		if (role.gameRole == GameRole.Player) {
			playerInput = GetComponent<PlayerInput>();
		} else { //ENEMy
			
		}

		if (isCanAttack) {
			attack = GetComponent<Attack>();
		}
	}

	void OnEnable () {
		animationControl.OnStartAnimation += StartAnimation;
		animationControl.OnExitAnimation += ExitAnimation;
	}

	void OnDisable () {
		animationControl.OnStartAnimation -= StartAnimation;
		animationControl.OnExitAnimation -= ExitAnimation;
	}

	// public bool isCheckBeforeAnimation {
	// 	get {return isCurrentCheckBeforeAnimation;}
	// 	set {
	// 		if (isCurrentCheckBeforeAnimation == value) return;

	// 		isCurrentCheckBeforeAnimation = value;
	// 	}
	// }
	
	public bool isCheckAfterAnimation {
		get {return isCurrentCheckAfterAnimation;}
		set {
			if (isCurrentCheckAfterAnimation == value) return;

			isCurrentCheckAfterAnimation = value;
		}
	}

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
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}

	void ExitAnimation (AnimationState state) {
		isCurrentCheckAfterAnimation = false;
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
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
		
		animState = state;
	}
}
