using UnityEngine;
using Unity.Entities;

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
			// Debug.Log("PlayerState = "+state);
			if (state == PlayerState.ATTACK || state == PlayerState.BLOCK_ATTACK || state == PlayerState.CHARGE || state == PlayerState.RAPID_SLASH || state == PlayerState.BOW) {
				// || state == PlayerState.COUNTER
				SpawnSlashEffect(attackMode);
			}
		}
	}

	public void SpawnSlashEffect (int mode) {
		int facing = attack.playerFacing.DirID-1;
		switch (mode) {
            case 1:
                SpawnNormalAttackObj (attack.slash1[facing]);
                break;
            case 2:
                SpawnNormalAttackObj (attack.slash2[facing]);
                break;
            case 3:
                SpawnNormalAttackObj (attack.slash3[facing]);
                break;
            case -1:
                SpawnChargeAttackObj (attack.chargeSlash);
                break;
            case -2:
                SpawnNormalAttackObj (attack.counterSlash[facing]);
                break;
            // case -3:
            //     SpawnObj (attack.counterSlash); //RAPIDSLASH
            //     break;
            case -4:
                SpawnArrowAttackObj (attack.regularArrow[facing]);
                break;
        }
		// Debug.Log("SpawnSlashEffect "+mode);
		attack.isAttacking = false;
    }

    void SpawnNormalAttackObj (GameObject obj) {
        Vector3 initPos = attack.transform.position;
        GameObject spawnedObj = GameObject.Instantiate(obj, attack.normalAttackSpawnPos.position,Quaternion.identity);
        spawnedObj.SetActive(true);
    }
    void SpawnArrowAttackObj (GameObject obj) {
		Vector3 targetPos = attack.normalAttackSpawnPos.position;
        Vector3 initPos = attack.transform.position;

		Vector3 deltaPos = Vector3.zero;
		float localTargetPosX = attack.normalAttackSpawnPos.localPosition.x;

		if (localTargetPosX != 0) {
			deltaPos = new Vector3 (targetPos.x - initPos.x, 0f, 0f); 
		} else {
			deltaPos = targetPos - initPos;
		}
		if (attack.playerFacing.DirID==2 || attack.playerFacing.DirID == 4)
			targetPos += new Vector3(0f,0f,0.8444f);
        GameObject spawnedObj = GameObject.Instantiate(obj, targetPos, SetFacingParent(deltaPos));
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
		
		// Debug.Log(spawnedObj.transform.GetChild(0).name);
		spawnedObj.transform.GetChild(0).rotation = SetFacingChild(deltaPos);

        spawnedObj.SetActive(true);
    }

	void SpawnChargeAttackObj (GameObject obj) {
        GameObject spawnedObj = GameObject.Instantiate(obj, attack.chargeAttackSpawnPos.position, Quaternion.identity);
        // spawnedBullet.transform.SetParent(attack.transform); //TEMPORARY
        spawnedObj.SetActive(true);
    }
	
	Quaternion SetFacingParent (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.x, resultPos.z) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, angle - 90f, 0f));

        return targetRot;
	}

    Quaternion SetFacingChild (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.z, resultPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle));
        // Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle - 90f));
        // Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle - 90f));

        return targetRot;
    }

	// void ResetSlashEffect (Attack atk, int mode) {
	// 	atk.SpawnSlashEffect(mode); //temp
	// 	atk.isAttacking = false;
	// }
}
