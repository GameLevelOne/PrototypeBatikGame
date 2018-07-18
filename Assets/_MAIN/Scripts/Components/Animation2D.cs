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
		// if (animName == "Slash") {
		// 	//enable attack effect
		// }
	}

	void ExitAnimation (string animName) {
		//disable attack effect
		if (animName == "Slash") {
			if (playerInput.slashComboVal.Count > 0) {
				animator.SetFloat("SlashCombo", playerInput.slashComboVal[0]);

				if (playerInput.slashComboVal[0] == 3) {					
					playerInput.slashComboVal.Clear();
				} else {
					playerInput.slashComboVal.RemoveAt(0);
				}

				if (playerInput.slashComboVal.Count == 0) {
					animator.SetFloat("SlashCombo", 0f);
					animator.SetBool("IsAttacking", false);

					StopAttackAnimation ();
				}
			}
		} else if (animName == "Charge") {
			animator.SetFloat("AttackMode", 0f);
			animator.SetBool("IsAttacking", false);

			StopAttackAnimation ();
		} else if (animName == "Dodge") {
			animator.SetFloat("MoveMode", 0f);
			playerInput.MoveMode = 0;
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

	void StopAttackAnimation () {
		if (role.gameRole == GameRole.Player) {
			playerInput.AttackMode = 0;
			attack.ReadyForAttacking ();
		} else { //ENEMy
			
		}
	}
}
