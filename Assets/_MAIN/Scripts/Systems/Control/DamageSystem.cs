using UnityEngine;
using Unity.Entities;

public class DamageSystem : ComponentSystem {
	public struct DamageData {
		public readonly int Length;
		public ComponentArray<Health> Health;
		public ComponentArray<Role> Role;
	}
	[InjectAttribute] DamageData damageData;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] GameFXSystem gameFXSystem;
	[InjectAttribute] PowerBraceletSystem powerBraceletSystem;

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
			CalculateDamageToPlayer();
		} else if (role.gameRole == GameRole.Enemy) {
			CalculateDamageToEnemy();
		} else if (role.gameRole == GameRole.Boss) {
			CalculateDamageToBoss();
		} else {
			Debug.Log("Unknown Role");
		}
	}

	void CalculateDamageToPlayer()
	{	
		Player player = health.player;
		PlayerState playerState = player.state;

		if (!player.isPlayerHit || playerState == PlayerState.DIE || player.damageReceive == null) return;
		else {
			Transform playerTransform = player.transform;
			string damageTag = player.damageReceive.tag;
			float damage = player.damageReceive.damage;

			if (damageTag == Constants.Tag.ENEMY_ATTACK || damageTag == Constants.Tag.ENEMY || damageTag == Constants.Tag.BOSS) {
				if (!CheckIfPlayerIsInvulnerable(player, playerState)) {
					if (!CheckIfPlayerIsCannotKnockedback(player, playerState)) {
						player.isPlayerKnockedBack = true;
					}
					
					if (player.isGuarding) {
						player.SetPlayerState(PlayerState.BLOCK_ATTACK);
						damage -= player.shieldPower;
						health.PlayerHP = ReduceHP(health.PlayerHP, damage, playerTransform.position);
					} else {
						if (!CheckIfPlayerIsOnSpecialAction(player, playerState)) {
							Debug.Log(playerState+" NOT SPECIAL ACTION");
							player.SetPlayerState(PlayerState.GET_HURT);
						}
						
						health.PlayerHP = ReduceHP(health.PlayerHP, damage, playerTransform.position);
					}
				}
				
				player.damageReceive = null;
				player.isPlayerHit = false;

				if (health.PlayerHP <= 0f) {
					player.SetPlayerState(PlayerState.DIE);
				}
			} else if (damageTag == Constants.Tag.VINES || damageTag == Constants.Tag.EXPLOSION) {
				if (!CheckIfPlayerIsInvulnerable(player, playerState)) {
					if (!CheckIfPlayerIsCannotKnockedback(player, playerState)) {
						player.isPlayerKnockedBack = true;
					}
					
					if (!CheckIfPlayerIsOnSpecialAction(player, playerState) || (playerState == PlayerState.POWER_BRACELET && damageTag == Constants.Tag.EXPLOSION && powerBraceletSystem.powerBracelet.liftable == null)) {
						player.SetPlayerState(PlayerState.GET_HURT);
					}
					
					health.PlayerHP = ReduceHP(health.PlayerHP, damage, playerTransform.position);
				}
				
				player.damageReceive = null;
				player.isPlayerHit = false;

				if (health.PlayerHP <= 0f) {
					player.SetPlayerState(PlayerState.DIE);
				}
			}
			
			if (player.damageReceive != null) {
				if (player.damageReceive.GetComponent<WaterShooterBullet>() != null) {
					player.damageReceive.GetComponent<WaterShooterBullet>().projectile.isSelfDestroying = true;
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
			case PlayerState.ATTACK:
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
			// case PlayerState.COUNTER:
			// 	return true;
			case PlayerState.PARRY:
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
				Debug.Log("State "+playerState+" detected at DamageSystem is out of INVULNERABLE list");
				return false;
		}
	}

	bool CheckIfPlayerIsOnSpecialAction (Player player, PlayerState playerState) {
		switch (playerState) {
			case PlayerState.POWER_BRACELET:
				return true;
			// case PlayerState.GET_HURT:
			// 	return true;
			//
			default: 
				Debug.Log("State "+playerState+" detected at DamageSystem is out of SPECIAL_ACTION list");
				return false;
		}
	}

	bool CheckIfPlayerIsCannotKnockedback (Player player, PlayerState playerState) {
		switch (playerState) {
			case PlayerState.FISHING:
				return true;
			// case PlayerState.GET_HURT:
			// 	return true;
			//
			default: 
				Debug.Log("State "+playerState+" detected at DamageSystem is out of CANNOT_KNOCKEDBACK list");
				return false;
		}
	}

	void CalculateDamageToEnemy()
	{
		Enemy currEnemy = health.enemy;

		if(!currEnemy.isHit) return;
		else{
			Transform enemyTransform = currEnemy.transform;
			string damageTag = currEnemy.damageReceive.tag;
			float damage = currEnemy.damageReceive.damage;
			currEnemy.damageSourcePos = currEnemy.damageReceive.transform.position;
			// Debug.Log(damageTag + " " + damage);

			if(damageTag == Constants.Tag.HAMMER){
				currEnemy.initDamaged = false;
				currEnemy.SetEnemyState(EnemyState.Damaged);

				if(currEnemy.hasArmor){
					currEnemy.hasArmor = false;
				}else{
					// health.EnemyHP -= damage;
					// gameFXSystem.SpawnObj(gameFXSystem.gameFX.hitEffect, enemyTransform.position);
					health.EnemyHP = ReduceHP(health.EnemyHP, damage, enemyTransform.position);
				}
			} else if (damageTag == Constants.Tag.PLAYER_DASH_ATTACK||
			damageTag == Constants.Tag.MAGIC_MEDALLION ||
			damageTag == Constants.Tag.ARROW) {
				currEnemy.initDamaged = false;
				currEnemy.SetEnemyState(EnemyState.Damaged);
				health.EnemyHP = ReduceHP(health.EnemyHP, damage, enemyTransform.position);
			} else if (damageTag == Constants.Tag.FIRE_ARROW ||
			damageTag == Constants.Tag.EXPLOSION) {
				currEnemy.initDamaged = false;
				currEnemy.isBurned = true;
				currEnemy.SetEnemyState(EnemyState.Damaged);
				//BURN
				health.EnemyHP = ReduceHP(health.EnemyHP, damage, enemyTransform.position);
			} else {
				if (damageTag == Constants.Tag.PLAYER_SLASH || damageTag == Constants.Tag.PLAYER_COUNTER) {
					currEnemy.initDamaged = false;
					currEnemy.SetEnemyState(EnemyState.Damaged);
					// playerInputSystem.player.isHitAnEnemy = true;
					
					health.EnemyHP = ReduceHP(health.EnemyHP, damage, enemyTransform.position);
				}
			}
			// currEnemy.isEnemyGetHurt = false;
			currEnemy.playerThatHitsEnemy = playerInputSystem.player;
			// currEnemy.damageSourcePos = damageTransform.position;
			currEnemy.damageReceive = null;
			currEnemy.isHit = false;
		}
	}

	void CalculateDamageToBoss()
	{
		Jatayu currEnemy = health.jatayu;

		if(!currEnemy.isHit && !currEnemy.isBurned) return;
		else{
			Transform enemyTransform = currEnemy.transform;
			string damageTag = currEnemy.damageReceive.tag;
			float damage = currEnemy.damageReceive.damage;
			// Debug.Log(damage);

			if (!currEnemy.invulnerable) {
				health.EnemyHP = ReduceHP(health.EnemyHP, damage, enemyTransform.position);
			}

			// currEnemy.isEnemyGetHurt = false;
			currEnemy.damageReceive = null;
			currEnemy.isHit = false;
		}
	}

	float ReduceHP (float initHP, float damage, Vector3 hitPos) {		
		gameFXSystem.SpawnObj(gameFXSystem.gameFX.hitEffect, hitPos);
		return initHP -= damage;
	}
}
