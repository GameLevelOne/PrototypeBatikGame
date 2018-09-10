using UnityEngine;
using Unity.Entities;

public class SystemManagerSystem : ComponentSystem {
	public struct SystemManagerComponent{
		public readonly int Length;
		public ComponentArray<SystemManager> SystemManager;
	}
	[InjectAttribute] SystemManagerComponent systemManagerComponent;[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] DamageSystem damageSystem;
	// [InjectAttribute] ToolSystem toolSystem;
	// [InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	SystemManager systemManager;

	protected override void OnUpdate()
	{
		for(int i = 0;i<systemManagerComponent.Length;i++){
			systemManager = systemManagerComponent.SystemManager[i];
			
			// if (!systemManager.isDoneInitDisabledSystem) {
				CheckSystemManager();
			// }
		}
	}

	void CheckSystemManager() {
		if (systemManager.isChangeScene) {
			SetSystems (true);
			systemManager.isChangeScene = false;
		}

		// systemManager.isDoneInitDisabledSystem = true;
	}

	public void SetSystems (bool value) {
		playerInputSystem.Enabled = value;
		damageSystem.Enabled = value;
	}
}
