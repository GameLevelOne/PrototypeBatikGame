using Unity.Entities;
using UnityEngine;

public class TreeSystem : ComponentSystem {

	public struct TreeComponent{
		public readonly int Length;
		public ComponentArray<Tree> tree;
		public ComponentArray<Animator> treeAnim;
	}

	[InjectAttribute] public TreeComponent treeComponent;
	public Tree currTree;
	public Animator currTreeAnim;

	protected override void OnUpdate()
	{
		for(int i = 0;i<treeComponent.Length;i++){
			currTree = treeComponent.tree[i];
			currTreeAnim = treeComponent.treeAnim[i];

			InitTree();
		}

	}

	void InitTree()
	{
		if(!currTree.initAnimSpeed){
			currTree.initAnimSpeed = true;
			float rnd = Random.Range(currTree.minAnimSpeed,currTree.maxAnimSpeed);
			currTree.animSpeed = rnd;
			currTreeAnim.speed = currTree.animSpeed;
		}
	}

}
