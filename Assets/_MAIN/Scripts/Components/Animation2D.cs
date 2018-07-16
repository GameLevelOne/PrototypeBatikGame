using UnityEngine;

public class Animation2D : MonoBehaviour {
	public delegate void AttackDirChange (float animValue, bool isVertical);
	public event AttackDirChange OnAttackDirChange;

	public PlayerInput playerInput;
	public AnimationControl animationControl;
	// public bool isAnimating;
	public Animator animator;

	bool isCurrentAttacking = false;

	void Awake () {
		animator.SetFloat("Move Mode", 0f);
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
			animator.SetBool("IsAttacking", false);
			//disable attack effect
		}
	}
}
