using UnityEngine;

public class GameFX : MonoBehaviour {
	public SpriteRenderer playerSprite;
	// public GameObject counterChargeEffect;
	// public GameObject counterEffect;
	[HeaderAttribute("Player Effect")]
	public ParticleSystem dodgeEffect;
	public ParticleSystem counterChargeEffect;
	public ParticleSystem runEffect;
	public ParticleSystem dashEffect;
	public GameObject parryEffect;
	public GameObject guardHitEffect;
	public GameObject hitEffect;
	public GameObject chargingEffect;
	// public GameObject chargingRunEffect;

	[HeaderAttribute("Tool Effect")]
	public GameObject cannotPushEffect;
	public GameObject dashBounceEffect;
	public GameObject fishingCaughtEffect;
	public GameObject fishingPullEffect;
	public GameObject gainTreasureEffect;
	public GameObject swimStartEffect;
	public GameObject swimIdleEffect;
	public GameObject swimMoveEffect;
	public GameObject swimEndEffect;

	[HeaderAttribute("Current")]
	public bool isEnableDodgeEffect = false;
}
