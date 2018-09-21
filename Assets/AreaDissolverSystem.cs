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

	public void DissolvedArea (int questIdx) {
		if (areaDissolver.levelQuestIndex == questIdx) {
			areaDissolver.isDissolveArea = true;
		}
	}

	public void DissableGreyDissolver (int questIdx) {
		if (areaDissolver.levelQuestIndex == questIdx) {
			for (int i=0; i<dissolverObjQty; i++) {
				for (int j=0; j<areaDissolver.dissolverObj[i].mRenderer.Count; j++) {
					areaDissolver.dissolverObj[i].mRenderer[j].gameObject.SetActive(false);
				}
			}
		}
	}

	public bool CheckCurrentLevelbyQuest (int questIdx) {
		if (areaDissolver.levelQuestIndex == questIdx && !areaDissolver.isAreaAlreadyDissolved) {
			return true;
		} else {
			return false;
		}
	}

	public bool CheckIfAreaIsAlreadyDissolved (int questIdx) {
		return areaDissolver.isAreaAlreadyDissolved;
	}
}
