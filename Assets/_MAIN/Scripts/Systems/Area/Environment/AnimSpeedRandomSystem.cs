using Unity.Entities;
using UnityEngine;

public class AnimSpeedRandomSystem : ComponentSystem {
	public struct AnimSpeedRandomComponent{
		public readonly int Length;
		public ComponentArray<AnimSpeedRandom> animSpeedRandom;
		public ComponentArray<Animator> animator;
	}

	[InjectAttribute] AnimSpeedRandomComponent animSpeedRandomComponent;
	AnimSpeedRandom currAnimSpeedRandom;
	Animator currAnimator;

	protected override void OnUpdate()
	{
		for(int i = 0;i<animSpeedRandomComponent.Length;i++)
		{
			currAnimSpeedRandom = animSpeedRandomComponent.animSpeedRandom[i];
			currAnimator = animSpeedRandomComponent.animator[i];

			RandomizedAnimSpeed();
		}
	}

	void RandomizedAnimSpeed()
	{
		if(!currAnimSpeedRandom.initRandomSpeed){
			currAnimSpeedRandom.initRandomSpeed = true;

			float rnd = Random.Range(currAnimSpeedRandom.minSpeed,currAnimSpeedRandom.maxSpeed);
			currAnimSpeedRandom.animSpeed = rnd;
			currAnimator.speed = currAnimSpeedRandom.animSpeed;
		}
	}
}
