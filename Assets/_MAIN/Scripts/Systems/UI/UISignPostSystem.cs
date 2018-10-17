using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class UISignPostSystem : ComponentSystem {

	public struct UISignPostData
	{
		public readonly int Length;
		public ComponentArray<UISignPost> uiSignPost;
	}


	[InjectAttribute]UISignPostData uiSignPostData;
	UISignPost uiSignPost;

	public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
	}
	[InjectAttribute] InputData inputData;
	PlayerInput playerInput;

	protected override void OnUpdate () {
		for (int j=0;j<inputData.Length;j++) {
			playerInput = inputData.PlayerInput[j];
		}
		for (int i=0;i<uiSignPostData.uiSignPost.Length;i++) {
			uiSignPost = uiSignPostData.uiSignPost[i];

			if (!uiSignPost.isInitSignPost) {
				Init();
				uiSignPost.anim.Play("Hide",0,1f);
			} else {		
				CheckCall();		
				CheckInput();
				CloseSignPost();
			}
		}
	}

	void Init() {
		// if (Input.GetJoystickNames().Length>0 && Input.GetJoystickNames()[0] != "") {
		// 	uiSignPost.joyStickPanel.SetActive(true);
		// 	uiSignPost.keyboardPanel.SetActive(false);
		// } else {
		// 	uiSignPost.joyStickPanel.SetActive(false);
		// 	uiSignPost.keyboardPanel.SetActive(true);
		// }
		uiSignPost.keyboardPanel.SetActive(true);
		uiSignPost.joyStickPanel.SetActive(false);
		uiSignPost.isInitSignPost = true;
		uiSignPost.isCloseSignPost = false;
		uiSignPost.curLeftPressed = false;
		uiSignPost.curRightPressed = false;
	}

	void CheckCall() {
		if (uiSignPost.call) {
			Time.timeScale = 0f;
			uiSignPost.call = false;			
			Init();
			uiSignPost.anim.Play("Show",0,0f);
			playerInput.isUIOpen = true;
		}
	}

	void SwitchPanel() {
		if (uiSignPost.joyStickPanel.activeSelf) {
			uiSignPost.joyStickPanel.SetActive(false);
			uiSignPost.keyboardPanel.SetActive(true);			
		} else {
			uiSignPost.joyStickPanel.SetActive(true);
			uiSignPost.keyboardPanel.SetActive(false);
		}
	}
	void CheckInput() {
		if(uiSignPost.anim.GetCurrentAnimatorStateInfo(0).IsName("Show")){
			if (!uiSignPost.curLeftPressed && GameInput.IsLeftDirectionHeld) {
				uiSignPost.curLeftPressed = true;
				SwitchPanel();
			} else if (!GameInput.IsLeftDirectionHeld) {
				uiSignPost.curLeftPressed = false;
			}

			if (!uiSignPost.curRightPressed && GameInput.IsRightDirectionHeld) {
				uiSignPost.curRightPressed = true;
				SwitchPanel();
			} else if (!GameInput.IsRightDirectionHeld) {
				uiSignPost.curRightPressed = false;						
			}

			if(GameInput.AnyButtonPressed){
				uiSignPost.isCloseSignPost = true;	
			}
		}				
	}

	void CloseSignPost() {
		if (uiSignPost.isCloseSignPost) {
			Time.timeScale = 1f;
			uiSignPost.anim.Play("Hide",0,0f);
			playerInput.isUIOpen = false;
			uiSignPost.isCloseSignPost = false;
		}
	}
}
