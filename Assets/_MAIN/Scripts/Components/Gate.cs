using UnityEngine;

public class Gate : MonoBehaviour {
	public Collider gateCol;
	public SpriteRenderer gateSpriteRen;
	public Animator animator;
	public AnimationControl animationControl;
	// public AudioClip lockedClip;
	public AudioClip lockedGateClip;
	public AudioClip unlockedClip;
	public AudioSource audioSource;
	public Sprite closedGateSprite;
	public Sprite openedGateSprite;
	public string textLockedInfo = "THIS GATE IS LOCKED";

	[HeaderAttribute("Saved ID")]
	public int gateID;

	[HeaderAttribute("Current")]
	public bool isInitGate = false;
	public bool isOpened = false;
	public bool isSelected = false;
	
	// [HeaderAttribute("Testing")]
	// public bool resetPrefKey;

	void OnEnable () {
		animationControl.OnExitAnimation += ExitAnimation;
	}

	void OnDisable () {
		animationControl.OnExitAnimation -= ExitAnimation;
	}
	void Awake () {
		// if (resetPrefKey) {
		// 	PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + gateID, 1);
		// 	PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.GATES_STATE + gateID, 0);
		// }
	}

	void ExitAnimation () {
		animator.Play(Constants.AnimationName.GATE_IDLE);
	}
}
