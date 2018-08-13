using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerAttackSystem : ComponentSystem {
	public struct AttackData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Attack> Attack;
	}
	[InjectAttribute] AttackData attackData;

	public Attack attack;
	Player player;

	PlayerState state;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (attackData.Length == 0) return;

		for (int i=0; i<attackData.Length; i++) {
			attack = attackData.Attack[i];
			PlayerInput input = attackData.PlayerInput[i];
			player = attackData.Player[i];
			state = player.state;
			
			int attackMode = input.AttackMode;
			bool isAttacking = attack.isAttacking;

			if (attackMode == 0 || (state == PlayerState.USING_TOOL) || (state == PlayerState.USING_TOOL)) continue;

			//Attack
        	// attack.isAttacking = true;
			if (isAttacking) {
				if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.RAPID_SLASH || state == PlayerState.BOW) {
					Debug.Log("Slash Attack : "+attackMode);
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
            // case -3:
            //     SpawnObj (attack.counterSlash); //RAPIDSLASH
            //     break;
            case -4:
                SpawnObj (attack.regularArrow);
                break;
        }

		attack.isAttacking = false;
    }

    void SpawnObj (GameObject obj) {
        GameObject spawnedBullet = GameObject.Instantiate(obj, attack.bulletSpawnPos.position, SetFacing());
        spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
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
