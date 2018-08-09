using Unity.Entities;
using UnityEngine;

public class EnemyFaceDirectionSystem : ComponentSystem {
	public struct FaceDirectionComponent
	{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Rigidbody2D> enemyRigidbody;
		public ComponentArray<Animator> enemyAnim;
	}

	#region injected component
	[InjectAttribute] FaceDirectionComponent faceDirectionComponent;
	Enemy currEnemy;
	Rigidbody2D currEnemyRigidbody;
	Animator currEnemyAnim;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<faceDirectionComponent.Length;i++){
			currEnemy = faceDirectionComponent.enemy[i];
			currEnemyAnim = faceDirectionComponent.enemyAnim[i];
			currEnemyRigidbody = faceDirectionComponent.enemyRigidbody[i];

			SetFaceDirection();
		}
	}

	void SetFaceDirection()
	{
		if(currEnemy.state == EnemyState.Patrol){
			Vector2 dir = GetDirection(currEnemyRigidbody.position,currEnemy.patrolDestination);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,dir.x);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,dir.y);
		}else if(currEnemy.state == EnemyState.Chase || currEnemy.state == EnemyState.Attack){
			Vector2 dir = GetDirection(currEnemyRigidbody.position,currEnemy.playerTransform.position);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,dir.x);
			currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,dir.y);
		}
	}

	/// <summary>
    /// <para>4 Directions: <br /></para>
	/// <para>UL = (-1, 1) <br /></para>
	/// <para>UR = ( 1, 1) <br /></para>
	/// <para>DL = (-1,-1) <br /></para>
	/// <para>DR = ( 1,-1) <br /></para>
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