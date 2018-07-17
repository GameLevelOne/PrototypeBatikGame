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
		if (animName == "Slash1") {
			//enable attack effect
		}
	}

	void ExitAnimation (string animName) {
		if (animName == "Slash") {
			//disable attack effect

			if (playerInput.slashComboVal.Count > 0) {
				animator.SetFloat("Slash Combo", playerInput.slashComboVal[0]);

				if (playerInput.slashComboVal[0] == 3) {					
					playerInput.slashComboVal.Clear();
				} else {
					playerInput.slashComboVal.RemoveAt(0);
				}

				if (playerInput.slashComboVal.Count == 0) {
					//disable attack effect
					animator.SetFloat("Attack Mode", 0f);
					animator.SetFloat("Slash Combo", 0f);
					animator.SetBool("IsAttacking", false);

					if (role.gameRole == GameRole.Player) {
						playerInput.Attack = 0;
						attack.ReadyForAttacking ();
					} else { //ENEMy
						
					}
				}
			}
		}
		// if (animName == "Attack") {
		// 	animator.SetFloat("Attack Mode", 0f);
		// 	animator.SetBool("IsAttacking", false);
		// 	//disable attack effect
		// 	if (role.gameRole == GameRole.Player) {
		// 		playerInput.Attack = 0;
		// 		attack.ReadyForAttacking ();
		// 	} else { //ENEMy
				
		// 	}
		// }
	}
}
