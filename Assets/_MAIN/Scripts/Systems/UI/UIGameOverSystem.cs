using Unity.Entities;
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


	[InjectAttribute]UIGameOverComponent uiGameOverComponent;
	UIGameOver uiGameOver;

	protected override void OnUpdate()
	{
		for(int i = 0;i<uiGameOverComponent.Length;i++){
			uiGameOver = uiGameOverComponent.uiGameOver[i];

			CheckCall();
			CheckInput();
			CheckButtonFunction();
		}
	}

	void CheckCall()
	{
		if(uiGameOver.call){
			uiGameOver.call = false;
			uiGameOver.gameOverAnim.SetBool("Show",true);
			uiGameOver.buttonRestart.Select();
		}
	}

	void CheckInput()
	{
		if(uiGameOver.endShow){
			if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.DownArrow)){
				// uiGameOver.buttonBackToMainMenu.Select();
				Debug.Log("BACK TO MAIN MENU SELECTED");
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(uiGameOver.buttonBackToMainMenu.gameObject);	
				Debug.Log("SELECTED = "+EventSystem.current.currentSelectedGameObject.name);			
			}

			if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
				// uiGameOver.buttonRestart.Select();
				Debug.Log("RESTART SELECTED");
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(uiGameOver.buttonRestart.gameObject);		
				Debug.Log("SELECTED = "+EventSystem.current.currentSelectedGameObject.name);
						
			}

			if(Input.GetKeyDown(KeyCode.Return)){
				uiGameOver.endShow = false;
				// uiGameOver.gameOverAnim.SetBool("Show",false);
				uiGameOver.panel.SetActive(false);
				uiGameOver.endHide = true;
			}
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
