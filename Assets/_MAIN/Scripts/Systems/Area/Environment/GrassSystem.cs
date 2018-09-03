using Unity.Entities;
using UnityEngine;

public class GrassSystem : ComponentSystem {

	public struct GrassComponent{
		public readonly int Length;
		public ComponentArray<Grass> grass;
		public ComponentArray<Animator> grassAnim;
		
	}

	[InjectAttribute] public GrassComponent grassComponent;
	Grass currGrass;
	Animator currGrassAnim;

	const string IDLE = "Idle";

	protected override void OnUpdate()
	{
		for(int i = 0;i<grassComponent.Length;i++){
			currGrass = grassComponent.grass[i];
			currGrassAnim = grassComponent.grassAnim[i];

			CheckGrass();
		}
	}

	void CheckGrass()
	{
		if(!currGrass.init) InitGrass();
		if(currGrass.interact) Interact();
	
		if(currGrass.animateEnd){ 
			currGrass.animateEnd = false;
			InitGrass();
		}
		
		if(currGrass.destroy) Destroy();
	}

	void InitGrass()
	{
		currGrass.init = true;

		float rnd = Random.value;
		currGrassAnim.Play(IDLE,0,rnd);
	}

	void Interact()
	{
		currGrass.interact = false;
		float rnd = Random.value;
		if(rnd < 0.5f) currGrassAnim.SetTrigger(Constants.AnimatorParameter.Trigger.GRASS_WAVElEFT);
		else currGrassAnim.SetTrigger(Constants.AnimatorParameter.Trigger.GRASS_WAVERIGHT);
	}

	void Destroy()
	{
		GameObject.Destroy(currGrass.gameObject);
		UpdateInjectedComponentGroups();
	}
}