using UnityEngine;
using Unity.Entities;

public class VinesSystem : ComponentSystem {
	public struct VinesData {
		public readonly int Length;
		public ComponentArray<Vines> Vines;
	}
	[InjectAttribute] VinesData vinesData;

	Vines vines;

	float deltaTime;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;

		for (int i=0; i<vinesData.Length; i++) {
			vines = vinesData.Vines[i];

			if (!vines.isInitVines) {
				InitVines();
			} else {
				CheckVines();
			}
		}
	}

	void InitVines () {
		bool isVinesAlreadyDestroyed = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.VINES_STATE + vines.vinesID, 0) == 1 ? true : false;

		if (isVinesAlreadyDestroyed) {
			vines.isDestroyed = true;
			DestroyVines ();
		}

		vines.isInitVines = true;
	}

	void CheckVines () {
		if (!vines.isDestroyed) {
			if (vines.isInitBurned) {
				if (!vines.isBurned) {
					PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + vines.vinesID, 1);
					vines.burnParticle.Play(true);

					vines.isBurned = true;
				} else {
					if (vines.burnTimer < vines.burnDuration) {
						vines.burnTimer += Time.deltaTime;
					} else {
						vines.burnParticle.Stop(true);
						DestroyVines ();
					}
				}
			}			
		}
	}

	void DestroyVines () {
		vines.vinesCol.enabled = false;
		vines.vinesObj.SetActive(false);
	}
}
