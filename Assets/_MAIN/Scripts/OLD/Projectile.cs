﻿using UnityEngine;

public enum ProjectileType {
	BULLET,
	CATAPULT,
}

public class Projectile : MonoBehaviour {
	public ProjectileType type;
	public float speed;
	public bool isStartLaunching;
	public bool isDestroyOnTriggering;
	public bool isDestroyOnColliding;

	[HeaderAttribute("CATAPULT Type Only (Need Gravity)")]
	public float elevationAngle;

	[HeaderAttribute("Current")]
	public bool isLaunching;
	public bool isCollideSomething;
	public bool isTriggerSomething;

	[HeaderAttribute("Current CATAPULT")]
	public Vector3 direction;

	// void OnCollisionEnter (Collision col) {
	// 	isCollideSomething = true;
	// }

	// void OnTriggerEnter (Collider col) {
	// 	isTriggerSomething = true;
	// }
}
