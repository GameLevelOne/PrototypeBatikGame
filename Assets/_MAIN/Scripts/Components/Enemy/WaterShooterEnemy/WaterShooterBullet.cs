﻿using UnityEngine;

public class WaterShooterBullet : MonoBehaviour {
	public Projectile Projectile;
	// public Vector3 direction;
	// public float speed;
	// public bool init = false;
	// public bool destroyed = false;

	// void OnCollisionEnter (Collision other)
	// {
	// 	if(other.gameObject.GetComponent<WaterShooterEnemy>() == null) Projectile.isCollideSomething = true;
	// }

	void OnTriggerEnter (Collider col) {
		if(col.tag == Constants.Tag.PLAYER || col.tag == Constants.Tag.BOUNDARIES) {
			// Debug.Log("null "+col.name);
			Projectile.isSelfDestroying = true;
		}
	}
}
