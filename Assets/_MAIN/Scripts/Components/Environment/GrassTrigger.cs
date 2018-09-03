using UnityEngine;

public class GrassTrigger : MonoBehaviour {
	public Grass grass;

	void OnTriggerEnter(Collider other){
		if(other.tag == Constants.Tag.PLAYER){
			grass.interact = true;
		}

		if(other.tag == Constants.Tag.PLAYER_SLASH){
			grass.destroy = true;
		}
	}
}
