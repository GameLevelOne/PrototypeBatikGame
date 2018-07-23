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
	public ToolType currentTool;
	public GameObject toolEffectAreaObj;

	public int Bow
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOW,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOW,value);}
            }

            public int Hook
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HOOK,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HOOK,value);}
            }

            public int Bomb
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOMB,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOMB,value);}
            }

            public int Hammer
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HAMMER,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_HAMMER,value);}
            }

            public int Net
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_NET,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_NET,value);}
            }

            public int FishingRod
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FISHINGROD,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FISHINGROD,value);}
            }

            public int Container1
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER1,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER1,value);}
            }

            public int Container2
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER2,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER2,value);}
            }

            public int Container3
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER3,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER3,value);}
            }

            public int Container4
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER4,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_CONTAINER4,value);}
            }

            public int Shovel
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_SHOVEL,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_SHOVEL,value);}
            }

            public int Lantern
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_LANTERN,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_LANTERN,value);}
            }

            public int InvisibilityCloak
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_INVISIBILITYCLOAK,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_INVISIBILITYCLOAK,value);}
            }

            public int MagicMedallion
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_MAGICMEDALLION,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_MAGICMEDALLION,value);}
            }

            public int FastTravel
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FASTTRAVEL,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FASTTRAVEL,value);}
            }

            public int PowerBracelet
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_POWERBRACELET,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_POWERBRACELET,value);}
            }

            public int Flippers
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FLIPPERS,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FLIPPERS,value);}
            }

            public int Boots
            {
                get{return PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOOTS,0);}
                set{PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOOTS,value);}
            }
}
