using UnityEngine;

// public enum AttackType {
//     Slash,
//     Charge,
//     Shot
// }

public class Attack : MonoBehaviour {
	public GameObject bullet;
    public Transform bulletSpawnPos; 
    public float attackRate = 0.5f;
    public bool isAttacking = false;

    public void ReadyForAttacking () {
        isAttacking = false;
    }
}
