using UnityEngine;

public class BombExplodeArea : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Explode impact");
	}

}