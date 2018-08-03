using UnityEngine;
using Unity.Entities;

public class BeeSystem : ComponentSystem {

	public struct BeeComponent
	{
		public readonly int Length;
		public ComponentArray<Bee> bee;
	}

	#region injected component
	[InjectAttribute] public BeeComponent beeComponent;
	Bee currBee;
	#endregion

	#region injected system
	[InjectAttribute] public BeeMovementSystem beeMovementSystem;
	#endregion

	protected override void OnUpdate()
	{
		// for(int i = 0;i<beeComponent.Length;i++){
		// 	currBee = beeComponent.bee[i];
		// 	CheckState();
		// }
	}

	void CheckState()
	{
		
	}

	public void SetBeeState(BeeState state)
	{
		currBee.beeState = state;
	}
}
