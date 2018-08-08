using UnityEngine;

public class WaterShooterEnemy : MonoBehaviour {
	[HeaderAttribute("WaterShooterEnemy Attributes")]
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;
	public GameObject projectile;

	[SpaceAttribute(10f)]
	public float shootInterval;

	float tShootInterval;

	public float TShootInterval{
		get{return tShootInterval;}
		set{tShootInterval = value;}
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnter += SetPlayerTransform;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnter -= SetPlayerTransform;
	}


	void SetPlayerTransform(GameObject player)
	{
		enemy.playerTransform = player.transform;
	}
}
