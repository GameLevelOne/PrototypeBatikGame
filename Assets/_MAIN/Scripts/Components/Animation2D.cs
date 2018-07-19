using UnityEngine;

public enum AnimationState {
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

	PlayerInput playerInput;
	Attack attack;

	[SerializeField] bool isCurrentDoneAnimation = false;

	void Awake () {
		animator.SetFloat("Move Mode", 0f);
		role = GetComponent<Role>();
		attack = GetComponent<Attack>();

		if (role.gameRole == GameRole.Player) {
			playerInput = GetComponent<PlayerInput>();
		} else { //ENEMy
			
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
	
	public bool isDoneAnimation {
		get {return isCurrentDoneAnimation;}
		set {
			if (isCurrentDoneAnimation == value) return;

			isCurrentDoneAnimation = value;
		}
	}

	void StartAnimation (string animName) {
		//enable attack effect
		
		// if (animName == "Slash") {
		// 	
		// }
	}

	void ExitAnimation (string animName) {
		isDoneAnimation = false;
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

		// if (animName == "Slash") {
		// 	if (playerInput.slashComboVal.Count > 0) {
		// 		animator.SetFloat("SlashCombo", playerInput.slashComboVal[0]);

		// 		if (playerInput.slashComboVal[0] == 3) {					
		// 			playerInput.slashComboVal.Clear();
		// 		} else {
		// 			playerInput.slashComboVal.RemoveAt(0);
		// 		}

		// 		if (playerInput.slashComboVal.Count == 0) {
		// 			animator.SetFloat("SlashCombo", 0f);
		// 			StopAttackAnimation ();
		// 		}
		// 	}
		// } else if (animName == "Charge") {
		// 	animator.SetFloat("AttackMode", 0f);
		// 	StopAttackAnimation ();
		// } else if (animName == "Dodge") {
		// 	animator.SetFloat("MoveMode", 0f);
		// 	playerInput.isDodging = false;
		// }
	}

	// void StopAttackAnimation () {
	// 	if (role.gameRole == GameRole.Player) {
	// 		animator.SetBool("IsAttacking", false);
	// 		playerInput.AttackMode = 0;
	// 		attack.ReadyForAttacking ();
	// 	} else { //ENEMy
			
	// 	}
	// }
}
