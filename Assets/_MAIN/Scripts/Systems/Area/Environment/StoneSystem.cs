using Unity.Entities;
using UnityEngine;

public class StoneSystem : ComponentSystem {
	
	public struct StoneComponent{
		public readonly int Length;
		public ComponentArray<Stone> stone;
		public ComponentArray<Transform> stoneTransform;
	}

	[InjectAttribute] StoneComponent stoneComponent;
	Stone stone;
	Transform stoneTransform;

	protected override void OnUpdate()
	{
		for(int i = 0;i<stoneComponent.Length;i++){
			stone = stoneComponent.stone[i];
			stoneTransform = stoneComponent.stoneTransform[i];

			CheckStone();
		}
	}

	void CheckStone()
	{
		if(stone.hit){
			stone.hit = false;
			if(stone.stoneGreyObj.activeSelf) stone.stoneGreyObj.SetActive(false);
			stone.stoneColorObj.SetActive(false);
			stone.stoneBrokenObj.SetActive(true);
			stone.stoneCollider.enabled = false;
			stone.stoneParticle.gameObject.SetActive(true);
			stone.stoneParticle.Play();
		}
	}

}
