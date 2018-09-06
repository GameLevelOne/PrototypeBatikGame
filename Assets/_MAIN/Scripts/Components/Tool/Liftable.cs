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

	public Transform mainTransform;
	// public Rigidbody2D shadowRigidbody;
	public Rigidbody mainRigidbody;
	public Collider mainCollider;

	public float weight;
	// public int weight;

	[HeaderAttribute("Current")]
	public GameObject attachedObject;
	// public Transform shadowTransform;
	// public Vector3 initPos;
	// public Vector3 targetDirPos;
	// public int dirID;
	public float speed;
	public float throwRange;

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag != Constants.Tag.PLAYER){
			attachedObject = col.gameObject;
		}
	}

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag != Constants.Tag.PLAYER){
	// 		attachedObject = col.gameObject;
	// 	}
	// }
}
