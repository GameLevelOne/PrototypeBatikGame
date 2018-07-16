using UnityEngine;

public class Sprite2D : MonoBehaviour {
	public SpriteRenderer spriteRen;

	public bool isStaticObject = false;
	
	void Awake () {
		if (isStaticObject)
			spriteRen.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
	}
}
