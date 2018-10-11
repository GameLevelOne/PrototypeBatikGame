using UnityEngine;
using System.Collections.Generic;

public enum MonkeyAudio {
	WALK,
	AGGRO,
	DIE
}

public class Monkey : MonoBehaviour {
	[HeaderAttribute("Monkey Attributes")]
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public MonkeyTriggerDetection monkeyTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public GameObject monkeyClawFX;
	public AudioSource audioSource;

	[HeaderAttribute("WALK, AGGRO, DIE")]
	public AudioClip[] audioClip;

	[HeaderAttribute("Current")]
	public List<Monkey> nearbyMonkeys;
	public Vector3 patrolArea;
	public bool isCollidingWithPlayer = false;
	public bool isHitByPlayer = false;
	public bool isInitSpawnAttackFX = false;
	
	void Start()
	{
		patrolArea = transform.position;
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == Constants.Tag.PLAYER)
		{
			enemy.playerTransform = other.transform;
			isCollidingWithPlayer = true;
		}
	}

	#region delegate event
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
		if(monkey != GetComponent<Monkey>()) nearbyMonkeys.Add(monkey);
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
		isInitSpawnAttackFX = true;
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

	void OnEndAggro()
	{
		enemy.isFinishAggro = true;
	}
	#endregion
}
