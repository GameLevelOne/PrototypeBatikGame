using UnityEngine;

public class Grass : MonoBehaviour {
	public bool init = false;
	public bool interact = false;
	public bool animateEnd = false;
	public bool destroy = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Constants.Tag.PLAYER) interact = true;

		if(other.tag == Constants.Tag.PLAYER_ATTACK) destroy = true;
	}

	void OnGrassAnimateEnd()
	{

	}
}
