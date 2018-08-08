using UnityEngine;

public class Stone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Constants.Tag.HAMMER)
		{
			Destroy(this.gameObject);
		}
	}
}