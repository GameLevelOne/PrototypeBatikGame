﻿using Unity.Entities;
using UnityEngine;

public class LightRaySystem : ComponentSystem {

	public struct LightRayComponent{
		public readonly int Length;
		public ComponentArray<LightRay> lightRay;
		public ComponentArray<Transform> lightRayTransform;
		
	}

	[InjectAttribute] public LightRayComponent lightRayComponent;
	LightRay currLightRay;
	Transform currLightRayTransform;

	protected override void OnUpdate()
	{
		for(int i = 0;i<lightRayComponent.Length;i++){
			currLightRay = lightRayComponent.lightRay[i];
			currLightRayTransform = lightRayComponent.lightRayTransform[i];

			SetAlpha();
		}

	}

	void SetAlpha()
	{
		foreach(MeshRenderer m in currLightRay.lightRays){
			m.material.color = new Color(m.material.color.r,m.material.color.g,m.material.color.b,AlphaValue());
		}
	}

	float AlphaValue()
	{
		float cameraZ = currLightRay.cameraTransform.position.z;
		float lightrayZ = currLightRayTransform.position.z;
		
		float delta = lightrayZ - cameraZ + 6f;

		if(delta < 0f) delta = 0f;
		else if(delta > currLightRay.distanceFactor) delta = currLightRay.distanceFactor;



		return delta / currLightRay.distanceFactor;
	}
}
