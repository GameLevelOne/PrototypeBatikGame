using UnityEngine;

public class Firefly : MonoBehaviour {

	public bool init = false;
	public bool fly = false;

	void OnEndIdle()
	{
		fly = true;
	}
}
