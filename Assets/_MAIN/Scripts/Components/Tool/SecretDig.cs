using UnityEngine;

public class SecretDig : MonoBehaviour {
	// public GameObject secretRewardObj;
	// public float posY;
	public float spawnItemProbability;

	[HeaderAttribute("Current")]
	public Vector3 digResultPos;
	public bool isSecretDigHit = false;
	public bool isAlreadyDigged = false;

	void OnTriggerEnter (Collider col) {
		// Debug.Log("Secret dig hit : " + col.tag);
		if (col.tag == Constants.Tag.DIG_RESULT) {
			digResultPos = col.transform.position;
			// digResultPos = new Vector3 (digResultPos.x, posY, digResultPos.z);
			isSecretDigHit = true;
		}
	}
}
