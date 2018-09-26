using UnityEngine;

public class TriggerEndGame : MonoBehaviour {

	public UIEndGame uiEndGame;
	public Player player;

	public bool triggered = false;

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.PLAYER) {
			triggered = true;
		}
	}
}
