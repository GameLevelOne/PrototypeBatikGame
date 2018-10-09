using Unity.Entities;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraShakerSystem : ComponentSystem {

	public struct CameraShakerComponent{
		public readonly int Length;
		public ComponentArray<Transform> cameraShakerTransform;
		public ComponentArray<CameraShaker> cameraShaker;
	}

	[InjectAttribute] CameraShakerComponent cameraShakerComponent;
	CameraShaker cameraShaker;
	Transform cameraShakerTransform;

	protected override void OnUpdate()
	{
		for(int i = 0;i<cameraShakerComponent.Length;i++){
			cameraShaker = cameraShakerComponent.cameraShaker[i];
			cameraShakerTransform = cameraShakerComponent.cameraShakerTransform[i];

			CheckShake();	
		}

	}

	void CheckShake()
	{
		if(cameraShaker.shake){
			cameraShaker.tShakeDelay += Time.deltaTime;
			if(cameraShaker.tShakeDelay >= cameraShaker.shakeDelay){
				cameraShaker.tShakeDelay = 0f;

				float x = Random.Range(-1f,1f) * cameraShaker.shakeMagnitude;
				float z = Random.Range(-1f,1f) * cameraShaker.shakeMagnitude;

				cameraShakerTransform.position = new Vector3(x,cameraShaker.originPos.y,z);
			}
		}else{
			cameraShakerTransform.position = cameraShaker.originPos;
		}
	}
}
