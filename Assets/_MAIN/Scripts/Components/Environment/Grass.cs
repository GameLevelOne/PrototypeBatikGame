using UnityEngine;

public class Grass : MonoBehaviour {
	public GameObject grassCutFX;
	[HeaderAttribute("Current")]
	public bool init = false;
	public bool interact = false;
	public bool animateEnd = false;
	public bool destroy = false;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == Constants.Tag.PLAYER) interact = true;

		if(other.tag == Constants.Tag.PLAYER_SLASH) destroy = true;
	}

	void OnGrassAnimateEnd()
	{
		animateEnd = true;
	}
}
