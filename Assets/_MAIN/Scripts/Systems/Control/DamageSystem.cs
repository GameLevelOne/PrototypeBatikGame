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
	CircleCollider2D col;

	protected override void OnUpdate () {
		if (damageData.Length == 0) return;
		
		for (int i=0; i<damageData.Length; i++) {
			health = damageData.Health[i];
			role = damageData.Role[i];
			col = damageData.Collider[i];
			
			if (role.gameRole == GameRole.Player) {
				player = health.GetComponent<Player>();

				if (player.IsPlayerGetHurt) {
					if (!player.IsPlayerDie) {
						health.HealthPower -= health.damage;
						player.IsPlayerGetHurt = false;

						if (health.HealthPower <= 0f) {
							player.IsPlayerDie = true;
							col.enabled = false;
						}
					}
				}
			} else if (role.gameRole == GameRole.Enemy) {
				enemy = health.GetComponent<Enemy>();

				if (enemy.IsEnemyGetHurt) {
					if (!enemy.IsEnemyDie) {
						health.HealthPower -= health.damage;
						enemy.IsEnemyGetHurt = false;

						if (health.HealthPower <= 0f) {
							enemy.IsEnemyDie = true;
							col.enabled = false;
						}
					}
				}
			} else {
				Debug.Log("Unknown");
			}
		}
	}
}
