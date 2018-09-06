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
	public Projectile projectile;
	public LiftableType liftableType;
	public LiftableState state;

	// public Transform testDir;
	public Transform mainTransform;
	// public Rigidbody2D shadowRigidbody;
	public Rigidbody mainRigidbody;
	public Collider mainCollider;

	public float weight;
	// public float initMass;
	public float initDrag;
	// public int weight;
	public float initGravityAwakeTime;

	[HeaderAttribute("Current")]
	// public Transform shadowTransform;
	// public Vector3 initPos;
	// public Vector3 targetDirPos;
	// public int dirID;
	// public Vector3 targetDir;
	// public float speed;
	// public float throwRange;
	public float gravityAwakeTimer;
	public bool isLanding;
	// public GameObject attachedObject;

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == Constants.Tag.GROUND){
			isLanding = true;
		}
	}

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag != Constants.Tag.PLAYER){
	// 		attachedObject = col.gameObject;
	// 	}
	// }
}
