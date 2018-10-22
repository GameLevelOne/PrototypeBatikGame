using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMainMenuSystem : ComponentSystem {
	public struct UIMainMenuData {
		public readonly int Length;
		public ComponentArray<UIMainMenu> UIMainMenu;
	}
	[InjectAttribute] UIMainMenuData uiMainMenuData;

	public struct PlayerData {
		public readonly int Length;
		public ComponentArray<Player> Player;
	}
	[InjectAttribute] PlayerData playerData;

	UIMainMenu uiMainmenu;

	// [InjectAttribute] GameStorageSystem gameStorageSystem;
	// [InjectAttribute] SystemManagerSystem systemManagerSystem;
	// [InjectAttribute] PlayerInputSystem playerInputSystem;

	protected override void OnUpdate () {

		for (int i=0; i<uiMainMenuData.Length; i++) {
			uiMainmenu = uiMainMenuData.UIMainMenu[i];

			if (!uiMainmenu.isInitialized) {
				Init();
			} else if (!uiMainmenu.isDoingStuff){
				CheckInput();
				ButtonAction();
			}
		}
	}

	void Init()
	{
		uiMainmenu.isInitialized = true;
		uiMainmenu.btnIndex = 0;

		if(PlayerPrefs.HasKey(Constants.PlayerPrefKey.LEVEL_CURRENT)){
			uiMainmenu.menuButtons[1].gameObject.SetActive(true);
		}else{
			uiMainmenu.menuButtons[1].gameObject.SetActive(false);
		}

		ShowCurrentSelected();
		uiMainmenu.isDoingStuff = false;
		uiMainmenu.isActionPressed = false;
		uiMainmenu.isUpPressed = false;
		uiMainmenu.isDownPressed = false;

		//FORCE PLAYER STATS HP TO 0
		for (int i=0;i< playerData.Length;i++) {
			playerData.Player[i].health.PlayerHP = 0f;
		}
	}

	void ShowCurrentSelected() {
		for (int i=0;i<uiMainmenu.menuButtons.Length;i++) {
			uiMainmenu.menuButtons[i].sprite = uiMainmenu.spriteNormal[i];
		}
		uiMainmenu.menuButtons[uiMainmenu.btnIndex].sprite = uiMainmenu.spriteSelect[uiMainmenu.btnIndex];
	}

	void CheckInput() {
		if (!uiMainmenu.isDownPressed && GameInput.IsDownDirectionHeld) {
			uiMainmenu.isDownPressed = true;
			uiMainmenu.btnIndex++;
			if (uiMainmenu.btnIndex==1 && !uiMainmenu.menuButtons[1].gameObject.activeSelf)
				uiMainmenu.btnIndex++;
			if (uiMainmenu.btnIndex>=uiMainmenu.menuButtons.Length)
				uiMainmenu.btnIndex = 0;

			ShowCurrentSelected();
			uiMainmenu.uiAudio.PlayOneShot(uiMainmenu.selectClip);
		} else if (!GameInput.IsDownDirectionHeld) {
			uiMainmenu.isDownPressed = false;
		}
		if (!uiMainmenu.isUpPressed && GameInput.IsUpDirectionHeld) {
			uiMainmenu.isUpPressed = true;
			uiMainmenu.btnIndex--;
			if (uiMainmenu.btnIndex==1 && !uiMainmenu.menuButtons[1].gameObject.activeSelf)
				uiMainmenu.btnIndex--;
			if (uiMainmenu.btnIndex<0)
				uiMainmenu.btnIndex = uiMainmenu.menuButtons.Length-1;

			uiMainmenu.uiAudio.PlayOneShot(uiMainmenu.selectClip);
			ShowCurrentSelected();
		} else if (!GameInput.IsUpDirectionHeld) {
			uiMainmenu.isUpPressed = false;
		}

		if (GameInput.IsAttackPressed || GameInput.IsActionPressed) {
			uiMainmenu.uiAudio.PlayOneShot(uiMainmenu.chooseClip);
			uiMainmenu.isActionPressed = true;
		}
	}

	void ButtonAction() {
		if (uiMainmenu.isActionPressed) {
			uiMainmenu.isDoingStuff = true;
			if (uiMainmenu.btnIndex==0) {//NEW GAME
				// PlayerPrefs.DeleteAll();	
				ResetPlayerPrefs();
				uiMainmenu.portal11.enabled = true;
			} else if (uiMainmenu.btnIndex==1) {//CONTINUE
				PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP, PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_HP, 100)); 
				PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP, PlayerPrefs.GetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_MP, 100)); 
				uiMainmenu.nextPortal.sceneDestination = PlayerPrefs.GetString(Constants.PlayerPrefKey.LEVEL_CURRENT);
				uiMainmenu.nextPortal.GetComponent<BoxCollider>().enabled = true;
			} else {//EXIT
				Debug.Log("EXIT GAME");
				if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
			}
		}
	}

	void ResetPlayerPrefs() {
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"0", 0);
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"1", 0);
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"2", 0);
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"3", 0);
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"4", 0);
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"5", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"0", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"1", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"2", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"3", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"4", 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"5", 0);

		//AREA DATA
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"OpeningMadaKari", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + "0", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.GATES_STATE + "0", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + "0", 0);
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"Level2-2", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_SPAWNER_STATE + "0", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + "2", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + "1", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CRACKED_WALL_STATE + "0", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_SPAWNER_STATE + "1", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + "3", 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + "2", 0);

		//PLAYER DATA
		PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_HP, 100f);
		PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_SAVED_MP, 100f);
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_STATS_COIN, 0);
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + "0", 0);
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_BOW, 0);
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_TOOL_FISHINGROD, 0);

		PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_HP, 100f);
		PlayerPrefs.SetFloat(Constants.PlayerPrefKey.PLAYER_STATS_MP, 100f);

		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_TOOL, 3);
		
		for (int i=0; i<4; i++) {
			PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_CONTAINER + i, 0);
		} 

		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_DEBUG_ATTACK_AREA, 0);
	}
}
