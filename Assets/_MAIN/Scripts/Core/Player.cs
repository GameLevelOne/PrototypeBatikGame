using UnityEngine;

public enum PlayerState {
	IDLE, 
	MOVE,
	ATTACK,
	CHARGE,
	COUNTER,
	DIE,
	USING_TOOL,
	HIT,
	GET_HURT,
	RAPID_SLASH,
	SLOW_MOTION,
	HOOK,
	BLOCK_ATTACK,
	DODGE,
	DASH,
	BOUNCE,
	BRAKE,
	HURT_MOVE,
	POWER_BRACELET,
	SWIM,
	FISHING
}

public class Player : MonoBehaviour {
	public PlayerState state;
	public Enemy enemyThatHitsPlayer;
	// public PlayerTool playerTool;
	public Collider2D playerCol;
	    
    [SerializeField] bool isPlayerHit = false; 
    [SerializeField] bool isHitAnEnemy = false;
	[SerializeField] bool isGuarding = false; 
	[SerializeField] bool isParrying = false; 
	[SerializeField] bool isBulletTiming = false;
	[SerializeField] bool isCanDigging = false;
	[SerializeField] bool isInvisible = false;
	[SerializeField] bool isHitLiftableObject = false;
	[SerializeField] bool isCanFishing = false;

	public bool IsPlayerHit {
		get {return isPlayerHit;}
		set {
			if (isPlayerHit == value) return;

			isPlayerHit = value;
		}
	}

	public bool IsHitAnEnemy {
		get {return isHitAnEnemy;}
		set {
			if (isHitAnEnemy == value) return;

			isHitAnEnemy = value;
		}
	}
	
	public bool IsGuarding {
		get {return isGuarding;}
		set {
			if (isGuarding == value) return;

			isGuarding = value;
		}
	}
	
	public bool IsParrying {
		get {return isParrying;}
		set {
			if (isParrying == value) return;

			isParrying = value;
		}
	}
	
	public bool IsBulletTiming {
		get {return isBulletTiming;}
		set {
			if (isBulletTiming == value) return;

			isBulletTiming = value;
		}
	}

	public bool IsCanDigging {
		get {return isCanDigging;}
		set {
			if (isCanDigging == value) return;

			isCanDigging = value;
		}
	}

	public bool IsInvisible {
		get {return isInvisible;}
		set {
			if (isInvisible == value) return;

			isInvisible = value;
		}
	}
	
	public bool IsHitLiftableObject {
		get {return isHitLiftableObject;}
		set {
			if (isHitLiftableObject == value) return;

			isHitLiftableObject = value;
		}
	}
	
	public bool IsCanFishing {
		get {return isCanFishing;}
		set {
			if (isCanFishing == value) return;

			isCanFishing = value;
		}
	}

    public int MaxHP{
        get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
        set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
    }

	void OnCollisionEnter2D (Collision2D col) {

		if (state == PlayerState.DASH) {
			SetPlayerState (PlayerState.BOUNCE);	
		}
		
		if (col.gameObject.GetComponent<Liftable>() != null && !IsHitLiftableObject) {
			IsHitLiftableObject = true;
		}
	}

	void OnCollisionExit2D (Collision2D col) {

		if (col.gameObject.GetComponent<Liftable>() != null && IsHitLiftableObject) {
			IsHitLiftableObject = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.DIG_AREA) {
			IsCanDigging = true;
		} 
	}

	void OnTriggerExit2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.DIG_AREA) {
			IsCanDigging = false;
		}
	}
	
	#region PLAYER STATE 
	public void SetPlayerState (PlayerState playerState) {
		if (playerState == PlayerState.MOVE) Debug.Log("Move");
		state = playerState;
	}

	public void SetPlayerIdle () {
		Debug.Log("Idle");
		state = PlayerState.IDLE;
	}
	#endregion
}