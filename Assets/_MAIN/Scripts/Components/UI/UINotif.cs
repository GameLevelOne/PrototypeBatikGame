using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotif : MonoBehaviour {

	public bool call = false;
	public bool endShow = false;
	public Animator anim;
	public GameObject boxBorder;
	public Text notifText;

	void OnEndShowing()
	{
		endShow = true;
	}	

	public string TextToShow {
		set {
			notifText.text = value;
		}
	}
}
