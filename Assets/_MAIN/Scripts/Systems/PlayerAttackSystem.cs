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
	PlayerInput input;

	PlayerState state;

	// bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (attackData.Length == 0) return;

		for (int i=0; i<attackData.Length; i++) {
			attack = attackData.Attack[i];
			input = attackData.PlayerInput[i];
			player = attackData.Player[i];
			state = player.state;

			CheckIfPlayerDash ();
			CheckIfPlayerAttack ();
		}
	}

	public void SpawnSlashEffect (int mode) {
		switch (mode) {
            case 1:
                SpawnNormalAttackObj (attack.slash);
                break;
            case 2:
                SpawnNormalAttackObj (attack.slash);
                break;
            case 3:
                SpawnNormalAttackObj (attack.slash);
                break;
            case -1:
                SpawnChargeAttackObj (attack.chargeSlash);
                break;
            case -2:
                SpawnNormalAttackObj (attack.counterSlash);
                break;
            // case -3:
            //     SpawnObj (attack.counterSlash); //RAPIDSLASH
            //     break;
            case -4:
                SpawnNormalAttackObj (attack.regularArrow);
                break;
        }

		attack.isAttacking = false;
    }

	void CheckIfPlayerDash () {
		if (state == PlayerState.DASH) {
			if (attack.isDashing) {
				attack.dashAttackArea.SetActive(true);
			} else {
				attack.dashAttackArea.SetActive(false);
			}
		}
	}

	void CheckIfPlayerAttack () {
		int attackMode = input.attackMode;
		
		if (attack.isAttacking && attackMode != 0) {
			Debug.Log("PlayerState = "+state);
			if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.RAPID_SLASH || state == PlayerState.BOW) {
				// || state == PlayerState.COUNTER
				SpawnSlashEffect(attackMode);
			}
		}
	}

    void SpawnNormalAttackObj (GameObject obj) {
        GameObject spawnedObj = GameObject.Instantiate(obj, attack.normalAttackSpawnPos.position, SetFacing());
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
        spawnedObj.SetActive(true);
    }

	void SpawnChargeAttackObj (GameObject obj) {
		// Vector3 spawnPos = attack.bulletSpawnPos.parent.position;
		Quaternion spawnRot = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        GameObject spawnedObj = GameObject.Instantiate(obj, attack.chargeAttackSpawnPos.position, spawnRot);
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
        spawnedObj.SetActive(true);
    }

    Quaternion SetFacing () {
        Vector3 targetPos = attack.normalAttackSpawnPos.position;
        Vector3 initPos = attack.transform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.z -= initPos.z;
        float angle = Mathf.Atan2 (targetPos.z, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle));

        return targetRot;
    }

	// void ResetSlashEffect (Attack atk, int mode) {
	// 	atk.SpawnSlashEffect(mode); //temp
	// 	atk.isAttacking = false;
	// }
}
