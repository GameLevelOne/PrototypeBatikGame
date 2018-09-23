using UnityEngine;

public class TeleportBulletTime : MonoBehaviour {
	public Player player;

	public Vector3 Teleport () {
		if (player.somethingThatHitsPlayer.GetComponent<Enemy>() != null) {
			GameObject target = player.somethingThatHitsPlayer.GetComponent<Facing2D>().blindArea;

			return target.transform.position;
		} else {
			return player.transform.position;
		}
	}
}
