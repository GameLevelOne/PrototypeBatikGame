using UnityEngine;
using Unity.Entities;

public class ProjectileSystem : ComponentSystem {
    public struct ProjectileData {
		public readonly int Length;
		public ComponentArray<Projectile> Projectile;
		public  ComponentArray<Transform> Transform;
		public  ComponentArray<Rigidbody> Rigidbody;
	}
	[InjectAttribute] ProjectileData projectileData;

	Projectile projectile;
    Transform tr;
    Rigidbody rb;

    ProjectileType projectileType;

	protected override void OnUpdate () {
		if (projectileData.Length == 0) return;

		for (int i=0; i<projectileData.Length; i++) {
			projectile = projectileData.Projectile[i];
			tr = projectileData.Transform[i];
			rb = projectileData.Rigidbody[i];

            projectileType = projectile.type;
			float speed = projectile.speed * 50f;

            if (projectile.isStartLaunching) {
                switch (projectileType) {
                    case ProjectileType.BULLET: 
                        if (!projectile.isLaunching) {
                            Vector3 bulletDir = new Vector3 (tr.right.x, 0f, tr.right.z);
                            rb.AddForce(bulletDir.normalized * speed);
                            projectile.isLaunching = true;
                        }

                        break;
                    case ProjectileType.CATAPULT: 
                        if (!projectile.isLaunching) {
                            Vector3 catapultDir = new Vector3 (tr.right.x, 0f, tr.right.z);             
                            catapultDir.Normalize();
                            catapultDir.y = Mathf.Sin(projectile.elevationAngle * Mathf.Deg2Rad);
                            rb.AddForce(catapultDir.normalized * speed);
                            
                            projectile.isLaunching = true;
                        }

                        break;
                }

                projectile.isStartLaunching = false;
            } else {
                if (projectile.isDestroyOnTriggering) {	
                    if (projectile.isTriggerSomething) {
                        DestroyProjectile();
                    }		
                }

                if (projectile.isDestroyOnColliding) {
                    if (projectile.isCollideSomething) {
                        DestroyProjectile();
                    }
                }
            }
		}
	}

    void DestroyProjectile () {
        GameObjectEntity.Destroy(projectile.gameObject);
        UpdateInjectedComponentGroups(); //TEMP, Error without this
    }
}
