using Unity.Entities;
using UnityEngine;

public class BeeBehaviorSystem : ComponentSystem {

	public struct BeeBehaviorComponent
	{
		public readonly int Length;
		public BeeBehavior beeBehavior;
		public Animator beeBehaviorAnim;
	}

	[InjectAttribute] public BeeBehaviorComponent beeBehaviorComponent;
	public Animator currAnim
	;

	protected override void OnUpdate()
	{
		// for(int i = 0;i<beeBehaviorComponent.Length;i++){
		// 	currAnim = beeBehaviorComponent.beeBehaviorAnim[i];
		// }
	}

	void Randomize()
	{
		
	}
	
}
