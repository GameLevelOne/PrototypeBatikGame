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
	OPEN_CHEST,
	ENGAGE
}

public class Player : MonoBehaviour {
	[HeaderAttribute("Player Attributes")]
	public Health health;
	public Mana mana;
	public PlayerState state;
	// public Transform playerWeaponPos;
	public Collider playerCol;
	// public UIGameOver uiGameOver;   <---- CHANGE TO INJECTATTRIBUTE IN PLAYERMOVEMENTSYSTEM
	public GameObject playerAttackAreaObj;
	public GameObject playerCounterTrigger;
	public Transform liftingParent;
	public float shieldPower;
    public int requiredBowMana;
	
	[HeaderAttribute("Current")]
	public TerrainType terrainType; 
	public Transform somethingThatHitsPlayer;
	public PlayerCounterTrigger currentCounterTrigger;
	// public PlayerTool playerTool;
    public bool isInitPlayer = false; 

	[SpaceAttribute(10f)]
	public Damage damageReceive;
    public bool isPlayerHit = false; 
    // public bool isHitAnEnemy = false;
	// public bool isPlayerGetHurt = false;
	public bool isPlayerKnockedBack = false;
	public bool isGuarding = false; 
	public bool isCanParry = false;
	public bool isUsingStand = false; //By Mana  
	public bool isBouncing = false;
	public bool isMoveAttack = false;
	// public bool isCanBulletTime = false;
	public bool isOnBulletTimePeriod = false;
	// public bool isInvisible = false;
	public Vector3 counterPos = Vector3.zero;
	public int counterDir = 0;
	
	[SpaceAttribute(10f)]
	public bool isHitGateObject = false;
	public bool isCanOpenGate = false; 
	public bool isHitChestObject = false;
	public bool isCanOpenChest = false;   
	public bool isCanDigging = false;
	public bool isHitLiftableObject = false;
	public bool isCanFishing = false;
	public bool isCanInteractWithNPC = false;
	public bool isInteractingWithNPC = false;
	public bool isInitAttackAreaObj = false;
	
	//JATAYU
	public bool isHitJatayuAttack2 = false;

	public int interactIndex;
	
	[SerializeField] float maxHP;
	[SerializeField] float maxMP;

	void OnEnable () {
		health.OnDamageCheck += DamageCheck;
	}

	void OnDisable () {
		health.OnDamageCheck -= DamageCheck;
		
		if (currentCounterTrigger != null)
			currentCounterTrigger.OnCounterTrigger -= OnCounterTrigger;
	}

	void DamageCheck (Damage damage) {
		damageReceive = damage;

		switch (damage.damageChar) {
			// case DamageCharacteristic.COUNTERABLE:
			// 	isCanBulletTime = true;
			// 	break;
			case DamageCharacteristic.PARRYABLE:
				isCanParry = true;
				break;
			// case DamageCharacteristic.COUNTER_AND_PARRYABLE:
			// 	isCanBulletTime = true;
			// 	isCanParry = true;
			// 	break;
			default: // DamageCharacteristic.NONE
				// isCanBulletTime = false;
				isCanParry = false;
				isPlayerHit = true;
				break;
		}
		// Debug.Log("DamageCheck with damageReceive : "+damageReceive+", and isPlayerHit : "+isPlayerHit);
	}

	void OnCounterTrigger () {
		if (state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
			isOnBulletTimePeriod = true;
			// Debug.Log("Set isOnBulletTimePeriod TRUE");
		}
	}

	public void ReferenceCounterTrigger () {
		currentCounterTrigger.OnCounterTrigger += OnCounterTrigger;
	}

    public float MaxHP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, maxHP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, value);
			// Debug.Log("Debug MaxHP : "+MaxHP);
		}
    }

    public float MaxMP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMP, maxMP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMP, value);
			// Debug.Log("Debug MaxMP : "+MaxMP);
		}
    }

	void OnCollisionEnter (Collision col) {
		if (state == PlayerState.DASH && !isBouncing) {
			// Debug.Log(col.gameObject.name);
			isBouncing = true;	
		}
		
		if (col.gameObject.GetComponent<Liftable>() != null && !isHitLiftableObject) {
			isHitLiftableObject = true;
		}

		if (col.gameObject.tag == Constants.Tag.CHEST) {
			isHitChestObject = true;
		}

		if (col.gameObject.tag == Constants.Tag.GATE) {
			isHitGateObject = true;
		}

		if (col.gameObject.tag == Constants.Tag.ENEMY || col.gameObject.tag == Constants.Tag.BOSS) {
			somethingThatHitsPlayer = col.transform;
		}
	}

	void OnCollisionExit (Collision col) {
		if (col.gameObject.GetComponent<Liftable>() != null && isHitLiftableObject) {
			isHitLiftableObject = false;
		}

		if (col.gameObject.tag == Constants.Tag.CHEST && isHitChestObject) {
			isHitChestObject = false;
		}

		if (col.gameObject.tag == Constants.Tag.GATE && isHitGateObject) {
			isHitGateObject = false;
		}
	}

	void OnTriggerEnter (Collider col) {		
		if (col.tag == Constants.Tag.DIG_AREA) {
			isCanDigging = true;
		} 

		if (col.tag == Constants.Tag.ENEMY_ATTACK) {
			// Enemy enemy = col.GetComponentInParent<Enemy>();
			// enemyThatHitsPlayer = enemy;
			if (col.GetComponent<WaterShooterBullet>() != null) {
				somethingThatHitsPlayer = col.transform;
			} else {
				somethingThatHitsPlayer = col.transform.parent;
			}
		} else if (col.tag == Constants.Tag.VINES || col.tag == Constants.Tag.EXPLOSION) {
			somethingThatHitsPlayer = col.transform;
		} else if (col.tag == Constants.Tag.JATAYU_ATTACK_2) {
			isHitJatayuAttack2 = true;
		} else if (col.tag == Constants.Tag.JATAYU_ATTACK_1 || col.tag == Constants.Tag.JATAYU_ATTACK_1) {
			somethingThatHitsPlayer = col.transform;
			isHitJatayuAttack2 = true;
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.DIG_AREA) {
			isCanDigging = false;
		}

		if (col.gameObject.tag == Constants.Tag.JATAYU_ATTACK_2) {
			isHitJatayuAttack2 = false;
		}
	}
	
	#region PLAYER STATE 
	public void SetPlayerState (PlayerState playerState) {
		// if (playerState == PlayerState.MOVE) Debug.Log("Move");
		// if (playerState == PlayerState.RAPID_SLASH) Debug.Log("RAPID_SLASH");
		state = playerState;
	}

	public void SetPlayerIdle () {
		// Debug.Log("Idle");
		state = PlayerState.IDLE;
	}
	#endregion
}