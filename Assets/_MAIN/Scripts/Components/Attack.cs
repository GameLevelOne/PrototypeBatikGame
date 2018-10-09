using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject[] slash1;
    public GameObject[] slash2;
    public GameObject[] slash3;
    public GameObject chargeSlash;
    public GameObject[] counterSlash;
    public GameObject[] regularArrow;
    public Transform normalAttackSpawnPos;
    public Transform chargeAttackSpawnPos; 

    // public float attackRate = 0.5f;
    public bool isAttacking = false;
    public bool isDashing = false;

    [HeaderAttribute("Current")]
    public bool isInitAttack = false;
    public GameObject dashAttackArea;
}
