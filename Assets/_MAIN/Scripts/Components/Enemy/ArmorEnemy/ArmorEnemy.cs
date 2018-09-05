using UnityEngine;

public class ArmorEnemy : MonoBehaviour {
	[HeaderAttribute("ArmorEnemy Attributes")]
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;
	public Collider selfCollider;
	
	[SpaceAttribute(15f)]
	public Vector3 patrolArea;
	public Vector3 rollTargetPos;
	public float rollSpeed;
	public float rollCooldown;
	public bool initRoll = false;
	public bool startRoll = false;
	public bool armorDestroyInit = false;

	public float rollInitDuration;
	
	float tRollInit;

	public float TRollInit{
		get{return tRollInit;}
		set{tRollInit = value;}
	}

	void Start()
	{
		patrolArea = transform.position;
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += SetPlayer;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayer;
	}

	void SetPlayer(GameObject player)
	{
		if(player == null) enemy.playerTransform = null;
		else enemy.playerTransform = player.transform;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == Constants.Tag.HAMMER)
		{
			enemy.hasArmor = false;
		}
	}
}
