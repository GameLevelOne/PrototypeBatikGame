using UnityEngine;

public class GrassCut : MonoBehaviour {
	public bool destroy = false;
	
	void OnEndAnimate()
	{
		destroy = true;
	}
}
