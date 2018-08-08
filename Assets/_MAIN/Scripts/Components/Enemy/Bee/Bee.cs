using UnityEngine;

public class Bee : MonoBehaviour {
	[HeaderAttribute("Bee Attributes")]
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;
	
	[SpaceAttribute(10f)]
	public float startledRange;

	[HeaderAttribute("Current")]
	public Transform beeHiveTransform;

	[SpaceAttribute(10f)]
	public bool initStartled = false;
	public bool isStartled = false;

	#region event delegate
	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnter += SetPlayerTransform;
		attackRangeTrigger.OnExecuteAttack += SetAttack;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnter -= SetPlayerTransform;
		attackRangeTrigger.OnExecuteAttack -= SetAttack;
	}

	void SetAttack(bool attack)
	{
		enemy.isAttack = attack;
	}

	void SetPlayerTransform(GameObject player)
	{
		enemy.playerTransform = player.transform;
	}
	#endregion

	#region animation event
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
	#endregion
}
