using UnityEngine;

public enum BeeAudio {
	PREPARE,
	ATTACK,
	DIE
}

public class Bee : MonoBehaviour {
	[HeaderAttribute("Bee Attributes")]
	public Enemy enemy;
	public QuestTrigger questTrigger;
	public ChestSpawner chestSpawner;
	public TriggerDetection playerTriggerDetection;
	public AttackRangeTrigger attackRangeTrigger;
	public ParticleSystem attackCodeFX;
	public ParticleSystem burnedFX;
	public GameObject beeThrustFX;
	public Beehive beeHive;
	public GameObject beeCutFX;
	public AudioSource audioSource;
	public AudioClip[] audioClip;
	[SpaceAttribute(10f)]
	public float startledRange;

	[HeaderAttribute("Current")]
	public Transform beeHiveTransform;

	[SpaceAttribute(10f)]
	public bool initStartled = false;
	public bool isStartled = false;
	public bool isInitSpawnAttackFX = false;

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
		}
	}
	#endregion

	#region BEE ANIMATION EVENT
	void EnableAttackHit()
	{
		enemy.attackHit = true;
		isInitSpawnAttackFX = true;
	}
	#endregion
}
