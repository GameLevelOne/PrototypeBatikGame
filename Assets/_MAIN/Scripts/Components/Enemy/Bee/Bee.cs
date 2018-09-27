using UnityEngine;

public class Bee : MonoBehaviour {
	[HeaderAttribute("Bee Attributes")]
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public TriggerDetection playerTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public Beehive beeHive;
	
	[SpaceAttribute(10f)]
	public float startledRange;

	[HeaderAttribute("Current")]
	public Transform beeHiveTransform;

	[SpaceAttribute(10f)]
	public bool initStartled = false;
	public bool isStartled = false;
	// public bool initAttackHitPosition = true;

	#region event delegate
	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += SetPlayerTransform;
		attackRangeTrigger.OnExecuteAttack += SetAttack;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayerTransform;
		attackRangeTrigger.OnExecuteAttack -= SetAttack;
	}

	void SetAttack(bool attack)
	{
		enemy.isAttack = attack;
	}

	void SetPlayerTransform(GameObject player)
	{
		if (player != null) {
			if (player.GetComponent<Player>().state != PlayerState.DIE) {
				enemy.playerTransform = player.transform;
			}			
		} else {
			//
		}
	}
	#endregion

	#region animation event
	void EnableAttackHit()
	{
		enemy.attackHit = true;
		// initAttackHitPosition = false;
	}

	void DisableAttackHit()
	{
		enemy.attackHit = false;
	}

	void OnEndAttack()
	{
		enemy.initAttack = false;
	}

	void OnEndDamaged()
	{
		enemy.isFinishDamaged = true;
	}
	#endregion
}
