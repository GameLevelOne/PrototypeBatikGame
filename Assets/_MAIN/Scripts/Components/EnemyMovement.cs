using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public Vector3 targetPos;
	public Transform chaseTransform;
	
	public float speed;

	public float chaseDuration = 5f;

	public bool isMoving = false;
	public bool isChasing = false;

	public Direction faceDirection;

	#region setter getter
	float tChase;

	public float TChase{
		get{return tChase;}
		set{tChase = value;}
	}
	#endregion
}
