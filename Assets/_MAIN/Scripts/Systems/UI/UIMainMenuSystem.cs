using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;


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
			Init();

			// if (uiMainmenu.titleState == UITitleState.NEWGAME) {
			// 	// uiMainmenu.btnStartGame.interactable = false;

			// 	OpenScene(Constants.SceneName.SCENE_LEVEL_1);
			// 	// uiMainmenu.fader.state = FaderState.FadeOut;
			// } else if(uiMainmenu.titleState == UITitleState.CONTINUE){
			// 	// uiMainmenu.isContinue = false;
			// 	// uiMainmenu.btnContinue.enabled = false;

				
			// 	OpenScene(GameStorage.Instance.CurrentScene);
			// }
			// uiMainmenu.titleState = UITitleState.NONE;

			// if(uiMainmenu.fader.state == FaderState.Black){
			// 	uiMainmenu.fader.state = FaderState.FadeIn;
			// 	playerInputSystem.Enabled = true;
			// 	uiMainmenu.canvas.SetActive(false);
			// }
		}
	}



	void OpenScene (string sceneName) {
		Debug.Log("Trying to load scene: "+sceneName);
		SceneManager.LoadSceneAsync(sceneName);
	}

	void Init()
	{
		if(!uiMainmenu.init){
			PlayerPrefs.DeleteAll();
			//playerInputSystem.Enabled = false;
			uiMainmenu.init = true;
			uiMainmenu.btnStartGame.Select();
			if(PlayerPrefs.HasKey(Constants.PlayerPrefKey.LEVEL_CURRENT)){
				uiMainmenu.btnContinue.gameObject.SetActive(true);
			}else{
				uiMainmenu.btnContinue.gameObject.SetActive(false);
			}
		}
	}
}
