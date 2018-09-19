using UnityEngine;

public class FogFX : MonoBehaviour {
	public SpriteRenderer sprite;
	public float minAnimSpeed = 1f;
	public float maxAnimSpeed = 1.5f;
	
	[HeaderAttribute("Current")]
	public float animSpeed;
	public bool init = false;
	public bool changeSpeed = false;

	void OnAnimEnd()
	{
		changeSpeed = true;
	}
}
