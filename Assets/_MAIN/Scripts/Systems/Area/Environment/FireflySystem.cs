using Unity.Entities;
using UnityEngine;

public class FireflySystem : ComponentSystem {

	public struct FireflyComponent{
		public readonly int Length;
		public ComponentArray<Firefly> firefly;
		public ComponentArray<Animator> fireflyAnim;
	}

	[InjectAttribute] public FireflyComponent fireflycomponent;
	Firefly currFirefly;
	Animator currFireflyAnim;

	protected override void OnUpdate()
	{
		for(int i = 0;i<fireflycomponent.Length;i++){
			currFirefly = fireflycomponent.firefly[i];
			currFireflyAnim = fireflycomponent.fireflyAnim[i];
			
			InitFirefly();
			Fly();
		}
	}

	void InitFirefly()
	{
		if(currFirefly.init) return;
		currFirefly.init = true;
		float rnd = Random.value;
		currFireflyAnim.Play("Idle",0,rnd);
	}

	void Fly()
	{
		if(currFirefly.fly){
			currFirefly.fly = false;

			float rnd = Random.value;
			if(rnd > 0.7f) currFireflyAnim.SetTrigger(Constants.AnimatorParameter.Trigger.FIREFLY_FLY);
		}
	}
	
}
