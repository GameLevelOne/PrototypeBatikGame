using UnityEngine;

public class WaterShooterBullet : MonoBehaviour {
	public Vector2 direction;
	public float speed;
	public bool init = false;
	public bool destroyed = false;

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.GetComponent<WaterShooterEnemy>() == null) destroyed = true;
	}
}
