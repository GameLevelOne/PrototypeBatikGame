using UnityEngine;

public class Hook : MonoBehaviour {
	[Header("Reference")]
	public Rigidbody2D rb;
	
	[Header("Variables")]
	public float speed;
	public float range;
	public bool isLaunching = false;
	public bool isReturning = false;

	[Header("Current")]
	public GameObject attachedObject;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "tag name (?)"){
			//what?
			//pull player to attached object if it is solid object
			//pull enemy to player if it is enemy
			//does damage

			//isLaunching is false
			//attach object to attachedObject
		}
	}
}
