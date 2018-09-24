using UnityEngine;

public class UIInfo : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelUIInfo;
	public Sprite initContainerSprite;
	public Sprite hpPotSprite;
	public Sprite mpPotSprite;
	public Sprite initToolSprite;
	public Sprite selectedToolSprite;
	// public CanvasGroup canvasInfoGroup;
	// public float showMultiplier;
	// public float hideMultiplier;
	public bool isInitUIInfo = false;
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
