using UnityEngine;

public enum ChestType {
	NORMAL,
	TREASURE,
}

public class Chest : MonoBehaviour {
	public ChestType chestType;
	
	public GameObject[] normalPrizes;
	public GameObject treasurePrize;

	public bool isOpened;
	public bool isSelected;
}
