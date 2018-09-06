// using Unity.Entities;
// using UnityEngine;

// public class WaterShooterBulletSystem : ComponentSystem {

// 	public struct WaterShooterBulletComponent{
// 		public readonly int Length;
// 		public ComponentArray<Transform> waterShooterBulletTransform;
// 		public ComponentArray<WaterShooterBullet> waterShooterBullet;
// 	}

// 	#region injected component
// 	[InjectAttribute] WaterShooterBulletComponent waterShooterBulletComponent;
// 	public Transform currWaterShooterBulletTransform;
// 	public WaterShooterBullet currWaterShooterBullet;
// 	#endregion

// 	float deltaTime;

// 	protected override void OnUpdate()
// 	{
// 		for(int i = 0;i<waterShooterBulletComponent.Length;i++){
// 			currWaterShooterBulletTransform = waterShooterBulletComponent.waterShooterBulletTransform[i];
// 			currWaterShooterBullet = waterShooterBulletComponent.waterShooterBullet[i];

// 			Fly();
// 		}
// 	}

// 	void Fly()
// 	{
// 		if(currWaterShooterBullet.init){
// 			deltaTime = Time.deltaTime;
// 			currWaterShooterBulletTransform.Translate(currWaterShooterBullet.direction * currWaterShooterBullet.speed * deltaTime);

// 			if(currWaterShooterBullet.destroyed){
// 				GameObject.Destroy(currWaterShooterBullet.gameObject);
// 				UpdateInjectedComponentGroups();
// 			}
// 		}
// 	}
// }