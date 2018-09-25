using Unity.Entities;
using UnityEngine;

public class TreeSystem : ComponentSystem {

	public struct TreeComponent{
		public readonly int Length;
		public ComponentArray<Tree> tree;
		public ComponentArray<Animator> treeAnim;
	}

	[InjectAttribute] public TreeComponent treeComponent;
	public Tree tree;
	public Animator treeAnim;

	protected override void OnUpdate()
	{
		for(int i = 0;i<treeComponent.Length;i++){
			tree = treeComponent.tree[i];
			treeAnim = treeComponent.treeAnim[i];

			CheckHit();
		}
	}

	void CheckHit()
	{
		if(tree.hit){
			tree.hit = false;
			treeAnim.SetTrigger(Constants.AnimatorParameter.Trigger.HIT);
		}
	}
}
