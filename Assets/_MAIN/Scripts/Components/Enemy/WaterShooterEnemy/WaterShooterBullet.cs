using UnityEngine;

public class WaterShooterBullet : MonoBehaviour {
	public Vector2 direction;
	public float speed;
	public bool destroyed = false;

	void OnCollisionEnter2D(Collision2D other)
	{
		destroyed = true;
	}
}
