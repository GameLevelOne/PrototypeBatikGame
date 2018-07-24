using UnityEngine;

public class TeleportBulletTime : MonoBehaviour {

	Player player;

	void Awake () {
		player = GetComponent<Player>();
	}

	public void Teleport () {
		GameObject target = player.enemyThatHitsPlayer.GetComponent<Facing2D>().blindArea;

		transform.position = target.transform.position;
		Time.timeScale = 0.1f;
	}
}
