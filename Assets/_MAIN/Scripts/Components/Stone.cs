using UnityEngine;

public class Stone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger " + other.tag);

		if(other.tag == Constants.Tag.HAMMER)
		{
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision " + other.gameObject.tag);

		if(other.gameObject.tag == Constants.Tag.HAMMER)
		{
			Destroy(this.gameObject);
		}
	}
}