﻿using UnityEngine;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem {
	public struct InputData {
		public int Length;
		public ComponentArray<PlayerInput> PlayerInput;
	}
	[InjectAttribute] InputData inputData;

	Vector2 currentDir = Vector2.zero;

	protected override void OnUpdate () {
		if (inputData.Length == 0) return;
		
		for (int i=0; i<inputData.Length; i++) {
			PlayerInput input = inputData.PlayerInput[i];
			float maxValue = input.moveAnimValue[2];
			float midValue = input.moveAnimValue[1];
			float minValue = input.moveAnimValue[0];

			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				ChangeDir(i, currentDir.x, maxValue);
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				ChangeDir(i, currentDir.x, minValue);
			} 
			
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				ChangeDir(i, maxValue, currentDir.y);
			} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				ChangeDir(i, minValue, currentDir.y);
			} 
			
			if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
				ChangeDir(i, midValue, currentDir.y);
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
				ChangeDir(i, currentDir.x, midValue);
			}

			if (Input.GetButtonDown("Fire1")) {
				input.Attack = 1;
			}
		}
	}

	void ChangeDir (int idx, float dirX, float dirY) {
		Vector2 newDir = new Vector2(dirX, dirY);
		PlayerInput input = inputData.PlayerInput[idx];
		
		if (currentDir != newDir) {
			currentDir = newDir;
			input.Move = currentDir;
		}
	}
}
