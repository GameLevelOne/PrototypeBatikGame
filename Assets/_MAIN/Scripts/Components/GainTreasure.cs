﻿using UnityEngine;

public class GainTreasure : MonoBehaviour {
	public Player player;
	public Transform liftingTreasureParent;

	public Lootable lootable;
	public Transform lootableTransform;

	public bool isLooting = false;
	
	void OnTriggerEnter (Collider col) {
		PlayerState state = player.state;
		Debug.Log(col.name);

		if (col.tag == Constants.Tag.LOOTABLE && !isLooting && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
			lootable = col.GetComponent<Lootable>();

			if (!lootable.isLooted) {
				lootable.isLooted = true;
				lootableTransform = lootable.transform;
				isLooting = true;
			}
		} 
	}
}
