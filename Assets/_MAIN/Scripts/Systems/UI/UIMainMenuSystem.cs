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

	[InjectAttribute] GameStorageSystem gameStorageSystem;
	[InjectAttribute] SystemManagerSystem systemManagerSystem;

	protected override void OnUpdate () {

		for (int i=0; i<uiMainMenuData.Length; i++) {
			Init();


			uiMainmenu = uiMainMenuData.UIMainMenu[i];

			if (uiMainmenu.isStartGame) {
				uiMainmenu.btnStartGame.interactable = false;
				uiMainmenu.isStartGame = false;

				OpenScene(Constants.SceneName.GAME_MAP_01);
			}

			if(uiMainmenu.isContinue){
				uiMainmenu.isContinue = false;
				uiMainmenu.btnContinue.enabled = false;

				OpenScene(GameStorage.Instance.CurrentScene);
			}
		}
	}



	void OpenScene (string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	void Init()
	{
		if(!uiMainmenu.init){
			uiMainmenu.init = true;
			if(PlayerPrefs.HasKey(Constants.PlayerPrefKey.LEVEL_CURRENT)){
				uiMainmenu.btnContinue.gameObject.SetActive(true);
			}else{
				uiMainmenu.btnContinue.gameObject.SetActive(false);
			}
		}
	}
}
