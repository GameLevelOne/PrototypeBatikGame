using UnityEngine;

public class UIInfo : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelUIInfo;
	// public CanvasGroup canvasInfoGroup;
	// public float showMultiplier;
	// public float hideMultiplier;
	public bool isPlayingAnimation;

	void OnEnable () {
		animationControl.OnExitAnimation += OnExitAnimation;
	}

	void OnDisable () {
		animationControl.OnExitAnimation -= OnExitAnimation;
	}

	void OnExitAnimation () {
		isPlayingAnimation = false;
	}
}
