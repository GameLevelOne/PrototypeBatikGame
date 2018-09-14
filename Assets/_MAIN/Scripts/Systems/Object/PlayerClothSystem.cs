using UnityEngine;
using Unity.Entities;

public class PlayerClothSystem : ComponentSystem {
	public struct PlayerClothData {
		public readonly int Length;
		public ComponentArray<PlayerCloth> PlayerCloth;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] PlayerClothData playerClothData;

	PlayerCloth playerCloth;
	Facing2D facing;

	Transform clothTransform;

	bool isReducingCloth = false;

	float playerHP;
	float maxHP;
	float clothReduceValue;
	float currentDissolveValue;
	float healthThreshold;

	protected override void OnUpdate () {
		if (playerClothData.Length == 0) return;

		for (int i=0; i<playerClothData.Length; i++) {
			playerCloth = playerClothData.PlayerCloth[i];
			clothTransform = playerClothData.Transform[i];
			
			if (!playerCloth.isInitCLoth) {
				InitCloth ();
			} else {
				CheckHP ();				
				CheckDirection ();
			}
		}
	}

	void InitCloth () {
		maxHP = playerCloth.player.MaxHP;
		clothReduceValue = playerCloth.clothReduceValue;
		
		// healthThreshold = currentDissolveValue; //OLD
		healthThreshold = playerCloth.clothRenderer.material.GetFloat("_Level");

		clothReduceValue = playerCloth.clothReduceValue;

		// playerCloth.cloth.ClearTransformMotion();

		DissolveCloth();
		DrawClothHP ();
		
		SetClothRotation(0f, Vector3.forward);
		playerCloth.isInitCLoth = true; 
	}

	void CheckHP () {
		if (playerCloth.isHPChange) {
			DissolveCloth();
		} else if (isReducingCloth) {
			if (healthThreshold < currentDissolveValue) {
				healthThreshold += clothReduceValue * Time.deltaTime;
				DrawClothHP();
			} else {
				isReducingCloth = false;
			}
		}
	}

	void DissolveCloth () {
		playerHP = playerCloth.playerHealth.PlayerHP;
		currentDissolveValue = 1 - playerHP / maxHP;
		// currentDissolveValue = playerCloth.clothRenderer.material.GetFloat("_Level");
		isReducingCloth = true;
		playerCloth.isHPChange = false;
	}

	void DrawClothHP () {
		playerCloth.clothRenderer.material.SetFloat("_Level", healthThreshold);
		// Debug.Log(healthThreshold);
	}

	void CheckDirection () {
		facing = playerCloth.facing;
		// Debug.Log(playerCloth.currentDirID+" : "+facing.DirID);
		if (playerCloth.currentDirID != facing.DirID) {
			playerCloth.currentDirID = facing.DirID;
			int currentDirID = playerCloth.currentDirID;

			switch (currentDirID) {
				case 1: //DOWN
					SetClothRotation(0f, new Vector3(2f, -1f, 2f)); 
					break;
				case 2: //LEFT
					SetClothRotation(90f, new Vector3(1f, -0.5f, -1f)); 
					break;
				case 3: //UP
					SetClothRotation(180f, new Vector3(-1f, -0.5f, -1f)); 
					break;
				case 4: //RIGHT
					SetClothRotation(-90f, new Vector3(-2f, -1f, 2f)); 
					break;
			}
		}
	}

	void SetClothRotation (float rotY, Vector3 accel) {
		// accel.y = -1;

		// Debug.Log("InverseTransformDirection "+playerCloth.cloth.transform.InverseTransformDirection(accel));
		// Debug.Log("InverseTransformPoint "+playerCloth.cloth.transform.InverseTransformPoint(accel));
		// Debug.Log("InverseTransformVector "+playerCloth.cloth.transform.InverseTransformVector(accel));
		// playerCloth.cloth.ClearTransformMotion();
		// clothTransform.localEulerAngles = new Vector3(0f, rotY, 0f);	
		playerCloth.cloth.externalAcceleration = accel * playerCloth.addOnAccel;	
	}
}
