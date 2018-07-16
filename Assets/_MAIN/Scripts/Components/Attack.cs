using UnityEngine;

public enum Type {
    Shot,
    Slash
}

public class Attack : MonoBehaviour {
	public GameObject bullet;
    public Transform bulletSpawnPos; 

    public void Attacking () {
        GameObject spawnedBullet = Instantiate(bullet, bulletSpawnPos.position, Quaternion.identity);
    }
}
