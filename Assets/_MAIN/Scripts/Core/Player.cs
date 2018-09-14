using UnityEngine;

public enum PlayerState {
	IDLE, 
	MOVE,
	ATTACK,
	CHARGE,
	// COUNTER,
	PARRY,
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
	public Transform playerWeaponPos;
	public Collider playerCol;
	public float shieldPower;
	
	[HeaderAttribute("Current")]
	public Enemy enemyThatHitsPlayer;
	// public PlayerTool playerTool;

	[SpaceAttribute(10f)]
	public Damage damageReceive;
    public bool isPlayerHit = false; 
    public bool isHitAnEnemy = false;
	public bool isPlayerGetHurt = false;
	public bool isGuarding = false; 
	public bool isCanParry = false;
	public bool isCanBulletTime = false;
	public bool isUsingStand = false; //By Mana  
	public bool isBouncing = false;
	public bool isMoveAttack = false;
	// public bool isInvisible = false;
	
	[SpaceAttribute(10f)]
	public bool isHitChestObject = false;
	public bool isCanOpenChest = false;   
	public bool isCanDigging = false;
	public bool isHitLiftableObject = false;
	public bool isCanFishing = false;
	public bool isInteractingWithNPC = false;
	// public bool isInteractingNotWithNPC = false;

	public int interactIndex;
	
	[SerializeField] float maxHP;
	[SerializeField] float maxMP;

	void OnEnable () {
		health.OnDamageCheck += DamageCheck;
	}

	void OnDisable () {
		health.OnDamageCheck -= DamageCheck;
	}

	void DamageCheck (Damage damage) {
		damageReceive = damage;
		isPlayerHit = true;

		switch (damage.damageChar) {
			case DamageCharacteristic.COUNTERABLE:
				isCanBulletTime = true;
				break;
			case DamageCharacteristic.PARRYABLE:
				isCanParry = true;
				break;
			case DamageCharacteristic.COUNTER_AND_PARRYABLE:
				isCanBulletTime = true;
				isCanParry = true;
				break;
			default: // DamageCharacteristic.NONE
				isCanBulletTime = false;
				isCanParry = false;
				break;
		}
		// Debug.Log("DamageCheck with damageReceive : "+damageReceive+", and isPlayerHit : "+isPlayerHit);
	}

	// public bool isUsingStand {
	// 	get {return isStand;}
	// 	set {
	// 		isStand = value;
	// 		Debug.Log("Stand "+isStand);
	// 	}
	// }

    public float MaxHP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, maxHP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, value);}
    }

    public float MaxMP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMANA, maxMP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMANA, value);}
    }

	void OnCollisionEnter (Collision col) {
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

	void OnCollisionExit (Collision col) {
		if (col.gameObject.GetComponent<Liftable>() != null && isHitLiftableObject) {
			isHitLiftableObject = false;
		}

		if (col.gameObject.tag == Constants.Tag.CHEST && isHitChestObject) {
			isHitChestObject = false;
		}
	}

	void OnTriggerEnter (Collider col) {		
		if (col.tag == Constants.Tag.DIG_AREA) {
			isCanDigging = true;
		} 

		if (col.tag == Constants.Tag.ENEMY_ATTACK) {
			Enemy enemy = col.GetComponentInParent<Enemy>();
			enemyThatHitsPlayer = enemy;
		}
	}

	void OnTriggerExit (Collider col) {
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