using UnityEngine;


public enum HookState{
	Idle,
	Launch,
	Return,
	Catch
}

public class Hook : MonoBehaviour {
	[Header("Reference")]
	public Rigidbody2D rb;

	[Header("Variables")]
	public HookState hookState = HookState.Idle;
	public float speed;
	public float range;

	public Vector2 startPos;
	public Vector2 destination;
	public bool hasSetDest = false;

	[Header("Current")]
	public GameObject attachedObject;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Constants.Tag.ENEMY){
			attachedObject = other.gameObject;
		}
	}
}
