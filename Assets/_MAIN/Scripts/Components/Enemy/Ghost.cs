using UnityEngine;

public class Ghost : MonoBehaviour {
	public Enemy enemy;
	public TriggerDetection playerTriggerDetection;

	void OnEnable()
	{
		playerTriggerDetection.OnTriggerEnterObj += OnDetectPlayer;
	}

	void OnDetectPlayer(GameObject playerObj)
	{
		if(playerObj != null) enemy.playerTransform = playerObj.transform;
	}
	
}
