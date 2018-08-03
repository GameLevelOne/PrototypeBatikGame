using UnityEngine;

public enum LiftableType {
	LIFTABLE,
	UNLIFTABLE,
	GRABABLE
}

public enum LiftableState {
	IDLE,
	THROW,
	FLY,
	LAND
}

public class Liftable : MonoBehaviour {
	public LiftableType liftableType;
	public LiftableState state;

	public GameObject attachedObject;
	public Transform shadowTransform;
	public Transform mainObjTransform;
	public Rigidbody2D shadowRigidbody;
	public Rigidbody2D mainObjRigidbody;
	public Collider2D collider;

	public Vector2 initPos;
	public Vector2 targetDirPos;
	public int dirID;
	public float speed;
	public float throwRange;

	// public float weight;

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log(col.gameObject.name);
		if(col.gameObject.tag != Constants.Tag.PLAYER){
			attachedObject = col.gameObject;
		}
	}
}
