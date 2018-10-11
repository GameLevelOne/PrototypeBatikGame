using UnityEngine;

public enum AnimationAudio {
	HAMMER,
	SHOVEL,
	BOUNCE,
	FISHING_THROW,
	PARRY,
	BLOCK,
	HURT
}

public class Animation2D : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public AudioSource audioSource;
	public AudioClip[] audioClip;

	public bool isInitAnimation = false;

	[HeaderAttribute("Current")]
	public string currentAnimName;
	public bool isFinishAttackAnimation = true;
	
	[SerializeField] bool isFinishAnyAnim = true;
	public bool isFinishAnyAnimation {
		get {return isFinishAnyAnim;}
		set {
			isFinishAnyAnim = value;

			// if (value) {
			// 	Debug.Log("Set isFinishAnyAnim TRUE");
			// }
		}
	}


	[HeaderAttribute("For Player & Enemy")]
	public bool isCheckAfterAnimation = false;
	public bool isCheckBeforeAnimation = false;


	// [SpaceAttribute(10f)]
	[HeaderAttribute("For Attack / Stand / Tool")]
	public bool isSpawnSomethingOnAnimation = false;
	public bool isCheckAfterStandAnimation = false;
	public bool isCheckBeforeStandAnimation = false;

	void OnEnable () {
		// animator.SetFloat(Constants.AnimatorParameter.Float.MOVE_MODE, 0f);
		animationControl.OnStartAnimation += StartAnimation;
		animationControl.OnExitAnimation += ExitAnimation;
		animationControl.OnSpawnSomethingOnAnimation += SpawnSomethingOnAnimation;
		animationControl.OnEndAttackAnimation += EndAttackAnimation;
		animationControl.OnStartStandAnimation += StartStandAnimation;
		animationControl.OnExitStandAnimation += ExitStandAnimation;
	}

	void OnDisable () {
		animationControl.OnStartAnimation -= StartAnimation;
		animationControl.OnExitAnimation -= ExitAnimation;
		animationControl.OnSpawnSomethingOnAnimation -= SpawnSomethingOnAnimation;
		animationControl.OnEndAttackAnimation -= EndAttackAnimation;
		animationControl.OnStartStandAnimation -= StartStandAnimation;
		animationControl.OnExitStandAnimation -= ExitStandAnimation;
	}

	#region Player and Enemy Animation
	void StartAnimation () {
		// isCheckBeforeAnimation = false;
		// Debug.Log("isCheckBeforeAnimation = false");
	}

	void ExitAnimation () {
		isCheckAfterAnimation = false;
		// Debug.Log("Check after animation FALSE");
	}

	void SpawnSomethingOnAnimation () {
		isSpawnSomethingOnAnimation = true;
	}

	void EndAttackAnimation () {
		isFinishAttackAnimation = true;
	}
	#endregion

	#region Stand Animation
	void StartStandAnimation () {
		isCheckBeforeStandAnimation = false;
		// Debug.Log("Set isCheckBeforeStandAnimation FALSE StartStan");
	}
	
	void ExitStandAnimation () {
		isCheckAfterStandAnimation = false;
		// Debug.Log("Set isCheckAfterStandAnimation FALSE ExitStand");
	}
	#endregion
}
