using UnityEngine;

public class Vines : MonoBehaviour {
	public ParticleSystem burnParticle;
	public Collider vinesCol;
	public GameObject vinesObj;

	public float burnDuration;	

	[HeaderAttribute("Saved ID")]
	public int vinesID;

	[HeaderAttribute("Current")]
	public float burnTimer;
	public bool isInitVines = false;
	public bool isInitBurned = false;
	public bool isBurned = false;
	public bool isDestroyed = false;
	
	[HeaderAttribute("Testing")]
	public bool isTesting;

	void Awake () {
		if (isTesting) {
			PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + vinesID, 0);
		}
	}

	public void OnCollisionEnter (Collision col) {
		Debug.Log("Collision "+col.gameObject.tag);
	}

	public void OnTriggerEnter (Collider col) {
		if (col.tag == Constants.Tag.FIRE_ARROW) {
			if (!isInitBurned) {
				isInitBurned = true;
			}

			col.GetComponent<Projectile>().isSelfDestroying = true;
		}
	}
}
