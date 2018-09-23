using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// to show UIGameOver: set uiGameOver.Call = true;
/// </summary>
public class UIGameOverSystem : ComponentSystem {

	public struct UIGameOverComponent
	{
		public readonly int Length;
		public ComponentArray<UIGameOver> uiGameOver;
	}


	[InjectAttribute]UIGameOverComponent uiGameOverComponent;
	UIGameOver uiGameOver;

	protected override void OnUpdate()
	{
		for(int i = 0;i<uiGameOverComponent.Length;i++){
			uiGameOver = uiGameOverComponent.uiGameOver[i];

			CheckCall();
			CheckInput();
			CheckSelected();
			CheckButtonFunction();
		}
	}

	void CheckCall()
	{
		if(uiGameOver.call){
			uiGameOver.call = false;
			uiGameOver.black.SetActive(true);
			uiGameOver.gameOverAnim.SetBool("Show",true);
		}
	}

	void CheckInput()
	{
		if(uiGameOver.endShow){
			if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.DownArrow)){
				if(uiGameOver.selected != 1){
					uiGameOver.selected = 1;
				}
			}

			if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
				if(uiGameOver.selected != 0){
					uiGameOver.selected = 0;
				}
			}

			if(Input.GetKeyDown(KeyCode.Return)){
				if(uiGameOver.endShow){
					uiGameOver.endShow = false;
					uiGameOver.gameOverAnim.SetBool("Show",false);
				}
			}
		}		
	}

	void CheckSelected()
	{
		if(uiGameOver.selected == 0){
			uiGameOver.buttonRestart.color = Color.red;
			uiGameOver.buttonReturnToTitle.color = Color.white;
		}else if (uiGameOver.selected == 1){
			uiGameOver.buttonRestart.color = Color.white;
			uiGameOver.buttonReturnToTitle.color = Color.red;
		}
	}

	void CheckButtonFunction()
	{
		if(uiGameOver.endHide){
			uiGameOver.endHide = false;
			if(uiGameOver.doRestart){
				uiGameOver.doRestart = false;
				Restart();
			}

			if(uiGameOver.doReturnToTitle){
				uiGameOver.doReturnToTitle = false;
				ReturnToTitle();
			}
		}
	}

	void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void ReturnToTitle()
	{
		SceneManager.LoadScene(Constants.SceneName.MAIN_MENU);
	}
}
