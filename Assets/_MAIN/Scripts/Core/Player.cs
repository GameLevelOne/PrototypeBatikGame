using UnityEngine;

public class Player : MonoBehaviour {
	public Enemy enemyThatHitsPlayer;
	    
    [SerializeField] bool currentIsPlayerHit = false;
    [SerializeField] bool currentIsHitAnEnemy = false;
	[SerializeField] bool currentIsGuarding = false;
	[SerializeField] bool currentIsParrying = false;
	[SerializeField] bool currentIsBulletTiming = false;
	[SerializeField] bool currentIsRapidSlashing = false;
	[SerializeField] bool currentIsSlowMotion = false;

	public bool isPlayerHit {
		get {return currentIsPlayerHit;}
		set {
			if (currentIsPlayerHit == value) return;

			currentIsPlayerHit = value;
			Debug.Log("currentIsPlayerHit " + currentIsPlayerHit);
		}
	}

	public bool isHitAnEnemy {
		get {return currentIsHitAnEnemy;}
		set {
			if (currentIsHitAnEnemy == value) return;

			currentIsHitAnEnemy = value;
			Debug.Log("currentIsPlayerHit " + currentIsHitAnEnemy);
		}
	}
	
	public bool isGuarding {
		get {return currentIsGuarding;}
		set {
			if (currentIsGuarding == value) return;

			currentIsGuarding = value;
		}
	}
	
	public bool isParrying {
		get {return currentIsParrying;}
		set {
			if (currentIsParrying == value) return;

			currentIsParrying = value;
		}
	}
	
	public bool isBulletTiming {
		get {return currentIsBulletTiming;}
		set {
			if (currentIsBulletTiming == value) return;

			currentIsBulletTiming = value;
		}
	}

	public bool isSlowMotion {
		get {return currentIsSlowMotion;}
		set {
			if (currentIsSlowMotion == value) return;

			currentIsSlowMotion = value;
		}
	}

	public bool isRapidSlashing {
		get {return currentIsRapidSlashing;}
		set {
			if (currentIsRapidSlashing == value) return;

			currentIsRapidSlashing = value;
		}
	}

    public int MaxHP{
        get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
        set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
    }
}