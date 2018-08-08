using UnityEngine;

public class WaterShooterEnemy : MonoBehaviour {
	[HeaderAttribute("WaterShooterEnemy Attributes")]
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;

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
