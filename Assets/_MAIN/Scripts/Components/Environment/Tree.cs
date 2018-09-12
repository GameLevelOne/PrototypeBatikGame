using UnityEngine;

public class Tree : MonoBehaviour {
	public bool initAnimSpeed = false;
	public float minAnimSpeed = 0.1f;
	public float maxAnimSpeed = 2f;

	[HeaderAttribute("Current")]
	public float animSpeed = 1f;

	void OnIdleEnd()
	{
		initAnimSpeed = false;
	}
}
