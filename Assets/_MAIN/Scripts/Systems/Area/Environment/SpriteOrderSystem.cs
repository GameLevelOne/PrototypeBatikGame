using Unity.Entities;
using UnityEngine;

public class SpriteOrderSystem : ComponentSystem {

	public struct SpriteOrderComponent{
		public readonly int Length;
		public ComponentArray<Transform> transform;
		public ComponentArray<SpriteRenderer> sprite;
		public ComponentArray<SpriteOrder> spriteOrder;
	}

	[InjectAttribute] public SpriteOrderComponent spriteOrderComponent;
	Transform currTransform;
	SpriteRenderer currSprite;
	SpriteOrder currSpriteOrder;

	protected override void OnUpdate()
	{
		for(int i = 0;i<spriteOrderComponent.Length;i++){
			currTransform = spriteOrderComponent.transform[i];
			currSprite = spriteOrderComponent.sprite[i];
			currSpriteOrder = spriteOrderComponent.spriteOrder[i];

			CheckSpriteOrder();
		}
	}

	void CheckSpriteOrder()
	{
		if(!currSpriteOrder.initOrder){
			currSpriteOrder.initOrder = true;
			currSprite.sortingOrder = GetSpriteOrder(currTransform.position.y);
		}
	}

	int GetSpriteOrder(float y)
	{
		int order = -1 * Mathf.FloorToInt(y *= 10f);

		return order;
	}
}
