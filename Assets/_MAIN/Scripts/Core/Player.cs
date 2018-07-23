using UnityEngine;

public class Player : MonoBehaviour {
	    
    [SerializeField] bool currentIsPlayerHit = false;
    [SerializeField] bool currentIsHitAnEnemy = false;
	[SerializeField] bool currentIsGuarding = false;
	[SerializeField] bool currentIsParrying = false;

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

    public int MaxHP{
        get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
        set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
    }
}