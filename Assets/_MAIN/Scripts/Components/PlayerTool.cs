using UnityEngine;

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
    /// Indicates standby tool selected by player
    /// </summary>
	public ToolType currentTool;

	public GameObject hammerAreaEffectObj;
	public GameObject shovelAreaEffectObj;
	public GameObject bootsAreaEffectObj;
	public GameObject magicMedallionAreaEffectObj;
	public GameObject powerBraceletAreaEffectObj;

	public int currentActiveContainer;

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
