using UnityEngine;

public class Cloak : MonoBehaviour {
	public SpriteRenderer spriteRen;

	public float invisibleAlpha = 0f;

	[SerializeField] bool isInvisible = false;
	
	public bool IsInvisible {
		get {return isInvisible;}
		set {
			if (isInvisible == value) return;

			isInvisible = value;
		}
	}
}
