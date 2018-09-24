using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndGameSystem : ComponentSystem {

	public struct UIEndGameComponent{
		public readonly int Length;
		public ComponentArray<UIEndGame> uiEndGame;
		public ComponentArray<Animator> uiEndGameAnim;
	}

	[InjectAttribute] UIEndGameComponent uiEndGameComponent;
	UIEndGame uiEndGame;
	Animator anim;

	[InjectAttribute] SystemManagerSystem systemManager;

	protected override void OnUpdate()
	{
		for(int i = 0;i < uiEndGameComponent.Length;i++){
			uiEndGame = uiEndGameComponent.uiEndGame[i];
			anim = uiEndGameComponent.uiEndGameAnim[i];

			CheckCall();
			CheckInput();
		}
	}

	void CheckCall()
	{
		if(uiEndGame.call){
			uiEndGame.call = false;
			anim.SetBool("Show",true);
			uiEndGame.buttonBackToMainMenu.Select();
			systemManager.SetSystems(false);
		}
	}

	void CheckInput()
	{
		if(uiEndGame.backToMainMenu){
			Debug.Log("BACK TO MAIN MENU");
			uiEndGame.backToMainMenu = false;
			SceneManager.LoadScene(Constants.SceneName.MAIN_MENU);
		}
	}
}
