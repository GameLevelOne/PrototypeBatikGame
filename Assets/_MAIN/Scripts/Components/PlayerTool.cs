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
}
