﻿using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PortalSystem : ComponentSystem {

	
	public struct PortalComponent{
		public readonly int Length;
		public ComponentArray<Portal> portal;
	}

	[InjectAttribute] PortalComponent portalComponent;
	[InjectAttribute] PlayerInputSystem playerInputSystem;
	// [InjectAttribute] PlayerMovementSystem playerMovementSystem;
	[InjectAttribute] UIFaderSystem uiFaderSystem;
	[InjectAttribute] SystemManagerSystem systemManagerSystem;
	// [InjectAttribute] DamageSystem damageSystem;

	Portal currPortal;

	//inject important systems here
	Portal portalToLoad = null;

	protected override void OnUpdate()
	{
		for(int i = 0;i<portalComponent.Length;i++){
			currPortal = portalComponent.portal[i];
			CheckPlayer();
		}

	}

	void CheckPlayer()
	{
		if(currPortal.triggered){
			currPortal.triggered = false;
			playerInputSystem.input.moveDir = GetDirection(currPortal.dir);
			
			PlayerPrefs.SetInt(Constants.PlayerPrefKey.LEVEL_PLAYER_START_POS,currPortal.startPosIndex);

			//disble control systems
			DisableSystems();
			portalToLoad = currPortal;
			//fader
			currPortal.uiFader.state = FaderState.FadeOut;
		} else {
			if (currPortal.uiFader.initBlack) {
				//change scene
				if(portalToLoad!=null){
					Debug.Log("Scene Destiantion = "+portalToLoad.sceneDestination);
					SceneManager.LoadScene(portalToLoad.sceneDestination);
					portalToLoad = null;
				}
			}
		}
	}

	void DisableSystems () {
		systemManagerSystem.SetSystems(false);
	}

	Vector3 GetDirection (int dirID) {
		Vector3 dir = Vector2.zero;

		switch (dirID) {
			case 1: { //DOWN
				return new Vector3 (0f, 0f, -1f);
			}
			case 2: { //LEFT
				return new Vector3 (-1f, 0f, 0f);
			}
			case 3: { //UP
				return new Vector3 (0f, 0f, 1f);
			}
			case 4: { //RIGHT
				return new Vector3 (1f, 0f, 0f);
			}
			default: {
				return Vector3.zero;
			}
		}
	}
}
