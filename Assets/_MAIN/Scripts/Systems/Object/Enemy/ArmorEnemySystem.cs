using Unity.Entities;
using UnityEngine;

public class ArmorEnemySystem : ComponentSystem {

	public struct ArmorEnemyComponent{
		public readonly int Length;
		public ComponentArray<Transform> armorEnemyTransform;
		public ComponentArray<ArmorEnemy> armorEnemy;
		public ComponentArray<Health> armorEnemyHealth;
		public ComponentArray<Rigidbody2D> armorEnemyRigidbody;
		public ComponentArray<Animator> armorEnemyAnim;
	}

	[InjectAttribute] ArmorEnemyComponent armorEnemyComponent;
	Transform currArmorEnemyTransform;
	ArmorEnemy currArmorEnemy;
	Health currArmorEnemyHealth;
	Rigidbody2D currArmorEnemyRigidbody;
	Animator currArmorEnemyAnim;

	protected override void OnUpdate()
	{
		// for(int i =0;i<armorEnemyComponent.Length;i++){
		// 	currArmorEnemyTransform = armorEnemyComponent.armorEnemyTransform[i];
		// 	currArmorEnemy = armorEnemyComponent.armorEnemy[i];
		// 	currArmorEnemyHealth = armorEnemyComponent.armorEnemyHealth[i];
		// 	currArmorEnemyRigidbody = armorEnemyComponent.armorEnemyRigidbody[i];
		// 	currArmorEnemyAnim = armorEnemyComponent.armorEnemyAnim[i];
		// }
	}

	void CheckState()
	{

	}

	void Idle(){}
	void Patrol(){}
	void Roll(){}
	
}
