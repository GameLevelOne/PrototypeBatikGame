﻿using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMainMenuSystem : ComponentSystem {
	public struct UIMainMenuData {
		public readonly int Length;
		public ComponentArray<UIMainMenu> UIMainMenu;
	}
	[InjectAttribute] UIMainMenuData uiMainMenuData;
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

			ShowCurrentSelected();
		} else if (!GameInput.IsUpDirectionHeld) {
			uiMainmenu.isUpPressed = false;
		}

		if (GameInput.IsAttackPressed) {
			uiMainmenu.isActionPressed = true;
		}
	}

	void ButtonAction() {
		if (uiMainmenu.isActionPressed) {
			uiMainmenu.isDoingStuff = true;
			if (uiMainmenu.btnIndex==0) {//NEW GAME
				PlayerPrefs.DeleteAll();	
				uiMainmenu.portal11.enabled = true;
			} else if (uiMainmenu.btnIndex==0) {//CONTINUE
				uiMainmenu.nextPortal.sceneDestination = PlayerPrefs.GetString(Constants.PlayerPrefKey.LEVEL_CURRENT);
				uiMainmenu.nextPortal.GetComponent<BoxCollider>().enabled = true;
			} else {//EXIT
				Debug.Log("EXIT GAME");
				Application.Quit();
			}
		}
	}
}
