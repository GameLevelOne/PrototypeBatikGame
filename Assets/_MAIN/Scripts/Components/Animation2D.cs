using UnityEngine;

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	// public bool isAnimating;
	public Animator animator;

	PlayerInput playerInput;
	Attack attack;
	Role role;

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

	void StartAnimation (string animName) {
		if (animName == "Attack") {
			//enable attack effect
		}
	}

	void ExitAnimation (string animName) {
		if (animName == "Attack") {
			animator.SetFloat("Attack Mode", 0f);
			animator.SetBool("IsAttacking", false);
			//disable attack effect
			if (role.gameRole == GameRole.Player) {
				playerInput.Attack = 0;
				attack.ReadyForAttacking ();
			} else { //ENEMy
				
			}
		}
	}
}
