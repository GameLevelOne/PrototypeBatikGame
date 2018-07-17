using UnityEngine;

public enum AttackType {
    Shot,
    Slash
}

public class Attack : MonoBehaviour {
	public GameObject bullet;
    public Transform bulletSpawnPos; 
    public float attackRate = 0.5f;
    public bool isAttacking = false;

    public void ReadyForAttacking () {
        isAttacking = false;
    }
}
