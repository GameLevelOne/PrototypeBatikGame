using Unity.Entities;
using UnityEngine.SceneManagement;

public class PortalSystem : ComponentSystem {

	
	public struct Component{
		public Portal portal;
	}

	protected override void OnUpdate()
	{
		foreach(var e in GetEntities<Component>()){
			if(e.portal.triggered){
				ChangeMap(e);
			}
		}
		
	}

	void ChangeMap(Component e)
	{
		SceneManager.LoadScene(e.portal.sceneDestination);
	}
}
