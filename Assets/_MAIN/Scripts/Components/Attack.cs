using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject slash;
    public GameObject heavySlash;
	public GameObject bullet;
    public Transform bulletSpawnPos; 
    public float attackRate = 0.5f;

    [SerializeField] bool currentIsAttacking = false;

    public void SpawnSlashEffect (int attackType) {
        switch (attackType) {
            case 1:
                SpawnObj (slash);
                break;
            case 2:
                SpawnObj (slash);
                break;
            case 3:
                SpawnObj (slash);
                break;
            case -1:
                SpawnObj (heavySlash);
                break;
        }
    }

    void SpawnObj (GameObject obj) {
        GameObject spawnedBullet = Instantiate(obj, bulletSpawnPos.position, SetFacing());
        spawnedBullet.transform.SetParent(this.transform); //TEMPORARY
        spawnedBullet.SetActive(true);
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

    public bool isAttacking {
        get {return currentIsAttacking;}
		set {
			if (currentIsAttacking == value) return;

			currentIsAttacking = value;
		}
    }
}
