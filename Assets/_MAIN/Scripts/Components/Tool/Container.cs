using UnityEngine;

public class Container : MonoBehaviour {
	// public int containerType = 0;
	public CollectibleType[] collectibleTypes = new CollectibleType[4]; //4 Container
	public GameObject[] collectibleObj = new GameObject[6]; //6 CollectibleType length

	// bool isContainingObject = false;

	// public bool IsContainingObject {
	// 	get {return isContainingObject;}
	// 	set {
	// 		if (isContainingObject == value) return;

	// 		isContainingObject = value;
	// 	}
	// }

	public bool CheckIfContainerIsEmpty (int collectibleTypeIdx) {
		if (collectibleTypes[collectibleTypeIdx] == CollectibleType.NONE) {
			return true;
		} else {
			return false;
		}
	}
}
