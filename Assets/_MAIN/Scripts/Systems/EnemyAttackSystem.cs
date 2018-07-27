using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class EnemyAttackSystem : ComponentSystem {
	public struct AttackData {
		public readonly int Length;
		public ComponentArray<EnemyInput> EnemyInput;
		public ComponentArray<Animation2D> Animation;
		public ComponentArray<Attack> Attack;
	}
	[InjectAttribute] AttackData attackData;
	
	Attack attack;
	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (attackData.Length == 0) return;
		
		// if (!isLocalVarInit) {
		// 	objTransform = new Transform[attackData.Length];
		// 	bulletSpawnPos = new Transform[attackData.Length];
		// 	bullet = new GameObject[attackData.Length];
		// 	isAttacking = new bool[attackData.Length];
		// 	isLocalVarInit = true;			
		// }

		for (int i=0; i<attackData.Length; i++) {
			EnemyInput input = attackData.EnemyInput[i];
			Animation2D anim = attackData.Animation[i];
			attack = attackData.Attack[i];
			
			int attackMode = input.AttackMode;
			AnimationState animState = anim.animState;
			bool isAttacking = attack.isAttacking;

			if (attackMode == 0) continue;

			//Attack
        	// attack.isAttacking = true;
			if (!isAttacking) {
				continue;
			} else {
				if ((animState == AnimationState.START_SLASH) || (animState == AnimationState.START_CHARGE) || 
				(animState == AnimationState.START_DODGE) || (animState == AnimationState.START_COUNTER)) {
					SpawnSlashEffect(attackMode);
				}
			}
		}
	}

	public void SpawnSlashEffect (int mode) {
		switch (mode) {
            case 1:
                SpawnObj (attack.slash);
                break;
            case 2:
                SpawnObj (attack.slash);
                break;
            case 3:
                SpawnObj (attack.slash);
                break;
            case -1:
                SpawnObj (attack.heavySlash);
                break;
            case -2:
                SpawnObj (attack.counterSlash);
                break;
        }

		attack.isAttacking = false;
    }

    void SpawnObj (GameObject obj) {
        GameObject spawnedBullet = GameObjectEntity.Instantiate(obj, attack.bulletSpawnPos.position, SetFacing());
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
        spawnedBullet.SetActive(true);
    }

    Quaternion SetFacing () {
        Vector2 targetPos = attack.bulletSpawnPos.position;
        Vector2 initPos = attack.transform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.y -= initPos.y;
        float angle = Mathf.Atan2 (targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, 0f, angle));

        return targetRot;
    }

	// void ResetSlashEffect (Attack atk, int mode) {
	// 	atk.SpawnSlashEffect(mode); //temp
	// 	atk.isAttacking = false;
	// }
}
