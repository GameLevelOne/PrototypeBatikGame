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
		animator.SetFloat("MoveMode", 0f);
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

	void StartAnimation (string animName) {
		attack.isAttacking  = true;
		//enable attack effect
		switch (animName) {
			case "Slash":
				animState = AnimationState.START_SLASH;
				break;
			case "Charge":
				animState = AnimationState.START_CHARGE;
				break;
			case "Dodge":
				animState = AnimationState.START_DODGE;
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}

	void ExitAnimation (string animName) {
		isCurrentCheckAfterAnimation = false;
		//disable attack effect
		switch (animName) {
			case "Slash":
				animState = AnimationState.AFTER_SLASH;
				break;
			case "Charge":
				animState = AnimationState.AFTER_CHARGE;
				break;
			case "Dodge":
				animState = AnimationState.AFTER_DODGE;
				break;
			default:
				Debug.LogWarning ("Unknown Animation played");
				break;
		}
	}
}
