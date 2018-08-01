using UnityEngine;

public enum PlayerState {
	IDLE, 
	MOVE,
	DODGE,
	ATTACK,
	CHARGE,
	COUNTER,
	DIE,
	USING_TOOL,
	HIT,
	GET_HURT,
	BLOCK_ATTACK,
	RAPID_SLASH,
	SLOW_MOTION,
	HOOK,
	DASH,
	BOUNCE,
	BRAKE
}

public class Player : MonoBehaviour {
	public PlayerState state;
	public Enemy enemyThatHitsPlayer;
	// public PlayerTool playerTool;
	    
    [SerializeField] bool isPlayerHit = false; 
    [SerializeField] bool isHitAnEnemy = false;
	[SerializeField] bool isGuarding = false; 
	[SerializeField] bool isParrying = false; 
	[SerializeField] bool isBulletTiming = false;
	[SerializeField] bool isCanDigging = false;

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

    public int MaxHP{
        get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
        set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
    }

	public bool IsCanDigging {
		get {return isCanDigging;}
		set {
			if (isCanDigging == value) return;

			isCanDigging = value;
		}
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (state == PlayerState.DASH) {
			SetPlayerState (PlayerState.BOUNCE);	
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.DIG_AREA) {
			Debug.Log("Can Digging");
			
			IsCanDigging = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		
		if (col.tag == Constants.Tag.DIG_AREA) {
			Debug.Log("Can't Digging");
			
			IsCanDigging = false;
		}
	}
	
	#region PLAYER STATE 
	public void SetPlayerState (PlayerState playerState) {
		state = playerState;
	}

	public void SetPlayerIdle () {
		Debug.Log("Idle");
		state = PlayerState.IDLE;
	}
	#endregion
}