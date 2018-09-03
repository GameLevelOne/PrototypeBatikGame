using UnityEngine;
// using UnityEngine.UI;

public class UIShop : MonoBehaviour {
	// public Dialog dialog;
	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelUIShop;
	// public CanvasGroup canvasShopGroup;
	// public float showMultiplier;
	// public float hideMultiplier;
	public bool isOpeningShop;
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
