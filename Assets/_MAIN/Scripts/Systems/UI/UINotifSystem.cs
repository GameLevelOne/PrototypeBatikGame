using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class UINotifSystem : ComponentSystem {
public struct UINotifComponent{
		public readonly int Length;
		public ComponentArray<UINotif> uiNotif;
	}
	[InjectAttribute] UINotifComponent uiNotifComponent;

	UINotif curUINotif;

	protected override void OnUpdate()
	{
		for(int i = 0;i < uiNotifComponent.Length;i++){
			curUINotif = uiNotifComponent.uiNotif[i];
			CheckCall();
			CheckEndShow();
		}
	}

	void CheckCall()
	{
		if(curUINotif.call){
			Debug.Log("OpenNotif with text: "+ curUINotif.notifText.text);
			curUINotif.call = false;
			curUINotif.endShow = false;
			curUINotif.boxBorder.SetActive(true);
			curUINotif.anim.Play("UINotifShow",0,0f);
			curUINotif.uiAudio.Play();
		}
	}

	void CheckEndShow()
	{
		if(curUINotif.endShow){
			curUINotif.call = false;
			curUINotif.endShow = false;
			curUINotif.boxBorder.SetActive(false);
		}
	}

}
