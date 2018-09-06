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
	}
	[InjectAttribute] DamageData damageData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	Health health;
	Role role;

	protected override void OnUpdate () {
		if (damageData.Length == 0) return;
		
		for (int i=0; i<damageData.Length; i++) {
			health = damageData.Health[i];
			role = damageData.Role[i];
			
			CheckRole();
		}
	}

	void CheckRole()
	{
		if (role.gameRole == GameRole.Player) {
			// Player player = health.player;
			// playerState = player.state;

			// if (playerState == PlayerState.DIE) return;

			// if (player.isPlayerGetHurt) {
			// 	health.HealthPower -= health.damage;
			// 	player.isPlayerGetHurt = false;

			// 	//Set Player Get Hurt Animation
			// 	if (player.isGuarding) {
			// 		player.SetPlayerState(PlayerState.BLOCK_ATTACK);
			// 	} else {
			// 		player.SetPlayerState(PlayerState.GET_HURT);
			// 	}

			// 	if (health.HealthPower <= 0f) {
			// 		player.SetPlayerState(PlayerState.DIE);
			// 		col.enabled = false;
			// 	}
			// }

			CalculateDamageToPlayer();
		} else if (role.gameRole == GameRole.Enemy) {
			// enemy = health.GetComponent<Enemy>();
			// enemyState = enemy.state;

			// if (enemyState == EnemyState.Die) return;

			// if (enemy.isHit) {
			// 	if(!enemy.hasArmor){
			// 		health.HealthPower -= health.damage;
				
			// 		//Set Enemy Get Hurt Animation;
			// 	}
			// 	enemy.isEnemyGetHurt = false;
			// }

			CalculateDamageToEnemy();
		} else {
			Debug.Log("Unknown");
		}
	}

	void CalculateDamageToPlayer()
	{	
		Player player = health.player;
		PlayerState playerState = player.state;

		if (!player.isPlayerHit || playerState == PlayerState.DIE || player.damageReceive == null) return;
		else {
			string damageTag = player.damageReceive.tag;
			float damage = player.damageReceive.damage;

			if (damageTag == Constants.Tag.ENEMY_ATTACK || damageTag == Constants.Tag.EXPLOSION) {
				if (CheckIfPlayerIsInvulnerable(player, playerState)) { //INVULNERABLE
					Debug.Log("Player is invulnerable");
				} else if (player.isGuarding) {
					player.SetPlayerState(PlayerState.BLOCK_ATTACK);
				} else {
					health.PlayerHP -= damage;
					player.SetPlayerState(PlayerState.GET_HURT);
				}
				
				player.damageReceive = null;
				player.isPlayerHit = false;

				if (health.PlayerHP <= 0f) {
					player.SetPlayerState(PlayerState.DIE);
				}
			}
		}
	}

	bool CheckIfPlayerIsInvulnerable (Player player, PlayerState playerState) {
		ToolType toolType = playerInputSystem.toolType;
		PlayerInput input = playerInputSystem.input;

		switch (playerState) {
			case PlayerState.IDLE:
				return false;
			case PlayerState.MOVE:
				return false;
			case PlayerState.USING_TOOL:
				switch (toolType) {
					case ToolType.MagicMedallion:
						return true;
					//
				}
				return true;
			case PlayerState.OPEN_CHEST:
				return true;
			case PlayerState.DODGE:
				return true;
			case PlayerState.COUNTER:
				return true;
			case PlayerState.CHARGE:
				return true;
			case PlayerState.DASH:
				if (player.isBouncing) return true; //BOUNCE
				else return false;
			case PlayerState.GET_TREASURE:
				return true;
			case PlayerState.FISHING:
				if (input.interactValue == 2) return true; //PULL
				else return false;
			case PlayerState.SWIM:
				if (input.interactValue == 0 || input.interactValue == 2) return true; //SWIM START & END
				else return false;
			case PlayerState.SLOW_MOTION:
				return true;
			case PlayerState.RAPID_SLASH:
				return true;
			case PlayerState.BLOCK_ATTACK:
				return true;
			case PlayerState.GET_HURT:
				return true;
			case PlayerState.DIE:
				return true;
			//
			default: 
				Debug.Log("State "+playerState+" detected at DamageSystem is out of invulnerable list");
				return false;
		}
	}

	void CalculateDamageToEnemy()
	{
		Enemy currEnemy = health.enemy;

		if(!currEnemy.isHit) return;
		else{
			string damageTag = currEnemy.damageReceive.tag;
			float damage = currEnemy.damageReceive.damage;

			if(damageTag == Constants.Tag.HAMMER){
				if(currEnemy.hasArmor){
					currEnemy.hasArmor = false;
				}else{
					health.EnemyHP -= damage;
				}
			} else if (damageTag == Constants.Tag.PLAYER_DASH_ATTACK || damageTag == Constants.Tag.EXPLOSION) {
				health.EnemyHP -= damage;
			} else {
				if (damageTag == Constants.Tag.PLAYER_SLASH) {
					playerInputSystem.player.isHitAnEnemy = true;
					health.EnemyHP -= damage;
				}
			}
			// currEnemy.isEnemyGetHurt = false;
			currEnemy.damageReceive = null;
			currEnemy.isHit = false;
		}
	}
}
