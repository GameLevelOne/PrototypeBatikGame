using UnityEngine;

public class Player : MonoBehaviour {
	    
    public class Stats
    {
       public int MaxHP{
           get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP);}
           set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_MAXHP,value);}
       }
    }
}