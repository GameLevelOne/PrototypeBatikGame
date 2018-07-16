using Unity.Entities;
using UnityEngine;

public class LevelDataSystem : ComponentSystem {

	public struct Component{
		public LevelData levelData;
	}

	// void OnLevelWasLoaded(int level)
	// {
	// 	this.Enabled = true;
	// }

	protected override void OnUpdate()
	{
		foreach(var e in GetEntities<Component>()){
			if(!e.levelData.isInitialied){
				InitializeLevelData(e);
			}
		}
		
	}

	void InitializeLevelData(Component e)
	{
		e.levelData.isInitialied = true;
		GameObject.Instantiate(e.levelData.playerObj,e.levelData.playerStartPos,Quaternion.identity);

		// this.Enabled = false;
	}
}
