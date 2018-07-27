using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject slash;
    public GameObject heavySlash;
    public GameObject counterSlash;
    // public GameObject minatoKunai;
	// public GameObject bullet;
    public Transform bulletSpawnPos; 
    public float attackRate = 0.5f;

    [SerializeField] bool currentIsAttacking = false;

    public bool isAttacking {
        get {return currentIsAttacking;}
		set {
			if (currentIsAttacking == value) return;

			currentIsAttacking = value;
		}
    }
}
