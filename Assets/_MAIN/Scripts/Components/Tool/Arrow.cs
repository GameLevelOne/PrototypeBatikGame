using UnityEngine;

public class Arrow : MonoBehaviour {
	public Projectile projectile;
	public float manaCost = 0;
	public Animator animator;
	public Collider collider;
	public Rigidbody rigidbody;
	// public float speed;
	// public Rigidbody rb;

	// public bool isShot = false;
	// public bool isHit = false;
	[HeaderAttribute("Current")]
	public Transform playerTransform;

	void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.BOUNDARIES) {
			// projectile.isSelfDestroying = true;
			// isHit = true;
			StartArrowBounceAnimation ();
		}
	}

	public void StartArrowBounceAnimation () {
		rigidbody.velocity = Vector3.zero;
		collider.enabled = false;
		// animator.Play(Constants.AnimatorParameter.Trigger.ARROW_BOUNCE);
		DestroyArrow();
	}

	public void DestroyArrow () {
		projectile.isSelfDestroying = true;
	}
}
