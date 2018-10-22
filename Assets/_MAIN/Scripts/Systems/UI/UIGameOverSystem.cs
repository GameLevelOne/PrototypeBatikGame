﻿using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// to show UIGameOver: set uiGameOver.Call = true;
/// </summary>
public class UIGameOverSystem : ComponentSystem {

	public struct UIGameOverComponent
	{
		public readonly int Length;
		public ComponentArray<UIGameOver> uiGameOver;
	}


	[InjectAttribute] UIGameOverComponent uiGameOverComponent;
	[InjectAttribute] PlayerSystem playerSystem;
	UIGameOver uiGameOver;
	bool curDownPressed = false;
	bool curUpPressed = false;

	protected override void OnUpdate()
	{
		for (int i=0; i<uiGameOverComponent.Length; i++) {
			uiGameOver = uiGameOverComponent.uiGameOver[i];

			if (!uiGameOver.isInitialized) {
				Init();
			} else if (!uiGameOver.isDoingStuff){
				CheckCall();
				CheckInput();
				ButtonAction();
			}
		}
	}
	void Init()
	{
		uiGameOver.isInitialized = true;
		uiGameOver.btnIndex = 0;

		ShowCurrentSelected();
		uiGameOver.isDoingStuff = false;
		uiGameOver.isActionPressed = false;
		uiGameOver.isUpPressed = false;
		uiGameOver.isDownPressed = false;
	}
	void ShowCurrentSelected() {
		for (int i=0;i<uiGameOver.menuButtons.Length;i++) {
			uiGameOver.menuButtons[i].sprite = uiGameOver.spriteNormal[i];
		}
		uiGameOver.menuButtons[uiGameOver.btnIndex].sprite = uiGameOver.spriteSelect[uiGameOver.btnIndex];
	}

	void CheckCall()
	{
		if(uiGameOver.call){
			Debug.Log("GAME OVER SCREEN LOADED!");
			uiGameOver.call = false;
			uiGameOver.gameOverAnim.SetBool("Show",true);
			ShowCurrentSelected();
		}
	}

	void CheckInput()
	{
		if(uiGameOver.endShow){
			if (!curDownPressed && GameInput.IsDownDirectionHeld) {
				curDownPressed = true;
				// uiGameOver.buttonBackToMainMenu.Select();
				uiGameOver.btnIndex = 1;	
				uiGameOver.uiAudio.PlayOneShot(uiGameOver.selectClip);
			} else if (!GameInput.IsDownDirectionHeld) {
				curDownPressed = false;
			}

			if (!curUpPressed && GameInput.IsUpDirectionHeld) {
				curUpPressed = true;
				// uiGameOver.buttonRestart.Select();
				uiGameOver.btnIndex = 0;	
				uiGameOver.uiAudio.PlayOneShot(uiGameOver.selectClip);
			} else if (!GameInput.IsUpDirectionHeld) {
				curUpPressed = false;						
			}

			if(GameInput.IsAttackPressed || GameInput.IsActionPressed){
				uiGameOver.endShow = false;
				// uiGameOver.gameOverAnim.SetBool("Show",false);
				uiGameOver.panel.SetActive(false);
				uiGameOver.endHide = true;
				uiGameOver.uiAudio.PlayOneShot(uiGameOver.chooseClip);
			}
			ShowCurrentSelected();
		}		
	}

	void ButtonAction()
	{
		if(uiGameOver.endHide){
			// Debug.Log("Game Over menu select");
			uiGameOver.endHide = false;
			uiGameOver.isDoingStuff = true;
			if(uiGameOver.btnIndex==0){
				Restart();
			} else {
				ReturnToTitle();
			}
		}
	}

	void Restart()
	{
		Debug.Log("GAME OVER LOAD SCENE: "+SceneManager.GetActiveScene().name);
		playerSystem.ResetPlayerHP();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void ReturnToTitle()
	{
		Debug.Log("GAME OVER LOAD SCENE: "+Constants.SceneName.MAIN_MENU);
		playerSystem.ResetPlayerHP();
		SceneManager.LoadScene(Constants.SceneName.MAIN_MENU);
	}
}
