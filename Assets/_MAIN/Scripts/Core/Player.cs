﻿using UnityEngine;

public enum PlayerState {
	IDLE, 
	MOVE,
	ATTACK,
	CHARGE,
	COUNTER,
	DIE,
	USING_TOOL,
	// HIT,
	GET_HURT,
	RAPID_SLASH,
	SLOW_MOTION,
	HOOK,
	BLOCK_ATTACK,
	DODGE,
	DASH,
	POWER_BRACELET,
	SWIM,
	FISHING,
	BOW,
	GET_TREASURE,
	OPEN_CHEST
}

public class Player : MonoBehaviour {
	[HeaderAttribute("Player Attributes")]
	public Health health;
	public PlayerState state;
	public Collider2D playerCol;
	
	[HeaderAttribute("Current")]
	public Enemy enemyThatHitsPlayer;
	// public PlayerTool playerTool;

	[SpaceAttribute(10f)]
	public Damage damageReceive;
	public bool isGuarding = false; 
	public bool isParrying = false;
	public bool isUsingStand = false; //By Mana  
	public bool isBouncing = false;
	public bool isHitChestObject = false;
	public bool isCanOpenChest = false;
	public bool isMoveAttack = false;
	    
    public bool isPlayerHit = false; 
	public bool isPlayerGetHurt = false;
    public bool isHitAnEnemy = false;
	public bool isBulletTiming = false;
	public bool isCanDigging = false;
	public bool isInvisible = false;
	public bool isHitLiftableObject = false;
	public bool isCanFishing = false;

	void OnEnable () {
		health.OnDamageCheck += DamageCheck;
	}

	void OnDisable () {
		health.OnDamageCheck -= DamageCheck;
	}

	void DamageCheck (Damage damage) {
		damageReceive = damage;
		isPlayerHit = true;
	}

    public int MaxHP{
        get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
        set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
    }

	void OnCollisionEnter2D (Collision2D col) {
		if (state == PlayerState.DASH && !isBouncing) {
			isBouncing = true;	
		}
		
		if (col.gameObject.GetComponent<Liftable>() != null && !isHitLiftableObject) {
			isHitLiftableObject = true;
		}

		if (col.gameObject.tag == Constants.Tag.CHEST) {
			isHitChestObject = true;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject.GetComponent<Liftable>() != null && isHitLiftableObject) {
			isHitLiftableObject = false;
		}

		if (col.gameObject.tag == Constants.Tag.CHEST && isHitChestObject) {
			isHitChestObject = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {		
		if (col.tag == Constants.Tag.DIG_AREA) {
			isCanDigging = true;
		} 
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == Constants.Tag.DIG_AREA) {
			isCanDigging = false;
		}
	}
	
	#region PLAYER STATE 
	public void SetPlayerState (PlayerState playerState) {
		// if (playerState == PlayerState.MOVE) Debug.Log("Move");
		state = playerState;
	}

	public void SetPlayerIdle () {
		// Debug.Log("Idle");
		state = PlayerState.IDLE;
	}
	#endregion
}