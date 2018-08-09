using UnityEngine;

public class ArmorEnemy : MonoBehaviour {
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;
	public TriggerDetection hammerTriggerDetection;
	public Collider2D armorCollider;
	public bool isArmored = true;
	public float rollCooldown;

	void OnEnable()
	{

	}

	void OnDisable()
	{

	}

	void SetPlayer(GameObject player)
	{
		enemy.playerTransform = player.transform;
	}

	void HitByHammer()
	{
		isArmored = false;
	}
}
