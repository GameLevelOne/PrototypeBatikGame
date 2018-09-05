using Unity.Entities;
using UnityEngine.SceneManagement;

public class PortalSystem : ComponentSystem {

	
	public struct PortalComponent{
		public readonly int Length;
		public ComponentArray<Portal> portal;
	}

	[InjectAttribute] PortalComponent portalComponent;
	Portal currPortal;

	//inject important systems here

	protected override void OnUpdate()
	{
		for(int i = 0;i<portalComponent.Length;i++){
			currPortal = portalComponent.portal[i];
			CheckPlayer();
		}
		
	}

	void CheckPlayer()
	{
		if(currPortal.triggered){
			currPortal.triggered = false;
			
			//disble control systems
			//fader
			//change scene
		}
	}

}
