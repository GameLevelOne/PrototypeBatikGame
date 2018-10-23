﻿using UnityEngine;
using UnityEngine.UI;

public enum FishingRodState {
	IDLE,
	THROW,
	STAY,
	RETURN,
}

public class FishingRod : MonoBehaviour {
	public FishingRodState state;
	public Player player;
	public AnimationControl animationControl;

	public GameObject fishingBuoyObj;
	public GameObject fishingBuoySplashObj;
	public Collider baitCol;
	public float fishingRange;
	public Text debugFishingRodText;

	
	[HeaderAttribute("Current")]
	public bool isInitFishingRod = false;
	public Fish fish;
	public GameObject fishObj;
	public bool isBaitLaunched = false;
	public bool isCatchSomething = false;
	public bool isBaitFish = false;

	FishState fishState;

	void OnEnable()
	{
		animationControl.OnExitAnimation += DisableSplash;
	}

	void OnDisable()
	{
		animationControl.OnExitAnimation -= DisableSplash;
	}

	void OnTriggerEnter (Collider col) {
		// Debug.Log(col.tag);
		
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.isCanFishing = true;
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.FISH) {
			if (!isBaitLaunched || player.state != PlayerState.FISHING) return;

			if (fish == null) {
				fish = col.GetComponent<Fish>();
			} else {
				fishState = fish.state;
				// Debug.Log("Fish state : "+fishState+" , isBaitFish : "+isBaitFish);
				if ((fishState == FishState.IDLE || fishState == FishState.PATROL) && !isBaitFish) {
					// Debug.Log("Get Fish");
					
					fishObj = fish.gameObject;

					fish.fishingRod = this;
					fish.targetPos = this.transform.position;
					fish.state = FishState.THINK;
					// fish.state = FishState.CHASE;
					// Debug.Log("Set Chase"); 

					isBaitFish = true;
				} else if (fishState == FishState.FLEE && isBaitFish) {
					// Debug.Log("Flee");
					fishObj = null;
					fish.fishingRod = null;
					fish = null;
					isBaitFish = false;
				}
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.FISHING_AREA) {
			player.isCanFishing = false;
		}
	}

	void DisableSplash () {
		fishingBuoySplashObj.SetActive(false);
	}
}
