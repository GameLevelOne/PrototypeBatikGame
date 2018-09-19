using Unity.Entities;
using UnityEngine;

public class FogFXSystem : ComponentSystem {

	public struct FogFXComponent{
		public readonly int Length;
		public ComponentArray<FogFX> fogFX;
		public ComponentArray<Animator> fogFXAnim;
	}

	[InjectAttribute] FogFXComponent fogFXComponent;
	FogFX fogFX;
	Animator anim;

	protected override void OnUpdate()
	{
		for(int i = 0;i<fogFXComponent.Length;i++){
			fogFX = fogFXComponent.fogFX[i];
			anim = fogFXComponent.fogFXAnim[i];

			CheckFogFX();
		}
	}

	void CheckFogFX()
	{
		if(!fogFX.init){
			Init();
		}

		if(fogFX.changeSpeed){
			ChangeAnimSpeed();
		}
	}

	void Init()
	{
		fogFX.init = true;

		RandomAlphaAndSpeed();

		float rnd = Random.value;
		anim.Play("Animate",0,rnd);
	}

	void ChangeAnimSpeed()
	{
		fogFX.changeSpeed = false;
		RandomAlphaAndSpeed();
	}

	void RandomAlphaAndSpeed()
	{
		float rnd = Random.Range(0.5f,1f);
		fogFX.sprite.color = new Color(1f,1f,1f,rnd);

		fogFX.animSpeed = Random.Range(fogFX.minAnimSpeed,fogFX.maxAnimSpeed);
		anim.speed = fogFX.animSpeed;
	}
}