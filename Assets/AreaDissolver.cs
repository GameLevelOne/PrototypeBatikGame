using UnityEngine;
using System.Collections.Generic;

public class AreaDissolver : MonoBehaviour {
	public List<Dissolver> dissolverObjs;

	public int levelQuestIndex;

	public bool initAutoReference = false;

	public bool isInitAreaDissolver = false;
	public bool isAreaAlreadyDissolved = false;

	[HeaderAttribute("Current")]
	public bool isDissolveArea = false;

	[HeaderAttribute("TESTING")]
	public bool isTesting = false;
}
