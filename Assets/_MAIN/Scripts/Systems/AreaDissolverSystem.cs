using UnityEngine;
using Unity.Entities;

public class AreaDissolverSystem : ComponentSystem {
	public struct AreaDissolverData {
		public readonly int Length;
		public ComponentArray<AreaDissolver> AreaDissolver;
	}

	public struct DissolverComponent{
		public readonly int Length;
		public ComponentArray<Dissolver> dissolver;
	}

	[InjectAttribute] AreaDissolverData areaDissolverData;
	[InjectAttribute] DissolverComponent dissolverComponent;

	AreaDissolver areaDissolver;
	Dissolver dissolver;

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

		if(!areaDissolver.initAutoReference){
			// Debug.Log("Dissolver count = "+dissolverComponent.Length);
			for(int i = 0;i<dissolverComponent.Length;i++){
				dissolver = dissolverComponent.dissolver[i];

				ValidateGreyObject();
				Debug.Log("i = "+i+", dissolverComponentLength = "+dissolverComponent.Length);
				if(i == dissolverComponent.Length-1){
					areaDissolver.initAutoReference = true;
				}
			}
		}
		

	}

	void ValidateGreyObject()
	{
		if(dissolver.greyLayerObj.activeSelf){
			areaDissolver.dissolverObjs.Add(dissolver);
		}
	}

	void InitAreaDissolver () {
		//dissolverObjQty = areaDissolver.dissolverObj.Length;
		


		if (areaDissolver.isTesting) {
			SaveAreaDissolver(areaDissolver.levelQuestIndex, 0);
		}

		LoadAreaDissolver ();

		areaDissolver.isInitAreaDissolver = true;
	}

	void CheckDissolve() {
		if (areaDissolver.isDissolveArea) {
			for (int i=0; i<areaDissolver.dissolverObjs.Count; i++) {
				areaDissolver.dissolverObjs[i].dissolve = true;
			}
			
			areaDissolver.isAreaAlreadyDissolved = true;
			areaDissolver.isDissolveArea = false;
			SaveAreaDissolver(areaDissolver.levelQuestIndex, 1);
		}
	}

	void LoadAreaDissolver () {
		string areaDissolverStr = Constants.DissolvedLevelPrefKey.LEVEL_INDEX + areaDissolver.levelQuestIndex;

		areaDissolver.isAreaAlreadyDissolved = PlayerPrefs.GetInt(areaDissolverStr, 0) == 0 ? false : true;

		Debug.Log(areaDissolverStr);
		Debug.Log(areaDissolver.isAreaAlreadyDissolved);
	}

	void SaveAreaDissolver (int questIdx, int value) {
		if (areaDissolver.levelQuestIndex == questIdx) {
			string areaDissolverStr = Constants.DissolvedLevelPrefKey.LEVEL_INDEX + areaDissolver.levelQuestIndex;

			PlayerPrefs.SetInt(areaDissolverStr, value);

			Debug.Log(areaDissolverStr);
			Debug.Log(areaDissolver.isAreaAlreadyDissolved);
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
				for (int j=0; j<areaDissolver.dissolverObjs[i].mRenderer.Count; j++) {
					areaDissolver.dissolverObjs[i].mRenderer[j].gameObject.SetActive(false);
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
