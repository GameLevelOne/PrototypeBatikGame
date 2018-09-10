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

	protected override void OnUpdate () {
		if (playerClothData.Length == 0) return;

		for (int i=0; i<playerClothData.Length; i++) {
			playerCloth = playerClothData.PlayerCloth[i];
			clothTransform = playerClothData.Transform[i];
			
			if (!playerCloth.isInitCLoth) {
				InitCloth ();
			} else {
				facing = playerCloth.facing;

				CheckDirection ();
			}
		}
	}

	void InitCloth () {
		SetClothRotation(0f, Vector3.forward);
		// playerCloth.cloth.ClearTransformMotion();

		playerCloth.isInitCLoth = true; 
	}

	void CheckDirection () {
		// Debug.Log(playerCloth.currentDirID+" : "+facing.DirID);
		if (playerCloth.currentDirID != facing.DirID) {
			playerCloth.currentDirID = facing.DirID;
			int currentDirID = playerCloth.currentDirID;

			switch (currentDirID) {
				case 1: //DOWN
					SetClothRotation(0f, new Vector3(1f, -1f, 0.5f)); 
					break;
				case 2: //LEFT
					SetClothRotation(90f, new Vector3(1f, -0.5f, -1f)); 
					break;
				case 3: //UP
					SetClothRotation(180f, new Vector3(-1f, -0.5f, -1f)); 
					break;
				case 4: //RIGHT
					SetClothRotation(-90f, new Vector3(-1f, -1f, 0.5f)); 
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
