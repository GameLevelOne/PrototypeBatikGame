﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class PlayerAttackSystem : ComponentSystem {
	public struct AttackData {
		public readonly int Length;
		public ComponentArray<Transform> Transform;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Attack> Attack;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] AttackData attackData;
	
	Transform[] objTransform = new Transform[0];
	Transform[] bulletSpawnPos = new Transform[0];
	GameObject[] bullet = new GameObject[0];
	bool[] isAttacking = new bool[0];

	bool isLocalVarInit = false;

	protected override void OnUpdate () {
		if (attackData.Length == 0) return;
		
		if (!isLocalVarInit) {
			objTransform = new Transform[attackData.Length];
			bulletSpawnPos = new Transform[attackData.Length];
			bullet = new GameObject[attackData.Length];
			isAttacking = new bool[attackData.Length];
			isLocalVarInit = true;			
		}

		for (int i=0; i<attackData.Length; i++) {
			if (attackData.Length == 0) return;

			PlayerInput input = attackData.PlayerInput[i];
			Attack attack = attackData.Attack[i];
			int attackMode = input.Attack;
			
			objTransform[i] = attackData.Transform[i];
			bulletSpawnPos[i] = attack.bulletSpawnPos;
			bullet[i] = attack.bullet;
			isAttacking[i] = attack.isAttacking;

			if (input.Attack == 0) return;

			if (isAttacking[i]) return;

			//Attack
			isAttacking[i] = true;

			switch(attackMode) {
				case -1:
					Attacking(i, AttackType.Slash);
					break;
				case 1:
					Attacking(i, AttackType.Shot);
					break;
			}
		}
	}

	void Attacking (int idx, AttackType attackType) {
        switch (attackType) {
            case AttackType.Slash:
                Debug.Log("Slash");
                break;
            case AttackType.Shot:
                // GameObject spawnedBullet = Instantiate(bullet[idx], bulletSpawnPos[idx].position, SetFacing(idx));
                // spawnedBullet.transform.SetParent(objTransform[idx]); //TEMPORARY
                // spawnedBullet.SetActive(true);
                break;
        }
    }

    Quaternion SetFacing (int idx) {
        Vector2 targetPos = bulletSpawnPos[idx].position;
        Vector2 initPos = objTransform[idx].position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.y -= initPos.y;
        float angle = Mathf.Atan2 (targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, 0f, angle));

        return targetRot;
    }
}
