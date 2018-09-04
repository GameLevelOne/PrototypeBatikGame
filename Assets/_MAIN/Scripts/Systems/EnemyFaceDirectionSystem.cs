﻿using Unity.Entities;
using UnityEngine;

public class EnemyFaceDirectionSystem : ComponentSystem {
	public struct FaceDirectionComponent
	{
		public readonly int Length;
		public ComponentArray<Enemy> enemy;
		public ComponentArray<Rigidbody> enemyRigidbody;
		public ComponentArray<Animator> enemyAnim;
		public ComponentArray<Facing2D> facing;
	}

	#region injected component
	[InjectAttribute] FaceDirectionComponent faceDirectionComponent;
	Enemy currEnemy;
	Rigidbody currEnemyRigidbody;
	Animator currEnemyAnim;
	Facing2D currEnemyFacing;
	#endregion

	protected override void OnUpdate()
	{
		for(int i = 0;i<faceDirectionComponent.Length;i++){
			currEnemy = faceDirectionComponent.enemy[i];
			currEnemyAnim = faceDirectionComponent.enemyAnim[i];
			currEnemyRigidbody = faceDirectionComponent.enemyRigidbody[i];
			currEnemyFacing = faceDirectionComponent.facing[i];

			SetFaceDirection();
		}
	}

	void SetFaceDirection()
	{
		if(currEnemy.state == EnemyState.Patrol){
			Vector3 dir = GetDirection(currEnemyRigidbody.position,currEnemy.patrolDestination);
			SetEnemyFacing(dir);
		}else if((currEnemy.state == EnemyState.Chase || currEnemy.state == EnemyState.Attack) && currEnemy.playerTransform != null){
			Vector3 dir = GetDirection(currEnemyRigidbody.position,currEnemy.playerTransform.position);
			SetEnemyFacing(dir);
		}
	}

	/// <summary>
    /// <para>4 Directions: <br /></para>
	/// <para>D = (0, -1) <br /></para>
	/// <para>L = (-1, 0) <br /></para>
	/// <para>R = (1, 0) <br /></para>
	/// <para>U = (0 ,1) <br /></para>
    /// </summary>
	Vector3 GetDirection(Vector3 self, Vector3 target)
	{
		Vector3 distance = target - self;
		float magnitude = distance.magnitude;
		Vector3 direction = distance / magnitude;

		float x = Mathf.RoundToInt(direction.x);
		float z = Mathf.RoundToInt(direction.z);
		

		// if(x < 0f) x = -1f;
		// else x = 1f;

		// if(y < 0f) y = -1f;
		// else y = 1f;
		currEnemyFacing.DirID = CheckDirID(x,z);
		return new Vector3(x, 0,z);
	}

	void SetEnemyFacing (Vector3 facingDir) {
		currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_X,facingDir.x);
		currEnemyAnim.SetFloat(Constants.AnimatorParameter.Float.FACE_Y,facingDir.z);
	}

	int CheckDirID (float dirX, float dirZ) {
		int dirIdx = 0;

		#region 4 Direction
		if (dirX == 0) {
			if (dirZ > 0) {
				dirIdx = 3;
			} else {
				dirIdx = 1;
			}
		} else if (dirX < 0) {
			dirIdx = 2;
		} else if (dirX > 0) {
			dirIdx = 4;
		}
		#endregion

#region 8 Direction
		// if (dirX == 0) {
		// 	if (dirY > 0) {
		// 		dirIdx = 5;
		// 	} else {
		// 		dirIdx = 1;
		// 	}
		// } else if (dirX < 0) {
		// 	if (dirY < 0) {
		// 		dirIdx = 2;
		// 	} else if (dirY > 0) {
		// 		dirIdx = 4;
		// 	} else {
		// 		dirIdx = 3;
		// 	}
		// } else if (dirX > 0) {
		// 	if (dirY < 0) {
		// 		dirIdx = 8;
		// 	} else if (dirY > 0) {
		// 		dirIdx = 6;
		// 	} else {
		// 		dirIdx = 7;
		// 	}
		// }
#endregion

		return dirIdx;
	}
}