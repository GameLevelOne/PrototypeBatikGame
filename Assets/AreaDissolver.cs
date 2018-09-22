using UnityEngine;

public class AreaDissolver : MonoBehaviour {
	public Dissolver[] dissolverObj;

	public int levelQuestIndex;

	public bool isInitAreaDissolver = false;
	public bool isAreaAlreadyDissolved = false;

	[HeaderAttribute("Current")]
	public bool isDissolveArea = false;

	[HeaderAttribute("TESTING")]
	public bool isTesting = false;
}
