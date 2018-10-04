using UnityEngine;

public class Ghost : MonoBehaviour {
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public TriggerDetection playerTriggerDetection;
	public TriggerDetection attackRangeDetection;
	public SpriteRenderer sprite;
	public ParticleSystem particle;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public ParticleSystem hitParticle;
	public Vector3 origin;

	public bool isAttacking = false;
	// public float damagedDuration;

	// [SerializeField] protected float tDamaged;
	// public float TDamaged{
	// 	get{return tDamaged;}
	// 	set{tDamaged = value;}
	// }

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
		Debug.Log("IS ATTACKING = "+isAttacking);
	}

	void Awake()
	{
		origin = transform.position;
	}

	void EnableAttackHit()
	{
		enemy.attackHit = true;
	}

	void DisableAttackHit()
	{
		enemy.attackHit = false;
		
	}

	void OnEndAttack()
	{
		enemy.initAttack = false;
	}

	void OnStartDamaged()
	{
		enemy.initDamaged = false;
	}

	void OnEndDamaged()
	{
		enemy.isFinishDamaged = true;
	}	
}
