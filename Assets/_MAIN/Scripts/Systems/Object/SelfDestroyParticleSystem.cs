using UnityEngine;
using Unity.Entities;

public class SelfDestroyParticleSystem : ComponentSystem {
	public struct SelfDestroyParticleData {
		public readonly int Length;
		public ComponentArray<SelfDestroyParticle> SelfDestroyParticle;
	}
	[InjectAttribute] SelfDestroyParticleData selfDestroyParticleData;

	public SelfDestroyParticle selfDestroyParticle;

	// Transform gameFXTransform;

	protected override void OnUpdate () {
		if (selfDestroyParticleData.Length == 0) return;

		for (int i=0; i<selfDestroyParticleData.Length; i++) {
			selfDestroyParticle = selfDestroyParticleData.SelfDestroyParticle[i];
			// playerFXTransform = gameFXData.Transform[i];

			CheckParticles ();
		}
	}

	void CheckParticles () {
		bool checker = false;
		int particleQty = selfDestroyParticle.particles.Length;

		for (int i=0; i<particleQty; i++) {
			checker |= selfDestroyParticle.particles[i].IsAlive();
		}

		if (!checker) {
			DestroyParticle();
			// Debug.Log("OK");
		}
	}

	void DestroyParticle () {
        GameObjectEntity.Destroy(selfDestroyParticle.gameObject);
        UpdateInjectedComponentGroups(); //TEMP, Error without this
    }
}
