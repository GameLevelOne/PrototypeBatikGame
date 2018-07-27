using UnityEngine;

public class TeleportBulletTime : MonoBehaviour {
	public Player player;

	public Vector3 Teleport () {
		GameObject target = player.enemyThatHitsPlayer.GetComponent<Facing2D>().blindArea;

		// transform.position = target.transform.position;
		// Time.timeScale = 0.1f;
		return target.transform.position;
	}
}
