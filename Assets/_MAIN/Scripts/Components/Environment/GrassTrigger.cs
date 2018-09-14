using UnityEngine;

public class GrassTrigger : MonoBehaviour {
	public Grass grass;

	void OnTriggerEnter(Collider other){
		if(other.tag == Constants.Tag.PLAYER){
			grass.interact = true;
		}else if(other.GetComponent<Damage>() != null){
			if(other.GetComponent<Damage>().isAffectGrass){
				// Debug.Log("AFFECT GRASS");
				grass.destroy = true;
			}
			
		}
	}
}
