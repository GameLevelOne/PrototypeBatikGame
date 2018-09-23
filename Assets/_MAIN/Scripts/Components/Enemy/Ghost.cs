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
	public Vector3 origin;

	public bool isAttacking = false;

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
	
}
