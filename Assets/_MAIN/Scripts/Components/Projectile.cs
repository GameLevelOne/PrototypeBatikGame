using UnityEngine;

public enum ProjectileType {
	NONE,
	BULLET,
	CATAPULT,
}

public class Projectile : MonoBehaviour {
	public ProjectileType type;
	public bool isDestroyOnTriggering;

	[HeaderAttribute("Ignore All If NONE Type")]
	public float speed;
	public bool isStartLaunching;
	public bool isDestroyOnColliding;

	[HeaderAttribute("CATAPULT Type Only (Need Gravity)")]
	public float elevationAngle;

	[HeaderAttribute("Current")]
	public bool isLaunching = false;
	public bool isCollideSomething = false;
	public bool isSelfDestroying = false;

	[HeaderAttribute("Current CATAPULT")]
	public Vector3 direction;

	[HeaderAttribute("For Specific Object")]
	public bool isDisableCameraShake = false;

	// void OnCollisionEnter (Collision col) {
	// 	isCollideSomething = true;
	// }

	// void OnTriggerEnter (Collider col) {
	// 	isTriggerSomething = true;
	// }
}
