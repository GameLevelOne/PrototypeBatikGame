using UnityEngine.UI;
using UnityEngine;

public enum HintMessage{
	OPEN,
	LIFT,
	TALK,
	READ
}

public class UIInteractionHint : MonoBehaviour {
	public GameObject hintObj;
	public Text textHint;
	public string[] hintMessages;
	
	[HeaderAttribute("Current")]
	public bool isActive = false;

	public void ShowHint(HintMessage message)
	{
		if(!isActive){
			textHint.text = hintMessages[(int)message];
			hintObj.SetActive(true);
			isActive = true;
		}
		if(isActive && message == HintMessage.OPEN){
			textHint.text = hintMessages[(int)message];
			hintObj.SetActive(true);
		}
		
	}

	public void HideHint()
	{
		isActive = false;
		hintObj.SetActive(false);
	}
	
}
