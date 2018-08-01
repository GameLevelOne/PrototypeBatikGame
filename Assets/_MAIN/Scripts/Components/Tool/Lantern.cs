using UnityEngine;

public class Lantern : MonoBehaviour {

	[SerializeField] bool isLightOn = false;
	
	public bool IsLightOn {
		get {return isLightOn;}
		set {
			if (isLightOn == value) return;

			isLightOn = value;
		}
	}
}
