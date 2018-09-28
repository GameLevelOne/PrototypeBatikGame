﻿using UnityEngine;

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
	// public bool throwByAccident = false;
	// public GameObject attachedObject;

	void OnCollisionEnter (Collision col)
	{
		if (state == LiftableState.THROW || state == LiftableState.FLY) {
			// Debug.Log("IgnoreCollision "+throwByAccident);
			if (col.gameObject.tag == Constants.Tag.PLAYER) {
				Physics.IgnoreCollision(mainCollider, col.collider);
			}
		}

		if(col.gameObject.tag == Constants.Tag.GROUND){
			isLanding = true;

			if (GetComponent<Bush>() != null) {
				GetComponent<Bush>().destroy = true;
			} else if (GetComponent<Stone>() != null) {
				GetComponent<Stone>().hit = true;
			}
		}
	}

	// void OnCollisionStay (Collision col) {
	// 	if (state == LiftableState.IDLE && throwByAccident) {
	// 		Debug.Log("throwByAccident "+throwByAccident);
	// 		if (col.gameObject.tag == Constants.Tag.PLAYER) {
	// 			mainCollider.isTrigger = true;
	// 		}
	// 	}
	// }

	// void OnTriggerEnter (Collider col) {
	// 	if (state == LiftableState.IDLE && !throwByAccident && mainCollider.isTrigger) {
	// 		Debug.Log("LiftableState.IDLE && throwByAccident "+throwByAccident);
	// 		if (col.tag == Constants.Tag.PLAYER) {
	// 			mainCollider.isTrigger = false;
	// 		}
	// 	}
	// }

	// void OnTriggerExit (Collider col) {
	// 	if (state == LiftableState.IDLE && throwByAccident) {
	// 		// Debug.Log("LiftableState.IDLE && throwByAccident "+throwByAccident);
	// 		if (col.tag == Constants.Tag.PLAYER) {
	// 			mainCollider.isTrigger = true;
	// 			throwByAccident = false;
	// 		}
	// 	} else if (state == LiftableState.IDLE && !throwByAccident) {
	// 		if (col.tag == Constants.Tag.PLAYER) {
	// 			mainCollider.isTrigger = false;
	// 		}
	// 	}
	// }

	// void OnCollisionEnter (Collision col)
	// {
	// 	if(col.gameObject.tag != Constants.Tag.PLAYER){
	// 		attachedObject = col.gameObject;
	// 	}
	// }
}
