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

			InitStone();
			CheckStone();
		}
	}

	void InitStone()
	{
		if(!stone.init){
			stone.init = true;
			if(stone.dissolver.isDissolved) stone.liftable.liftableType = LiftableType.LIFTABLE;
			else stone.liftable.liftableType = LiftableType.UNLIFTABLE;
		}
	}

	void CheckStone()
	{
		// if(stone.hit && !stone.stoneBrokenObj.activeSelf){
		if(stone.hit){
			// stone.hit = false;
			GameObject.Instantiate(stone.stoneCrushFX, stoneTransform.position, Quaternion.Euler(40f, 0f, 0f));

			if(stone.stoneGreyObj.activeSelf) stone.stoneGreyObj.SetActive(false);
			// stone.stoneColorObj.SetActive(false);
			// // stone.stoneBrokenObj.SetActive(true);
			// stone.stoneCollider.enabled = false;
			// stone.parentCollider.enabled = false;
			// // stone.stoneParticle.gameObject.SetActive(true);
			// // stone.stoneParticle.Play();
			// stone.audioSource.Play();

			GameObject.Destroy(stone.gameObject);
			UpdateInjectedComponentGroups();
		}
		if(stone.dissolver.isDissolved) stone.liftable.liftableType = LiftableType.LIFTABLE;
	}
}
