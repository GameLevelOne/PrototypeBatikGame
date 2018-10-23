using UnityEngine;

// ASSET JATAYU yang IDLE itu adalah MOVE!
public enum JatayuState{
	Entrance,
	Move,
	CloseWings,
	Attack1,
	FlapFast,
	Attack2,
	Attack3,
	Attack3Return,
	HP50,
	Hit,
	Burned,
	BurnedFallen,
	Die
}

public enum JatayuSFX{
	JatayuAttack1,
	JatayuAttack2,
	JatayuAttack3FlyUp,
	JatayuCloseWings,
	JatayuEntrance,
	JatayuFlying,
	JatayuHit,
	JatayuLanding,
	JatayuDie,
	JatayuScream
}

public class Jatayu : MonoBehaviour {
	public Player playerObject;
	public GameObject attack1Object;
	public GameObject attack1HP50Object;
	public GameObject attack2Object;
	public GameObject attack3Object;
	public CapsuleCollider jatayuCollider;
	public SpriteRenderer headSprite;
	public SpriteRenderer bodySprite;
	public Transform playerTransform;
	public Animator movementAnim;
	public Health health;
	public ParticleSystem particleDie;
	public ParticleSystem burnedFX;
	public TimelineEventTrigger timelineEventTrigger;
	public CameraShaker cameraShaker;
	public ParticleSystem landingParticle1, landingParticle2;
	public ParticleSystem hitParticle;
	public ParticleSystem screamParticle;
	public ParticleSystem closeWingsParticle;
	public ParticleSystem[] burnedParticle;
	[SpaceAttribute(10f)]
	public AudioSource audioSource;
	public AudioClip[] clips;
	[SpaceAttribute(10f)]
	public float movementAnimSpeed = 0.5f;
	public float xMinMove = 5f, xMaxMove = 17f;
	public float zMinMove = 5f, zMaxMove = 11f;
	public int minAttackCounter, maxAttackCounter;
	public float attack3ReturnSpeed = 5f;
	public float hitColorFadeSpeed = 1f;
	public float burnedDuration = 3f;
	public float burnedCooldown = 10f;
	public float burnedYDrop;
	[HeaderAttribute("Current")]
	public Damage damageReceive;
	public bool initJatayu = false;
	public bool initJatayuHP50 = false;
	public bool invulnerable = false;
	public JatayuState state;
	[SpaceAttribute(10f)]
	public float maxHealth;
	public int AttackCounter = 0;
	public float tDelayFace = 1f;
	public float tColorFade = 1f;
	public float tBurned = 0f;
	public float tBurnedCooldown = 10f;
	public float tDieColorChange = 0f;
	[SpaceAttribute(10f)]
	public bool initEntrance = false;
	public bool initMove = false;
	public bool initCloseWings = false;
	public bool initFlapFast = false;
	public bool initAttack1 = false;
	public bool initAttack2 = false;
	public bool initAttack3 = false;
	public bool initAttack3Return = false;
	public bool initHP50 = false;
	public bool initHit = false;
	public bool initBurned = false;
	public bool initDie = false;
	public bool endEntrance = false;
	public bool endMove = false;
	public bool endCloseWings = false;
	public bool endInitFlapFast = false;
	public bool endAttack1 = false;
	public bool endAttack2 = false;
	public bool endAttack3 = false;
	public bool endHP50 = false;
	public bool endHit = false;
	public bool endBurned = false;
	public bool endDie = false;
	[SpaceAttribute(10f)]
	public bool isColorFading = false;
	public bool attack3SetPos = false;
	public bool attack3Drop = false;
	public bool isBurned = false;
	public bool isCBurnedCooldown = false;
	public bool initParticleDie = false;
	public bool isHit = false;
	[HeaderAttribute("SET FALSE FOR RELEASE! DEBUG ONLY!")]
	public bool testAttack1Only = false;
	public bool testAttack2Only = false;
	public bool testAttack3Only = false;

	#region delegate event
	void OnEnable()
	{
		health.OnDamageCheck += DamageCheck;
	}

	void OnDisable()
	{
		health.OnDamageCheck -= DamageCheck;
	}

	void DamageCheck(Damage damage)
	{
		damageReceive = damage;
	}
	#endregion

	public void PlaySFX(JatayuSFX sfx)
	{
		audioSource.PlayOneShot(clips[(int)sfx]);
	}

#region animation event
	void StartEntrance()
	{
		PlaySFX(JatayuSFX.JatayuEntrance);
	}

	void Landing()
	{
		landingParticle1.Play();
		landingParticle2.Play();
		PlaySFX(JatayuSFX.JatayuLanding);
	}

	void Scream()
	{
		screamParticle.Play();
		PlaySFX(JatayuSFX.JatayuScream);
	} 

	void StartShakeCamera()
	{
		cameraShaker.shake = true;
	}

	void StopShakeCamera()
	{
		cameraShaker.shake = false;
	}
	void StartShakeCameraBlur()
	{
		cameraShaker.shake = true;
		cameraShaker.blurBehaviour.enabled = true;
	}

	void StopShakeCameraBlur()
	{
		cameraShaker.shake = false;
		cameraShaker.blurBehaviour.enabled = false;
	}

	void OnEndEntranceAnim()
	{
		endEntrance = true;
	}

	void MoveSound()
	{
		PlaySFX(JatayuSFX.JatayuFlying);
	}

	void StartCloseWings()
	{
		closeWingsParticle.Play(true);
		PlaySFX(JatayuSFX.JatayuCloseWings);
	}

	void EnableAttack1Object()
	{
		attack1Object.SetActive(true);
	}
	void EnableAttack1HP50Object()
	{
		if(health.EnemyHP/maxHealth <= 0.5f){
			attack1HP50Object.SetActive(true);
			PlaySFX(JatayuSFX.JatayuAttack1);
		}
	}

	void EnableAttack2Object()
	{
		attack2Object.SetActive(true);
		PlaySFX(JatayuSFX.JatayuAttack2);
	}
	void EnableAttack3Object()
	{
		attack3Object.SetActive(true);
		attack3SetPos = true;
		PlaySFX(JatayuSFX.JatayuAttack3FlyUp);
	}
	void Attack3Drop()
	{
		attack3Drop = true;
	}
	void OnEndAttack1Anim()
	{
		endAttack1 = true;
	}
	void OnEndAttack2Anim()
	{
		attack2Object.SetActive(false);
		playerObject.isHitJatayuAttack2 = false;
		endAttack2 = true;
	}
	void OnEndAttack3Anim()
	{
		endAttack3 = true;
	}
	void OnEndCloseWingsAnim()
	{
		endCloseWings = true;
	}
	void OnEndFlapFastAnim()
	{
		endInitFlapFast = true;
	}
	void OnEndHP50Anim()
	{
		endHP50 = true;
	}
	void OnEndHitAnim()
	{
		endHit = true;
	}
	void OnEndBurnedAnim()
	{
		endBurned = true;
	}
	void EmitDieParticle()
	{
		initParticleDie = true;
	}
	void OnEndDieAnim()
	{
		endDie = true;
	}
#endregion

#region collision
	void OnTriggerEnter(Collider other){
		if( other.tag == Constants.Tag.PLAYER_SLASH || 
			other.tag == Constants.Tag.HAMMER || 
			other.tag == Constants.Tag.PLAYER_DASH_ATTACK ||
			other.tag == Constants.Tag.ARROW ||
			other.tag == Constants.Tag.EXPLOSION ||
			other.tag == Constants.Tag.MAGIC_MEDALLION) {
				initHit = true;
				isHit = true;
		}

		if(other.tag == Constants.Tag.FIRE_ARROW){
			//burned
			isBurned = true;
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Boundary"){
			Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
		}
	}
#endregion
}
