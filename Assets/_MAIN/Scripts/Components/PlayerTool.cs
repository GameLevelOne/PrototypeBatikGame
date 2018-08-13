using UnityEngine;
using UnityEngine.UI;

public enum ToolType{
	None = 0,
	Bow,
	Hook,
	Bomb,
	Hammer,
	Net,
	FishingRod,
	Container1,
	Container2,
	Container3,
	Container4,
	Shovel,
	Lantern,
	InvisibilityCloak,
	MagicMedallion,
	FastTravel,
	PowerBracelet,
	Flippers,
	Boots,
}

public class PlayerTool : MonoBehaviour {
	/// <summary>
    /// Indicates current tool
    /// </summary>
	public ToolType currentTool;

	public Transform areaSpawnPos;
	
	public GameObject arrowObj;
	public GameObject hookObj;
	public GameObject bombObj;
	public GameObject hammerAreaEffectObj;
	public GameObject netObj;
	public GameObject fishingBaitObj;
	public GameObject shovelAreaEffectObj;
	public GameObject lanternObj;
	public GameObject invisibleCloak;
	public GameObject magicMedallionAreaEffectObj;
	public GameObject powerBraceletAreaEffectObj;
	public GameObject flippersObj;
	public GameObject bootsObj;

	// public GameObject regularArrow;

	public Text textToolName;

	public int currentActiveContainer;
	public float dashSpeed = 500f;

	[SerializeField] bool isActToolReady = false;
	[SerializeField] bool isPowerBraceletSelected = false;
	// [SerializeField] bool isFlipperSelected = false;

	void Start()
	{	
		Bow = 1;
		Hook = 0; //
		Bomb = 1;
		Hammer = 1;
		Net = 0; //
		FishingRod = 1;
		Container1 = 1;
		Container2 = 1;
		Container3 = 1;
		Container4 = 1;
		Shovel = 1;
		Lantern = 0; //
		InvisibilityCloak = 0; //
		MagicMedallion = 1;
		FastTravel = 0; //
		Flippers = 0; //
		Boots = 1;
	}

	void Awake () {
		textToolName.text = currentTool.ToString();
	}

	public GameObject GetObj (int toolType) {
        switch (toolType) {
            case 1:
                return arrowObj;
            case 2:
                return hookObj;
            case 3:
                return bombObj;
            case 4:
                return hammerAreaEffectObj;
            case 5:
                return netObj;
            case 6:
                return fishingBaitObj;
            case 11:
                return shovelAreaEffectObj;
            case 12:
                return lanternObj;
            case 13:
                return invisibleCloak;
            case 14:
                return magicMedallionAreaEffectObj;
            case 16:
                return powerBraceletAreaEffectObj;
            case 17:
                return flippersObj;
            case 18:
                return bootsObj;
			default:
				Debug.Log("Unknown Tool Object");
				return null;
        }
    }

	public int CheckIfToolHasBeenUnlocked (int toolType) {
		switch (toolType) {
            case 1:
                return Bow;
            case 2:
                return Hook;
            case 3:
                return Bomb;
            case 4:
                return Hammer;
            case 5:
                return Net;
            case 6:
                return FishingRod;
            case 7:
                return Container1;
            case 8:
                return Container2;
            case 9:
                return Container3;
            case 10:
                return Container4;
            case 11:
                return Shovel;
            case 12:
                return Lantern;
            case 13:
                return InvisibilityCloak;
            case 14:
                return MagicMedallion;
            case 15:
                return FastTravel;
            case 16:
                return PowerBracelet;
            case 17:
                return Flippers;
            case 18:
                return Boots;
			default:
				Debug.Log("You have no tools");
				return 0;
        }
	}

	public bool IsActToolReady {
        get {return isActToolReady;}
		set {
			if (isActToolReady == value) return;

			isActToolReady = value;
		}
    }

	public bool IsPowerBraceletSelected {
        get {return isPowerBraceletSelected;}
		set {
			if (isPowerBraceletSelected == value) return;

			isPowerBraceletSelected = value;
		}
    }

	// public bool IsFlipperSelected {
    //     get {return isPowerBraceletSelected;}
	// 	set {
	// 		if (isPowerBraceletSelected == value) return;

	// 		isPowerBraceletSelected = value;
	// 	}
    // }

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Bow
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOW,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOW,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Hook
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HOOK,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HOOK,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Bomb
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOMB,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOMB,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Hammer
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HAMMER,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HAMMER,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Net
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_NET,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_NET,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int FishingRod
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FISHINGROD,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FISHINGROD,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked, empty<br /></para>
	/// <para>\>1: contained something</para>
    /// </summary>
	public int Container1
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER1,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER1,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked, empty<br /></para>
	/// <para>\>1: contained something</para>
    /// </summary>
	public int Container2
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER2,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER2,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked, empty<br /></para>
	/// <para>\>1: contained something</para>
    /// </summary>
	public int Container3
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER3,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER3,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked, empty<br /></para>
	/// <para>\>1: contained something</para>
    /// </summary>
	public int Container4
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER4,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER4,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Shovel
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_SHOVEL,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_SHOVEL,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Lantern
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_LANTERN,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_LANTERN,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int InvisibilityCloak
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_INVISIBILITYCLOAK,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_INVISIBILITYCLOAK,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int MagicMedallion
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_MAGICMEDALLION,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_MAGICMEDALLION,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int FastTravel
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FASTTRAVEL,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FASTTRAVEL,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int PowerBracelet
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_POWERBRACELET,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_POWERBRACELET,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Flippers
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FLIPPERS,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FLIPPERS,value);}
	}

	/// <summary>
    /// <para>Value:<br /></para>
	/// <para>0: locked<br /></para>
	/// <para>1: unlocked<br /></para>
	/// <para>\>1: upgraded</para>
    /// </summary>
	public int Boots
	{
		get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOOTS,0);}
		set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOOTS,value);}
	}
}
