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
	public GameObject shovelAreaEffectObj;
	public GameObject lanternObj;
	public GameObject invisibleCloak;
	public GameObject powerBraceletAreaEffectObj;
	public GameObject magicMedallionAreaEffectObj;
	public GameObject bootsObj;
	// public Boots bootsScriptObj;


	public Text textToolName;

	public int currentActiveContainer;
	public float dashSpeed = 500f;
	// public bool isUsingTool = false;

	[SerializeField] bool isActToolReady = false;
	[SerializeField] bool isPowerBraceletSelected = false;

	void Start()
	{
		Bow = 1;
		Hook = 1;
		Bomb = 1;
		Hammer = 1;
		Net = 1;
		FishingRod = 1;
		Container1 = 1;
		Container2 = 1;
		Container3 = 1;
		Container4 = 1;
		Shovel = 1;
		Lantern = 1;
		InvisibilityCloak = 1;
		MagicMedallion = 1;
		FastTravel = 1;
		Flippers = 1;
		Boots = 1;
	}

	void Awake () {
		textToolName.text = currentTool.ToString();
	}

	public void SpawnSlashEffect (int toolType) {
        switch (toolType) {
            case 1:
                SpawnObj (arrowObj, false, false);
                break;
            case 2:
                SpawnObj (hookObj, false, false);
                break;
            case 3:
                SpawnObj (bombObj, false, true);
                break;
            case 4:
                SpawnObj (hammerAreaEffectObj, false, false);
                break;
            case 5:
                SpawnObj (netObj, false, false);
                break;
            case 11:
                SpawnObj (shovelAreaEffectObj, false, false);
                break;
            case 12:
                SpawnObj (lanternObj, false, false);
                break;
            case 14:
                SpawnObj (magicMedallionAreaEffectObj, true, true);
                break;
            case 16:
                // SpawnObj (powerBraceletAreaEffectObj, false, false);
                break;
            case 18:
                // SetEquipmentTools (bootsScriptObj);
                break;
        }
    }

    void SpawnObj (GameObject obj, bool isSpawnAtPlayerPos, bool isAlwaysUp) {
        GameObject spawnedBullet = Instantiate(obj, areaSpawnPos.position, SetFacing());
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY
		
		if (isSpawnAtPlayerPos) {
			spawnedBullet.transform.position = transform.root.position;
		}

		if (isAlwaysUp) {
			spawnedBullet.transform.eulerAngles = Vector3.zero;
		}

        spawnedBullet.SetActive(true);
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
            case 18:
                return bootsObj;
			default:
				Debug.Log("Unknown Tool Object");
				return null;
        }
    }

    Quaternion SetFacing () {
        Vector2 targetPos = areaSpawnPos.position;
        Vector2 initPos = transform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.y -= initPos.y;
        float angle = Mathf.Atan2 (targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, 0f, angle));

        return targetRot;
    }

	// void SetEquipmentTools (Boots boots) {
	// 	dashSpeed = boots.bootsSpeed;
	// 	Debug.Log("Use Boots with " + dashSpeed + " speed");
	// }

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
