using UnityEngine;

public enum ChestType {
	NORMAL,
	TREASURE,
}

public class Chest : MonoBehaviour {
	public ChestType chestType;
	
	public Animator chestAnimator;
	public GameObject treasurePrize;
	public GameObject[] normalPrizes;

	public bool isOpened;
	public bool isSelected;
}
