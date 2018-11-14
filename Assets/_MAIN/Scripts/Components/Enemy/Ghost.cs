using UnityEngine;

public enum GhostSFX{
	GhostWalk1,
	GhostWalk2,
	GhostAttack,
	GhostDie,
	GhostAppear
}

public class Ghost : MonoBehaviour {
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public TriggerDetection playerTriggerDetection;
	public TriggerDetection attackRangeDetection;
	public Collider addonCollider;
	public SpriteRenderer sprite;
	public ParticleSystem particle;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public ParticleSystem hitParticle;
	public Vector3 origin;
	public AudioSource audioSource;
	public AudioClip[] clips;
	public bool isAttacking = false;
	// public float damagedDuration;

	// [SerializeField] protected float tDamaged;
	// public float TDamaged{
	// 	get{return tDamaged;}
	// 	set{tDamaged = value;}
	// }

	public void PlaySFX(GhostSFX sfx)
	{
		audioSource.PlayOneShot(clips[(int)sfx]);
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += OnDetectPlayer;
		attackRangeDetection.OnTriggerEnterObj += SetIsAttacking;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj += OnDetectPlayer;
		attackRangeDetection.OnTriggerEnterObj -= SetIsAttacking;
	}

	void OnDetectPlayer(GameObject playerObj)
	{
		if(playerObj != null) enemy.playerTransform = playerObj.transform;
	}

	void SetIsAttacking(GameObject playerObj){
		isAttacking = playerObj == null ? false : true;
		 // Debug.Log("IS ATTACKING = "+isAttacking);
	}

	void Awake()
	{
		origin = transform.position;
	}

	#region animation event
	void EnableAttackHit()
	{
		enemy.attackHit = true;
	}

	void PlaySFXWalkLeftFoot()
	{
		PlaySFX(GhostSFX.GhostWalk1);
	}
	void PlaySFXWalkRightFoot()
	{
		PlaySFX(GhostSFX.GhostWalk2);
	}
	void PlaySFXAttack()
	{
		PlaySFX(GhostSFX.GhostAttack);
	}
	#endregion
}
