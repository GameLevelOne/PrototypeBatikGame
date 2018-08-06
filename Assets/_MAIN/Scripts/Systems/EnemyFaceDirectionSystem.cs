using Unity.Entities;
using UnityEngine;

/// <summary>
/// for bee only. (sementara)
/// </summary>
public class EnemyFaceDirectionSystem : ComponentSystem {
	public struct FaceDirectionComponent
	{
		public readonly int Length;
		public ComponentArray<Bee> bee;
		public ComponentArray<BeeMovement> beeMovement;
		public ComponentArray<Rigidbody2D> beeRigidbody;
		public ComponentArray<Animator> enemyAnim;
		
	}

	#region injected component
	[InjectAttribute] FaceDirectionComponent faceDirectionComponent;
	Bee currBee;
	BeeMovement currBeeMovement;
	Rigidbody2D currBeeRigidbody;
	Animator currEnemyAnim;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<faceDirectionComponent.Length;i++){
			currBee = faceDirectionComponent.bee[i];
			currBeeMovement = faceDirectionComponent.beeMovement[i];
			currEnemyAnim = faceDirectionComponent.enemyAnim[i];
			currBeeRigidbody = faceDirectionComponent.beeRigidbody[i];

			SetFaceDirection();
		}
	}

	void SetFaceDirection()
	{
		if(currBee.beeState == BeeState.Patrol){
			Vector2 dir = GetDirection(currBeeRigidbody.position,currBeeMovement.patrolDestination);
			//Debug.Log("Direction = "+dir);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,dir.x);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,dir.y);
		}else if(currBee.beeState == BeeState.Chase){
			Vector2 dir = GetDirection(currBeeRigidbody.position,currBee.playerTransform.position);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,dir.x);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,dir.y);
		}
	}

	/// <summary>
    /// 4 Directions (UL, UR, DL, DR)
    /// </summary>
	Vector2 GetDirection(Vector2 self, Vector2 target)
	{
		Vector3 distance = target - self;
		float magnitude = distance.magnitude;
		Vector3 direction = distance / magnitude;

		float x = direction.x;
		float y = direction.y;
		
		if(x < 0f) x = -1f;
		else x = 1f;

		if(y < 0f) y = -1f;
		else y = 1f;

		return new Vector2(x,y);
	}
}