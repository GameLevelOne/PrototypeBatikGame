﻿using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using System.Collections.Generic;

public class DamageSystem : ComponentSystem {
	public struct DamageData {
		public readonly int Length;
		public ComponentArray<Health> Health;
		public ComponentArray<Role> Role;
		public ComponentArray<CircleCollider2D> Collider;
	}
	[InjectAttribute] DamageData damageData;

	Health health;
	Role role;
	Player player;
	Enemy enemy;

	PlayerState playerState;

	CircleCollider2D col;

	protected override void OnUpdate () {
		if (damageData.Length == 0) return;
		
		for (int i=0; i<damageData.Length; i++) {
			health = damageData.Health[i];
			role = damageData.Role[i];
			col = damageData.Collider[i];
			
			if (role.gameRole == GameRole.Player) {
				player = health.GetComponent<Player>();
				playerState = player.playerState;

				if (playerState == PlayerState.DIE) continue;

				if (playerState == PlayerState.GET_HURT) {
					health.HealthPower -= health.damage;
					// player.IsPlayerGetHurt = false;
					player.SetPlayerIdle();

					if (health.HealthPower <= 0f) {
						// player.IsPlayerDie = true;
						player.SetPlayerState(PlayerState.DIE);
						col.enabled = false;
					}
				}
			} else if (role.gameRole == GameRole.Enemy) {
				enemy = health.GetComponent<Enemy>();

				if (enemy.IsEnemyDie) return;

				if (enemy.IsEnemyGetHurt) {
					health.HealthPower -= health.damage;
					enemy.IsEnemyGetHurt = false;

					if (health.HealthPower <= 0f) {
						enemy.IsEnemyDie = true;
						col.enabled = false;
					}
				}
			} else {
				Debug.Log("Unknown");
			}
		}
	}
}
