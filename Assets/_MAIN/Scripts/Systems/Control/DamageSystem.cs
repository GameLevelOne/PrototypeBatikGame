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

			CalculateDamaageToPlayer();
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

	void CalculateDamaageToPlayer()
	{	
		Player player = health.player;
		PlayerState playerState = player.state;
		Debug.Log("CalculateDamaageToPlayer");

		if (playerState == PlayerState.DIE) return;

		if (!player.isPlayerHit) return;
		else {
			Debug.Log("Check player status");
			if (CheckIfPlayerIsInvulnerable(player, playerState)) { //INVULNERABLE
				Debug.Log("Player is invulnerable");
			} else if (player.isGuarding) {
				player.SetPlayerState(PlayerState.BLOCK_ATTACK);
			} else {
				health.HealthPower -= player.damageReceive.damage;
				player.SetPlayerState(PlayerState.GET_HURT);
			}
			
			player.damageReceive = null;
			player.isPlayerHit = false;
		}

		if (health.HealthPower <= 0f) {
			player.SetPlayerState(PlayerState.DIE);
		}
	}

	bool CheckIfPlayerIsInvulnerable (Player player, PlayerState playerState) {
		ToolType toolType = playerInputSystem.toolType;
		PlayerInput input = playerInputSystem.input;

		switch (playerState) {
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
				Debug.Log("Unknown state detected at DamageSystem");
				return false;
		}
	}

	void CalculateDamageToEnemy()
	{
		Enemy currEnemy = health.enemy;

		if(!currEnemy.isHit) return;
		else{
			if (currEnemy.damageReceive.tag == Constants.Tag.PLAYER_SLASH) {
				playerInputSystem.player.isHitAnEnemy = true;
			}

			if(currEnemy.damageReceive.tag == Constants.Tag.HAMMER){
				if(currEnemy.hasArmor){
					currEnemy.hasArmor = false;
				}else{
					health.HealthPower -= currEnemy.damageReceive.damage;
				}
			}else{
				health.HealthPower -= currEnemy.damageReceive.damage;
			}
			// currEnemy.isEnemyGetHurt = false;
			currEnemy.damageReceive = null;
			currEnemy.isHit = false;
		}
	}
}
