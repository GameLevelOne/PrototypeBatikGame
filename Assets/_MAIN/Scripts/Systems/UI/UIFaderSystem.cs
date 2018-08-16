using Unity.Entities;
using UnityEngine.UI;
using UnityEngine;

public class UIFaderSystem : ComponentSystem {

	public struct UIFaderComponent{
		public readonly int Length;
		public ComponentArray<UIFader> uiFader;
		public ComponentArray<Image> uiFaderImage;
	}

	[InjectAttribute] UIFaderComponent uiFaderComponent;
	UIFader currUIFader;
	Image currUIFaderImage;

	Color faderColor;
	float tFade;
	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i=0;i<uiFaderComponent.Length;i++){
			currUIFader = uiFaderComponent.uiFader[i];
			currUIFaderImage = uiFaderComponent.uiFaderImage[i];

			CheckFaderState();

		}
	}

	void CheckFaderState()
	{
		if(currUIFader.state == FaderState.Clear){
			Clear();
		}else if(currUIFader.state == FaderState.Black){
			Black();
		}else if(currUIFader.state == FaderState.FadeIn){
			FadeIn();
		}else if(currUIFader.state == FaderState.FadeOut){
			FadeOut();
		}
	}

	void FadeIn()
	{
		
		if(!currUIFader.initFadingIn){
			currUIFader.initClear = false;
			currUIFader.initBlack = false;
			currUIFader.initFadingIn = true;
			faderColor = currUIFaderImage.color;
			tFade = 0f;
			deltaTime = Time.fixedDeltaTime;
		}else{
			faderColor = Color.Lerp(Color.black,Color.clear,tFade);
			currUIFaderImage.color = faderColor;
			tFade += deltaTime;

			if(tFade >= 1f){
				currUIFader.initFadingIn = false;
				currUIFaderImage.color = Color.clear;
				currUIFader.state = FaderState.Clear;
			}
		}
	}

	void FadeOut()
	{
		if(!currUIFader.initFadingOut){
			currUIFader.initClear = false;
			currUIFader.initBlack = false;
			currUIFader.initFadingOut = true;
			faderColor = currUIFaderImage.color;
			tFade = 0f;
			deltaTime = Time.fixedDeltaTime;
		}else{
			faderColor = Color.Lerp(Color.clear,Color.black,tFade);
			currUIFaderImage.color = faderColor;
			tFade += deltaTime;

			if(tFade >= 1f){
				currUIFader.initFadingOut = false;
				currUIFaderImage.color = Color.black;
				currUIFader.state = FaderState.Black;
			}
		}
	}

	void Clear()
	{
		if(!currUIFader.initClear){
			currUIFader.initClear = true;
			currUIFaderImage.color = Color.clear;
		}
	}

	void Black()
	{
		if(!currUIFader.initBlack){
			currUIFader.initBlack = true;
			currUIFaderImage.color = Color.black;
		}
	}
}
