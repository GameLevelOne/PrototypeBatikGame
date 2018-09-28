using UnityEngine;
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
	[InjectAttribute] ContainerSystem containerSystem;
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

			//Save container
			SaveContainer();

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

	void SaveContainer () {
		LootableType[] lootableTypes = containerSystem.container.lootableTypes;

		for (int i=0; i<lootableTypes.Length; i++) {
			switch (lootableTypes[i]) {
				case LootableType.HP_POTION:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 1);
					break;
				case LootableType.MANA_POTION:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 2);
					break;
				default:
					PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 0);
					break;
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
