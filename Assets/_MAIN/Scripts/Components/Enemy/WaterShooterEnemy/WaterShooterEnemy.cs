using UnityEngine;

public class WaterShooterEnemy : MonoBehaviour {
	[HeaderAttribute("WaterShooterEnemy Attributes")]
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;
	public GameObject bullet;

	[SpaceAttribute(10f)]
	public float shootInterval;

	float tShootInterval;

	public float TShootInterval{
		get{return tShootInterval;}
		set{tShootInterval = value;}
	}

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += SetPlayerTransform;
	}

	void OnDisable()
	{
		playerTriggerDetection.OnTriggerEnterObj -= SetPlayerTransform;
	}


	void SetPlayerTransform(GameObject player)
	{
		if(player == null) enemy.playerTransform = null;
		else enemy.playerTransform = player.transform;
	}
}
