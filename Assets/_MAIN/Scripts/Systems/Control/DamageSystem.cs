using UnityEngine;
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
	EnemyState enemyState;

	CircleCollider2D col;

	protected override void OnUpdate () {
		if (damageData.Length == 0) return;
		
		for (int i=0; i<damageData.Length; i++) {
			health = damageData.Health[i];
			role = damageData.Role[i];
			col = damageData.Collider[i];
			
			if (role.gameRole == GameRole.Player) {
				player = health.GetComponent<Player>();
				playerState = player.state;

				if (playerState == PlayerState.DIE) continue;

				if (player.IsPlayerGetHurt) {
					health.HealthPower -= health.damage;
					player.IsPlayerGetHurt = false;

					//Set Player Get Hurt Animation
					if (player.isGuarding) {
						player.SetPlayerState(PlayerState.BLOCK_ATTACK);
					} else {
						player.SetPlayerState(PlayerState.GET_HURT);
					}

					if (health.HealthPower <= 0f) {
						player.SetPlayerState(PlayerState.DIE);
						col.enabled = false;
					}
				}
			} else if (role.gameRole == GameRole.Enemy) {
				enemy = health.GetComponent<Enemy>();
				enemyState = enemy.state;

				if (enemyState == EnemyState.Die) return;

				if (enemy.IsEnemyGetHurt) {
					health.HealthPower -= health.damage;
					enemy.IsEnemyGetHurt = false;

					//Set Enemy Get Hurt Animation;
				}
			} else {
				Debug.Log("Unknown");
			}
		}
	}
}
