﻿using UnityEngine;

public class Flippers : MonoBehaviour {
	// public Collider2D waterCol;
	public Player player;
	public PlayerInput input;
	public Water water;
	
	public Collider waterBoundariesCol;
	public bool isEquipped = false;
	public bool isPlayerOnWater = false;
	public bool isPlayerSwimming = false;
	
    bool isSetPlayerSwim = false;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.SWIM_AREA && !isPlayerOnWater) {
			isPlayerOnWater = true;
			water = col.GetComponent<Water>();
			waterBoundariesCol = water.waterBoundariesCol;
		}

		if (!isSetPlayerSwim) {
			if (col == waterBoundariesCol) {
				isPlayerSwimming = true;
				isSetPlayerSwim = true;
				input.interactValue = 0;
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col == waterBoundariesCol) {
			isPlayerSwimming = false;
			isSetPlayerSwim = false;
			input.interactValue = 2;
		}

		if (col.tag == Constants.Tag.SWIM_AREA) {
			isPlayerOnWater = false;
			water = null;
			waterBoundariesCol = null;
		}
	}
}
