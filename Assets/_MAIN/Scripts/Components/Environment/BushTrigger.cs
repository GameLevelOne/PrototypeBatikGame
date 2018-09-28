using UnityEngine;

public class BushTrigger : MonoBehaviour {

	public Bush bush;

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Damage>() != null){
			if(other.GetComponent<Damage>().isAffectBush){
				// Debug.Log("AFFECT GRASS");
				bush.destroy = true;
			}
			
		}
	}
}
