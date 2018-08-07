using UnityEngine;

public class Flippers : MonoBehaviour {
	// public Collider2D waterCol;
	public Player player;
	public PlayerInput input;
	public Water water;
	
	public Collider2D waterBoundariesCol;
	
    bool isSetPlayerSwim = false;
	[SerializeField] bool isPlayerOnWater = false;
	[SerializeField] bool isEquipped = false;
	[SerializeField] bool isPlayerSwimming = false;

	public bool IsEquipped {
		get {return isEquipped;}
		set {
			if (isEquipped == value) return;

			isEquipped = value;
		}
	}

	public bool IsPlayerOnWater {
		get {return isPlayerOnWater;}
		set {
			if (isPlayerOnWater == value) return;

			isPlayerOnWater = value;
		}
	}

	public bool IsPlayerSwimming {
		get {return isPlayerSwimming;}
		set {
			if (isPlayerSwimming == value) return;

			isPlayerSwimming = value;
		}
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.tag == Constants.Tag.SWIM_AREA && !isPlayerOnWater) {
			IsPlayerOnWater = true;
			water = col.GetComponent<Water>();
			waterBoundariesCol = water.waterBoundariesCol;
		}

		if (!isSetPlayerSwim) {
			if (col == waterBoundariesCol) {
				IsPlayerSwimming = true;
				isSetPlayerSwim = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col == waterBoundariesCol) {
			IsPlayerSwimming = false;
			isSetPlayerSwim = false;
		}

		if (col.tag == Constants.Tag.SWIM_AREA) {
			IsPlayerOnWater = false;
			water = null;
			waterBoundariesCol = null;
		}
	}
}
