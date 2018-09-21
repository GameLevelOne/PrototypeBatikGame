using UnityEngine;
using Unity.Entities;

public class AreaDissolverSystem : ComponentSystem {
	public struct AreaDissolverData {
		public readonly int Length;
		public ComponentArray<AreaDissolver> AreaDissolver;
	}
	[InjectAttribute] AreaDissolverData areaDissolverData;

	AreaDissolver areaDissolver;

	float dissolverObjQty;

	protected override void OnUpdate () {
		for (int i=0; i<areaDissolverData.Length; i++) {
			areaDissolver = areaDissolverData.AreaDissolver[i];

			if (!areaDissolver.isInitAreaDissolver) {
				InitAreaDissolver();
			} else {
				CheckDissolve();
			}
		}
	}

	void InitAreaDissolver () {
		dissolverObjQty = areaDissolver.dissolverObj.Length;

		if (!areaDissolver.isInitAreaDissolver) {
			//
		}

		areaDissolver.isInitAreaDissolver = true;
	}

	void CheckDissolve() {
		if (areaDissolver.isDissolveArea) {
			for (int i=0; i<dissolverObjQty; i++) {
				areaDissolver.dissolverObj[i].dissolve = true;
			}
			
			areaDissolver.isAreaAlreadyDissolved = true;
			areaDissolver.isDissolveArea = false;
		}
	}

	public void CheckCurrentLevelbyQuest (int questIdx) {
		// if (areaDissolver.levelQuestIndex == questIdx && !areaDissolver.isAreaAlreadyDissolved) {
		// 	areaDissolver.isDissolveArea = true;
		// }
	}
}
