using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class PlayerClothSystem : ComponentSystem {
	public struct PlayerClothData {
		public readonly int Length;
		public ComponentArray<PlayerCloth> PlayerCloth;
		public ComponentArray<Transform> Transform;
	}
	[InjectAttribute] PlayerClothData playerClothData;

	PlayerCloth playerCloth;
	PlayerInput playerInput;
	Facing2D facing;
	Transform clothTransform;
	Rigidbody playerRigidbody;

	float maxHP;
	float clothReduceValue;
	float currentDissolveValue;
	float healthThreshold;
	float clothRandomCountdownTime;
	float deltaTime;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		
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
		healthThreshold = playerCloth.clothRenderer.material.GetFloat("_Level");

		// playerCloth.cloth.ClearTransformMotion();
		playerInput = playerCloth.playerInput;
		facing = playerCloth.facing;
		playerRigidbody = playerCloth.player.GetComponent<Rigidbody>();
		clothRandomCountdownTime = playerCloth.randomCountdownTime;
		playerCloth.randomCountdownTimer = clothRandomCountdownTime;

		DissolveCloth();
		DrawClothHP();		
		SetClothRotation(0f, -Vector3.forward);
		
		playerCloth.isInitCLoth = true; 
	}

	void CheckHP () {
		if (playerCloth.isHPChange) {
			DissolveCloth();
		} else if (playerCloth.isReducingCloth) {
			if (healthThreshold < currentDissolveValue) {
				healthThreshold += clothReduceValue * deltaTime;
				DrawClothHP();
			} else if (healthThreshold > currentDissolveValue) {
				healthThreshold -= clothReduceValue * deltaTime;
				DrawClothHP();
			} else {
				playerCloth.isReducingCloth = false;
			}
		}
	}

	void DissolveCloth () {
		float playerHP = playerCloth.playerHealth.PlayerHP;
		currentDissolveValue = 1 - playerHP / maxHP;
		// currentDissolveValue = playerCloth.clothRenderer.material.GetFloat("_Level");
		playerCloth.isReducingCloth = true;
		playerCloth.isHPChange = false;
	}

	void DrawClothHP () {
		playerCloth.clothRenderer.material.SetFloat("_Level", healthThreshold);
		// Debug.Log(healthThreshold);
	}

	void CheckDirection () {
		// Debug.Log(playerCloth.currentDirID+" : "+facing.DirID);
		if (playerCloth.currentDirID != facing.DirID) {
			playerCloth.currentDirID = facing.DirID;
			int currentDirID = playerCloth.currentDirID;

			switch (currentDirID) {
				case 1: //DOWN
					SetClothRotation(0f, new Vector3(1f, 0.5f, 1f)); 
					break;
				case 2: //LEFT
					SetClothRotation(90f, new Vector3(1f, 0.5f, -1f)); 
					break;
				case 3: //UP
					SetClothRotation(180f, new Vector3(-1f, 0.5f, -1f)); 
					break;
				case 4: //RIGHT
					SetClothRotation(-90f, new Vector3(-1f, 0.5f, 1f)); 
					break;
			}
		} else {
			RandomClothAccelY();
		}
	}

	void SetClothRotation (float rotY, Vector3 accel) {
		playerCloth.cloth.externalAcceleration = accel * playerCloth.addOnAccel;	
	}

	void RandomClothAccelY () {
		if (playerInput.moveDir == Vector3.zero && playerRigidbody.velocity == Vector3.zero) {
			if (playerCloth.randomCountdownTimer > 0f) {
				Vector3 newAccel = playerCloth.cloth.externalAcceleration;

				if (playerCloth.randomMultiplierToggle) {
					newAccel.y += deltaTime * playerCloth.addRandomAccel;
					// Debug.Log("True : "+newAccel.y);
				} else {
					newAccel.y -= deltaTime * playerCloth.addRandomAccel;
					// Debug.Log("false : "+newAccel.y);
				}

				playerCloth.cloth.externalAcceleration = newAccel;
				playerCloth.randomCountdownTimer -= deltaTime;
			} else {
				bool toggle = playerCloth.randomMultiplierToggle;
				toggle = !toggle;
				playerCloth.randomMultiplierToggle = toggle;
				playerCloth.randomCountdownTimer = clothRandomCountdownTime;
			}
		} else {
			playerCloth.randomMultiplierToggle = false;
			playerCloth.randomCountdownTimer = clothRandomCountdownTime;
		}
	}
}
