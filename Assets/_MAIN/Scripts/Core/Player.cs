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
	GUARD,
	PARRY,
	RAPID_SLASH,
	SLOW_MOTION,
	DASH
}

public class Player : MonoBehaviour {
	public PlayerState playerState;
	public Enemy enemyThatHitsPlayer;
	public PlayerTool playerTool;
	    
    [SerializeField] bool isPlayerHit = false; //
    [SerializeField] bool isPlayerGetHurt = false; //
    [SerializeField] bool isPlayerDie = false; //
    [SerializeField] bool isHitAnEnemy = false;
	[SerializeField] bool isGuarding = false; //
	[SerializeField] bool isParrying = false; //
	[SerializeField] bool isBulletTiming = false;
	[SerializeField] bool isRapidSlashing = false; //
	[SerializeField] bool isSlowMotion = false; //
	[SerializeField] bool isHooking = false; //
	[SerializeField] bool isCanDigging = false;
	[SerializeField] bool isDashing = false; //

	public bool IsPlayerHit {
		get {return isPlayerHit;}
		set {
			if (isPlayerHit == value) return;

			isPlayerHit = value;
		}
	}

	public bool IsPlayerGetHurt {
		get {return isPlayerGetHurt;}
		set {
			if (isPlayerGetHurt == value) return;

			isPlayerGetHurt = value;
		}
	}

	public bool IsPlayerDie {
		get {return isPlayerDie;}
		set {
			if (isPlayerDie == value) return;

			isPlayerDie = value;
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

	public bool IsSlowMotion {
		get {return isSlowMotion;}
		set {
			if (isSlowMotion == value) return;

			isSlowMotion = value;
		}
	}

	public bool IsRapidSlashing {
		get {return isRapidSlashing;}
		set {
			if (isRapidSlashing == value) return;

			isRapidSlashing = value;
		}
	}

	public bool IsHooking {
		get {return isHooking;}
		set {
			if (isHooking == value) return;

			isHooking = value;
		}
	}

	public bool IsDashing {
		get {return isDashing;}
		set {
			if (isDashing == value) return;

			isDashing = value;
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
}