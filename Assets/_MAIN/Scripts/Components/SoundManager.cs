using UnityEngine;

public enum BGM{

}

public enum SFX{
	ArrowHit,
	BeeAttack,
	BeeFlying,
	BombDrop,
	BombExplode,
	Bounce,
	BowPull,
	BowRelease,
	Burn,
	BurningTorch,
	BusheesCut,
	FishingGet,
	FishingStart,
	GhostAttack,
	GhostDie,
	GrassCut,
	HammerHitRocks,
	HammerSwing,
	HoeHit,
	JatayuAppear,
	JatayuAttack1,
	JatayuAttack2,
	JattayuCloseWings,
	JatayuDropDown,
	JatayuFlyUp,
	JatayuHitWhileFlying,
	LootDrop,
	LootPickUp,
	MoneyDrop,
	MonkeyDie,
	MonkeyMoving,
	MonkeyYell,
	PlayerAttack1,
	PlayerAttack2,
	PlayerAttack3,
	PlayerAttackHit1,
	PlayerAttackHit2,
	PlayerAttackHit3,
	PlayerPickUp,
	PrepareToAttack,
	TreeRustling,
	UnlockGate,
	WalkOnGrass1,
	WalkOnGrass2,
	WalkOnGround1,
	WalkOnGround2,
	WalkOnStone1,
	WalkOnStone2,
	WalkOnWater1,
	WalkOnWater2,
	WalkOnWater3,
	WaterfallInHiddenCave,
	Waterfalls
}

public class SoundManager : MonoBehaviour {

#region SINGLETON
	private static SoundManager instance = null;
	public static SoundManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<SoundManager>();
			} 

			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			GameObject.Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
#endregion
	public AudioSource bgmSource;
	public AudioSource sfxSource;

	public AudioClip[] BGMs;
	public AudioClip[] SFXs;
	[HeaderAttribute("Current")]
	public bool init = false;

	public void PlaySFX(SFX sfx)
	{
		sfxSource.PlayOneShot(SFXs[(int)sfx]);
	}

	public void PlayBGM(BGM bgm)
	{
		bgmSource.Stop();
		bgmSource.clip = BGMs[(int)bgm];
		bgmSource.Play();
	}	
}