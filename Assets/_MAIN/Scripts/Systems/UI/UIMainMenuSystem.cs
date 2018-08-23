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

	protected override void OnUpdate () {
		if (uiMainMenuData.Length == 0) return;

		for (int i=0; i<uiMainMenuData.Length; i++) {
			uiMainmenu = uiMainMenuData.UIMainMenu[i];

			if (uiMainmenu.isStartGame) {
				uiMainmenu.btnStartGame.interactable = false;
				uiMainmenu.isStartGame = false;

				OpenScene(Constants.SceneName.GAME_MAP_01);
			}
		}
	}

	void OpenScene (string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}
