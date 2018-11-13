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
	public UIInteractionHint uiInteractionHint;
	// public Transform playerWeaponPos;
	public Collider playerCol;
	// public UIGameOver uiGameOver;   <---- CHANGE TO INJECTATTRIBUTE IN PLAYERMOVEMENTSYSTEM
	public GameObject playerAttackAreaObj;
	public GameObject playerCounterTrigger;
	public GameObject playerParryTrigger;
	public Transform liftingParent;
	public float shieldPower;
    public int requiredBowMana;
	
	[HeaderAttribute("Current")]
	public TerrainType terrainType; 
	public Transform somethingThatHitsPlayer;
	public PlayerCounterTrigger currentCounterTrigger;
	public PlayerParryTrigger currentParryTrigger;
	// public PlayerTool playerTool;
    public bool isInitPlayer = false; 

	[SpaceAttribute(10f)]
	public Damage damageReceive;
    public bool isPlayerHit = false; 
    // public bool isHitAnEnemy = false;
	// public bool isPlayerGetHurt = false;
	public bool isPlayerKnockedBack = false;
	public bool isGuarding = false; 
	// public bool isParrying = false; 
	public bool isCanParry = false;
	public bool isUsingStand = false; //By Mana  
	public bool isBouncing = false;
	public bool isMoveAttack = false;
	public bool isCanBulletTime = false;
	// public bool isOnParryPeriod = false;
    // public bool isAfterParry = false; 
	// public bool isInvisible = false;
	public Vector3 counterPos = Vector3.zero;
	public int counterDir = 0;
	public Vector3 parryPos = Vector3.zero;
	public int parryDir = 0;
	
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
	public bool isInitRapidSlash = false;
	public bool isCanShowHint = true;
	
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
			
		if (currentParryTrigger != null)
			currentParryTrigger.OnParryTrigger -= OnParryTrigger;
	}

	void DamageCheck (Damage damage) {
		damageReceive = damage;
		DamageCharacteristic damageChar = damage.damageChar;

		// if (damageChar == DamageCharacteristic.PARRYABLE || damageChar == DamageCharacteristic.COUNTER_AND_PARRYABLE) {
		// 	if (isGuarding) {
		// 		// isCanParry = true;
		// 	// } else {
		// 		isPlayerHit = true;
		// 		// isCanParry = false;
		// 	} 
		// } else {
			isPlayerHit = true;
		// }
	}

	void OnCounterTrigger () {
		if (state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
			isCanBulletTime = true;
			 // Debug.Log("isCanBulletTime");
			//  // Debug.Log("Set isOnBulletTimePeriod TRUE");
		}
	}

	void OnParryTrigger () {
		if (state != PlayerState.BLOCK_ATTACK && state != PlayerState.PARRY) {
			isCanParry = true;
			 // Debug.Log("isCanBulletTime");
			//  // Debug.Log("Set isOnBulletTimePeriod TRUE");
		}
	}

	public void ReferenceCounterTrigger () {
		currentCounterTrigger.OnCounterTrigger += OnCounterTrigger;
	}

	public void ReferenceParryTrigger () {
		currentParryTrigger.OnParryTrigger += OnParryTrigger;
	}

    public float MaxHP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, maxHP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP, value);
			//  // Debug.Log("Debug MaxHP : "+MaxHP);
		}
    }

    public float MaxMP{
        get{return PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMP, maxMP);}
        set{PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MAXMP, value);
			//  // Debug.Log("Debug MaxMP : "+MaxMP);
		}
    }

	void OnCollisionEnter (Collision col) {
		if (state == PlayerState.DASH && !isBouncing) {
			//  // Debug.Log(col.gameObject.name);
			isBouncing = true;	
		}
		
		// if (col.gameObject.tag == Constants.Tag.LIFTABLE) {
			if (!isHitLiftableObject) {
				if (col.gameObject.GetComponent<Liftable>() != null) {
					//  // Debug.Log("Parent");
					isHitLiftableObject = true;
				} else if (col.gameObject.GetComponentInParent<Liftable>() != null) {
					//  // Debug.Log("Child");
					isHitLiftableObject = true;
				}
			}
		// }

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
		// if (col.gameObject.tag == Constants.Tag.LIFTABLE) {
			if (isHitLiftableObject) {
				if (col.gameObject.GetComponent<Liftable>() != null) {
					isHitLiftableObject = false;
				} else if (col.gameObject.GetComponentInParent<Liftable>() != null) {
					isHitLiftableObject = false;
				}
			}
		// }

		if (col.gameObject.tag == Constants.Tag.CHEST && isHitChestObject) {
			isHitChestObject = false;
		}

		if (col.gameObject.tag == Constants.Tag.GATE && isHitGateObject) {
			isHitGateObject = false;
		}
	}

	void OnTriggerEnter (Collider col) {	
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
		} 
		// else if (col.tag == Constants.Tag.JATAYU_ATTACK_1 || col.tag == Constants.Tag.JATAYU_ATTACK_1) {
		// 	somethingThatHitsPlayer = col.transform;
		// 	isHitJatayuAttack2 = true;
		// }	

		// if (col.tag == Constants.Tag.DIG_AREA) {
		// 	isCanDigging = true;
		// } 
		// if (col.tag == Constants.Tag.LIFTABLE) {
			if (!isHitLiftableObject) {
				if (col.GetComponent<Liftable>() != null) {
					//  // Debug.Log("Parent");
					isHitLiftableObject = true;
				} else if (col.GetComponentInParent<Liftable>() != null) {
					//  // Debug.Log("Child");
					isHitLiftableObject = true;
				}
			}
		// }
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.JATAYU_ATTACK_2) {
			isHitJatayuAttack2 = false;
		}
		
		// if (col.tag == Constants.Tag.DIG_AREA) {
		// 	isCanDigging = false;
		// }

		// if (col.tag == Constants.Tag.LIFTABLE) {
			if (isHitLiftableObject) {
				if (col.GetComponent<Liftable>() != null) {
					//  // Debug.Log("Parent");
					isHitLiftableObject = true;
				} else if (col.GetComponentInParent<Liftable>() != null) {
					//  // Debug.Log("Child");
					isHitLiftableObject = true;
				}
			}
		// }
	}
	
	#region PLAYER STATE 
	public void SetPlayerState (PlayerState playerState) {
		// if (playerState == PlayerState.MOVE)  // Debug.Log("Move");
		// if (playerState == PlayerState.RAPID_SLASH)  // Debug.Log("RAPID_SLASH");
		state = playerState;
	}

	public void SetPlayerIdle () {
		//  // Debug.Log("Idle");
		state = PlayerState.IDLE;
	}
	#endregion

	//SET UI INTERACTION HINT
	public void ShowInteractionHint (HintMessage message) {
		if (isCanShowHint) {
			uiInteractionHint.ShowHint(message);
			// isCanShowHint = false;
		}
	}

	public void HideHint () {
		// if (!isCanShowHint) {
			uiInteractionHint.HideHint();
			// isCanShowHint = true;
		// }
	}

	public void ResetHint (bool value) {
		isCanShowHint = value;
	}
}