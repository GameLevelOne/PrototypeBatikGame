using UnityEngine;

public enum BeeState
{
	Idle,
	Patrol,
	Chase,
	Startled,
	Attack,
	Damaged,
	Die
}

public class Bee : MonoBehaviour {
	[HeaderAttribute("Bee Attributes")]
	public TriggerDetection playerTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;
	public GameObject beeAttackObject;

	[HeaderAttribute("Current")]
	public Transform playerTransform;
	public BeeState beeState;
	public bool isChase = false;
	public bool isAttack = false;
	public bool attackHit = false;

	public bool initAttack = false;

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
		isAttack = attack;
	}

	void SetPlayerTransform(GameObject player)
	{
		playerTransform = player.transform;
	}
	#endregion

	#region animation event
	void EnableAttackHit()
	{
		attackHit = true;
	}

	void DisableAttackHit()
	{
		attackHit = false;
	}

	void OnEndAttack()
	{
		initAttack = false;
	}
	#endregion
}
