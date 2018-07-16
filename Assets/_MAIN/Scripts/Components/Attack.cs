using UnityEngine;

public enum AttackType {
    Shot,
    Slash
}

public class Attack : MonoBehaviour {
	public GameObject bullet;
    public Transform bulletSpawnPos; 
    public float attackRate = 0.5f;

    [SerializeField] bool isAttacking = false;

    public void Attacking (AttackType attackType) {
        if (isAttacking) return;

        switch (attackType) {
            case AttackType.Slash:
                Debug.Log("Slash");
                break;
            case AttackType.Shot:
                isAttacking = true;
                GameObject spawnedBullet = Instantiate(bullet, bulletSpawnPos.position, SetFacing());
                spawnedBullet.transform.SetParent(this.transform); //TEMPORARY
                spawnedBullet.SetActive(true);
                break;
        }
    }

    Quaternion SetFacing () {
        Vector2 targetPos = bulletSpawnPos.position;
        Vector2 initPos = transform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.y -= initPos.y;
        float angle = Mathf.Atan2 (targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, 0f, angle));

        return targetRot;
    }

    public void ReadyForAttacking () {
        isAttacking = false;
    }
}
