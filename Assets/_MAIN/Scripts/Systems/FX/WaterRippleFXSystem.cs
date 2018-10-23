using Unity.Entities;
using UnityEngine;

public class WaterRippleFXSystem : ComponentSystem {

	public struct WaterRippleComponent{
		public readonly int Length;
		public ComponentArray<WaterRippleFX> waterRipple;
		public ComponentArray<Animator> anim;
	}

	[InjectAttribute] WaterRippleComponent waterRippleComponent;
	WaterRippleFX waterRipple;
	Animator anim;

	protected override void OnUpdate()
	{
		for(int i = 0;i<waterRippleComponent.Length;i++){
			waterRipple = waterRippleComponent.waterRipple[i];
			anim = waterRippleComponent.anim[i];

			CheckAnim();
		}
	}

	void CheckAnim()
	{
		if(waterRipple.animDone){
			DoDelay();
		}
	}

	void DoDelay()
	{
		if(!waterRipple.initDelay){
			waterRipple.initDelay = true;
			waterRipple.tDelay = Random.Range(3f,5f);
		}else{
			waterRipple.tDelay -= Time.deltaTime;
			
			if(waterRipple.tDelay <= 0f){
				waterRipple.initDelay = false;
				anim.Play("Animate",0,0f);
				waterRipple.animDone = false;
			}
		}
	}
}
