using UnityEngine;
using System.Collections.Generic;

public class Monkey : MonoBehaviour {
	[HeaderAttribute("Monkey Attributes")]
	public Enemy enemy;
	public MonkeyTriggerDetection monkeyTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;

	[HeaderAttribute("Current")]
	public List<Monkey> nearbyMonkeys;
	public Vector2 patrolArea;
	public bool isHitByPlayer = false;
	#region delegate event

	void Start()
	{
		patrolArea = transform.position;
	}


	void OnEnable()
	{
		monkeyTriggerDetection.OnAddMonkey += AddMonkey;
		monkeyTriggerDetection.OnRemoveMonkey += RemoveMonkey;
		attackRangeTrigger.OnExecuteAttack += SetAttack;
	}

	void OnDisable()
	{
		monkeyTriggerDetection.OnAddMonkey -= AddMonkey;
		monkeyTriggerDetection.OnRemoveMonkey -= RemoveMonkey;
		attackRangeTrigger.OnExecuteAttack -= SetAttack;
	}
	
	void AddMonkey(Monkey monkey)
	{
		nearbyMonkeys.Add(monkey);
	}

	void RemoveMonkey(Monkey monkey)
	{
		nearbyMonkeys.Remove(monkey);
	}

	void SetAttack(bool attack)
	{
		enemy.isAttack = attack;
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
