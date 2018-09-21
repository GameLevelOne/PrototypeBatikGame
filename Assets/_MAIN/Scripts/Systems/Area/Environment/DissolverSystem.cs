using Unity.Entities;
using UnityEngine;

public class DissolverSystem : ComponentSystem {

	public struct DissolverComponent{
		public readonly int Length;
		public ComponentArray<Dissolver> dissolver;
	}

	[InjectAttribute] DissolverComponent dissolverComponent;
	Dissolver currDissolver;
	float dissolveValue;

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i = 0; i < dissolverComponent.Length; i++){
			currDissolver = dissolverComponent.dissolver[i];
			InitDissolver();
			UpdateDissolver();

			CheckDissolve();
			CheckUnDissolve();
		}
	}

	void InitDissolver()
	{
		if(currDissolver.init) return;

		for(int i = 0;i< currDissolver.mRenderer.Count;i++){
			currDissolver.dissolveValue.Add(currDissolver.mRenderer[i].material.GetFloat("_Level"));
		}
		currDissolver.init = true;
	}

	void UpdateDissolver()
	{
		if(currDissolver.init){
			for(int i = 0;i<currDissolver.mRenderer.Count;i++)
				currDissolver.mRenderer[i].material.SetFloat("_Level",currDissolver.dissolveValue[i]);
		}
	}

	void CheckDissolve()
	{
		if(currDissolver.dissolve){
			deltaTime = Time.deltaTime;
			int maxIndex = currDissolver.dissolveValue.Count-1;
			
			if(currDissolver.dissolveValue[maxIndex] >= 1f){
				//DISABLE GREY MESH
				for (int i=0; i<currDissolver.mRenderer.Count; i++) {
					currDissolver.mRenderer[i].gameObject.SetActive(false);
				}

				currDissolver.dissolve = false;
				return;
			} 

			for(int i = 0;i<currDissolver.dissolveValue.Count;i++){
				currDissolver.dissolveValue[i] += deltaTime * currDissolver.dissolveSpeed;
			}
			
		}
	}

	void CheckUnDissolve()
	{
		if(currDissolver.unDissolve){
			deltaTime = Time.deltaTime;
			int maxIndex = currDissolver.dissolveValue.Count-1;
			
			if(currDissolver.dissolveValue[maxIndex] <= 0f){
				currDissolver.unDissolve = false;
				return;
			} 

			for(int i = 0;i<currDissolver.dissolveValue.Count;i++){
				currDissolver.dissolveValue[i] -= deltaTime * currDissolver.dissolveSpeed;
			}
			
		}
	}
}
