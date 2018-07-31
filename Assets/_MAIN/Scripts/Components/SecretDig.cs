using UnityEngine;

public class SecretDig : MonoBehaviour {
	public GameObject secretRewardObj;

	public bool isAlreadyDigged = false;

	[SerializeField] bool isSecretDigHit = false;

	public bool IsSecretDigHit {
		get {return isSecretDigHit;}
		set {
			if (isSecretDigHit == value) return;

			isSecretDigHit = value;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		Debug.Log("Secret dig hit : " + col.tag);
		if (col.tag == Constants.Tag.DIG_RESULT) {
			IsSecretDigHit = true;
		}
	}
}
