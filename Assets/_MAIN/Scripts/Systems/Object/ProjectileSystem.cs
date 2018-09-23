using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
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

    float deltaTime;

	protected override void OnUpdate () {
        deltaTime = Time.deltaTime;
		// if (projectileData.Length == 0) return;
        Vector3 verticalMultVector = new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier);

		for (int i=0; i<projectileData.Length; i++) {
			projectile = projectileData.Projectile[i];
			tr = projectileData.Transform[i];
			rb = projectileData.Rigidbody[i];

            projectileType = projectile.type;

            if (projectile.isStartLaunching) {
			    float speed = projectile.speed * 50f;

                switch (projectileType) {
                    case ProjectileType.BULLET: 
                        if (!projectile.isLaunching) {
                            Vector3 bulletDir = new Vector3 (tr.right.x, 0f, tr.right.z).normalized;

                            rb.AddForce(Vector3.Scale(bulletDir,verticalMultVector) * speed);
                            projectile.isLaunching = true;
                        }

                        break;
                    case ProjectileType.CATAPULT: 
                        if (!projectile.isLaunching) {
                            Vector3 catapultDir = projectile.direction;           
                            catapultDir.Normalize();
                            catapultDir.y = Mathf.Sin(projectile.elevationAngle * Mathf.Deg2Rad);

                            rb.AddForce(Vector3.Scale(catapultDir,verticalMultVector) * speed);
                            projectile.isLaunching = true;
                        }

                        break;
                }

                projectile.isStartLaunching = false;
            } else {
                if (projectile.isDestroyOnTriggering) {	
                    if (projectile.isSelfDestroying) {
                        DestroyProjectile();
                    }		
                }

                if (projectile.isDestroyOnColliding) {
                    if (projectile.isCollideSomething) {
                        //
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
