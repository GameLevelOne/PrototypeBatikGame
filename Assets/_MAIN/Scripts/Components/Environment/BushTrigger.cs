using UnityEngine;

public class BushTrigger : MonoBehaviour {

	public Bush bush;

	void OnTriggerEnter (Collider other){
		if(other.GetComponent<Damage>() != null){
			// Debug.Log("Trigger "+other.name);
			if(other.GetComponent<Damage>().isAffectBush){
				// Debug.Log("AFFECT GRASS");
				bush.destroy = true;
			}
			
		}
	}

	// void OnCollisionEnter (Collision other){
	// 	if(other.gameObject.GetComponent<Damage>() != null){
	// 		Debug.Log("Collision "+other.gameObject.name);
	// 		if(other.gameObject.GetComponent<Damage>().isAffectBush){
	// 			// Debug.Log("AFFECT GRASS");
	// 			bush.destroy = true;
	// 		}
	// 	}
	// }
}
